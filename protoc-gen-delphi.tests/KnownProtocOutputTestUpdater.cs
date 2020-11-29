/// Copyright 2020 Connor Roehricht (connor.work)
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

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Tests
{
    /// <summary>
    /// Utility class for updating test vectors for <see cref="KnownProtocOutputTest"/>.
    /// </summary>
    public static class KnownProtocOutputTestUpdater
    {
        /// <summary>
        /// Updates the expected output files of a test vector to match the current implementation.
        /// </summary>
        /// <param name="vectorName">Name of the test vector</param>
        /// <param name="outputDirectory">The test vector's expected output folder</param>
        public static void UpdateKnownProtocOutputs(string vectorName, string outputDirectory)
        {
            (bool success, _, string? protocError) = KnownProtocOutputTest.ConstructProtocOperation(new KnownProtocOutputTest.TestVector(vectorName), outputDirectory).Perform();
            if (!success) throw new InvalidOperationException(protocError);
        }
    }
}
