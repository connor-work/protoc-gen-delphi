/// Copyright 2020 Connor Roehricht (connor.work)
/// Copyright 2020 Sotax AG
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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi.RuntimeTests
{
    /// <summary>
    /// Extensions similar to <see cref="System.Linq"/>, used for resource set definition
    /// </summary>
    public static partial class LinqExtensions
    {
        /// <summary>
        /// Filters a <see cref="string"/> sequence by only accepting elements with a prefix and then removing the prefix.
        /// </summary>
        /// <param name="source">The sequence</param>
        /// <param name="prefixRegex">Regular expression matching the prefix</param>
        /// <returns>The filtered sequence, without the prefixes</returns>
        public static IEnumerable<string> WherePrefixed(this IEnumerable<string> source, Regex prefixRegex)
        {
            Regex regex = new Regex("^" + prefixRegex.ToString(), prefixRegex.Options, prefixRegex.MatchTimeout);
            foreach (string element in source)
            {
                Match match = regex.Match(element);
                if (!match.Success) continue;
                yield return element.Substring(match.Length);
            }
        }

        /// <summary>
        /// Filters a <see cref="string"/> sequence by only accepting elements with a suffix and then removing the suffix.
        /// </summary>
        /// <param name="source">The sequence</param>
        /// <param name="suffixRegex">Regular expression matching the suffix</param>
        /// <returns>The filtered sequence, without the suffixes</returns>
        public static IEnumerable<string> WhereSuffixed(this IEnumerable<string> source, Regex suffixRegex)
        {
            Regex regex = new Regex(suffixRegex.ToString() + "$", suffixRegex.Options | RegexOptions.RightToLeft, suffixRegex.MatchTimeout);
            foreach (string element in source)
            {
                Match match = regex.Match(element);
                if (!match.Success) continue;
                yield return element.Substring(0, element.Length - match.Length);
            }
        }
    }

    /// <summary>
    /// Represents a subset of embedded resources mapped by custom ID strings
    /// </summary>
    interface IResourceSet
    {
        /// <summary>
        /// Root resource set containing all embedded resources. The ID of a resource is equal to its logical name.
        /// </summary>
        public static readonly IResourceSet Root = new RootResourceSet();

        /// <summary>
        /// Constructs a resource set that provides access to an external file tree. The ID of a resource is equal to its relative file path, using slashes (<c>/</c>) as path separators.
        /// </summary>
        /// <param name="root">Root path of the external file tree</param>
        /// <returns>The external resource tree</returns>
        public static IResourceSet External(string root) => new ExternalResourceSet(root);

        /// <summary>
        /// Gets a resource stream for a resource.
        /// </summary>
        /// <param name="resourceID">ID of the resource</param>
        /// <returns>The resource stream for the resource, if it is available</returns>
        public Stream? GetResourceStream(string resourceID);

        /// <summary>
        /// Determines IDs of all available resources.
        /// </summary>
        /// <returns>Sequence of resource IDs</returns>
        public IEnumerable<string> GetIDs();

        /// <summary>
        /// Reads the full content of a resource.
        /// </summary>
        /// <param name="resourceID">ID of the resource</param>
        /// <returns>The content of the resource, if it is available</returns>
        public string? ReadResource(string resourceID)
        {
            Stream? stream = GetResourceStream(resourceID);
            if (stream == null) return null;
            using StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// Reads the full contents of all available resources.
        /// </summary>
        /// <returns>Sequence of ID and content for each available resource</returns>
        public IEnumerable<(string id, string content)> ReadAllResources() => GetIDs().Select(resource => (resource, ReadResource(resource)!));

        /// <summary>
        /// Reads the full contents of some resources.
        /// </summary>
        /// <param name="resourceIDs">IDs of the resourcse</param>
        /// <returns>Sequence of ID and content for each available resource</returns>
        public IEnumerable<(string id, string content)> ReadResources(IEnumerable<string> resourceIDs)
        {
            foreach (string expectedId in resourceIDs)
            {
                string? actualContent = ReadResource(expectedId);
                if (actualContent != null) yield return (expectedId, actualContent);
            }
        }

        /// <summary>
        /// Derives a nested resource set by filtering IDs for a prefix.
        /// </summary>
        /// <param name="prefix">The ID prefix string</param>
        /// <returns>Resource set where IDs have the prefix removed</returns>
        public IResourceSet Nest(string prefix) => new NestedResourceSet(this, prefix);

        /// <summary>
        /// Derives a combined resource set that looks up resources in a backing resource set if they are not present in this one.
        /// </summary>
        /// <param name="backingSet">The backing resource set</param>
        /// <returns>Combined resource set</returns>
        public IResourceSet Or(IResourceSet backingSet) => new CombinedResourceSet(this, backingSet);

        /// <summary>
        /// Root resource set containing all embedded resources. The ID of a resource is equal to its logical name, with all sequences of slashes (<c>/</c>) and backslashes <c>\</c> replaced by a single slash.
        /// </summary>
        private class RootResourceSet : IResourceSet
        {
            /// <summary>
            /// Regular expression used for slash sequence replacement
            /// </summary>
            private static readonly Regex slashSequenceRegex = new Regex("[/\\\\]+");
            /// <summary>
            /// Assembly containing the embedded resources
            /// </summary>
            private readonly Assembly assembly = Assembly.GetExecutingAssembly();

            /// <summary>
            /// Map of resource IDs to their logical name
            /// </summary>
            private readonly IDictionary<string, string> resourceNames;

            /// <summary>
            /// Constructs a new root resource set
            /// </summary>
            public RootResourceSet()
            {
                resourceNames = new Dictionary<string, string>();
                foreach (string logicalName in assembly.GetManifestResourceNames())
                {
                    string resourceID = slashSequenceRegex.Replace(logicalName, "/");
                    if (resourceNames.ContainsKey(resourceID)) throw new InvalidDataException($"Two embedded resources ({resourceNames[resourceID]} and {logicalName}) have the same normalized name");
                    resourceNames[resourceID] = logicalName;
                }
            }

            public IEnumerable<string> GetIDs() => resourceNames.Keys;

            public Stream? GetResourceStream(string resourceID) => resourceNames.ContainsKey(resourceID) ? assembly.GetManifestResourceStream(resourceNames[resourceID])! : null;
        }

        /// <summary>
        /// Nested resource set that filters the IDs of its parent resource set for a prefix.
        /// </summary>
        private class NestedResourceSet : IResourceSet
        {
            /// <summary>
            /// Containing parent resource set
            /// </summary>
            private readonly IResourceSet parent;

            /// <summary>
            /// ID prefix used for filtering
            /// </summary>
            private readonly string prefix;

            /// <summary>
            /// Constructs a nested resource set that filters the IDs of its parent resource set for a prefix.
            /// </summary>
            /// <param name="parent">Containing parent resource set</param>
            /// <param name="prefix">ID prefix used for filtering</param>
            public NestedResourceSet(IResourceSet parent, string prefix)
            {
                this.parent = parent;
                this.prefix = prefix;
            }

            public IEnumerable<string> GetIDs() => parent.GetIDs().WherePrefixed(new Regex(Regex.Escape(prefix)));

            public Stream? GetResourceStream(string resourceID) => parent.GetResourceStream($"{prefix}{resourceID}");
        }

        /// <summary>
        /// Resource set that combines two resource sets by looking up resources in the second set if they are not present in the first.
        /// </summary>
        private class CombinedResourceSet : IResourceSet
        {
            /// <summary>
            /// First combined resource set
            /// </summary>
            private readonly IResourceSet first;

            /// <summary>
            /// Second combined resource set (shadowed by first)
            /// </summary>
            private readonly IResourceSet second;

            /// <summary>
            /// Constructs a combined resource set that looks up resources in the second set if they are not present in the first.
            /// </summary>
            /// <param name="first">First resource set</param>
            /// <param name="second">Second resource set, shadowed by <paramref name="first" /></param>
            public CombinedResourceSet(IResourceSet first, IResourceSet second)
            {
                this.first = first;
                this.second = second;
            }

            public IEnumerable<string> GetIDs() => first.GetIDs().Concat(second.GetIDs()).Distinct();

            public Stream? GetResourceStream(string resourceID) => first.GetResourceStream(resourceID) ?? second.GetResourceStream(resourceID);
        }

        /// <summary>
        /// Resource set that provides access to an external file tree. The ID of a resource is equal to its relative file path, using slashes (<c>/</c>) as path separators.
        /// </summary>
        private class ExternalResourceSet : IResourceSet
        {
            /// <summary>
            /// Root path of the external file tree
            /// </summary>
            private readonly string root;

            /// <summary>
            /// Constructs a resource set that provides access to an external file tree.
            /// </summary>
            /// <param name="root">Root path of the external file tree</param>
            public ExternalResourceSet(string root) => this.root = root;

            public IEnumerable<string> GetIDs() => Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories).Select(path => Path.GetRelativePath(root, path));

            public Stream? GetResourceStream(string resourceID)
            {
                string file = Path.Join(root, resourceID);
                if (!File.Exists(file)) return null;
                return new FileStream(file, FileMode.Open, FileAccess.Read);
            }
        }
    }
}
