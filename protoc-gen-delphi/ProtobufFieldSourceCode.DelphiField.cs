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
using Work.Connor.Delphi;
using static Work.Connor.Delphi.CodeWriter.StringExtensions;

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi;

internal sealed partial class ProtobufFieldSourceCode
{
    /// <summary>
    /// Scheme for mapping a Protobuf field name to the name of a Delphi field that contains the field value.
    /// </summary>
    private static IdentifierGenerator<string> DelphiFieldNameGenerator { get; } = new IdentifierTemplate<string>(x => x,
        collisionAvoidanceSuffix: "_ProtobufField",
        @case: IdentifierCase.Pascal,
        prefix: "F",
        caseSensitive: false);

    /// <summary>
    /// Name of <see cref="DelphiField"/>.
    /// </summary>
    public string DelphiFieldName => DelphiFieldNameGenerator.Generate(Field.Name, reservedIdentifiers: ProtocGenDelphi.ReservedIdentifiers);

    /// <summary>
    /// Type of the Delphi field that contains the field value.
    /// </summary>
    public string DelphiFieldType
    {
        get
        {
            if (Field.Label is FieldDescriptorProto.Types.Label.Repeated) throw new System.NotImplementedException("TODO");
            return Field.Type switch
            {
                FieldDescriptorProto.Types.Type.Message => ProtocGenDelphi.ConstructDelphiInterfaceName(Field.TypeName),
                FieldDescriptorProto.Types.Type.Enum => ProtocGenDelphi.ConstructDelphiTypeName(Field.TypeName),
                _ => Field.Type.DelphiSingularFieldType()
            };
        }
    }

    /// <summary>
    /// Source code of the generated Delphi field.
    /// </summary>
    public DelphiFieldSourceCode DelphiField => new()
    {
        Comment = $"""
            /// <summary>
            /// Backing field of <see cref="{DelphiPropertyName}"/>.
            /// </summary>
            """.AnnotationComment(),
        Visibility = Visibility.Private,
        Name = DelphiFieldName,
        Type = DelphiFieldType,
    };
}
