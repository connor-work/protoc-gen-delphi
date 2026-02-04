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

// EXTRACT Delphi Code Writer

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi;

/// <summary>
/// Extensions to <see cref="ConstDeclaration"/>.
/// </summary>
public static class ExtConstDeclaration
{
    /// <summary>
    /// Declares the constant as a nested constant within a class.
    /// </summary>
    /// <param name="declaration">The declaration of the constant</param>
    /// <returns>The declaration within the class declaration</returns>
    public static ClassDeclarationNestedDeclaration InClass(this ConstDeclaration declaration, Visibility visibility) => new()
    {
        Visibility = visibility,
        NestedConstDeclaration = declaration,
    };
}
