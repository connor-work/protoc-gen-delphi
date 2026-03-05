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
using Work.Connor.Delphi.CodeWriter;

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi;

/// <summary>
/// Aggregation of Delphi source code elements that are part of a generated Delphi property.
/// </summary>
internal sealed class DelphiPropertySourceCode
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
    public bool ReadOnly { get; init; } = false;

    /// <summary>
    /// TODO
    /// </summary>
    public bool WriteOnly { get; init; } = false;

    /// <summary>
    /// TODO
    /// </summary>
    public string? BackingFieldName { get; init; } = null;

    /// <summary>
    /// TODO
    /// </summary>
    public string? GetterName { get; init; } = null;

    /// <summary>
    /// TODO
    /// </summary>
    public string? SetterName { get; init; } = null;

    /// <summary>
    /// TODO
    /// </summary>
    public required string Type { get; init; }

    /// <summary>
    /// TODO
    /// </summary>
    /// <returns></returns>
    public ClassDeclarationNestedDeclaration DeclareInClass()
        => new PropertyDeclaration
        {
            Name = Name,
            Type = Type,
            ReadSpecifier = WriteOnly ? "" : (GetterName ?? BackingFieldName),
            WriteSpecifier = ReadOnly ? "" : (SetterName ?? BackingFieldName),
            Comment = Comment,
        }.InClass(Visibility);

    /// <summary>
    /// TODO
    /// </summary>
    /// <returns></returns>
    public InterfaceMemberDeclaration DeclareInInterface() => new()
    {
        PropertyDeclaration = new PropertyDeclaration
        {
            Name = Name,
            Type = Type,
            ReadSpecifier = WriteOnly ? "" : GetterName,
            WriteSpecifier = ReadOnly ? "" : SetterName,
            Comment = Comment,
        },
    };
}
