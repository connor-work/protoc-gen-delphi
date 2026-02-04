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

using Work.Connor.Delphi;
using Binding = Work.Connor.Delphi.MethodInterfaceDeclaration.Types.Binding;
using Visibility = Work.Connor.Delphi.Visibility;

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi;

internal sealed partial class ProtobufMessageTypeSourceCode
{
    /// <summary>
    /// TODO
    /// </summary>
    public DelphiMethodSourceCode EncodeJsonMethod => new()
    {
        Comment = """
            <summary>
            Encodes the message as a JSON object using the ProtoJSON format and writes it to a <see cref="TJSONObject"/>.
            </summary>
            <param name="aDest">The <see cref="TJSONObject"/> that the encoded message is written to</param>
            """.AnnotationComment(),
        Visibility = Visibility.Public,
        RoutineType = Prototype.Types.Type.Procedure,
        Name = "EncodeJson",
        ParameterList = {
            EncodeJsonMethodDestParameter,
        },
        Binding = Binding.Override,
        // NOTE This method should be a final method, once the Delphi Code Writer supports it.
        // TODO statements
    };

    /// <summary>
    /// TODO
    /// </summary>
    public Parameter EncodeJsonMethodDestParameter => new()
    {
        Name = "aDest",
        Type = "TJSONObject",
    };
}
