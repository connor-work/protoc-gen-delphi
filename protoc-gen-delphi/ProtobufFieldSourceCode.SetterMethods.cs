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
    /// Scheme for mapping a Protobuf field name to the name of a Delphi setter method that assigns the field value.
    /// </summary>
    private static IdentifierGenerator<string> DelphiSetterMethodNameGenerator { get; } = new IdentifierTemplate<string>(x => x,
        collisionAvoidanceSuffix: "_ProtobufField",
        @case: IdentifierCase.Pascal,
        prefix: "Set",
        caseSensitive: false);

    /// <summary>
    /// Scheme for mapping a Protobuf field name to the name of a a private Delphi method that implements an interface type's Delphi setter method that assigns the field value.
    /// </summary>
    private static IdentifierGenerator<string> DelphiInterfaceSetterImplementationMethodNameGenerator { get; } = new IdentifierTemplate<string>(x => x,
        collisionAvoidanceSuffix: "_ProtobufField",
        @case: IdentifierCase.Pascal,
        prefix: "SetInterfaced",
        caseSensitive: false);

    /// <summary>
    /// Name of Delphi setter methods for the field value.
    /// </summary>
    public string DelphiSetterMethodName => DelphiSetterMethodNameGenerator.Generate(Field.Name, reservedIdentifiers: ProtocGenDelphi.ReservedIdentifiers);

    /// <summary>
    /// Name of <see cref="DelphiInterfaceSetterImplementationMethod"/>.
    /// </summary>
    public string DelphiInterfaceSetterImplementationMethodName => DelphiInterfaceSetterImplementationMethodNameGenerator.Generate(Field.Name, reservedIdentifiers: ProtocGenDelphi.ReservedIdentifiers);

    /// <summary>
    /// Name of the single parameter of Delphi setter methods for the field value.
    /// </summary>
    private static string DelphiSetterMethodParameterName => "aValue";

    /// <summary>
    /// Single parameter of <see cref="DelphiInterfaceSetterMethod"/>.
    /// </summary>
    private Parameter DelphiInterfaceSetterMethodParameter => new()
    {
        Name = DelphiSetterMethodParameterName,
        Type = DelphiInterfacePropertyType,
    };

    /// <summary>
    /// Source code of the optional Delphi setter method for the field value, in the Delphi interface generated for the message type.
    /// </summary>
    /// <remarks>
    /// Absent for repeated fields.
    /// </remarks>
    public DelphiMethodSourceCode? DelphiInterfaceSetterMethod
    {
        get
        {
            if (Field.Label is FieldDescriptorProto.Types.Label.Repeated) return null;
            return new()
            {
                Comment = $"""
                    <summary>
                    Setter for <see cref="{DelphiPropertyName}"/>.
                    </summary>
                    <param name="aValue">The new value of the Protobuf field <c>{Field.Name}</c></param>
                    """.AnnotationComment(),
                Visibility = Visibility.Public,
                RoutineType = Prototype.Types.Type.Procedure,
                Name = DelphiSetterMethodName,
                ParameterList = {
                    DelphiInterfaceSetterMethodParameter
                },
                Binding = MethodInterfaceDeclaration.Types.Binding.Static,
            };
        }
    }

    /// <summary>
    /// Single parameter of <see cref="DelphiClassSetterMethod"/>.
    /// </summary>
    private Parameter DelphiClassSetterMethodParameter => new()
    {
        Name = DelphiSetterMethodParameterName,
        Type = DelphiClassPropertyType,
    };

    /// <summary>
    /// Source code of the optional Delphi setter method for the field value, in the Delphi class generated for the message type.
    /// </summary>
    /// <remarks>
    /// Absent for repeated fields.
    /// </remarks>
    public DelphiMethodSourceCode? DelphiClassSetterMethod
    {
        get
        {
            if (Field.Label is FieldDescriptorProto.Types.Label.Repeated) return null;
            DelphiMethodSourceCode result = new()
            {
                Comment = $"""
                    <summary>
                    Setter for <see cref="{DelphiPropertyName}"/>.
                    </summary>
                    <param name="aValue">The new value of the Protobuf field <c>{Field.Name}</c></param>
                    """.AnnotationComment(),
                Visibility = Visibility.Public,
                RoutineType = Prototype.Types.Type.Procedure,
                Name = DelphiSetterMethodName,
                ParameterList = {
                    DelphiClassSetterMethodParameter
                },
                Binding = MethodInterfaceDeclaration.Types.Binding.Static,
                LocalDeclarations = { },
            };
            result.Statements.AddRange($"""
                {DelphiFieldName} := {DelphiSetterMethodParameterName};
                """.Lines());
            return result;
        }
    }

    /// <summary>
    /// Source code of the optional method in the Delphi class generated for the message type, that implements <see cref="DelphiInterfaceSetterMethod"/>.
    /// </summary>
    /// <remarks>
    /// Absent if <see cref="DelphiInterfaceSetterMethod"/> is absent or implemented by <see cref="DelphiClassSetterMethod"/>.
    /// </remarks>
    public DelphiMethodSourceCode? DelphiInterfaceSetterImplementationMethod
    {
        get
        {
            if (Field.Label is FieldDescriptorProto.Types.Label.Repeated) return null;
            if (Field.Type is not FieldDescriptorProto.Types.Type.Message) return null;
            DelphiMethodSourceCode result = new()
            {
                Comment = $"""
                    <summary>
                    Implementation of <see cref="{MessageType.DelphiInterfaceName}.{DelphiSetterMethodName}"/>.
                    </summary>
                    <param name="aValue">The new value of the Protobuf field <c>{Field.Name}</c></param>
                    """.AnnotationComment(),
                Visibility = Visibility.Private,
                RoutineType = Prototype.Types.Type.Procedure,
                Name = DelphiSetterMethodName,
                ParameterList = {
                    DelphiInterfaceSetterMethodParameter
                },
                Binding = MethodInterfaceDeclaration.Types.Binding.Static,
                LocalDeclarations = { },
            };
            result.Statements.AddRange($"""
                {DelphiFieldName} := {DelphiSetterMethodParameterName};
                """.Lines());
            return result;
        }
    }
}
