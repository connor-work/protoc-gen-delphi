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
using System;
using System.Collections.Generic;
using System.Linq;
using Work.Connor.Delphi;

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi;

/// <summary>
/// Aggregation of generated Delphi source code elements that represent a Protobuf message type.
/// </summary>
internal sealed partial class ProtobufMessageTypeSourceCode
{
    /// <summary>
    /// The represented Protobuf message type.
    /// </summary>
    public required DescriptorProto MessageType { get; init; }

    /// <summary>
    /// TODO
    /// </summary>
    private IEnumerable<ProtobufFieldSourceCode> FieldsSourceCode
        => MessageType.Field.Select(@field => new ProtobufFieldSourceCode
        {
            MessageType = this,
            Field = @field,
        });

    /// <summary>
    /// TODO
    /// </summary>
    public string DelphiInterfaceName => ProtocGenDelphi.ConstructDelphiInterfaceName(MessageType.Name);

    private Guid DelphiInterfaceGuid { get; } = Guid.NewGuid(); // TODO generate v5 UUID

    /// <summary>
    /// TODO
    /// </summary>
    private DelphiInterfaceSourceCode DelphiInterface
    {
        get
        {
            DelphiInterfaceSourceCode result = new()
            {
                Comment = $"""
                    <summary>
                    TODO contract
                    </summary>
                    <remarks>
                    This interface corresponds to the Protobuf message type <c>{MessageType.Name}</c>.
                    </remarks>
                    """.AnnotationComment(),
                Name = DelphiInterfaceName,
                Ancestor = GeneratedDelphiInterfaceAncestorName,
                Guid = DelphiInterfaceGuid,
                Methods = {
                    AssignOwnFieldsMethod,
                    ClearOwnFieldsMethod,
                    EncodeOwnFieldsMethod,
                    MergeFieldFromMethod,
                    CalculateOwnFieldsSizeMethod,
                    GetTypeUrlMethod,
                    EncodeJsonMethod,
                    MergeFieldFromJsonMethod,
                },
            };
            result.Methods.AddRange(FieldsSourceCode.SelectMany<ProtobufFieldSourceCode, DelphiMethodSourceCode>(@field => [
                @field.DelphiInterfaceGetterMethod,
                ..@field.DelphiInterfaceSetterMethod.CollectIfPresent(),
            ]));
            result.Properties.AddRange(FieldsSourceCode.Select(@field => @field.DelphiInterfaceProperty));
            return result;
        }
    }

    /// <summary>
    /// TODO
    /// </summary>
    public string DelphiClassName => ProtocGenDelphi.ConstructDelphiTypeName(MessageType.Name);

    /// <summary>
    /// TODO
    /// </summary>
    private DelphiClassSourceCode DelphiClass
    {
        get
        {
            DelphiClassSourceCode result = new()
            {
                Comment = $"""
                    <summary>
                    TODO contract
                    </summary>
                    <remarks>
                    This class corresponds to the Protobuf message type <c>{MessageType.Name}</c>.
                    </remarks>
                    """.AnnotationComment(),
                Name = DelphiClassName,
                InheritanceModifier = ClassDeclaration.Types.InheritanceModifier.Sealed,
                Ancestor = GeneratedDelphiClassAncestorName,
                Constants = {
                    ProtobufTypeUrlConstant,
                },
                Methods = {
                    CreateMethod,
                    AssignOwnFieldsMethod,
                    ClearOwnFieldsMethod,
                    EncodeOwnFieldsMethod,
                    MergeFieldFromMethod,
                    CalculateOwnFieldsSizeMethod,
                    GetTypeUrlMethod,
                    EncodeJsonMethod,
                    MergeFieldFromJsonMethod,
                },
            };
            result.Methods.AddRange(FieldsSourceCode.SelectMany<ProtobufFieldSourceCode, DelphiMethodSourceCode>(@field => [
                @field.DelphiClassGetterMethod,
                ..@field.DelphiClassSetterMethod.CollectIfPresent(),
                ..@field.DelphiInterfaceGetterImplementationMethod.CollectIfPresent(),
                ..@field.DelphiInterfaceSetterImplementationMethod.CollectIfPresent(),
            ]));
            // TODO emit field name and number constants
            // TODO emit fields
            // TODO emit method resolution clauses if required
            result.Properties.AddRange(FieldsSourceCode.Select(@field => @field.DelphiClassProperty));
            return result;
        }
    }

    /// <summary>
    /// TODO
    /// </summary>
    /// <returns></returns>
    public IEnumerable<InterfaceDeclaration> Declare() => [
        new InterfaceDeclaration { InterfaceTypeDeclaration = DelphiInterface.Declare() },
        new InterfaceDeclaration { ClassDeclaration = DelphiClass.Declare() },
    ];

    /// <summary>
    /// TODO
    /// </summary>
    /// <returns></returns>
    public IEnumerable<ImplementationDeclaration> Implement() => DelphiClass.Implement()
        .Select(method => new ImplementationDeclaration
        {
            MethodDeclaration = method,
        });

    /// <summary>
    /// TODO
    /// </summary>
    /// <returns></returns>
    public IEnumerable<string> Initialize() => [
        $"{ProtocGenDelphi.ProtobufRuntimeGlobalTypeRegistryDelphiObject}.{ProtocGenDelphi.ProtobufRuntimeRegisterNotWellKnownTypeDelphiMethodName}({DelphiClassName}.{ProtobufTypeUrlConstant.Name}, {DelphiClassName});",
    ];
}
