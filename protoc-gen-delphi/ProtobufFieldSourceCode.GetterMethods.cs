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
    /// Scheme for mapping a Protobuf field name to the name of a Delphi getter method that returns the field value.
    /// </summary>
    private static IdentifierGenerator<string> DelphiGetterMethodNameGenerator { get; } = new IdentifierTemplate<string>(x => x,
        collisionAvoidanceSuffix: "_ProtobufField",
        @case: IdentifierCase.Pascal,
        prefix: "Get",
        caseSensitive: false);

    /// <summary>
    /// Scheme for mapping a Protobuf field name to the name of a private Delphi method that implements an interface type's Delphi getter method that returns the field value.
    /// </summary>
    private static IdentifierGenerator<string> DelphiInterfaceGetterImplementationMethodNameGenerator { get; } = new IdentifierTemplate<string>(x => x,
        collisionAvoidanceSuffix: "_ProtobufField",
        @case: IdentifierCase.Pascal,
        prefix: "GetInterfaced",
        caseSensitive: false);

    /// <summary>
    /// Name of Delphi getter methods for the field value.
    /// </summary>
    public string DelphiGetterMethodName => DelphiGetterMethodNameGenerator.Generate(Field.Name, reservedIdentifiers: ProtocGenDelphi.ReservedIdentifiers);

    /// <summary>
    /// Name of <see cref="DelphiInterfaceGetterImplementationMethod"/>.
    /// </summary>
    public string DelphiInterfaceGetterImplementationMethodName => DelphiInterfaceGetterImplementationMethodNameGenerator.Generate(Field.Name, reservedIdentifiers: ProtocGenDelphi.ReservedIdentifiers);

    /// <summary>
    /// Source code of the Delphi getter method for the field value, in the Delphi interface generated for the message type.
    /// </summary>
    public DelphiMethodSourceCode DelphiInterfaceGetterMethod => new()
    {
        Comment = $"""
            <summary>
            Getter for <see cref="{DelphiPropertyName}"/>.
            </summary>
            <returns>The value of <see cref="{DelphiPropertyName}"/></param>
            """.AnnotationComment(),
        Visibility = Visibility.Public,
        RoutineType = Prototype.Types.Type.Function,
        Name = DelphiGetterMethodName,
        ParameterList = { },
        ReturnType = DelphiInterfacePropertyType,
    };

    /// <summary>
    /// Source code of the Delphi getter method for the field value, in the Delphi class generated for the message type.
    /// </summary>
    public DelphiMethodSourceCode DelphiClassGetterMethod
    {
        get
        {
            DelphiMethodSourceCode result = new()
            {
                Comment = $"""
                    <summary>
                    Getter for <see cref="{DelphiPropertyName}"/>.
                    </summary>
                    <returns>The value of <see cref="{DelphiPropertyName}"/></param>
                    """.AnnotationComment(),
                Visibility = Visibility.Public,
                RoutineType = Prototype.Types.Type.Function,
                Name = DelphiGetterMethodName,
                ParameterList = { },
                ReturnType = DelphiClassPropertyType,
                LocalDeclarations = { },
            };
            if (Field.Type is FieldDescriptorProto.Types.Type.Message)
            {
                result.Statements.AddRange($"""
                    result := {DelphiClassPropertyType}({DelphiFieldName});
                    """.Lines());
            }
            else
            {
                result.Statements.AddRange($"""
                    result := {DelphiFieldName};
                    """.Lines());
            }
            return result;
        }
    }

    /// <summary>
    /// Source code of the optional method in the Delphi class generated for the message type, that implements <see cref="DelphiInterfaceGetterMethod"/>.
    /// </summary>
    /// <remarks>
    /// Absent if <see cref="DelphiInterfaceGetterMethod"/> is implemented by <see cref="DelphiClassGetterMethod"/>.
    /// </remarks>
    public DelphiMethodSourceCode? DelphiInterfaceGetterImplementationMethod
    {
        get
        {
            if (Field.Label is FieldDescriptorProto.Types.Label.Repeated) return null;
            if (Field.Type is not FieldDescriptorProto.Types.Type.Message) return null;
            DelphiMethodSourceCode result = new()
            {
                Comment = $"""
                    <summary>
                    Implementation of <see cref="{MessageType.DelphiInterfaceName}.{DelphiGetterMethodName}"/>.
                    </summary>
                    <returns>The value of <see cref="{DelphiPropertyName}"/></param>
                    """.AnnotationComment(),
                Visibility = Visibility.Private,
                RoutineType = Prototype.Types.Type.Function,
                Name = DelphiGetterMethodName,
                ParameterList = { },
                ReturnType = DelphiInterfacePropertyType,
                LocalDeclarations = { },
            };
            result.Statements.AddRange($"""
                result := {DelphiFieldName};
                """.Lines());
            return result;
        }
    }
}
