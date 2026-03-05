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
    /// Scheme for mapping a Protobuf field name to the name of a Delphi property that contains the field value.
    /// </summary>
    private static IdentifierGenerator<string> DelphiPropertyNameGenerator { get; } = new IdentifierTemplate<string>(x => x,
        collisionAvoidanceSuffix: "_ProtobufField",
        @case: IdentifierCase.Pascal,
        caseSensitive: false);

    /// <summary>
    /// Name of Delphi properties that contain the field value.
    /// </summary>
    public string DelphiPropertyName => DelphiPropertyNameGenerator.Generate(Field.Name, reservedIdentifiers: ProtocGenDelphi.ReservedIdentifiers);

    /// <summary>
    /// Type of the Delphi property that contains the field value, in the Delphi interface generated for the message type.
    /// </summary>
    public string DelphiInterfacePropertyType
    {
        get
        {
            string singularType = Field.Type switch
            {
                FieldDescriptorProto.Types.Type.Message => ProtocGenDelphi.ConstructDelphiInterfaceName(Field.TypeName),
                FieldDescriptorProto.Types.Type.Enum => ProtocGenDelphi.ConstructDelphiTypeName(Field.TypeName),
                _ => Field.Type.DelphiSingularFieldType()
            };
            return Field.Label is FieldDescriptorProto.Types.Label.Repeated ? $"{RepeatedFieldDelphiInterfaceName}<{singularType}>" : singularType;
        }
    }

    /// <summary>
    /// Type of the Delphi property that contains the field value, in the Delphi class generated for the message type.
    /// </summary>
    public string DelphiClassPropertyType
    {
        get
        {
            if (Field.Label is FieldDescriptorProto.Types.Label.Repeated) return Field.Type switch
            {
                FieldDescriptorProto.Types.Type.Message => $"{RepeatedMessageFieldDelphiClassName}<{ProtocGenDelphi.ConstructDelphiTypeName(Field.TypeName)}>",
                FieldDescriptorProto.Types.Type.Enum => $"{RepeatedEnumFieldDelphiClassName}<{ProtocGenDelphi.ConstructDelphiTypeName(Field.TypeName)}>",
                _ => Field.Type.DelphiRepeatedFieldConcreteType()
            };
            return Field.Type switch
            {
                FieldDescriptorProto.Types.Type.Message
             or FieldDescriptorProto.Types.Type.Enum => ProtocGenDelphi.ConstructDelphiTypeName(Field.TypeName),
                _ => Field.Type.DelphiSingularFieldType()
            };
        }
    }

    /// <summary>
    /// Source code of the Delphi property that contains the field value, in the Delphi interface generated for the message type.
    /// </summary>
    public DelphiPropertySourceCode DelphiInterfaceProperty => new()
    {
        Comment = $"""
            TODO
            <remarks>
            This property corresponds to the Protobuf field <c>{Field.Name}</c>.
            </remarks>
            """.AnnotationComment(),
        Visibility = Visibility.Public,
        Name = DelphiPropertyName,
        ReadOnly = Field.Label is FieldDescriptorProto.Types.Label.Repeated,
        GetterName =  DelphiGetterMethodName,
        SetterName = DelphiSetterMethodName,
        Type = DelphiInterfacePropertyType,
    };

    /// <summary>
    /// Source code of the Delphi property that contains the field value, in the Delphi class generated for the message type.
    /// </summary>
    public DelphiPropertySourceCode DelphiClassProperty => new()
    {
        Comment = $"""
            TODO
            <remarks>
            This property corresponds to the Protobuf field <c>{Field.Name}</c>.
            </remarks>
            """.AnnotationComment(),
        Visibility = Visibility.Public,
        Name = DelphiPropertyName,
        ReadOnly = Field.Label is FieldDescriptorProto.Types.Label.Repeated,
        GetterName = DelphiGetterMethodName,
        SetterName = DelphiSetterMethodName,
        Type = DelphiClassPropertyType,
    };
}
