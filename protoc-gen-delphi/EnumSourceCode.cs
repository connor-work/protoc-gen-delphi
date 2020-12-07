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
    internal class EnumSourceCode
    {
        /// <summary>
        /// Protobuf enum to generate code for
        /// </summary>
        public EnumDescriptorProto Enum { get; }

        /// <summary>
        /// Protobuf schema definition that this enum is part of
        /// </summary>
        private SchemaSourceCode Schema { get; }

        /// <summary>
        /// Constructs Delphi source code representing a protobuf enum.
        /// </summary>
        /// <param name="enum">Protobuf enum to generate code for</param>
        /// <param name="schema">Protobuf schema definition that this enum is part of</param>
        public EnumSourceCode(EnumDescriptorProto @enum, SchemaSourceCode schema)
        {
            Enum = @enum;
            Schema = schema;
        }

        /// <summary>
        /// Name of the Delphi enumerated type
        /// </summary>
        public string DelphiEnumName => ProtocGenDelphi.ConstructDelphiTypeName(Enum.Name); // TODO handling of absent name?

        /// <summary>
        /// Delphi source code representations of the protobuf enum values
        /// </summary>
        private IEnumerable<EnumValueSourceCode> EnumValues => Enum.Value.Select(enumValue => new EnumValueSourceCode(Enum.Name.ToCase(IdentifierCase.Pascal), enumValue));

        /// <summary>
        /// Constructs a Delphi identifier for the type, qualifying it if required.
        /// </summary>
        /// <param name="referencingSchema">Schema from which the type is referenced</param>
        /// <returns>The Delphi type identifier</returns>
        public string QualifiedDelphiTypeName(SchemaSourceCode referencingSchema) => referencingSchema.Equals(Schema) ? DelphiEnumName
                                                                                                                      : $"{Schema.DelphiUnitName}.{DelphiEnumName}";

        /// <summary>
        /// Generated Delphi enumerated type
        /// </summary>
        public EnumDeclaration DelphiEnum => new EnumDeclaration()
        {
            Name = DelphiEnumName,
            Values = { EnumValues.Select(enumValue => enumValue.DelphiEnumValue) },
            Comment = new AnnotationComment() { CommentLines = { EnumComment } }
            // TODO annotate
        };

        /// <summary>
        /// XML documentation comment for the generated Delphi enumerated type
        /// </summary>
        private IEnumerable<string> EnumComment => // TODO transfer protobuf comment
$@"<remarks>
This enumerated type corresponds to the protobuf enum <c>{Enum.Name}</c>.
</remarks>".Lines();
    }
}
