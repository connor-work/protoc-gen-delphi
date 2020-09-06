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
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Tests
{
    /// <summary>
    /// Tests <see cref="ProtocGenDelphi"/> as a plug-in to <c>protoc</c> with known inputs and outputs.
    /// </summary>
    public class KnownAnswerProtocIntegrationTest
    {
        /// <summary>
        /// Suffix to the name of a folder containing input files in test data
        /// </summary>
        public static readonly string inputFolderSuffix = ".input_folder";

        /// <summary>
        /// Suffix to the name of a folder containing expected output files in test data
        /// </summary>
        public static readonly string outputFolderSuffix = ".output_folder";

        /// <summary>
        /// Prefix to the name of .proto files in test input folders that shall be used as input files
        /// </summary>
        public static readonly string inputFilePrefix = "input_file_";

        /// <summary>
        /// Marker string in the name of of test vectors that indicates that the default runtime shall be used
        /// </summary>
        public static readonly string defaultRuntimeMarker = "with_default_runtime";

        /// <summary>
        /// Utility function to create a temporary scratch folder for testing.
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
        /// Determines the platform identifier in <c>protoc</c>'s path.
        /// </summary>
        /// <returns>The <c>protoc</c> platform identifier string</returns>
        private static string GetProtocPlatform()
        {
            if (!Environment.Is64BitOperatingSystem) throw new NotImplementedException("Unsupported non-64-bit OS");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return "windows_x64";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) return "macosx_x64";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) return "linux_x64";
            throw new NotImplementedException("Unsupported OS");
        }

        /// <summary>
        /// When used as a plug-in to <c>protoc</c>, <see cref="ProtocGenDelphi"/> produces the expected files.
        /// </summary>
        /// <param name="protocArgs">Arguments passed to <c>protoc</c>, without the plug-in reference and output specifier</param>
        /// <param name="expectedFiles">Mapping of relative file paths in the plug-in output directory to expected file content</param>
        /// <param name="useDefaultRuntime"><see langword="true"/> if the default runtime shall be used (instead of the stub)</param>
        [Theory]
        [MemberData(nameof(KnownGeneratedFileSets))]
        public void GeneratesExpectedFilesWithProtoc(IEnumerable<string> protocArgs, IDictionary<string, string> expectedFiles, bool useDefaultRuntime)
        {
            // Create a scratch folder as output folder for the plug-in
            string outputFolder = CreateScratchFolder();
            // Run protoc
            using Process protoc = new Process();
            protoc.StartInfo.FileName = Path.Join("Google.Protobuf.Tools", "tools", GetProtocPlatform(), GetExecutableName("protoc"));
            // A leading dot seems to be required in the plugin folder name for protoc
            protoc.StartInfo.ArgumentList.Add($"--plugin={Path.Join(".", GetExecutableName("protoc-gen-delphi"))}");
            protoc.StartInfo.ArgumentList.Add($"--delphi_out={outputFolder}");
            if (!useDefaultRuntime) protoc.StartInfo.ArgumentList.Add($"--delphi_opt={ProtocGenDelphi.customRuntimeOption}={IRuntimeSupport.Stub.DelphiNamespace}");
            foreach (string arg in protocArgs) protoc.StartInfo.ArgumentList.Add(arg);
            protoc.StartInfo.CreateNoWindow = true;
            protoc.StartInfo.UseShellExecute = false;
            protoc.StartInfo.RedirectStandardError = true;
            string error = "";
            protoc.Start();
            protoc.ErrorDataReceived += delegate (object sender, DataReceivedEventArgs e) { error += e.Data; };
            protoc.BeginErrorReadLine();
            protoc.WaitForExit();
            // Check protoc success
            Assert.Equal(0, protoc.ExitCode);
            // Check that expected files are generated
            foreach ((string path, string expectedContent) in expectedFiles) Assert.Equal(expectedContent, File.ReadAllText(Path.Join(outputFolder, path)));
            // Check that no other files are generated
            foreach (string path in Directory.GetFiles(outputFolder, "*", SearchOption.AllDirectories)) Assert.Contains(Path.GetRelativePath(outputFolder, path), expectedFiles.Keys);

        }

        public static IEnumerable<object[]> KnownGeneratedFileSets()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            // Sets up scratch folders and constructs protoc args
            object[] buildArgs(string outputFolderName, IEnumerable<string> protoFileNames, IEnumerable<string> inputFileNames, bool useDefaultRuntime)
            {
                // The names of manifest resources of the expected output files are prefixed with the folder resource name and a dot
                string outputFilePrefix = outputFolderName + ".";
                // Read expected output files
                Dictionary<string, string> expectedFiles = new Dictionary<string, string>();
                foreach (string outputFileName in assembly.GetManifestResourceNames().Where(name => name.StartsWith(outputFilePrefix)))
                {
                    // TODO improve this, will not handle sub-directories correctly
                    string expectedFilePath = outputFileName.Substring(outputFilePrefix.Length);
                    using StreamReader outputFileReader = new StreamReader(assembly.GetManifestResourceStream(outputFileName));
                    expectedFiles.Add(expectedFilePath, outputFileReader.ReadToEnd());
                }
                // Set up input files for protoc in a scratch folder
                string inputFolder = CreateScratchFolder();
                // Read input files
                List<string> inputFilePaths = new List<string>();
                foreach (string protoFileName in protoFileNames)
                {
                    using StreamReader protoFileReader = new StreamReader(assembly.GetManifestResourceStream(protoFileName));
                    // TODO improve this, will not handle sub-directories and dots in file names correctly
                    string[] protoFileNameSegments = protoFileName.Split(".");
                    string protoFileNameLastSegment = $"{protoFileNameSegments[^2]}.{protoFileNameSegments[^1]}";
                    string protoFilePath = Path.Join(inputFolder, protoFileNameLastSegment);
                    // Some .proto files are used as input files to protoc
                    if (inputFileNames.Contains(protoFileName) || protoFileNameLastSegment.StartsWith(inputFilePrefix)) inputFilePaths.Add(protoFilePath);
                    File.WriteAllText(protoFilePath, protoFileReader.ReadToEnd());
                }
                string[] protocArgs = inputFilePaths.Prepend($"-I{inputFolder}").ToArray();
                return new object[] { protocArgs, expectedFiles, useDefaultRuntime };
            }

            // Resources contain pairs of protobuf schema definitions and output folders containing expected Delphi unit source code files
            foreach (string inputFileName in assembly.GetManifestResourceNames().Where(name => name.EndsWith(ProtocGenDelphi.protoFileExtension)
                                                                                            && !name.Contains(inputFolderSuffix)))
            {
                string outputFolderName = inputFileName.Substring(0, inputFileName.Length - ProtocGenDelphi.protoFileExtension.Length - 1) + outputFolderSuffix;
                yield return buildArgs(outputFolderName, new[] { inputFileName }, new[] { inputFileName }, inputFileName.Contains(defaultRuntimeMarker));
            }

            // Resources contain pairs of input folders containing protobuf schema definitions and output folders containing expected Delphi unit source code files
            List<string> inputFolderNames = new List<string>();
            Regex folderNameRegex = new Regex("^(.*" + Regex.Escape(inputFolderSuffix) + ")");
            foreach (string fileName in assembly.GetManifestResourceNames())
            {
                Match match = folderNameRegex.Match(fileName);
                if (!match.Success) continue;
                string folderName = match.Groups[1].Value;
                if (!inputFolderNames.Contains(folderName)) inputFolderNames.Add(folderName);
            }
            foreach (string inputFolderName in inputFolderNames)
            {
                string outputFolderName = inputFolderName.Substring(0, inputFolderName.Length - inputFolderSuffix.Length) + outputFolderSuffix;
                yield return buildArgs(outputFolderName, assembly.GetManifestResourceNames().Where(name => name.StartsWith(inputFolderName)),
                    Enumerable.Empty<string>(), inputFolderName.Contains(defaultRuntimeMarker));
            }
        }
    }
}
