﻿/// Copyright 2020 Connor Roehricht (connor.work)
/// Copyright 2020 Sotax AG
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

using Google.Protobuf;
using Google.Protobuf.Compiler;
using Google.Protobuf.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Work.Connor.Delphi;
using Work.Connor.Delphi.CodeWriter;
using Work.Connor.Delphi.Commons.CodeWriterExtensions;
using static Work.Connor.Delphi.CodeWriter.StringExtensions;

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi
{
    /// <summary>
    /// Plug-in for the protobuf compiler <c>protoc</c> that generates Delphi unit source code files for protobuf schema definitions.
    /// </summary>
    public class ProtocGenDelphi
    {
        /// <summary>
        /// Mapping of protobuf type names to identifiers for Delphi types
        /// </summary>
        private static IdentifierGenerator<string> TypeIdentifier => new IdentifierTemplate<string>("type", x => x, "_ProtobufType", IdentifierCase.Pascal, "T", caseSensitive: false);

        /// <summary>
        /// Mapping of protobuf schema names to identifiers for Delphi units
        /// </summary>
        private static IdentifierGenerator<string> UnitIdentifier => new IdentifierTemplate<string>("unit", x => x, "_ProtobufSchema", IdentifierCase.Pascal, "u", caseSensitive: false);

        /// <summary>
        /// Reserved identifiers that shall not be used by generated source code
        /// </summary>
        internal static IEnumerable<string> ReservedIdentifiers => Enum.GetValues<ReservedWord>().Select(word => word.ToSourceCode());

        /// <summary>
        /// File name extension (without leading dot) for protobuf schema definitions
        /// </summary>
        public static readonly string protoFileExtension = "proto";

        /// <summary>
        /// File name extension (without leading dot) for generated Delphi unit source files
        /// </summary>
        public static readonly string unitSourceFileExtension = "pas";

        /// <summary>
        /// File name extension (without leading dot) for Delphi include files
        /// </summary>
        public static readonly string includeFileExtension = "inc";

        /// <summary>
        /// Path separator used in protobuf file names
        /// </summary>
        public static readonly string protoFileNamePathSeparator = "/";

        /// <summary>
        /// Support definition for the targetted protobuf runtime
        /// </summary>
        private readonly IRuntimeSupport runtime = IRuntimeSupport.Default;

        public static void Main(string[] args)
        {
            if (args.Length != 0) throw new ArgumentException("protoc-gen-delphi does not expect program arguments");
            // protoc communicates with the plug-in through stdin and stdout
            using Stream input = Console.OpenStandardInput();
            using Stream output = Console.OpenStandardOutput();
            CodeGeneratorRequest request = CodeGeneratorRequest.Parser.ParseFrom(input);
            ProtocGenDelphi plugIn = new();
            CodeGeneratorResponse response = plugIn.HandleRequest(request);
            response.WriteTo(output);
        }

        /// <summary>
        /// Handles a request from code generation that <c>protoc</c> passed to the plug-in,
        /// by generating a response message.
        /// </summary>
        /// <param name="request">The request from <c>protoc</c></param>
        /// <returns>The response to <c>protoc</c></returns>
        public CodeGeneratorResponse HandleRequest(CodeGeneratorRequest request)
        {
            if (request.Parameter.Length != 0) ApplyOptions(request.Parameter.Split(","));
            CodeGeneratorResponse response = new();
            FileDescriptorProto lookupProtoFile(string name) => request.ProtoFile.First(file => file.Name == name);
            // Generate one source code file for each .proto file
            foreach (string protoFileName in request.FileToGenerate) response.File.Add(GenerateSourceFile(lookupProtoFile(protoFileName), lookupProtoFile));
            return response;
        }

        /// <summary>
        /// Applies custom plug-in options passed through <c>protoc</c>.
        /// </summary>
        /// <param name="options">Sequence of option strings</param>
        private void ApplyOptions(IEnumerable<string> options)
        {
            foreach (string option in options)
            {
                string[] optionSegments = option.Split("=", 2);
                ApplyOption(optionSegments[0], optionSegments.Length > 0 ? optionSegments[1] : null);
            }
        }

        /// <summary>
        /// Applies a custom plug-in option passed through <c>protoc</c>.
        /// </summary>
        /// <param name="optionKey">Key of the option</param>
        /// <param name="optionValue">Optional value of the option</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1172:Unused method parameters should be removed", Justification = "Considered internal API")]
        private void ApplyOption(string optionKey, string? optionValue)
        {
            switch (optionKey)
            {
                default: throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Generates a Delphi source code file for a protobuf schema definition.
        /// </summary>
        /// <param name="protoFile">The .proto file defining the schema</param>
        /// <returns>The source code file in the format expected by <c>protoc</c></returns>
        private CodeGeneratorResponse.Types.File GenerateSourceFile(FileDescriptorProto protoFile, Func<string, FileDescriptorProto> lookupProtoFile)
        {
            SchemaSourceCode schema = new(lookupProtoFile, runtime, protoFile);
            // Generate a new Delphi unit
            Unit unit = schema.DelphiUnit;
            unit.AdaptForDelphiCommons();
            return new()
            {
                Name = string.Join(protoFileNamePathSeparator, unit.ToSourceFilePath()),
                Content = unit.ToSourceCode()
            };
        }

        /// <summary>
        /// Constructs a Delphi type name for a type that represents a protobuf message type or enum type.
        /// </summary>
        /// <param name="typeName">The protobuf type's name</param>
        /// <returns>The Delphi type name</returns>
        internal static string ConstructDelphiTypeName(string typeName)
        {
            if (!typeName.StartsWith(".")) return TypeIdentifier.Generate(typeName, reservedIdentifiers: ReservedIdentifiers);
            string[] typeNameSegments = typeName.Split(".", StringSplitOptions.RemoveEmptyEntries).ToArray();
            string unqualifiedName = ConstructDelphiTypeName(typeNameSegments[^1]);
            if (typeNameSegments.Length < 2) return unqualifiedName;
            return $"{ConstructUnitIdentifier(typeNameSegments[0..^2], typeNameSegments[^2]).ToSourceCode()}.{unqualifiedName}";
        }

        /// <summary>
        /// Constructs a Delphi unit identifier that corresponds to a protobuf qualified schema name.
        /// </summary>
        /// <param name="nameSpaceSegments">Segments in the protobuf namespace string</param>
        /// <param name="schemaName">Unqualified schema name (should be .proto file name without extension)</param>
        /// <returns>The unit identifier</returns>
        internal static UnitIdentifier ConstructUnitIdentifier(string[] nameSpaceSegments, string schemaName) => new()
        {
            Unit = UnitIdentifier.Generate(schemaName, reservedIdentifiers: ReservedIdentifiers),
            Namespace = { nameSpaceSegments.Select(segment => segment.ToCase(IdentifierCase.Pascal)) }
        };
    }
}
