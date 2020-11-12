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

using System.Collections.Generic;
using System.Linq;
using Google.Protobuf.Reflection;
using Work.Connor.Delphi;
using Work.Connor.Delphi.CodeWriter;
using System;

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi
{
    /// <summary>
	/// Aggregation of Delphi source code elements that represent a protobuf schema definition.
    /// </summary>
    /// <remarks>
    /// The protobuf schema definition is mapped to a Delphi unit.
    /// </remarks>
    internal class SchemaSourceCode
    {
        /// <summary>
        /// Function that looks up a known protobuf schema definition by name
        /// </summary>
        private readonly Func<string, FileDescriptorProto> lookupProtofile;

        /// <summary>
        /// Support definition for the targetted protobuf runtime
        /// </summary>
        private readonly IRuntimeSupport runtime;

        /// <summary>
        /// Protobuf schema definition to generate code for
        /// </summary>
        private readonly FileDescriptorProto protoFile;

        /// <summary>
        /// Constructs Delphi source code representing a protobuf schema definition.
        /// </summary>
        /// <param name="lookupProtofile">Function that looks up a known protobuf schema definition by name</param>
        /// <param name="runtime">Support definition for the targetted protobuf runtime</param>
        /// <param name="protoFile">Protobuf schema definition to generate code for</param>
        public SchemaSourceCode(Func<string, FileDescriptorProto> lookupProtofile, IRuntimeSupport runtime, FileDescriptorProto protoFile)
        {
            this.lookupProtofile = lookupProtofile;
            this.runtime = runtime;
            this.protoFile = protoFile;
        }

        /// <summary>
        /// Delphi source code representations of the top-level protobuf enums
        /// </summary>
        private IEnumerable<EnumSourceCode> Enums => protoFile.EnumType.Select(@enum => new EnumSourceCode(@enum));

        /// <summary>
        /// Delphi source code representations of the top-level protobuf message types
        /// </summary>
        private IEnumerable<MessageTypeSourceCode> MessageTypes => protoFile.MessageType.Select(messageType => new MessageTypeSourceCode(messageType));

        /// <summary>
        /// Generated Delphi unit
        /// </summary>
        public Unit DelphiUnit => new Unit()
        {
            Heading = GenerateUnitIdentifier(protoFile),
            Interface = Interface,
            Implementation = Implementation,
            Comment = new AnnotationComment { CommentLines = { UnitComment } }
        };

        /// <summary>
        /// Generates a Delphi unit identifier for a protobuf schema definition.
        /// </summary>
        /// <param name="protoFile">The protobuf schema definition</param>
        /// <returns>The unit identifier</returns>
        private UnitIdentifier GenerateUnitIdentifier(FileDescriptorProto protoFile)
        {
            string[] fileNameSegments = protoFile.Name.Split(ProtocGenDelphi.protoFileNamePathSeparator);
            // Use namespace that matches the package, if no package is set, match the path
            string[] nameSpaceSegments = protoFile.HasPackage ? protoFile.Package.Split(".")
                                                              : fileNameSegments[0..^1];
            // Split off extension from file name
            return ProtocGenDelphi.ConstructUnitIdentifier(nameSpaceSegments, fileNameSegments[^1].Split(".")[0].ToPascalCase());
        }

        /// <summary>
        /// Interface section of the generated Delphi unit
        /// </summary>
        private Interface Interface
        {
            get
            {
                Interface @interface = new Interface()
                {
                    UsesClause = { Dependencies },
                    Declarations = { InterfaceDeclarations }
                };
                @interface.UsesClause.SortUsesClause();
                return @interface;
            }
        }

        /// <summary>
        /// Declarations within the interface section of the generated Delphi unit
        /// </summary>
        private IEnumerable<InterfaceDeclaration> InterfaceDeclarations
        {
            get
            {
                foreach (EnumSourceCode @enum in Enums) yield return new InterfaceDeclaration() { EnumDeclaration = @enum.DelphiEnum };
                foreach (MessageTypeSourceCode messageType in MessageTypes) yield return new InterfaceDeclaration() { ClassDeclaration = messageType.DelphiClass };
            }
        }

        /// <summary>
        /// Determines the required unit references within the generated Delphi unit
        /// </summary>
        /// <returns>Sequence of required unit references</returns>
        private IEnumerable<UnitReference> Dependencies => protoFile.Dependency.Select(name => new UnitReference() { Unit = GenerateUnitIdentifier(lookupProtofile(name)) })
                                                   .Concat(MessageTypes.SelectMany(messageType => messageType.Dependencies(runtime)))
                                                               .Distinct();

        /// <summary>
        /// Implementation section of the generated Delphi unit
        /// </summary>
        private Implementation Implementation => new Implementation()
        {
            Declarations = { MessageTypes.SelectMany(messageType => messageType.MethodDeclarations)
                                         .Select(method => new ImplementationDeclaration() { MethodDeclaration = method }) }
        };

        /// <summary>
        /// XML documentation comment for the generated Delphi unit
        /// </summary>
        private IEnumerable<string> UnitComment => // TODO transfer protobuf comment
$@"<remarks>
This unit corresponds to the protobuf schema definition (.proto file) <c>{protoFile.Name}</c>.
</remarks>".Lines();
    }
}
