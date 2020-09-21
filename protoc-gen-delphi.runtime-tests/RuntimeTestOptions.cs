
using System;
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
namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi.RuntimeTests
{
    /// <summary>
    /// Collection of configurable options for the runtime integration tests.
    /// </summary>
    public static class RuntimeTestOptions
    {
        /// <summary>
        /// Optional location of the runtime library sources.
        /// If this value is absent, <see cref="UseStubRuntimeLibrary"/> is <see langword="true"/>.
        /// </summary>
        public static string? RuntimeLibrarySourcePath => Environment.GetEnvironmentVariable("Work_Connor_Protobuf_Delphi_ProtocGenDelphi_RuntimeTests_RuntimeLibrarySourcePath");

        /// <summary>
        /// <see langword="true"/> if the embedded stub runtime library shall be used for testing and all tests requiring runtime functionality will be skipped.
        /// </summary>
        public static bool UseStubRuntimeLibrary => RuntimeLibrarySourcePath == null;
    }
}
