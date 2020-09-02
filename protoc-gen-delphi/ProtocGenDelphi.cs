/// Copyright 2020 Connor Roehricht (connor.work)
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

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi
{
    /// <summary>
    /// Plug-in for the protobuf compiler <c>protoc</c> that generates Delphi unit source code files for protobuf schema definitions.
    /// </summary>
    public class ProtocGenDelphi
    {
        /// <summary>
        /// File name extension (without leading dot) for protobuf schema definitions
        /// </summary>
        public static readonly string protoFileExtension = "proto";

        /// <summary>
        /// File name extension (without leading dot) for generated Delphi unit source files
        /// </summary>
        public static readonly string unitSourceFileExtension = "pas";

        /// <summary>
        /// Path separator used in protobuf file names
        /// </summary>
        public static readonly string protoFileNamePathSeparator = "/";

        static void Main(string[] args)
        {
            // protoc communicates with the plug-in through stdin and stdout
            using Stream input = Console.OpenStandardInput();
            using Stream output = Console.OpenStandardOutput();
            CodeGeneratorRequest request = CodeGeneratorRequest.Parser.ParseFrom(input);
            ProtocGenDelphi plugIn = new ProtocGenDelphi();
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
            CodeGeneratorResponse response = new CodeGeneratorResponse();
            // Generate one source code file for each .proto file
            foreach (string protoFileName in request.FileToGenerate) response.File.Add(GenerateSourceFile(request.ProtoFile.First(file => file.Name == protoFileName)));
            return response;
        }

        /// <summary>
        /// Generates a Delphi source code file for a protobuf schema definition.
        /// </summary>
        /// <param name="protoFile">The .proto file defining the schema</param>
        /// <returns>The source code file in the format expected by <c>protoc</c></returns>
        private CodeGeneratorResponse.Types.File GenerateSourceFile(FileDescriptorProto protoFile)
        {
            // Generate a new Delphi unit
            Unit unit = GenerateUnit(protoFile);
            return new CodeGeneratorResponse.Types.File()
            {
                Name = string.Join(protoFileNamePathSeparator, unit.ToSourceFilePath()),
                Content = unit.ToSourceCode()
            };
        }

        /// <summary>
        /// Generates a Delphi unit for a protobuf schema definition.
        /// </summary>
        /// <param name="protoFile">The .proto file defining the schema</param>
        /// <returns>The unit</returns>
        private Unit GenerateUnit(FileDescriptorProto protoFile)
        {
            return new Unit()
            {
                Heading = GenerateUnitIdentifier(protoFile),
                Interface = new Interface(),
                Implementation = new Implementation()
            };
        }

        /// <summary>
        /// Generates a Delphi unit identifer for a protobuf schema definition.
        /// </summary>
        /// <param name="protoFile">The .proto file defining the schema</param>
        /// <returns>The unit identifier</returns>
        private UnitIdentifier GenerateUnitIdentifier(FileDescriptorProto protoFile) => new UnitIdentifier()
        {
            // Use .proto file filename without extensions as identifier
            Unit = $"u{protoFile.Name.Split(protoFileNamePathSeparator)[^1].Split(".")[0].ToPascalCase()}"
        };
    }
}
