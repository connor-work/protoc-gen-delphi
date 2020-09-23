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
using Work.Connor.Delphi;
using Work.Connor.Delphi.CodeWriter;
using Xunit;
using Xunit.Abstractions;

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi.RuntimeTests
{
    // TODO extract common functionality with DelphiCompatibilityTest
    /// <summary>
    /// Tests if Delphi code produced by <see cref="ProtocGenDelphi"/> as a plug-in to <c>protoc</c> can encode and decode a known message,
    /// when combined with a functional runtime library implementation.
    /// </summary>
    public class KnownMessageEncodeDecodeTest
    {
        /// <summary>
        /// Resource set of all test resource files for this kind of test
        /// </summary>
        private static readonly IResourceSet testResources = IResourceSet.Root.Nest("[known message]");

        /// <summary>
        /// Resource set of all test resource files within folders containing <c>protoc</c> input (and support files)
        /// </summary>
        private static readonly IResourceSet allInputFolderResources = IResourceSet.Root.Nest("[known schema folder]");

        /// <summary>
        /// Resource set of all test resource files that are used as a single input protobuf schema definition file for <c>protoc</c>
        /// </summary>
        private static readonly IResourceSet allInputFileResources = IResourceSet.Root.Nest("[known schema file]");

        /// <summary>
        /// Resource set of all Delphi units that contain support source code for generated files
        /// </summary>
        private static readonly IResourceSet supportCodeUnitResources = IResourceSet.Root.Nest("[support code unit]");

        /// <summary>
        /// Resource set of all Delphi programs that encode a known message (IDs end with "/")
        /// </summary>
        private static readonly IResourceSet allEncodePrograms = testResources.Nest("[encode program]");

        /// <summary>
        /// Resource set of all Delphi programs that decode a known message and assert that the expected content is decoded (IDs end with "/")
        /// </summary>
        private static readonly IResourceSet allDecodePrograms = testResources.Nest("[decode program]");

        /// <summary>
        /// Resource set of all Delphi units that contain support source code for testing
        /// </summary>
        private static readonly IResourceSet testSupportCodeUnitResources = IResourceSet.Root.Nest("[Delphi test support code unit]");

        /// <summary>
        /// Names of all known test vectors
        /// </summary>
        private static IEnumerable<string> TestVectorNames => allDecodePrograms.GetIDs().WhereSuffixed(new Regex(Regex.Escape("/")));

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
        /// Represents a test vector for this kind of test
        /// </summary>
        public class TestVector : IXunitSerializable
        {
            /// <summary>
            /// Name of the test vector
            /// </summary>
            private string name;

            /// <summary>
            /// Resource set of all test resource files that are used as <c>protoc</c> input or support files for this test
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

            /// <summary>
            /// Helper function that sets up the test vector's resource sets.
            /// </summary>
            private void InitializeResourceSets() => inputFolderResources = allInputFolderResources.Nest($"{name}.protoc-input/");

            /// <summary>
            /// Name of the optional test resource file that is used as a single input protobuf schema definition file for <c>protoc</c> for this test
            /// </summary>
            private string InputSchemaFileName => $"{name}.proto";

            /// <summary>
            /// Name and contents of all .proto files that are used as <c>protoc</c> input for this test
            /// </summary>
            private IEnumerable<(string, string)> ProtoFilesToSetup => inputFolderResources.ReadResources(inputFolderResources.GetIDs().Where(name => name.EndsWith($".{ProtocGenDelphi.protoFileExtension}")))
                                                         .Concat(allInputFileResources.ReadResources(new[] { InputSchemaFileName }));

            /// <summary>
            /// Source code for the encode program
            /// </summary>
            public string EncodeProgramSource => allEncodePrograms.ReadResource($"{name}/") ?? throw new FileNotFoundException($"Encode program for {name}");

            /// <summary>
            /// Source code for the decode program
            /// </summary>
            public string DecodeProgramSource => allDecodePrograms.ReadResource($"{name}/")!;

            /// <summary>
            /// Name and contents of all files that should be copied to the plug-in output folder before performing further tests (support files)
            /// </summary>
            public IEnumerable<(string, string)> SupportFiles => inputFolderResources.ReadResources(inputFolderResources.GetIDs().Where(name => !name.EndsWith($".{ProtocGenDelphi.protoFileExtension}")))
                                                         .Concat(testSupportCodeUnitResources.ReadAllResources());

            /// <summary>
            /// Names of all .proto files that shall be specified for generation in the <c>protoc</c> arguments
            /// </summary>
            public IEnumerable<string> InputProtoFileNames => inputFolderResources.GetIDs().Where(name => name.Contains(inputFilePrefix) && name.EndsWith($".{ProtocGenDelphi.protoFileExtension}"))
                                                      .Concat(allInputFileResources.GetIDs().Where(name => name.Equals(InputSchemaFileName)));

            /// <summary>
            /// Paths of all folders that should be added to the <c>protoc</c> search path
            /// </summary>
            public IEnumerable<string> ProtoPath
            {
                get
                {
                    yield return inputFolder ?? throw new InvalidOperationException($"Test vector file tree was not setup using {nameof(SetupInputFileTree)}");
                }
            }

            /// <summary>
            /// Determines the Delphi namespace for the expected unit generated from a .proto file.
            /// </summary>
            /// <param name="name">Name of the .proto file</param>
            /// <returns>Segments of the namespace identifier</returns>
            private static IEnumerable<string> GetNamespaceSegmentsForProtoFile(string name) => name.Split(ProtocGenDelphi.protoFileNamePathSeparator)[0..^1].Select(segment => segment.ToPascalCase());

            /// <summary>
            /// All Delphi unit references that are required for test compilation.
            /// </summary>
            public IEnumerable<UnitReference> ReferencedUnits => InputProtoFileNames.Select(name => new UnitReference()
            {
                Unit = new UnitIdentifier()
                {
                    Namespace = { GetNamespaceSegmentsForProtoFile(name) },
                    Unit = $"u{Path.GetFileName(name).Split(".")[0].ToPascalCase()}"
                }
            });

            /// <summary>
            /// Determines the folders on the unit path for the test compilation.
            /// </summary>
            /// <param name="plugInOutputFolder">Folder containing all Delphi units produced by the plug-in</param>
            /// <returns>Paths of all folders on the unit path</returns>
            public IEnumerable<string> GetUnitPathFolders(string plugInOutputFolder) => InputProtoFileNames.Select(name => Path.Join(GetNamespaceSegmentsForProtoFile(name).Prepend(plugInOutputFolder).ToArray())).Distinct();

            /// <summary>
            /// Creates a temporary file tree containing input files, required before using the test vector.
            /// </summary>
            public void SetupInputFileTree()
            {
                inputFolder = CreateScratchFolder();
                foreach ((string name, string content) in ProtoFilesToSetup)
                {
                    string path = Path.Join(inputFolder, name);
                    Directory.CreateDirectory(Directory.GetParent(path).FullName);
                    File.WriteAllText(path, content);
                }
            }

            public void Deserialize(IXunitSerializationInfo info)
            {
                name = info.GetValue<string>(nameof(name));
                InitializeResourceSets();
            }

            public void Serialize(IXunitSerializationInfo info) => info.AddValue(nameof(name), name);

            public override string? ToString() => name;
        }

        /// <summary>
        /// All known test vectors
        /// </summary>
        public static IEnumerable<object[]> TestVectors => TestVectorNames.Select(name => new object[] { new TestVector(name) });

        /// <summary>
        /// <see cref="ProtocGenDelphi"/> produces Delphi code when used as a <c>protoc</c> plug-in, that can encode and decode a known message,
        /// when combined with a functional runtime library implementation.
        /// <param name="vector">Test vector</param>
        [Theory]
        [MemberData(nameof(TestVectors))]
        public void ProducesOutputThatCanEncodeAndDecodeAKnownMessage(TestVector vector)
        {
            // TODO this test should actually be skipped, waiting for xUnit support https://github.com/xunit/xunit/issues/2073#issuecomment-673632823
            //if (RuntimeTestOptions.UseStubRuntimeLibrary) return;
            //IResourceSet runtimeUnitResources = IResourceSet.External(RuntimeTestOptions.RuntimeLibrarySourcePath!);
            IResourceSet runtimeUnitResources = IResourceSet.External(@"C:\work\Software\forks\pikaju\protobuf-delphi\source");

            // Setup file tree as input for protoc, according to the test vector
            vector.SetupInputFileTree();
            // Run protoc
            ProtocOperation.PlugInOperation plugIn = new ProtocOperation.PlugInOperation("delphi") { OutDir = CreateScratchFolder() };
            ProtocOperation protoc = new ProtocOperation();
            protoc.ProtoPath.AddRange(vector.ProtoPath);
            protoc.ProtoFiles.AddRange(vector.InputProtoFileNames);
            protoc.PlugIns.Add(plugIn);
            (bool protocSuccess, _, string? protocError) = protoc.Perform();
            Assert.True(protocSuccess, protocError!);

            // Compiles a program using FPC
            string compile(string programFile)
            {
                // Run FPC
                FpcOperation fpc = new FpcOperation(programFile) { OutputPath = CreateScratchFolder() };
                if (Debugger.IsAttached) fpc.GenerateDebugInfo = true;
                fpc.UnitPath.AddRange(vector.GetUnitPathFolders(plugIn.OutDir));
                // Adds units from a resource set to FPC
                void addUnits(IEnumerable<(string name, string content)> resources, string rootFolder)
                {
                    foreach ((string name, string content) in resources)
                    {
                        string path = Path.Join(rootFolder, name);
                        string folder = Directory.GetParent(path).FullName;
                        Directory.CreateDirectory(folder);
                        File.WriteAllText(path, content);
                        if (!fpc.UnitPath.Contains(folder)) fpc.UnitPath.Add(folder);
                    }
                }
                // Create a scratch folder to hold the runtime-independent support source code
                addUnits(supportCodeUnitResources.ReadAllResources(), CreateScratchFolder());
                // Create a scratch folder to hold the runtime source code
                addUnits(runtimeUnitResources.ReadAllResources(), CreateScratchFolder());
                // Add support files (may contain required source code)
                addUnits(vector.SupportFiles, CreateScratchFolder());
                (bool fpcSuccess, _, string? fpcError) = fpc.Perform();
                Assert.True(fpcSuccess, fpcError!);
                return Path.Join(fpc.OutputPath, GetExecutableName(Regex.Replace(Path.GetFileName(programFile), $"{Regex.Escape($".{DelphiSourceCodeWriter.programSourceFileExtension}")}$", "")));
            }

            // Create and run encode/decode programs
            string encodeProgramFile = Path.Join(CreateScratchFolder(), "KnownMessageEncode.pas");
            File.WriteAllText(encodeProgramFile, vector.EncodeProgramSource);
            string encodeExeFile = compile(encodeProgramFile);
            string decodeProgramFile = Path.Join(CreateScratchFolder(), "KnownMessageDecode.pas");
            File.WriteAllText(decodeProgramFile, vector.DecodeProgramSource);
            string decodeExeFile = compile(decodeProgramFile);
            using Process encode = new Process();
            encode.StartInfo.FileName = encodeExeFile;
            encode.StartInfo.CreateNoWindow = true;
            encode.StartInfo.UseShellExecute = false;
            encode.StartInfo.RedirectStandardOutput = true;
            using Process decode = new Process();
            decode.StartInfo.FileName = decodeExeFile;
            decode.StartInfo.CreateNoWindow = true;
            decode.StartInfo.UseShellExecute = false;
            decode.StartInfo.RedirectStandardInput = true;
            decode.StartInfo.RedirectStandardError = true;
            StringBuilder decodeError = new StringBuilder();
            encode.Start();
            decode.Start();
            decode.ErrorDataReceived += delegate (object sender, DataReceivedEventArgs e) { decodeError.AppendLine(e.Data); };
            decode.BeginErrorReadLine();
            encode.StandardOutput.BaseStream.CopyTo(decode.StandardInput.BaseStream);
            encode.WaitForExit();
            decode.WaitForExit();
            Assert.True(decode.ExitCode == 0, decodeError.ToString());
        }
    }
}
