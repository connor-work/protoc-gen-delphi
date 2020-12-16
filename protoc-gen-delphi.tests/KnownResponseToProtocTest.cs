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

using Google.Protobuf;
using Google.Protobuf.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Tests
{
    /// <summary>
    /// Tests <see cref="ProtocGenDelphi"/> with known requests from and responses to <c>protoc</c>.
    /// </summary>
    public class KnownResponseToProtocTest
    {
        /// <summary>
        /// File name extension (without leading dot) for JSON-encoded protobuf messages in test data
        /// </summary>
        public static readonly string protobufJsonFileExtension = "pb.json";

        /// <summary>
        /// File name extension (without leading dot) for JSON-encoded <see cref="CodeGeneratorRequest"/>s in test data
        /// </summary>
        public static readonly string requestFileExtension = "request." + protobufJsonFileExtension;

        /// <summary>
        /// File name extension (without leading dot) for JSON-encoded <see cref="CodeGeneratorResponse"/>s in test data
        /// </summary>
        public static readonly string responseFileExtension = "response." + protobufJsonFileExtension;

        /// <summary>
        /// Resource set of all test resource files for this kind of test
        /// </summary>
        private static readonly IResourceSet testResources = IResourceSet.Root.Nest("[known response to protoc]");

        /// <summary>
        /// Resource set of all test resource files that define expected responses to <c>protoc</c>
        /// </summary>
        private static readonly IResourceSet allExpectedResponseResources = testResources.Nest("[expected response]");

        /// <summary>
        /// Resource set of all test resource files containing requests from <c>protoc</c>
        /// </summary>
        private static readonly IResourceSet allRequestResources = testResources.Nest("[request]");

        /// <summary>
        /// Names of all known test vectors
        /// </summary>
        private static IEnumerable<string> TestVectorNames => allExpectedResponseResources.GetIDs().WhereSuffixed(new Regex(Regex.Escape($".{responseFileExtension}")));

        /// <summary>
        /// Formatter settings for encoding protobuf messages as JSON for test data. Can be used when creating new test vectors.
        /// </summary>
        public static readonly JsonFormatter.Settings protobufJsonFormatSettings = JsonFormatter.Settings.Default.WithFormatDefaultValues(false).WithFormatEnumsAsIntegers(false);

        /// <summary>
        /// Parser settings for decoding protobuf messages from JSON for test data
        /// </summary>
        public static readonly JsonParser.Settings protobufJsonParseSettings = JsonParser.Settings.Default.WithIgnoreUnknownFields(false);

        /// <summary>
        /// Represents a test vector for this kind of test
        /// </summary>
        public class TestVector : IXunitSerializable
        {
            /// <summary>
            /// Parser for JSON-encoded protobuf test data
            /// </summary>
            private static readonly JsonParser jsonParser = new(protobufJsonParseSettings);

            /// <summary>
            /// Name of the test vector
            /// </summary>
            private string name;

            /// <summary>
            /// Constructs a new test vector for deserialization by xUnit.
            /// </summary>
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable. -> Initialized during deserialization by xUnit
            public TestVector() { }
#pragma warning restore CS8618

            /// <summary>
            /// Constructs a new test vector.
            /// </summary>
            /// <param name="name">Name of the test vector</param>
            public TestVector(string name) => this.name = name;

            /// <summary>
            /// Request from <c>protoc</c>
            /// </summary>
            public CodeGeneratorRequest Request
            {
                get
                {
                    string resourceName = $"{name}.{requestFileExtension}";
                    using StreamReader reader = new(allRequestResources.GetResourceStream(resourceName) ?? throw new FileNotFoundException(resourceName));
                    return jsonParser.Parse<CodeGeneratorRequest>(reader);
                }
            }

            /// <summary>
            /// Expected response to <c>protoc</c>
            /// </summary>
            public CodeGeneratorResponse ExpectedResponse
            {
                get
                {
                    using StreamReader reader = new(allExpectedResponseResources.GetResourceStream($"{name}.{responseFileExtension}")!);
                    return jsonParser.Parse<CodeGeneratorResponse>(reader);
                }
            }

            public void Deserialize(IXunitSerializationInfo info) => name = info.GetValue<string>(nameof(name));

            public void Serialize(IXunitSerializationInfo info) => info.AddValue(nameof(name), name);

            public override string? ToString() => name;
        }

        /// <summary>
        /// All known test vectors
        /// </summary>
        public static IEnumerable<object[]> TestVectors => TestVectorNames.Select(name => new object[] { new TestVector(name) });

        /// <summary>
        /// <see cref="ProtocGenDelphi"/> handles a request from <c>protoc</c> with the expected response.
        /// </summary>
        /// <param name="vector">Test vector</param>
        [Theory]
        [MemberData(nameof(TestVectors))]
        public void ProducesExpectedResponse(TestVector vector) => Assert.Equal(vector.ExpectedResponse, new ProtocGenDelphi().HandleRequest(vector.Request));
    }
}
