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

using Work.Connor.Delphi;

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi
{
    /// <summary>
    /// Provides support for a runtime library that is compatible with <c>protoc-gen-delphi</c>.
    /// </summary>
    interface IRuntimeSupport
    {
        /// <summary>
        /// Provides the required unit reference for using compiled protobuf messages.
        /// </summary>
        /// <returns>The Delphi unit reference</returns>
        public UnitReference GetMessageDependency();

        /// <summary>
        /// Support for the default runtime library implementation.
        /// </summary>
        public class Default : IRuntimeSupport
        {
            /// <summary>
            /// Constructs a unit reference for an unqualified unit name in the library's default namespace.
            /// </summary>
            /// <param name="unit">The unqualified unit name</param>
            /// <returns>The unit reference</returns>
            private UnitReference GetUnitReference(string unit) => new UnitReference()
            {
                Unit = new UnitIdentifier()
                {
                    Namespace = { "Com", "GitHub", "Pikaju", "Protobuf", "Delphi"},
                    Unit = unit
                }
            };

            public UnitReference GetMessageDependency() => GetUnitReference("uProtobufMessage");
        }
    }
}
