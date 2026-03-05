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
    /// Name of <see cref="MergeFieldFromSourceParameter"/>.
    /// </summary>
    public static string MergeFieldFromSourceParameterName => "aSource";

    /// <summary>
    /// Name of <see cref="MergeFieldFromTagParameter"/>.
    /// </summary>
    public static string MergeFieldFromTagParameterName => "aTag";

    /// <summary>
    /// Name of <see cref="MergeFieldFromRemainingLengthParameter"/>.
    /// </summary>
    public static string MergeFieldFromRemainingLengthParameterName => "aRemainingLength";
    
    /// <summary>
    /// TODO
    /// </summary>
    public DelphiMethodSourceCode MergeFieldFromMethod
    {
        get
        {
            DelphiMethodSourceCode result = new()
            {
                Comment = """
                    TODO contract
                    """.AnnotationComment(),
                Visibility = Visibility.Public,
                RoutineType = Prototype.Types.Type.Procedure,
                Name = "MergeFieldFrom",
                ParameterList = {
                    MergeFieldFromSourceParameter,
                    MergeFieldFromTagParameter,
                    MergeFieldFromRemainingLengthParameter,
                },
                Binding = Binding.Override,
                IsFinal = true,
            };
            if (FieldsSourceCode.Any())
            {
                result.Statements.AddRange($"""
                    // TODO is this the correct merge behavior?
                    case {MergeFieldFromTagParameter.Name}.{ProtocGenDelphi.TagFieldNumberDelphiFieldName} of
                    """.Lines());
                result.Statements.AddRange(FieldsSourceCode.SelectMany(
                    fieldSourceCode => fieldSourceCode.MergeFieldFromStatements.Select(statement => $"  {statement}")));
                result.Statements.AddRange($"""
                      else {MergeUnknownFieldFromDelphiMethodName}({MergeFieldFromSourceParameter.Name}, {MergeFieldFromTagParameter.Name}, {MergeFieldFromRemainingLengthParameter.Name})
                    end;
                    """.Lines());
            }
            return result;
        }
    }

    /// <summary>
    /// TODO
    /// </summary>
    public Parameter MergeFieldFromSourceParameter => new()
    {
        Name = MergeFieldFromSourceParameterName,
        Type = "TStream",
    };

    /// <summary>
    /// TODO
    /// </summary>
    public Parameter MergeFieldFromTagParameter => new()
    {
        Name = MergeFieldFromTagParameterName,
        Type = ProtocGenDelphi.TagDelphiTypeName,
    };

    /// <summary>
    /// TODO
    /// </summary>
    public Parameter MergeFieldFromRemainingLengthParameter => new()
    {
        Name = MergeFieldFromRemainingLengthParameterName,
        Type = "PUInt32",
    };
}
