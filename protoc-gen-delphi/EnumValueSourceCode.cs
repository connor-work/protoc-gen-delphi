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

using System.Collections.Generic;
using Google.Protobuf.Reflection;
using Work.Connor.Delphi;
using static Work.Connor.Delphi.CodeWriter.StringExtensions;

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi
{
    /// <summary>
    /// Aggregation of Delphi source code elements that represent a protobuf enum value.
    /// </summary>
    /// <remarks>
    /// The protobuf enum value is mapped to a Delphi enumerated value.
    /// </remarks>
    internal class EnumValueSourceCode
    {
        /// <summary>
        /// Optional prefix to the generated identifier
        /// </summary>
        private readonly string? prefix;

        /// <summary>
        /// Mapping of protobuf enum values to identifiers for Delphi enumerated values
        /// </summary>
        private IdentifierGenerator<EnumValueDescriptorProto> EnumValueIdentifier => new IdentifierTemplate<EnumValueDescriptorProto>("enumerated value", x => x.Name, "_ProtobufEnumValue", IdentifierCase.Pascal, prefix ?? "", caseSensitive: false);

        /// <summary>
        /// Protobuf enum value to generate code for
        /// </summary>
        private readonly EnumValueDescriptorProto enumValue;

        /// <summary>
        /// Constructs Delphi source code representing a protobuf enum value.
        /// </summary>
        /// <param name="prefix">Optional prefix to the generated identifier</param>
        /// <param name="enumValue">Protobuf enum value to generate code for</param>
        public EnumValueSourceCode(string? prefix, EnumValueDescriptorProto enumValue)
        {
            this.prefix = prefix;
            this.enumValue = enumValue;
        }

        /// <summary>
        /// Name of the Delphi enumerated value
        /// </summary>
        private string DelphiEnumValueName => EnumValueIdentifier.Generate(enumValue, reservedIdentifiers: ProtocGenDelphi.ReservedIdentifiers);

        /// <summary>
        /// Generated Delphi enumerated value
        /// </summary>
        public EnumValueDeclaration DelphiEnumValue => new()
        {
            Name = DelphiEnumValueName,
            Ordinality = enumValue.Number,
            Comment = new() { CommentLines = { EnumValueComment } }
        };

        /// <summary>
        /// XML documentation comment for the generated Delphi enumerated value
        /// </summary>
        private IEnumerable<string> EnumValueComment => // TODO transfer protobuf comment
$@"<remarks>
This enumerated value corresponds to the protobuf enum constant <c>{enumValue.Name}</c>.
</remarks>".Lines();
    }
}
