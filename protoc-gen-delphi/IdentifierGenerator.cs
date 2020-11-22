/// Copyright 2020 Connor Roehricht (connor.work)
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
/// 
///     http://www.apache.org/licenses/LICENSE-2.0
/// 
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Work.Connor.Delphi.CodeWriter;
using static Work.Connor.Delphi.CodeWriter.StringExtensions;

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi
{
    /// <summary>
    /// Scheme for mapping entities of a specific type to source code identifiers,
    /// including an optional mechanism to avoid collision between generated identifiers or existing identifiers.
    /// </summary>
    /// <typeparam name="T">The type of entity to map</typeparam>
    internal abstract class IdentifierGenerator<T>
    {
        /// <summary>
        /// Human-readable name of the kind of identifier generated, e.g., "variable name"
        /// </summary>
        protected abstract string IdentifierType { get; }

        /// <summary>
        /// Generates an identifier for an entity.
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <param name="avoidCollision">If <see langword="true"/>, uses the collision avoidance mechanism</param>
        /// <returns>The generated identifier</returns>
        protected abstract string GenerateOwn(T entity, bool avoidCollision);

        /// <summary>
        /// Tests whether an identifier is potential output of the mapping scheme.
        /// </summary>
        /// <param name="identifier">The identifier</param>
        /// <returns> <see langword="true"/> if the identifier is a potential output</returns>
        protected abstract bool CouldGenerate(string identifier);

        public override string? ToString() => $"[{IdentifierType} generator]";

        /// <summary>
        /// Generates an identifier for an entity.
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <param name="colliders">Optional other mapping schemes that the identifier must not collide with</param>
        /// <param name="reservedIdentifiers">Optional reserved identifiers that the identifier must not collide with</param>
        /// <returns>The generated identifier</returns>
        public string Generate(T entity, IEnumerable<IdentifierGenerator<T>>? colliders = null, IEnumerable<string>? reservedIdentifiers = null)
        {
            colliders ??= Enumerable.Empty<IdentifierGenerator<T>>();
            reservedIdentifiers ??= Enumerable.Empty<string>();
            string identifier = GenerateOwn(entity, false);
            IdentifierGenerator<T>? avoidedCollider = colliders.FirstOrDefault(generator => generator.CouldGenerate(identifier));
            string? avoidedReservedIdentifier = null;
            if (avoidedCollider is null && reservedIdentifiers.Contains(identifier)) avoidedReservedIdentifier = identifier;
            if (avoidedReservedIdentifier is null) return identifier;
            identifier = GenerateOwn(entity, true);
            IdentifierGenerator<T>? unavoidableCollider = colliders.FirstOrDefault(generator => generator.CouldGenerate(identifier));
            if (!(unavoidableCollider is null)) throw new ArgumentException($"Even after collision avoidance, generated identifier \"{identifier}\" for entity {entity} might collide with {unavoidableCollider}", nameof(entity));
            return identifier;
        }
    }

    /// <summary>
    /// Scheme for mapping entities of a specific type to source code identifiers, based on a template.
    /// Uses an optional suffix string to avoid identifier collision.
    /// </summary>
    /// <typeparam name="T">The type of entity to map</typeparam>
    internal class IdentifierTemplate<T> : IdentifierGenerator<T>
    {
        /// <summary>
        /// Human-readable name of the kind of identifier templated, e.g., "variable name"
        /// </summary>
        private string Type { get; }

        /// <summary>
        /// Function producing the identifier core string for a mapped entity
        /// </summary>
        private Func<T, string> Converter { get; }

        /// <summary>
        /// Suffix appended to the identifier core (before applying case style) when a collision needs to be avoided
        /// </summary>
        private string CollisionAvoidanceSuffix { get; }

        /// <summary>
        /// Identifier case style for the generated identifier
        /// </summary>
        private IdentifierCase Case { get; }

        /// <summary>
        /// Prefix prepended to the case-styled identifier core
        /// </summary>
        private string Prefix { get; }

        /// <summary>
        /// Suffix appended to the case-styled identifier core
        /// </summary>
        private string Suffix { get; }

        /// <summary>
        /// <see langword="true"/> if collision avoidance checks shall be case-sensitive
        /// </summary>
        private bool CaseSensitive { get; }

        /// <summary>
        /// Regular expression matching a potential output of the generation scheme (case-sensitive)
        /// </summary>
        private Regex IdentifierRegex => new Regex($@"^{Regex.Escape(Prefix)}{Case.IdentifierRegex()}{Regex.Escape(Suffix)}$");

        /// <summary>
        /// Regular expression matching a potential output of the generation scheme (case-insensitive version)
        /// </summary>
        private Regex IdentifierCaseInsensitiveRegex => new Regex($@"(?i){IdentifierRegex}(?-i)");

        protected override string IdentifierType => Type;

        /// <summary>
        /// Constructs a new template-based identifier generation scheme.
        /// </summary>
        /// <param name="type">Human-readable name of the kind of identifier templated, e.g., "variable name"</param>
        /// <param name="converter">Function producing the identifier core string for a mapped entity</param>
        /// <param name="collisionAvoidanceSuffix">Suffix appended to the identifier core (before applying case style) when a collision needs to be avoided</param>
        /// <param name="case">Identifier case style for the generated identifier</param>
        /// <param name="prefix">Prefix prepended to the case-styled identifier core</param>
        /// <param name="suffix">Suffix appended to the case-styled identifier core</param>
        /// <param name="caseSensitive"><see langword="true"/> if collision avoidance checks shall be case-sensitive</param>
        public IdentifierTemplate(string type, Func<T, string> converter, string collisionAvoidanceSuffix,
            IdentifierCase @case = IdentifierCase.None, string prefix = "", string suffix = "", bool caseSensitive = true)
        {
            Type = type;
            Converter = converter;
            CollisionAvoidanceSuffix = collisionAvoidanceSuffix;
            Case = @case;
            Prefix = prefix;
            Suffix = suffix;
            CaseSensitive = caseSensitive;
        }

        protected override bool CouldGenerate(string identifier) => (CaseSensitive ? IdentifierRegex : IdentifierCaseInsensitiveRegex).IsMatch(identifier);

        protected override string GenerateOwn(T entity, bool avoidCollision) => $"{Prefix}{(Converter.Invoke(entity) + (avoidCollision ? CollisionAvoidanceSuffix : "")).ToCase(Case)}{Suffix}";
    }
}
