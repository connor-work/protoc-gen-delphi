/// Copyright 2025 Connor Erdmann (connor.work)
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

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Work.Connor.Delphi;
using Binding = Work.Connor.Delphi.MethodInterfaceDeclaration.Types.Binding;

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi;

/// <summary>
/// Aggregation of Delphi source code elements that are part of a generated Delphi property.
/// </summary>
/// <remarks>
/// Properties with getter methods are currently not supported.
/// Write-only properties are currently not supported.
/// </remarks>
internal sealed class DelphiPropertySourceCode
{
    /// <summary>
    /// TODO
    /// </summary>
    public AnnotationComment? Comment { get; init; } = null;

    /// <summary>
    /// TODO
    /// </summary>
    public required Visibility Visibility { get; init; }

    /// <summary>
    /// TODO
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// TODO
    /// </summary>
    public required bool ReadOnly { get; init; } = false;

    /// <summary>
    /// TODO
    /// </summary>
    public sealed class SetterSourceCode
    {
        /// <summary>
        /// TODO
        /// </summary>
        public string? ParameterComment { get; init; } = null;

        /// <summary>
        /// TODO
        /// </summary>
        public IList<string> LocalDeclarations { get; } = [];

        /// <summary>
        /// TODO
        /// </summary>
        public IList<string> Statements { get; } = [];
    }

    /// <summary>
    /// TODO
    /// </summary>
    public SetterSourceCode? Setter { get; init; } = null;

    /// <summary>
    /// TODO
    /// </summary>
    public required string Type { get; init; }

    /// <summary>
    /// TODO
    /// </summary>
    public AnnotationComment? BackingFieldComment { get; init; } = null;

    /// <summary>
    /// TOOD
    /// </summary>
    private string BackingFieldName => $"F{Name}";

    /// <summary>
    /// TODO
    /// </summary>
    private AnnotationComment DefaultBackingFieldComment => $"""
        /// <summary>
        /// Backing field of <see cref="{Name}"/>.
        /// </summary>
        """.AnnotationComment();

    /// <summary>
    /// TODO
    /// </summary>
    private FieldDeclaration BackingFieldDeclaration => new()
    {
        Comment = BackingFieldComment ?? DefaultBackingFieldComment,
        Name = BackingFieldName,
        Type = Type,
    };

    /// <summary>
    /// TODO
    /// </summary>
    [MemberNotNullWhen(true, nameof(Setter))]
    [MemberNotNullWhen(true, nameof(SetterMethod))]
    private bool HasSetter => !ReadOnly && Setter is not null;

    /// <summary>
    /// TODO
    /// </summary>
    private string SetterMethodName => $"Set{Name}";

    /// <summary>
    /// TODO
    /// </summary>
    private string DefaultSetterParameterComment => $"The new value of <see cref=\"{Name}\"/>";

    /// <summary>
    /// TODO
    /// </summary>
    private Parameter SetterParameter => new()
    {
        Name = "aValue",
        Type = Type,
    };

    /// <summary>
    /// TODO
    /// </summary>
    private DelphiMethodSourceCode? SetterMethod
    {
        get
        {
            if (!HasSetter) return null;
            DelphiMethodSourceCode result = new()
            {
                Comment = $"""
                    <summary>
                    Setter for <see cref="{Name}"/>.
                    </summary>
                    /// <param name="aValue">{Setter.ParameterComment ?? DefaultSetterParameterComment}</param>
                    """.AnnotationComment(),
                Visibility = Visibility.Private,
                Name = SetterMethodName,
                RoutineType = Prototype.Types.Type.Procedure,
                ParameterList = { SetterParameter },
                Binding = Binding.Static,
            };
            result.LocalDeclarations.AddRange(Setter.LocalDeclarations);
            result.Statements.AddRange(Setter.Statements);
            return result;
        }
    }

    /// <summary>
    /// TODO
    /// </summary>
    private PropertyDeclaration PropertyDeclaration => new()
    {
        Comment = Comment,
        Name = Name,
        ReadSpecifier = BackingFieldName,
        WriteSpecifier = ReadOnly ? null
                                  : (HasSetter ? SetterMethodName : BackingFieldName),
        Type = Type,
    };

    /// <summary>
    /// TODO
    /// </summary>
    /// <returns></returns>
    public IEnumerable<ClassDeclarationNestedDeclaration> Declare() => [
        BackingFieldDeclaration.InClass(Visibility.Private),
        ..(SetterMethod?.Declare()).CollectIfPresent(),
        PropertyDeclaration.InClass(Visibility),
    ];

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="className"></param>
    /// <returns></returns>
    public IEnumerable<MethodDeclaration> Implement(string className) => [
        ..(SetterMethod?.Implement(className)).CollectIfPresent(),
    ];
}
