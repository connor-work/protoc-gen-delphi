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
using System.Text.RegularExpressions;
using Work.Connor.Delphi.CodeWriter;
using Xunit;
using Xunit.Abstractions;

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Tests
{
    /// <summary>
    /// Tests if Delphi code produced by <see cref="ProtocGenDelphi"/> as a plug-in to <c>protoc</c> can be compiled.
    /// </summary>
    public class DelphiCompilabilityTest
    {
        // TODO separate input files, plus common schema folder?

        /// <summary>
        /// Resource set of all test resource files for this kind of test
        /// </summary>
        private static readonly IResourceSet testResources = IResourceSet.Root.Nest("[known protoc output]");

        /// <summary>
        /// Resource set of all test resource files within folders containing <c>protoc</c> input
        /// </summary>
        private static readonly IResourceSet allInputFolderResources = testResources.Nest("[input folder]");

        /// <summary>
        /// Resource set of all test resource files that are used as a single input protobuf schema definition file for <c>protoc</c>
        /// </summary>
        private static readonly IResourceSet allInputFileResources = testResources.Nest("[input schema file]");

        /// <summary>
        /// Resource set of all Delphi units that contain support source code for generated files
        /// </summary>
        private static readonly IResourceSet supportCodeUnitResources = IResourceSet.Root.Nest("[support code unit]");

        /// <summary>
        /// Resource set of all Delphi units that form the stub runtime library implementation
        /// </summary>
        private static readonly IResourceSet stubRuntimeUnitResources = IResourceSet.Root.Nest("[stub runtime unit]");

        /// <summary>
        /// Names of all known test vectors
        /// </summary>
        /// <returns>Enumeration of test vector names</returns>
        private static IEnumerable<string> TestVectorNames() => allInputFolderResources.GetIDs().WhereSuffixed(new Regex($"{Regex.Escape(".protoc-input")}/.*"))
                                                        .Concat(allInputFileResources.GetIDs().WhereSuffixed(new Regex(Regex.Escape(".proto"))))
                                                        .Distinct();

        /// <summary>
        /// Prefix to the name of .proto files in test input folders that shall be used as input files
        /// </summary>
        public static readonly string inputFilePrefix = "input_file_";

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
        /// Represents a test vector for this kind of test
        /// </summary>
        public class TestVector : IXunitSerializable
        {
            /// <summary>
            /// Name of the test vector
            /// </summary>
            private string name;

            /// <summary>
            /// Resource set of all test resource files that are used as <c>protoc</c> input for this test
            /// </summary>
            private IResourceSet inputFolderResources;

            /// <summary>
            /// Folder containing the input file tree for <c>protoc</c>. Only present after test file tree setup.
            /// </summary>
            private string? inputFolder;

            /// <summary>
            /// Constructs a new test vector for deserialization by xUnit.
            /// </summary>
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable. -> Initialized during deserialization by xUnit or through InitializeResourceSet
            public TestVector() { }

            /// <summary>
            /// Constructs a new test vector.
            /// </summary>
            /// <param name="name">Name of the test vector</param>
            public TestVector(string name) 
            {
                this.name = name;
                InitializeResourceSets();
            }
#pragma warning restore CS8618

            private void InitializeResourceSets()
            {
                inputFolderResources = allInputFolderResources.Nest($"{name}.protoc-input/");
            }

            /// <summary>
            /// Name of the optional test resource file that is used as a single input protobuf schema definition file for <c>protoc</c> for this test
            /// </summary>
            private string InputSchemaFileName { get => $"{name}.proto"; }

            /// <summary>
            /// Name and contents of all .proto files that are used as <c>protoc</c> input for this test
            /// </summary>
            private IEnumerable<(string, string)> ProtoFilesToSetup { get => inputFolderResources.ReadAllResources().Concat(allInputFileResources.ReadResources(new[] { InputSchemaFileName })); }

            /// <summary>
            /// Names of all .proto files that shall be specified for generation in the <c>protoc</c> arguments
            /// </summary>
            private IEnumerable<string> InputProtoFileNames
            {
                get => inputFolderResources.GetIDs().Where(name => name.Contains(inputFilePrefix)).Concat(
                       allInputFileResources.GetIDs().Where(name => name.Equals(InputSchemaFileName)));
            }

            /// <summary>
            /// Determines the Delphi namespace for the expected unit generated from a .proto file.
            /// </summary>
            /// <param name="name">Name of the .proto file</param>
            /// <returns>Segments of the namespace identifier</returns>
            private static IEnumerable<string> GetNamespaceSegmentsForProtoFile(string name) => name.Split(ProtocGenDelphi.protoFileNamePathSeparator)[0..^1].Select(segment => segment.ToPascalCase());

            /// <summary>
            /// Names of all Delphi units that shall be referenced during the test compilation.
            /// </summary>
            public IEnumerable<string> ReferencedUnits { get => InputProtoFileNames.Select(name => string.Join(".", GetNamespaceSegmentsForProtoFile(name).Append($"u{Path.GetFileName(name).Split(".")[0].ToPascalCase()}"))); }

            /// <summary>
            /// Determines the folders on the unit path for the test compilation.
            /// </summary>
            /// <param name="plugInOutputFolder">Folder containing all Delphi units produced by the plug-in</param>
            /// <returns>Paths of all folders on the unit path</returns>
            public IEnumerable<string> GetUnitPathFolders(string plugInOutputFolder) => InputProtoFileNames.Select(name => Path.Join(GetNamespaceSegmentsForProtoFile(name).Prepend(plugInOutputFolder).ToArray())).Distinct();

            /// <summary>
            /// Creates a temporary file tree containing input files, required before using the test vector
            /// </summary>
            public void SetupFileTree()
            {
                inputFolder = CreateScratchFolder();
                foreach ((string name, string content) in ProtoFilesToSetup)
                {
                    string path = Path.Join(inputFolder, name);
                    Directory.CreateDirectory(Directory.GetParent(path).FullName);
                    File.WriteAllText(path, content);
                }
            }

            /// <summary>
            /// Arguments to <c>protoc</c>
            /// </summary>
            public IEnumerable<string> ProtocArgs { get => InputProtoFileNames.Prepend($"-I{inputFolder ?? throw new InvalidOperationException($"Test vector file tree was not setup using {nameof(SetupFileTree)}")}"); }

            public void Deserialize(IXunitSerializationInfo info)
            {
                name = info.GetValue<string>(nameof(name));
                InitializeResourceSets();
            }

            public void Serialize(IXunitSerializationInfo info)
            {
                info.AddValue(nameof(name), name);
            }

            public override string? ToString() => name;
        }

        /// <summary>
        /// All known test vectors
        /// </summary>
        public static IEnumerable<object[]> TestVectors { get => TestVectorNames().Select(name => new object[] { new TestVector(name) }); }

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
        /// <see cref="ProtocGenDelphi"/> produces Delphi code when used as a <c>protoc</c> plug-in, that can be compiled
        /// </summary>
        /// <param name="vector">Test vector</param>
        [Theory]
        [MemberData(nameof(TestVectors))]
        public void ProducesOutputThatCanBeCompiled(TestVector vector)
        {
            // TODO we should actually compile all from input folder (for import test)
            // Setup file tree as input for protoc, according to the test vector
            vector.SetupFileTree();
            // Create a scratch folder as output folder for the plug-in
            string plugInOutputFolder = CreateScratchFolder();
            // Run protoc
            using Process protoc = new Process();
            protoc.StartInfo.FileName = Path.Join("Google.Protobuf.Tools", "tools", GetProtocPlatform(), GetExecutableName("protoc"));
            // A leading dot seems to be required in the plugin folder name for protoc
            protoc.StartInfo.ArgumentList.Add($"--plugin={Path.Join(".", GetExecutableName("protoc-gen-delphi"))}");
            protoc.StartInfo.ArgumentList.Add($"--delphi_out={plugInOutputFolder}");
            protoc.StartInfo.ArgumentList.Add($"--delphi_opt={ProtocGenDelphi.customRuntimeOption}={IRuntimeSupport.Stub.DelphiNamespace}");
            foreach (string arg in vector.ProtocArgs) protoc.StartInfo.ArgumentList.Add(arg);
            protoc.StartInfo.CreateNoWindow = true;
            protoc.StartInfo.UseShellExecute = false;
            protoc.StartInfo.RedirectStandardError = true;
            string protocError = "";
            protoc.Start();
            protoc.ErrorDataReceived += delegate (object sender, DataReceivedEventArgs e) { protocError += e.Data; };
            protoc.BeginErrorReadLine();
            protoc.WaitForExit();
            // Check protoc success
            Assert.Equal(0, protoc.ExitCode);

            // Create a scratch folder as output folder for FPC
            string fpcOutputFolder = CreateScratchFolder();
            // Create a test runner program as input for FPC
            string fpcProgramFile = Path.Join(CreateScratchFolder(), "DelphiCompilationTestProgram.pas");
            // TODO: Generate using Delphi Source Code Writer (needs to support "program")
            // TODO: This currently supports only one referenced unit
            File.WriteAllText(fpcProgramFile,
$@"
program DelphiCompilationTestProgram;

{{$IFDEF FPC}}
  {{$MODE DELPHI}}
{{$ENDIF}}

uses {vector.ReferencedUnits.First()};

begin
    
end.
"
                );
            // Create a scratch folder to hold the runtime-independent support source code
            string supportCodeFolder = CreateScratchFolder();
            foreach ((string name, string content) in supportCodeUnitResources.ReadAllResources()) File.WriteAllText(Path.Join(supportCodeFolder, name), content);
            // Create a scratch folder to hold the runtime source code
            string runtimeFolder = CreateScratchFolder();
            foreach ((string name, string content) in stubRuntimeUnitResources.ReadAllResources()) File.WriteAllText(Path.Join(runtimeFolder, name), content);
            // Run FPC
            using Process fpc = new Process();
            fpc.StartInfo.FileName = GetExecutableName("fpc");
            fpc.StartInfo.ArgumentList.Add($"-FE{fpcOutputFolder}");
            fpc.StartInfo.ArgumentList.Add($"-Fu{supportCodeFolder}");
            fpc.StartInfo.ArgumentList.Add($"-Fu{runtimeFolder}");
            foreach (string unitPathFolder in vector.GetUnitPathFolders(plugInOutputFolder)) fpc.StartInfo.ArgumentList.Add($"-Fu{unitPathFolder}");
            fpc.StartInfo.ArgumentList.Add(fpcProgramFile);
            fpc.StartInfo.CreateNoWindow = true;
            fpc.StartInfo.UseShellExecute = false;
            fpc.StartInfo.RedirectStandardOutput = true;
            StringBuilder fpcOut = new StringBuilder();
            fpc.Start();
            fpc.OutputDataReceived += delegate (object sender, DataReceivedEventArgs e) { fpcOut.AppendLine(e.Data); };
            fpc.BeginOutputReadLine();
            fpc.WaitForExit();
            // Check FPC success
            Assert.Equal(0, fpc.ExitCode);
        }
    }
}
