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

using System;
using System.Collections.Generic;
using Work.Connor.Delphi;

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi;

/// <summary>
/// Aggregation of Delphi source code elements that are part of a generated Delphi interface.
/// </summary>
internal sealed class DelphiInterfaceSourceCode
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
    public required string Ancestor { get; init; }

    /// <summary>
    /// TODO
    /// </summary>
    public required Guid Guid { get; init; }

    // TODO that means we need getter properties without backing field

    /// <summary>
    /// TODO
    /// </summary>
    public List<DelphiPropertySourceCode> Properties { get; } = [];

    /// <summary>
    /// TODO
    /// </summary>
    public List<DelphiMethodSourceCode> Methods { get; } = [];

    // TODO declare as interface (Delphi Code Writer needs to support this)
}
