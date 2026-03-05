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

using Visibility = Work.Connor.Delphi.Visibility;

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi;

internal sealed partial class ProtobufMessageTypeSourceCode
{
    /// <summary>
    /// TODO
    /// </summary>
    public DelphiClassConstantSourceCode ProtobufTypeUrlConstant => new()
    {
        Comment = """
            <summary>
            Protobuf type URL of this message type.
            </summary>
            """.AnnotationComment(),
        Visibility = Visibility.Public,
        Name = ProtobufTypeUrlDelphiConstantName,
        Value = $"{ProtocGenDelphi.ProtobufTypeUrlDefaultPrefixDelphiConstantName} + '{MessageType.Name}'"
    };
}
