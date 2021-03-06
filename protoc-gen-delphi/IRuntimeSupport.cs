﻿/// Copyright 2020 Connor Roehricht (connor.work)
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
using Work.Connor.Delphi;
using Type = Google.Protobuf.Reflection.FieldDescriptorProto.Types.Type;

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi
{
    /// <summary>
    /// Provides support definitions for a runtime library that is compatible with <c>protoc-gen-delphi</c>.
    /// </summary>
    public interface IRuntimeSupport
    {
        /// <summary>
        /// Support definitions for a runtime library implementation implementing the default runtime API.
        /// </summary>
        public static readonly ReferenceRuntimeSupport Default = new();

        /// <summary>
        /// Provides the required unit reference for using compiled protobuf messages.
        /// </summary>
        /// <returns>The Delphi unit reference</returns>
        public UnitReference GetDependencyForMessages();

        /// <summary>
        /// Provides the required runtime-specific unit references for handling protobuf singular fields of a specific field type. 
        /// </summary>
        /// <param name="type">The protobuf field type</param>
        /// <returns>The Delphi unit references</returns>
        public IEnumerable<UnitReference> GetDependenciesForSingularFieldType(Type type);

        /// <summary>
        /// Provides the required runtime-specific unit references for handling protobuf repeated fields of a specific field type.
        /// </summary>
        /// <param name="type">The protobuf field type</param>
        /// <returns>The Delphi unit reference</returns>
        public IEnumerable<UnitReference> GetDependenciesForRepeatedFieldType(Type type);

        /// <summary>
        /// Provides support definitions for a runtime library implementation that follows the structure of the reference stub runtime,
        /// implementing the default runtime API.
        /// </summary>
        public class ReferenceRuntimeSupport : IRuntimeSupport
        {
            /// <summary>
            /// Delphi identifier of the runtime API's public namespace
            /// </summary>
            public string PublicNamespace => "Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime";

            /// <summary>
            /// Delphi identifier of the runtime API's internal namespace
            /// </summary>
            public string InternalNamespace => "Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal";

            /// <summary>
            /// Constructs a unit reference for an unqualified unit name in the runtime API's public namespace.
            /// </summary>
            /// <param name="unit">The unqualified unit name</param>
            /// <returns>The unit reference</returns>
            private UnitReference GetPublicUnitReference(string unit) => new()
            {
                Unit = new()
                {
                    Unit = unit,
                    Namespace = { PublicNamespace.Split(".") }
                }
            };

            /// <summary>
            /// Constructs a unit reference for an unqualified unit name in the runtime API' internal namespace.
            /// </summary>
            /// <param name="unit">The unqualified unit name</param>
            /// <returns>The unit reference</returns>
            private UnitReference GetInternalUnitReference(string unit) => new()
            {
                Unit = new()
                {
                    Unit = unit,
                    Namespace = { InternalNamespace.Split(".") }
                }
            };

            public UnitReference GetDependencyForMessages() => GetPublicUnitReference("uProtobufMessage");

            public IEnumerable<UnitReference> GetDependenciesForSingularFieldType(Type type)
            {
                yield return type switch
                {
                    Type.Double   => GetInternalUnitReference("uProtobufDouble"),
                    Type.Float    => GetInternalUnitReference("uProtobufFloat"),
                    Type.Int32    => GetInternalUnitReference("uProtobufInt32"),
                    Type.Int64    => GetInternalUnitReference("uProtobufInt64"),
                    Type.Uint32   => GetInternalUnitReference("uProtobufUint32"),
                    Type.Uint64   => GetInternalUnitReference("uProtobufUint64"),
                    Type.Sint32   => GetInternalUnitReference("uProtobufSint32"),
                    Type.Sint64   => GetInternalUnitReference("uProtobufSint64"),
                    Type.Fixed32  => GetInternalUnitReference("uProtobufFixed32"),
                    Type.Fixed64  => GetInternalUnitReference("uProtobufFixed64"),
                    Type.Sfixed32 => GetInternalUnitReference("uProtobufSfixed32"),
                    Type.Sfixed64 => GetInternalUnitReference("uProtobufSfixed64"),
                    Type.Bool     => GetInternalUnitReference("uProtobufBool"),
                    Type.String   => GetInternalUnitReference("uProtobufString"),
                    Type.Bytes    => GetInternalUnitReference("uProtobufBytes"),
                    Type.Enum     => GetInternalUnitReference("uProtobufEnum"),
                    Type.Message  => GetPublicUnitReference("uProtobufMessage"),
                    _ => throw new NotImplementedException()
                };
            }

            public IEnumerable<UnitReference> GetDependenciesForRepeatedFieldType(Type type)
            {
                yield return GetPublicUnitReference("uIProtobufRepeatedFieldValues");
                foreach (UnitReference dependency in GetDependenciesForSingularFieldType(type)) yield return dependency;
                yield return type switch
                {
                    Type.Double   => GetInternalUnitReference("uProtobufRepeatedDouble"),
                    Type.Float    => GetInternalUnitReference("uProtobufRepeatedFloat"),
                    Type.Int32    => GetInternalUnitReference("uProtobufRepeatedInt32"),
                    Type.Int64    => GetInternalUnitReference("uProtobufRepeatedInt64"),
                    Type.Uint32   => GetInternalUnitReference("uProtobufRepeatedUint32"),
                    Type.Uint64   => GetInternalUnitReference("uProtobufRepeatedUint64"),
                    Type.Sint32   => GetInternalUnitReference("uProtobufRepeatedSint32"),
                    Type.Sint64   => GetInternalUnitReference("uProtobufRepeatedSint64"),
                    Type.Fixed32  => GetInternalUnitReference("uProtobufRepeatedFixed32"),
                    Type.Fixed64  => GetInternalUnitReference("uProtobufRepeatedFixed64"),
                    Type.Sfixed32 => GetInternalUnitReference("uProtobufRepeatedSfixed32"),
                    Type.Sfixed64 => GetInternalUnitReference("uProtobufRepeatedSfixed64"),
                    Type.Bool     => GetInternalUnitReference("uProtobufRepeatedBool"),
                    Type.String   => GetInternalUnitReference("uProtobufRepeatedString"),
                    Type.Bytes    => GetInternalUnitReference("uProtobufRepeatedBytes"),
                    Type.Enum     => GetInternalUnitReference("uProtobufRepeatedEnum"),
                    Type.Message  => GetInternalUnitReference("uProtobufRepeatedMessage"),
                    _ => throw new NotImplementedException()
                };
            }
        }
    }
}
