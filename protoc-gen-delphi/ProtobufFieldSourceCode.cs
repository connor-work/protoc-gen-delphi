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

using Google.Protobuf.Reflection;
using System.Globalization;
using static Work.Connor.Delphi.CodeWriter.StringExtensions;
using Visibility = Work.Connor.Delphi.Visibility;

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi;

/// <summary>
/// Aggregation of generated Delphi source code elements that represent a Protobuf field.
/// </summary>
internal sealed partial class ProtobufFieldSourceCode
{
    /// <summary>
    /// Scheme for mapping a Protobuf field name to the name of a Delphi constant that contains the field number.
    /// </summary>
    private static IdentifierGenerator<string> FieldNumberDelphiConstantNameGenerator { get; } = new IdentifierTemplate<string>(x => x,
        collisionAvoidanceSuffix: "",
        @case: IdentifierCase.ScreamingSnake,
        prefix: "PROTOBUF_FIELD_NUMBER_",
        caseSensitive: false);

    /// <summary>
    /// Scheme for mapping a Protobuf field name to the name of a Delphi constant that contains the field name.
    /// </summary>
    private static IdentifierGenerator<string> FieldNameDelphiConstantNameGenerator { get; } = new IdentifierTemplate<string>(x => x,
        collisionAvoidanceSuffix: "",
        @case: IdentifierCase.ScreamingSnake,
        prefix: "PROTOBUF_FIELD_NAME_",
        caseSensitive: false);

    /// <summary>
    /// Source code that represents the Protobuf message type that contains the field.
    /// </summary>
    public required ProtobufMessageTypeSourceCode MessageType { get; init; }

    /// <summary>
    /// The represented Protobuf field.
    /// </summary>
    public required FieldDescriptorProto Field { get; init; }

    /// <summary>
    /// TODO
    /// </summary>
    public string FieldNumberDelphiConstantName => FieldNumberDelphiConstantNameGenerator.Generate(Field.Name, reservedIdentifiers: ProtocGenDelphi.ReservedIdentifiers);

    /// <summary>
    /// TODO
    /// </summary>
    public string FieldNameDelphiConstantName => FieldNameDelphiConstantNameGenerator.Generate(Field.Name, reservedIdentifiers: ProtocGenDelphi.ReservedIdentifiers);

    /// <summary>
    /// TODO
    /// </summary>
    public DelphiClassConstantSourceCode FieldNumberDelphiConstant => new()
    {
        Comment = $"""
            <summary>
            Protobuf field number of the Protobuf field <c>{Field.Name}</c>.
            </summary>
            """.AnnotationComment(),
        Visibility = Visibility.Public,
        Name = FieldNumberDelphiConstantName,
        Value = Field.Number.ToString(CultureInfo.InvariantCulture),
    };

    /// <summary>
    /// TODO
    /// </summary>
    public DelphiClassConstantSourceCode FieldNameDelphiConstant => new()
    {
        Comment = $"""
            <summary>
            Protobuf field name of the Protobuf field <c>{Field.Name}</c>.
            </summary>
            """.AnnotationComment(),
        Visibility = Visibility.Public,
        Name = FieldNameDelphiConstantName,
        Value = Field.Name,
    };
}
