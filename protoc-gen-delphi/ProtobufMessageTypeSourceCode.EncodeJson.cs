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

using System.Linq;
using Work.Connor.Delphi;
using Binding = Work.Connor.Delphi.MethodInterfaceDeclaration.Types.Binding;
using Visibility = Work.Connor.Delphi.Visibility;

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi;

internal sealed partial class ProtobufMessageTypeSourceCode
{
    /// <summary>
    /// Name of <see cref="EncodeJsonMethodDestParameter"/>.
    /// </summary>
    internal static string EncodeJsonMethodDestParameterName => "aDest";
    
    /// <summary>
    /// TODO
    /// </summary>
    public DelphiMethodSourceCode EncodeJsonMethod
    {
        get
        {
            DelphiMethodSourceCode result = new()
            {
                Comment = $"""
                    <summary>
                    Encodes the message as a JSON object using the ProtoJSON format and writes it to a <see cref="TJSONObject"/>.
                    </summary>
                    <param name="{EncodeJsonMethodDestParameterName}">The <see cref="TJSONObject"/> that the encoded message is written to</param>
                    """.AnnotationComment(),
                Visibility = Visibility.Public,
                RoutineType = Prototype.Types.Type.Procedure,
                Name = "EncodeJson",
                ParameterList = {
                    EncodeJsonMethodDestParameter,
                },
                Binding = Binding.Override,
                IsFinal = true,
            };
            result.Statements.AddRange(FieldsSourceCode.SelectMany(fieldSourceCode => fieldSourceCode.EncodeJsonStatements));
            return result;
        }
    }

    /// <summary>
    /// TODO
    /// </summary>
    public Parameter EncodeJsonMethodDestParameter => new()
    {
        Name = EncodeJsonMethodDestParameterName,
        Type = "TJSONObject",
    };
}
