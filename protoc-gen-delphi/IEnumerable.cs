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

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi;

/// <summary>
/// Extensions to <see cref="IEnumerable{T}"/>.
/// </summary>
public static class ExtIEnumerable
{
    /// <summary>
    /// TODO
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="element"></param>
    /// <returns></returns>
    public static IEnumerable<TElement> Intersperse<TElement>(this IEnumerable<TElement> source, TElement separator)
    {
        bool first = true;
        foreach (TElement element in source)
        {
            if (!first) yield return separator;
            first = false;
            yield return element;
        }
    }
}
