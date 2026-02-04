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

using System.Collections.Generic;
using System.Linq;
using Work.Connor.Delphi;

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi;

/// <summary>
/// Aggregation of Delphi source code elements that are part of a generated Delphi class.
/// </summary>
/// <remarks>
/// Fields that are not backing fields of a property are currently not supported.
/// </remarks>
internal sealed class DelphiClassSourceCode
{
    /// <summary>
    /// TODO
    /// </summary>
    public AnnotationComment? Comment { get; init; } = null;

    /// <summary>
    /// TODO
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// TODO
    /// </summary>
    public string? Ancestor { get; init; } = null;

    /// <summary>
    /// TODO
    /// </summary>
    public List<DelphiClassConstantSourceCode> Constants { get; } = [];

    /// <summary>
    /// TODO
    /// </summary>
    public List<DelphiPropertySourceCode> Properties { get; } = [];

    /// <summary>
    /// TODO
    /// </summary>
    public List<DelphiMethodSourceCode> Methods { get; } = [];

    /// <summary>
    /// TODO
    /// </summary>
    /// <returns></returns>
    public ClassDeclaration Declare()
    {
        ClassDeclaration result = new()
        {
            Name = Name,
            Ancestor = Ancestor,
            Comment = Comment,
        };
        result.NestedDeclarations.AddRange(Constants.Select(constant => constant.Declare()));
        result.NestedDeclarations.AddRange(Properties.SelectMany(property => property.Declare()));
        result.NestedDeclarations.AddRange(Methods.Select(method => method.Declare()));
        return result;
    }

    /// <summary>
    /// TODO
    /// </summary>
    /// <returns></returns>
    public IEnumerable<MethodDeclaration> Implement() => [
        ..Properties.SelectMany(property => property.Implement(Name)),
        ..Methods.Select(method => method.Implement(Name)),
    ];
}
