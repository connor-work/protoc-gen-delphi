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
using static Work.Connor.Delphi.CodeWriter.StringExtensions;

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi
{
    /// <summary>
    /// Aggregation of Delphi source code elements that represent a protobuf enum.
    /// </summary>
    /// <remarks>
    /// The protobuf enum is mapped to a Delphi enumerated type.
    /// </remarks>
    internal class EnumSourceCode : TypeSourceCode
    {
        /// <summary>
        /// Protobuf enum to generate code for
        /// </summary>
        public EnumDescriptorProto Enum { get; }

        /// <summary>
        /// Constructs Delphi source code representing a protobuf enum.
        /// </summary>
        /// <param name="enum">Protobuf enum to generate code for</param>
        /// <param name="schema">Protobuf schema definition that this enum is part of</param>
        /// <param name="containerType">Representation of the message type that this enum is nested in, absent if this is not a nested enum</param>
        public EnumSourceCode(EnumDescriptorProto @enum, SchemaSourceCode schema, MessageTypeSourceCode? containerType) : base(schema, containerType) => Enum = @enum;

        public override string TypeName => Enum.Name; // TODO handling of absent name?

        public override InterfaceDeclaration InterfaceDeclaration => new InterfaceDeclaration() { EnumDeclaration = DelphiEnum };

        public override NestedTypeDeclaration NestedTypeDeclaration => new NestedTypeDeclaration() { EnumDeclaration = DelphiEnum };

        /// <summary>
        /// Delphi source code representations of the protobuf enum values
        /// </summary>
        private IEnumerable<EnumValueSourceCode> EnumValues => Enum.Value.Select(enumValue => new EnumValueSourceCode(Enum.Name.ToCase(IdentifierCase.Pascal), enumValue));

        /// <summary>
        /// Generated Delphi enumerated type
        /// </summary>
        private EnumDeclaration DelphiEnum => new EnumDeclaration()
        {
            Name = DelphiTypeName,
            Values = { EnumValues.Select(enumValue => enumValue.DelphiEnumValue) },
            Comment = new AnnotationComment() { CommentLines = { EnumComment } }
            // TODO annotate
        };

        /// <summary>
        /// XML documentation comment for the generated Delphi enumerated type
        /// </summary>
        private IEnumerable<string> EnumComment => // TODO transfer protobuf comment
$@"<remarks>
This enumerated type corresponds to the protobuf enum <c>{TypeName}</c>.
</remarks>".Lines();
    }
}
