using Google.Protobuf;
using Google.Protobuf.Compiler;
using Google.Protobuf.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Tests
{
    /// <summary>
    /// Tests <see cref="ProtocGenDelphi"/> with known inputs and outputs.
    /// </summary>
    public class KnownAnswerTest
    {
        /// <summary>
        /// Formatter settings for encoding protobuf messages as JSON for test data. Can be used when creating new test vectors.
        /// </summary>
        public static readonly JsonFormatter.Settings protobufJsonFormatSettings = JsonFormatter.Settings.Default.WithFormatDefaultValues(false).WithFormatEnumsAsIntegers(false);

        /// <summary>
        /// Parser settings for decoding protobuf messages from JSON for test data
        /// </summary>
        public static readonly JsonParser.Settings protobufJsonParseSettings = JsonParser.Settings.Default.WithIgnoreUnknownFields(false);

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
        /// <see cref="ProtocGenDelphi"/> handles a request with the expected response.
        /// </summary>
        /// <param name="request">Incoming request from <c>protoc</c></param>
        /// <param name="expectedResponse">Expected response</param>
        [Theory]
        [MemberData(nameof(KnownResponses))]
        public void ProducesExpectedResponse(CodeGeneratorRequest request, CodeGeneratorResponse expectedResponse) => Assert.Equal(expectedResponse, new ProtocGenDelphi().HandleRequest(request));

        public static IEnumerable<object[]> KnownResponses()
        {
            JsonParser jsonParser = new JsonParser(protobufJsonParseSettings);
            Assembly assembly = Assembly.GetExecutingAssembly();
            // Resources contain pairs of JSON-encoded request and expected response messages
            foreach (string requestFileName in assembly.GetManifestResourceNames().Where(name => name.EndsWith(requestFileExtension)))
            {
                string responseFileName = requestFileName.Substring(0, requestFileName.Length - requestFileExtension.Length) + responseFileExtension;
                using StreamReader responseReader = new StreamReader(assembly.GetManifestResourceStream(responseFileName) ?? throw new FileNotFoundException(responseFileName));
                using StreamReader requestReader = new StreamReader(assembly.GetManifestResourceStream(requestFileName));
                yield return new object[] { jsonParser.Parse<CodeGeneratorRequest>(requestReader), jsonParser.Parse<CodeGeneratorResponse>(responseReader) };
            }
        }
    }
}
