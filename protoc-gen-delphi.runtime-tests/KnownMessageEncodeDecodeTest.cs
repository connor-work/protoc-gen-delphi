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
using Work.Connor.Delphi.Tools;
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
        /// Resource set of all Delphi units that construct a known message (IDs end with "/")
        /// </summary>
        private static readonly IResourceSet allConstructorUnits = testResources.Nest("[constructor unit]");

        /// <summary>
        /// Resource set of all Delphi units that validate a known message (IDs end with "/")
        /// </summary>
        private static readonly IResourceSet allValidatorUnits = testResources.Nest("[validator unit]");

        /// <summary>
        /// Resource set of all Delphi units that contain support source code for testing
        /// </summary>
        private static readonly IResourceSet testSupportCodeUnitResources = IResourceSet.Root.Nest("[Delphi test support code unit]");

        /// <summary>
        /// Resource set of all Delphi include files that contain support source code for testing
        /// </summary>
        private static readonly IResourceSet testSupportCodeIncludeFileResources = IResourceSet.Root.Nest("[Delphi test support code include file]");

        /// <summary>
        /// Resource set of all Delphi programs that contain support source code for testing
        /// </summary>
        private static readonly IResourceSet testSupportCodeProgramResources = IResourceSet.Root.Nest("[Delphi test support code program]");

        /// <summary>
        /// Names of all known test message validators
        /// </summary>
        private static IEnumerable<string> TestValidatorNames => allValidatorUnits.GetIDs().WhereSuffixed(new Regex(Regex.Escape("/")));

        /// <summary>
        /// Delphi compilers used for testing
        /// </summary>
        private static IEnumerable<DelphiCompiler> TestCompilers => (DelphiCompiler[])  Enum.GetValues(typeof(DelphiCompiler));

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
            /// Name of the test message validator
            /// </summary>
            private string validatorName;

            /// <summary>
            /// Compiler used for testing
            /// </summary>
            private DelphiCompiler compiler;

            /// <summary>
            /// Name of the test vector
            /// </summary>
            public string Name => $"{validatorName}-{compiler}";

            /// <summary>
            /// Compiler used for testing
            /// </summary>
            public DelphiCompiler Compiler => compiler;

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
            /// <param name="validatorName">Name of the test message validator</param>
            /// <param name="compiler">Compiler used for testing</param>
            public TestVector(string validatorName, DelphiCompiler compiler)
            {
                this.validatorName = validatorName;
                this.compiler = compiler;
                InitializeResourceSets();
            }
#pragma warning restore CS8618

            /// <summary>
            /// Helper function that sets up the test vector's resource sets.
            /// </summary>
            private void InitializeResourceSets() => inputFolderResources = allInputFolderResources.Nest($"{validatorName}.protoc-input/");

            /// <summary>
            /// Name of the optional test resource file that is used as a single input protobuf schema definition file for <c>protoc</c> for this test
            /// </summary>
            private string InputSchemaFileName => $"{validatorName}.proto";

            /// <summary>
            /// Name and contents of all .proto files that are used as <c>protoc</c> input for this test
            /// </summary>
            private IEnumerable<(string, string)> ProtoFilesToSetup => inputFolderResources.ReadResources(inputFolderResources.GetIDs().Where(name => name.EndsWith($".{ProtocGenDelphi.protoFileExtension}")))
                                                         .Concat(allInputFileResources.ReadResources(new[] { InputSchemaFileName }));

            /// <summary>
            /// Source code for message construction
            /// </summary>
            private string ConstructorUnitSource => allConstructorUnits.ReadResource($"{validatorName}/") ?? throw new FileNotFoundException($"Constuctor unit for {validatorName}");

            /// <summary>
            /// Source code for message validation
            /// </summary>
            private string ValidatorUnitSource => allValidatorUnits.ReadResource($"{validatorName}/")!;

            /// <summary>
            /// Name and contents of all files that should be copied to the plug-in output folder before performing tests
            /// </summary>
            public IEnumerable<(string, string)> SupportFiles => inputFolderResources.ReadResources(inputFolderResources.GetIDs().Where(name => !name.EndsWith($".{ProtocGenDelphi.protoFileExtension}")))
                                                         .Concat(testSupportCodeUnitResources.ReadAllResources())
                                                         .Concat(new (string, string)[] {
                                                             ("uConstruct.pas", ConstructorUnitSource),
                                                             ("uValidate.pas", ValidatorUnitSource)
                                                         });

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
                validatorName = info.GetValue<string>(nameof(validatorName));
                compiler = info.GetValue<DelphiCompiler>(nameof(compiler));
                InitializeResourceSets();
            }

            public void Serialize(IXunitSerializationInfo info)
            {
                info.AddValue(nameof(validatorName), validatorName);
                info.AddValue(nameof(compiler), compiler);
            }

            public override string? ToString() => Name;
        }

        /// <summary>
        /// All known test vectors
        /// </summary>
        public static IEnumerable<object[]> TestVectors => TestValidatorNames.SelectMany(validatorName => TestCompilers, (validatorName, compiler) => new object[] { new TestVector(validatorName, compiler) });

        /// <summary>
        /// <see cref="ProtocGenDelphi"/> produces Delphi code when used as a <c>protoc</c> plug-in, that can encode and decode a known message,
        /// when combined with a functional runtime library implementation.
        /// <param name="vector">Test vector</param>
        [Theory]
        [MemberData(nameof(TestVectors))]
        public void ProducesOutputThatCanEncodeAndDecodeAKnownMessage(TestVector vector)
        {
            // TODO these tests should actually be skipped, waiting for xUnit support https://github.com/xunit/xunit/issues/2073#issuecomment-673632823
            if (vector.Compiler == DelphiCompiler.DCC64
             && RuntimeTestOptions.DisableDCC64) return;
            if (RuntimeTestOptions.UseStubRuntimeLibrary) return;
            IResourceSet runtimeUnitResources = IResourceSet.External(RuntimeTestOptions.RuntimeLibrarySourcePath!);

            // Setup file tree as input for protoc, according to the test vector
            vector.SetupInputFileTree();
            // Run protoc
            ProtocOperation.PlugInOperation plugIn = new ProtocOperation.PlugInOperation("delphi")
            {
                ExecutableFolder = "exe-protoc-gen-delphi",
                FallbackToPath = true,
                OutDir = CreateScratchFolder()
            };
            ProtocOperation protoc = new ProtocOperation();
            protoc.ProtoPath.AddRange(vector.ProtoPath);
            protoc.ProtoFiles.AddRange(vector.InputProtoFileNames);
            protoc.PlugIns.Add(plugIn);
            (bool protocSuccess, _, string? protocError) = protoc.Perform();
            Assert.True(protocSuccess, protocError!);

            // Compiles a program using the Delphi compiler
            string compile(string programFile)
            {
                // Run the Delphi compiler
                DelphiCompilerOperation compilation = DelphiCompilerOperation.Plan(vector.Compiler, programFile);
                compilation.OutputPath = CreateScratchFolder();
                if (Debugger.IsAttached) compilation.GenerateDebugInfo = true;
                compilation.UnitPath.AddRange(vector.GetUnitPathFolders(plugIn.OutDir));
                // Adds units from a resource set to the compilation
                void addUnits(IEnumerable<(string name, string content)> resources, string rootFolder)
                {
                    foreach ((string name, string content) in resources)
                    {
                        string path = Path.Join(rootFolder, name);
                        string folder = Directory.GetParent(path).FullName;
                        Directory.CreateDirectory(folder);
                        File.WriteAllText(path, content);
                        if (!compilation.UnitPath.Contains(folder)) compilation.UnitPath.Add(folder);
                    }
                }
                // Adds include files from a resource set to the compilation
                void addIncludeFiles(IEnumerable<(string name, string content)> resources, string rootFolder)
                {
                    foreach ((string name, string content) in resources)
                    {
                        string path = Path.Join(rootFolder, name);
                        string folder = Directory.GetParent(path).FullName;
                        Directory.CreateDirectory(folder);
                        File.WriteAllText(path, content);
                        if (!compilation.IncludePath.Contains(folder)) compilation.IncludePath.Add(folder);
                    }
                }
                // Create a scratch folder to hold the runtime-independent support source code
                addUnits(supportCodeUnitResources.ReadAllResources(), CreateScratchFolder());
                addIncludeFiles(testSupportCodeIncludeFileResources.ReadAllResources(), CreateScratchFolder());
                // Create a scratch folder to hold the runtime source code
                addUnits(runtimeUnitResources.ReadAllResources(), CreateScratchFolder());
                // Add support files (may contain required source code)
                addUnits(vector.SupportFiles, CreateScratchFolder());
                (bool compilationSuccess, _, string? compilationError) = compilation.Perform();
                Assert.True(compilationSuccess, compilationError!);
                return Path.Join(compilation.OutputPath, GetExecutableName(Regex.Replace(Path.GetFileName(programFile), $"{Regex.Escape($".dpr")}$", "")));
            }

            // Create message encode/decode test program
            string programFile = Path.Join(CreateScratchFolder(), "MessageEncodeDecodeTest.dpr");
            File.WriteAllText(programFile, testSupportCodeProgramResources.ReadResource("MessageEncodeDecodeTest.dpr")!);


            // Run encode/decode test program
            string executableFile = compile(programFile);
            using Process process = new Process();
            process.StartInfo.FileName = executableFile;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardError = true;
            StringBuilder error = new StringBuilder();
            process.Start();
            process.ErrorDataReceived += delegate (object sender, DataReceivedEventArgs e) { error.AppendLine(e.Data); };
            process.BeginErrorReadLine();
            process.WaitForExit();
            Assert.True(process.ExitCode == 0, error.ToString());
        }
    }
}
