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
using System.Collections.Generic;
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
    public string DelphiInterfaceName => "TODO";

    /// <summary>
    /// TODO
    /// </summary>
    private DelphiInterfaceSourceCode DelphiInterface
    {
        get
        {
            DelphiInterfaceSourceCode result = new()
            {
                Comment = """
                    /// <summary>
                    /// TODO contract
                    /// </summary>
                    /// <remarks>
                    /// This interface corresponds to the Protobuf message type <c>MessageY</c>.
                    /// </remarks>
                """.AnnotationComment(),
                Name = DelphiInterfaceName,
                Ancestor = ProtocGenDelphi.MessageTypeDelphiAncestorInterfaceName,
                Guid = default, // TODO generate v5 UUID
            };
            // TODO add properties
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
                Comment = """
                    /// <summary>
                    /// TODO contract
                    /// </summary>
                    /// <remarks>
                    /// This class corresponds to the Protobuf message type <c>MessageY</c>.
                    /// </remarks>
                """.AnnotationComment(),
                Name = DelphiClassName,
                Ancestor = ProtocGenDelphi.MessageTypeDelphiAncestorClassName,
                Constants = {

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
            // TODO add properties
            return result;

            // TODO current problem: Does the caller get the messages as classes or interfaces? If they get them as classes, we have to manage transfer of ownership. If they get them as interfaces, we have to make sure that they do not get the classes!
            // TODO only as interfaces?
        }
    }

    /// <summary>
    /// TODO
    /// </summary>
    /// <returns></returns>
    public IEnumerable<InterfaceDeclaration> Declare() => [
        // TODO declare interface type
        new InterfaceDeclaration { ClassDeclaration = DelphiClass.Declare() },
    ];

    /// <summary>
    /// TODO
    /// </summary>
    /// <returns></returns>
    public IEnumerable<MethodDeclaration> Implement() => DelphiClass.Implement();

    // TODO Initialize() (Delphi Code Writer needs to support initialization section)
}
