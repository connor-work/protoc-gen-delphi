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

using Google.Protobuf.Reflection;
using System;
using System.Collections.Generic;
using Work.Connor.Delphi;

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi
{
    /// <summary>
    /// Provides support definitions for a runtime library that is compatible with <c>protoc-gen-delphi</c>.
    /// </summary>
    public interface IRuntimeSupport
    {
        /// <summary>
        /// Support definitions for the stub runtime library implementation.
        /// The stub runtime library is not functional and shall only be used as a reference for runtime library implementation as well as compilation tests.
        /// </summary>
        public static readonly ReferenceRuntimeSupport Stub = new ReferenceRuntimeSupport("Work.Connor.Protobuf.Delphi.ProtocGenDelphi.StubRuntime");

        /// <summary>
        /// Support definitions for the default runtime library implementation.
        /// The default runtime library is called <c>protobuf-delphi</c> and is maintained by <a href="https://github.com/pikaju">@pikaju</a> on <a href="https://github.com/pikaju/protobuf-delphi">GitHub</a>.
        /// </summary>
        public static readonly ReferenceRuntimeSupport Default = new ReferenceRuntimeSupport("Com.GitHub.Pikaju.Protobuf.Delphi");

        /// <summary>
        /// Provides the required unit reference for using compiled protobuf enums.
        /// </summary>
        /// <returns>The Delphi unit reference</returns>
        public UnitReference GetDependencyForEnums();

        /// <summary>
        /// Provides the required unit reference for using compiled protobuf messages.
        /// </summary>
        /// <returns>The Delphi unit reference</returns>
        public UnitReference GetDependencyForMessages();

        /// <summary>
        /// Provides the required unit references for handling protobuf repeated fields of a specific field type.
        /// </summary>
        /// <param name="type">The protobuf field type</param>
        /// <returns>The Delphi unit reference</returns>
        public IEnumerable<UnitReference> GetDependenciesForRepeatedFieldType(FieldDescriptorProto.Types.Type type);

        /// <summary>
        /// Provides the required unit reference for handling protobuf singular fields of a specific field type. 
        /// </summary>
        /// <param name="type">The protobuf field type</param>
        /// <returns>The Delphi unit reference</returns>
        public UnitReference GetDependencyForSingularFieldType(FieldDescriptorProto.Types.Type type);

        /// <summary>
        /// Provides support definitions for a runtime library implementation that follows the structure of the reference stub runtime.
        /// </summary>
        public class ReferenceRuntimeSupport : IRuntimeSupport
        {
            /// <summary>
            /// Constructs a new support definitionfor a runtime library implementation that follows the structure of the reference stub runtime.
            /// </summary>
            /// <param name="delphiNamespace">Delphi namespace identifier of the runtime library</param>
            public ReferenceRuntimeSupport(string delphiNamespace) => DelphiNamespace = delphiNamespace;

            /// <summary>
            /// Delphi namespace identifier of the runtime library
            /// </summary>
            public string DelphiNamespace { get; }

            /// <summary>
            /// Constructs a unit reference for an unqualified unit name in the library's default namespace.
            /// </summary>
            /// <param name="unit">The unqualified unit name</param>
            /// <returns>The unit reference</returns>
            private UnitReference GetUnitReference(string unit) => new UnitReference()
            {
                Unit = new UnitIdentifier()
                {
                    Unit = unit,
                    Namespace = { DelphiNamespace.Split(".") }
                }
            };

            public UnitReference GetDependencyForEnums() => GetUnitReference("uProtobufEnum");

            public UnitReference GetDependencyForMessages() => GetUnitReference("uProtobufMessage");

            public IEnumerable<UnitReference> GetDependenciesForRepeatedFieldType(FieldDescriptorProto.Types.Type type)
            {
                yield return GetUnitReference("uProtobufRepeatedField");
                yield return GetDependencyForSingularFieldType(type);
                yield return GetUnitReference(type switch
                {
                    FieldDescriptorProto.Types.Type.String => "uProtobufRepeatedString",
                    FieldDescriptorProto.Types.Type.Uint32 => "uProtobufRepeatedUint32",
                    FieldDescriptorProto.Types.Type.Bool => "uProtobufRepeatedBool",
                    FieldDescriptorProto.Types.Type.Enum => "uProtobufRepeatedEnum",
                    FieldDescriptorProto.Types.Type.Message => "uProtobufRepeatedMessage",
                    _ => throw new NotImplementedException()
                });
            }

            public UnitReference GetDependencyForSingularFieldType(FieldDescriptorProto.Types.Type type) => GetUnitReference(type switch
                {
                    FieldDescriptorProto.Types.Type.String => "uProtobufString",
                    FieldDescriptorProto.Types.Type.Uint32 => "uProtobufUint32",
                    FieldDescriptorProto.Types.Type.Bool => "uProtobufBool",
                    FieldDescriptorProto.Types.Type.Enum => "uProtobufEnum",
                    FieldDescriptorProto.Types.Type.Message => "uProtobufMessage",
                    _ => throw new NotImplementedException()
                });
        }
    }
}
