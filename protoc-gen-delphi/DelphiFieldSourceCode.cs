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

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi;

/// <summary>
/// Aggregation of Delphi source code elements that are part of a generated Delphi field.
/// </summary>
internal sealed class DelphiFieldSourceCode
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
    public required string Name { get; init; }

    /// <summary>
    /// TODO
    /// </summary>
    public required string Type { get; init; }

    /// <summary>
    /// TODO
    /// </summary>
    /// <returns></returns>
    public ClassDeclarationNestedDeclaration Declare() => new FieldDeclaration
    {
        Name = Name,
        Type = Type,
        Comment = Comment,
    }.InClass(Visibility);
}
