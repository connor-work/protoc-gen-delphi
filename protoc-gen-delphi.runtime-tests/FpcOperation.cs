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
using System.Runtime.InteropServices;
using System.Text;

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi.RuntimeTests
{
    // TODO this should be extracted to delphi-tools
    /// <summary>
    /// Represents a planned invocation of the Free Pascal Compiler (FPC).
    /// </summary>
    public class FpcOperation
    {
        /// <summary>
        /// Optional location of the <c>fpc</c> executable.
        /// If this value is absent, the executable is located by the system (usually using working directory and <c>PATH</c>).
        /// </summary>
        public string? FpcExecutablePath { get; set; }

        /// <summary>
        /// <i>Unit path</i> for FPC.
        /// </summary>
        public List<string> UnitPath { get; } = new List<string>();

        /// <summary>
        /// <i>Output path</i> for FPC.
        /// </summary>
        public string? OutputPath { get; set; }

        /// <summary>
        /// <i>Input file</i> for FPC.
        /// </summary>
        public string InputFile { get; }

        /// <summary>
        /// Constructs a new planned FPC invocation.
        /// </summary>
        /// <param name="InputFile">FPC input file, see <see cref="InputFile"/></param>
        public FpcOperation(string InputFile) => this.InputFile = InputFile;


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
        /// Performs the planned FPC invocation.
        /// </summary>
        /// <returns><see langword="true" /> if the operation succeeded, the exit code of FPC and an optional error message</returns>
        public (bool success, int exitCode, string? errorText) Perform()
        {
            using Process fpc = new Process();
            // By default, fpc resides in PATH
            fpc.StartInfo.FileName = FpcExecutablePath ?? GetExecutableName("fpc");
            foreach (string unitPathFolder in UnitPath) fpc.StartInfo.ArgumentList.Add($"-Fu{unitPathFolder}");
            if (OutputPath != null) fpc.StartInfo.ArgumentList.Add($"-FE{OutputPath}");
            fpc.StartInfo.ArgumentList.Add(InputFile);
            fpc.StartInfo.CreateNoWindow = true;
            fpc.StartInfo.UseShellExecute = false;
            fpc.StartInfo.RedirectStandardOutput = true;
            StringBuilder error = new StringBuilder();
            fpc.Start();
            fpc.OutputDataReceived += delegate (object sender, DataReceivedEventArgs e) { error.AppendLine(e.Data); };
            fpc.BeginOutputReadLine();
            fpc.WaitForExit();
            return (fpc.ExitCode == 0, fpc.ExitCode, error.Length == 0 ? null : error.ToString());
        }
    }
}
