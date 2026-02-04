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
using Work.Connor.Delphi;
using Binding = Work.Connor.Delphi.MethodInterfaceDeclaration.Types.Binding;

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi;

/// <summary>
/// Aggregation of Delphi source code elements that are part of a generated Delphi method.
/// </summary>
internal sealed class DelphiMethodSourceCode
{
    /// <summary>
    /// TODO
    /// </summary>
    public AnnotationComment? Comment { get; init; } = null;

    /// <summary>
    /// TODO
    /// </summary>
    public required Visibility Visibility { get; init; }

    /// <summary>
    /// TODO
    /// </summary>
    public Prototype.Types.Type RoutineType { get; init; }

    /// <summary>
    /// TODO
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// TODO
    /// </summary>
    public List<Parameter> ParameterList { get; } = [];

    /// <summary>
    /// TODO
    /// </summary>
    public string? ReturnType { get; init; } = null;

    /// <summary>
    /// TODO
    /// </summary>
    public Binding Binding { get; init; } = Binding.Static;

    // TODO HasImplementation (false for abstract and for interface method)

    /// <summary>
    /// TODO
    /// </summary>
    public List<string> LocalDeclarations { get; } = [];

    /// <summary>
    /// TODO
    /// </summary>
    public List<string> Statements { get; } = [];

    /// <summary>
    /// TODO
    /// </summary>
    private Prototype Prototype => new()
    {
        Name = Name,
        Type = RoutineType,
        ParameterList = { ParameterList },
        ReturnType = ReturnType ?? "",
    };

    /// <summary>
    /// TODO
    /// </summary>
    /// <returns></returns>
    public ClassDeclarationNestedDeclaration Declare() => new MethodInterfaceDeclaration
    {
        Binding = Binding,
        Prototype = Prototype,
        Comment = Comment,
    }.InClass(Visibility);

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="className"></param>
    /// <returns></returns>
    public MethodDeclaration Implement(string className) => new()
    {
        Class = className,
        Prototype = Prototype,
        LocalDeclarations = { LocalDeclarations },
        Statements = { Statements },
    };
}
