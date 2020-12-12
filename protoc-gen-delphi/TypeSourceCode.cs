
using Work.Connor.Delphi;
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
namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi
{
    /// <summary>
    /// Aggregation of Delphi source code elements that represent a protobuf type.
    /// </summary>
    /// <remarks>
    /// The protobuf type is mapped to a Delphi type.
    /// </remarks>
    internal abstract class TypeSourceCode
    {
        /// <summary>
        /// Protobuf schema definition that this type is part of
        /// </summary>
        protected SchemaSourceCode Schema { get; }

        /// <summary>
        /// Representation of the message type that this type is nested in, absent if this is not a nested type
        /// </summary>
        public MessageTypeSourceCode? ContainerType { get; }

        /// <summary>
        /// Constructs Delphi source code representing a protobuf type.
        /// </summary>
        /// <param name="schema">Protobuf schema definition that this type is part of</param>
        /// <param name="containerType">Representation of the message type that this type is nested in, absent if this is not a nested type</param>
        protected TypeSourceCode(SchemaSourceCode schema, MessageTypeSourceCode? containerType)
        {
            Schema = schema;
            ContainerType = containerType;
        }

        /// <summary>
        /// Unqualified name of the protobuf type
        /// </summary>
        /// <remarks>
        /// Includes neither package nor container types.
        /// </remarks>
        public abstract string TypeName { get; }

        /// <summary>
        /// Container-qualified name of the protobuf type
        /// </summary>
        /// <remarks>
        /// Includes container types, but no package
        /// </remarks>
        public string ContainerQualifiedTypeName => ContainerType is null ? TypeName
                                                                          : $"{ContainerType.ContainerQualifiedTypeName}.{TypeName}";

        /// <summary>
        /// Unqualified name of the corresponding Delphi type
        /// </summary>
        /// <remarks>
        /// Includes neither unit nor container types.
        /// </remarks>
        public string DelphiTypeName => ProtocGenDelphi.ConstructDelphiTypeName(TypeName);

        /// <summary>
        /// Container-qualified name of the corresponding Delphi type
        /// </summary>
        /// <remarks>
        /// Includes container types, but no unit
        /// </remarks>
        public string ContainerQualifiedDelphiTypeName => ContainerType is null ? DelphiTypeName
                                                                                : $"{ContainerType.ContainerQualifiedDelphiTypeName}.{DelphiTypeName}";

        /// <summary>
        /// Constructs a Delphi identifier for referencing the type, fully qualifying it if required.
        /// </summary>
        /// <param name="referencingSchema">Schema from which the type is referenced</param>
        /// <returns>The Delphi type identifier</returns>
        public string ReferenceDelphiTypeName(SchemaSourceCode referencingSchema) => referencingSchema.Equals(Schema) ? ContainerQualifiedDelphiTypeName
                                                                                                                      : $"{Schema.DelphiUnitName}.{ContainerQualifiedDelphiTypeName}";

        /// <summary>
        /// Interface section declaration of the corresponding Delphi type
        /// </summary>
        public abstract InterfaceDeclaration InterfaceDeclaration { get; }

        /// <summary>
        /// Nested type declaration of the corresponding Delphi type
        /// </summary>
        public abstract NestedTypeDeclaration NestedTypeDeclaration { get; }
    }
}
