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
using static Work.Connor.Delphi.Visibility;
using static Work.Connor.Delphi.MethodInterfaceDeclaration.Types.Binding;
using Work.Connor.Delphi.CodeWriter;

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
        private readonly EnumDescriptorProto @enum;

        /// <summary>
        /// Constructs Delphi source code representing a protobuf enum.
        /// </summary>
        /// <param name="enum">Protobuf enum to generate code for</param>
        public EnumSourceCode(EnumDescriptorProto @enum) => this.@enum = @enum;

        /// <summary>
        /// Name of the Delphi enumerated type
        /// </summary>
        private string DelphiEnumName => ProtocGenDelphi.ConstructDelphiTypeName(@enum.Name); // TODO handling of absent name?

        /// <summary>
        /// Delphi source code representations of the protobuf enum values
        /// </summary>
        private IEnumerable<EnumValueSourceCode> EnumValues => @enum.Value.Select(enumValue => new EnumValueSourceCode(@enum.Name.ToPascalCase(), enumValue));

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
This enumerated type corresponds to the protobuf enum <c>{@enum.Name}</c>.
</remarks>".Lines();
    }
}
