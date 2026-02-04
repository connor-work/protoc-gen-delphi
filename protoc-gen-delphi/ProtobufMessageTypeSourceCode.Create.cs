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
    public DelphiMethodSourceCode CreateMethod => new()
    {
        Comment = """
            TODO contract
            """.AnnotationComment(),
        Visibility = Visibility.Public,
        RoutineType = Prototype.Types.Type.Constructor,
        Name = "Create",
        Binding = Binding.Override,
        // NOTE This method should be a final method, once the Delphi Code Writer supports it.
        // TODO statements
    };
}
