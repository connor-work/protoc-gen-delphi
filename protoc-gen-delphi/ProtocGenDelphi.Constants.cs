/// Copyright 2025 Connor Erdmann (connor.work)
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

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi
{
    public partial class ProtocGenDelphi
    {
        /// <summary>
        /// Name of the Delphi constant that contains the default prefix of Protobuf type URLs.
        /// </summary>
        public static string ProtobufTypeUrlDefaultPrefixDelphiConstantName => "PROTOBUF_TYPE_URL_DEFAULT_PREFIX";

        /// <summary>
        /// Name of the Delphi type that represents type URLs of Protobuf message types.
        /// </summary>
        public static string TypeUrlDelphiTypeName => "TProtobufTypeUrl";

        /// <summary>
        /// Name of the Delphi type that represents Protobuf tags.
        /// </summary>
        public static string TagDelphiTypeName => "TProtobufTag";

        /// <summary>
        /// Name of the Delphi record field that contains the field number of a Protobuf tag.
        /// </summary>
        public static string TagFieldNumberDelphiFieldName => "FieldNumber";

        /// <summary>
        /// Name of the Delphi record field that contains the wire type of a Protobuf tag.
        /// </summary>
        public static string TagWireTypeDelphiFieldName => "WireType";

        /// <summary>
        /// Delphi expression that refers to the global type registry object of the Protobuf runtime.
        /// </summary>
        public static string ProtobufRuntimeGlobalTypeRegistryDelphiObject => "TProtobufTypeRegistry.Global";

        /// <summary>
        /// Name of the Delphi method used to register the generated code for a Protobuf message type with the global type registry object of the Protobuf runtime.
        /// </summary>
        public static string ProtobufRuntimeRegisterNotWellKnownTypeDelphiMethodName => "RegisterNotWellKnownType";
    }
}
