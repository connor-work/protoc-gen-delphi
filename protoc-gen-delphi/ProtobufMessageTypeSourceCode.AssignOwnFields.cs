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
    /// Name of the local variable of <see cref="AssignOwnFieldsMethod"/> that holds TODO.
    /// </summary>
    internal static string AssignOwnFieldsSourceLocalVariableName => "lSource";

    /// <summary>
    /// TODO
    /// </summary>
    public DelphiMethodSourceCode AssignOwnFieldsMethod
    {
        get
        {
            DelphiMethodSourceCode result = new()
            {
                Comment = """
                    TODO contract
                    """.AnnotationComment(),
                Visibility = Visibility.Public,
                RoutineType = Prototype.Types.Type.Function,
                Name = "AssignOwnFields",
                ParameterList = { AssignOwnFieldsMethodSourceParameter },
                ReturnType = "Boolean",
                Binding = Binding.Override,
                IsFinal = true,
                LocalDeclarations =
                {
                    $"{AssignOwnFieldsSourceLocalVariableName}: {DelphiClassName};",
                },
            };
            result.Statements.AddRange($"""
                {AssignOwnFieldsSourceLocalVariableName} := {AssignOwnFieldsMethodSourceParameter.Name} as {DelphiClassName};
                if (not Assigned({AssignOwnFieldsSourceLocalVariableName})) then Exit(False);
                result := True;
                """.Lines());
            result.Statements.AddRange(FieldsSourceCode.SelectMany(fieldSourceCode => fieldSourceCode.AssignOwnFieldsStatements));
            return result;
        }
    }

    /// <summary>
    /// TODO
    /// </summary>
    public Parameter AssignOwnFieldsMethodSourceParameter => new()
    {
        Name = "aSource",
        Type = AllMessageTypesDelphiClassAncestorName,
    };
}
