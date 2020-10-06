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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi.RuntimeTests
{
    // TODO this should be extracted to protobuf-tools
    /// <summary>
    /// Represents a planned invocation of the protobuf compiler <c>protoc</c>.
    /// </summary>
    public class ProtocOperation
    {
        /// <summary>
        /// Optional location of the <c>protoc</c> executable.
        /// If this value is absent, the executable is located by the system (usually using working directory and <c>PATH</c>).
        /// </summary>
        public string? ProtocExecutablePath { get; set; }

        /// <summary>
        /// Mutable list of protobuf schema definitions (<c>.proto</c> files) used as input.
        /// Must not be empty when performing operation.
        /// </summary>
        public List<string> ProtoFiles { get; } = new List<string>();

        /// <summary>
        /// Mutable list of directories in which <c>protoc</c> searches for imports (in order).
        /// This implementation does not support searches based on the current working directory.
        /// </summary>
        public List<string> ProtoPath { get; } = new List<string>();

        /// <summary>
        /// Mutable list of planned plug-in invocations that <c>protoc</c> shall perform.
        /// Each plug-in (identified by <see cref="PlugInOperation.Name" />) may only be included once.
        /// </summary>
        public List<PlugInOperation> PlugIns { get; } = new List<PlugInOperation>();

        /// <summary>
        /// Utility function to create a temporary scratch folder.
        /// </summary>
        /// <returns>Path of the new folder</returns>
        private static string CreateScratchFolder()
        {
            string path = Path.GetTempFileName();
            File.Delete(path);
            Directory.CreateDirectory(path);
            return path;
        }

        /// <summary>
        /// Constructs an executable file name for the current platform.
        /// </summary>
        /// <param name="name">The base name, without extension</param>
        /// <returns>The executable file name</returns>
        private static string GetExecutableName(string name)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return $"{name}.exe";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) return name;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) return name;
            throw new NotImplementedException("Unsupported OS");
        }

        /// <summary>
        /// Performs the planned <c>protoc</c> invocation.
        /// </summary>
        /// <returns><see langword="true" /> if the operation succeeded, the exit code of <c>protoc</c> and an optional error message</returns>
        public (bool success, int exitCode, string? errorText) Perform()
        {
            using Process protoc = new Process();
            protoc.StartInfo.FileName = ProtocExecutablePath ?? GetExecutableName("protoc");
            foreach (string plugInArg in PlugIns.SelectMany(plugIn => plugIn.ProtocArgs)) protoc.StartInfo.ArgumentList.Add(plugInArg);
            foreach (string protoPathFolder in ProtoPath) protoc.StartInfo.ArgumentList.Add($"--proto_path={protoPathFolder}");
            foreach (string protoFile in ProtoFiles) protoc.StartInfo.ArgumentList.Add(protoFile);
            protoc.StartInfo.CreateNoWindow = true;
            protoc.StartInfo.UseShellExecute = false;
            protoc.StartInfo.RedirectStandardError = true;
            // Prevent plug-ins being affected by working directory
            protoc.StartInfo.WorkingDirectory = CreateScratchFolder();
            StringBuilder error = new StringBuilder();
            protoc.Start();
            protoc.ErrorDataReceived += delegate (object sender, DataReceivedEventArgs e) { error.AppendLine(e.Data); };
            protoc.BeginErrorReadLine();
            protoc.WaitForExit();
            return (protoc.ExitCode == 0, protoc.ExitCode, error.Length == 0 ? null : error.ToString());
        }

        /// <summary>
        /// Represents a planned invocation of a <c>protoc</c> plug-in.
        /// </summary>
        public class PlugInOperation
        {
            /// <summary>
            /// Name of the plug-in excluding the default prefix, e.g. <c>delphi</c> for <c>protoc-gen-delphi</c>.
            /// </summary>
            public string Name { get; }

            /// <summary>
            /// Optional location of the plug-in executable.
            /// If both this value and <see cref="ExecutableFolder"/> are absent, the executable is located by <c>protoc</c> (using <c>PATH</c>).
            /// </summary>
            public string? ExecutablePath { get; set; } = null;

            /// <summary>
            /// Optional location of the folder containing the plug-in executable.
            /// The executable name is constructed by adding the default prefix, e.g., <c>protoc-gen-delphi</c> for <c>delphi</c>.
            /// Ignored if <see cref="ExecutablePath"/> is present.
            /// If both this value and <see cref="ExecutableFolder"/> are absent, the executable is located by <c>protoc</c> (using <c>PATH</c>).
            /// </summary>
            public string? ExecutableFolder { get; set; } = null;

            /// <summary>
            /// If <see langword="true"/>, and the executable file specified by <see cref="ExecutablePath"/> or <see cref="ExecutableFolder"/> is missing,
            /// the executable is located by <c>protoc</c> (using <c>PATH</c>). Otherwise, <see cref="Perform"/> will fail if the file is missing.
            /// </summary>
            public bool FallbackToPath { get; set; } = false;

            /// <summary>
            /// Optional output folder for files generated by the plug-in.
            /// </summary>
            public string? OutDir { get; set; } = null;

            /// <summary>
            /// Mutable dictionary of custom options that shall be passed to the plug-in.
            /// </summary>
            public IDictionary<string, string> Options { get; } = new Dictionary<string, string>();

            /// <summary>
            /// Constructs a new planned plug-in invocation.
            /// </summary>
            /// <param name="name">Name of the plug-in, see <see cref="Name"/></param>
            public PlugInOperation(string name) => Name = name;

            /// <summary>
            /// Actual location of the plug-in executable (if known).
            /// </summary>
            private string? ActualExecutablePath => ExecutablePath ?? (ExecutableFolder == null ? null
                                                                                                : Path.Join(ExecutableFolder, GetExecutableName($"protoc-gen-{Name}")));

            /// <summary>
            /// Arguments that shall be passed to <c>protoc</c>, to include the plug-in invocation.
            /// </summary>
            internal IEnumerable<string> ProtocArgs
            {
                get
                {
                    string? pluginPath = ActualExecutablePath;
                    // Relative paths usually seem to work, but protoc output indicates that this is not intentional, see e.g., https://github.com/protocolbuffers/protobuf/issues/791#issuecomment-539814712
                    if (pluginPath != null && !(FallbackToPath && !File.Exists(pluginPath))) yield return $"--plugin=protoc-gen-{Name}={Path.GetFullPath(pluginPath)}";
                    if (OutDir != null) yield return $"--{Name}_out={OutDir}";
                    if (Options.Count != 0) yield return $"--{Name}_opt={string.Join(",", Options.Select(pair => $"{pair.Key}={pair.Value}"))}";
                }
            }
        }
    }
}
