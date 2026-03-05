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

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi;

internal sealed partial class ProtobufMessageTypeSourceCode
{
    /// <summary>
    /// Name of the Delphi class that is the ancestor of all Delphi classes that represent Protobuf message types.
    /// </summary>
    public static string AllMessageTypesDelphiClassAncestorName => "TProtobufMessageBase";

    /// <summary>
    /// Name of the Delphi class that is the ancestor of all generated Delphi interfaces that represent Protobuf message types.
    /// </summary>
    public static string GeneratedDelphiInterfaceAncestorName => "IProtobufNotWellKnownTypeMessage";

    /// <summary>
    /// Name of the Delphi class that is the ancestor of all generated Delphi classes that represent Protobuf message types.
    /// </summary>
    public static string GeneratedDelphiClassAncestorName => "TProtobufNotWellKnownTypeMessageBase";

    /// <summary>
    /// Name of the Delphi constants nested in Delphi classes that represent Protobuf message types, that contain the type URL of the message type.
    /// </summary>
    public static string ProtobufTypeUrlDelphiConstantName => "PROTOBUF_TYPE_URL";

    /// <summary>
    /// Name of the public Delphi method of a Delphi class that represents a Protobuf message type, that TODO.
    /// </summary>
    public static string DecodeJsonDelphiMethodName => "DecodeJson";

    /// <summary>
    /// Name of the protected Delphi method of a generated Delphi class that represents a Protobuf message type, that TODO.
    /// </summary>
    public static string MergeUnknownFieldFromDelphiMethodName => "MergeUnknownFieldFrom";
}
