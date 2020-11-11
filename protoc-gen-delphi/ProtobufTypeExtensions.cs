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
    /// Extensions to protobuf types for Delphi source code production.
    /// </summary>
    internal static partial class ProtobufTypeExtensions
    {
        /// <summary>
        /// Unit reference for the Delphi <c>SysUtils</c> unit
        /// </summary>
        private static UnitReference SysUtilsReference => new UnitReference() { Unit = new UnitIdentifier() { Unit = "SysUtils", Namespace = { "System" } } };

        /// <summary>
        /// Determines the Delphi type identifier of the Delphi type that is used to represent the protobuf field contents of a specific protobuf field,
        /// when communicating with client code.
        /// </summary>
        /// <param name="field">The protobuf field</param>
        /// <param name="generator">Function that generates a Delphi type name for a protobuf message type or enumerated type name</param>
        /// <returns>Corresponding Delphi type identifier</returns>
        internal static string GetPublicDelphiType(this FieldDescriptorProto field, Func<string, string> generator) => field.Label switch
        {
            FieldDescriptorProto.Types.Label.Optional => GetPublicDelphiSingleValueType(field.Type, field.TypeName, generator),
            FieldDescriptorProto.Types.Label.Repeated => $"IProtobufRepeatedFieldValues<{GetPublicDelphiElementType(field.Type, field.TypeName, generator)}>",
            _ => throw new NotImplementedException()
        };

        /// <summary>
        /// Determines the Delphi type identifier of the Delphi type that is used to represent the protobuf field contents of a specific protobuf field,
        /// when communicating with internal (runtime) code.
        /// </summary>
        /// <param name="field">The protobuf field</param>
        /// <param name="generator">Function that generates a Delphi type name for a protobuf message type or enumerated type name</param>
        /// <returns>Corresponding Delphi type identifier</returns>
        internal static string GetPrivateDelphiType(this FieldDescriptorProto field, Func<string, string> generator) => field.Label switch
        {
            FieldDescriptorProto.Types.Label.Optional => GetPrivateDelphiSingleValueType(field.Type, field.TypeName, generator),
            FieldDescriptorProto.Types.Label.Repeated => GetDelphiRepeatedFieldSubclass(field.Type, field.TypeName, generator),
            _ => throw new NotImplementedException()
        };

        /// <summary>
        /// Provides the required Delphi-specific unit references for representing the protobuf field contents of a specific protobuf field,
        /// when communicating with client code. 
        /// </summary>
        /// <param name="field">The protobuf field</param>
        /// <returns>The Delphi unit references</returns>
        internal static IEnumerable<UnitReference> GetPublicDelphiDependencies(this FieldDescriptorProto field)
        {
            if (field.Type == FieldDescriptorProto.Types.Type.Bytes) yield return SysUtilsReference; // For TBytes
        }

        /// <summary>
        /// Provides the required Delphi-specific unit references for representing the protobuf field contents of a specific protobuf field,
        /// when communicating with internal (runtime) code. 
        /// </summary>
        /// <param name="field">The protobuf field</param>
        /// <returns>The Delphi unit references</returns>
        internal static IEnumerable<UnitReference> GetPrivateDelphiDependencies(this FieldDescriptorProto field)
        {
            if (field.Type == FieldDescriptorProto.Types.Type.Bytes) yield return SysUtilsReference; // For TBytes
        }

        /// <summary>
        /// Determines the Delphi identifier of the runtime instance of <c>TProtobufWireCodec<!<![CDATA[<T>]]></c> that is used for encoding
        /// and decoding values of a specific protobuf field type in the protobuf binary wire format.
        /// </summary>
        /// <param name="fieldType">The protobuf field type</param>
        /// <returns>Delphi identifier of the codec instance</returns>
        /// <remarks>This is not used for protobuf message types</remarks>
        internal static string GetDelphiWireCodec(this FieldDescriptorProto.Types.Type fieldType) => fieldType switch
        {
            FieldDescriptorProto.Types.Type.Double   => "gProtobufWireCodecDouble",
            FieldDescriptorProto.Types.Type.Float    => "gProtobufWireCodecFloat",
            FieldDescriptorProto.Types.Type.Int32    => "gProtobufWireCodecInt32",
            FieldDescriptorProto.Types.Type.Int64    => "gProtobufWireCodecInt64",
            FieldDescriptorProto.Types.Type.Uint32   => "gProtobufWireCodecUint32",
            FieldDescriptorProto.Types.Type.Uint64   => "gProtobufWireCodecUint64",
            FieldDescriptorProto.Types.Type.Sint32   => "gProtobufWireCodecSint32",
            FieldDescriptorProto.Types.Type.Sint64   => "gProtobufWireCodecSint64",
            FieldDescriptorProto.Types.Type.Fixed32  => "gProtobufWireCodecFixed32",
            FieldDescriptorProto.Types.Type.Fixed64  => "gProtobufWireCodecFixed64",
            FieldDescriptorProto.Types.Type.Sfixed32 => "gProtobufWireCodecSfixed32",
            FieldDescriptorProto.Types.Type.Sfixed64 => "gProtobufWireCodecSfixed64",
            FieldDescriptorProto.Types.Type.Bool     => "gProtobufWireCodecBool",
            FieldDescriptorProto.Types.Type.String   => "gProtobufWireCodecString",
            FieldDescriptorProto.Types.Type.Bytes    => "gProtobufWireCodecBytes",
            FieldDescriptorProto.Types.Type.Enum     => "gProtobufWireCodecEnum",
            _ => throw new NotImplementedException()
        };

        /// <summary>
        /// Determines the Delphi expression that can be used to set the default value for a specific protobuf field type.
        /// </summary>
        /// <param name="fieldType">The protobuf field type</param>
        /// <returns>Delphi default value expression</returns>
        internal static string GetDelphiDefaultValueExpression(this FieldDescriptorProto.Types.Type fieldType) => fieldType switch
        {
            FieldDescriptorProto.Types.Type.Double   => "PROTOBUF_DEFAULT_VALUE_DOUBLE",
            FieldDescriptorProto.Types.Type.Float    => "PROTOBUF_DEFAULT_VALUE_FLOAT",
            FieldDescriptorProto.Types.Type.Int32    => "PROTOBUF_DEFAULT_VALUE_INT32",
            FieldDescriptorProto.Types.Type.Int64    => "PROTOBUF_DEFAULT_VALUE_INT64",
            FieldDescriptorProto.Types.Type.Uint32   => "PROTOBUF_DEFAULT_VALUE_UINT32",
            FieldDescriptorProto.Types.Type.Uint64   => "PROTOBUF_DEFAULT_VALUE_UINT64",
            FieldDescriptorProto.Types.Type.Sint32   => "PROTOBUF_DEFAULT_VALUE_SINT32",
            FieldDescriptorProto.Types.Type.Sint64   => "PROTOBUF_DEFAULT_VALUE_SINT64",
            FieldDescriptorProto.Types.Type.Fixed32  => "PROTOBUF_DEFAULT_VALUE_FIXED32",
            FieldDescriptorProto.Types.Type.Fixed64  => "PROTOBUF_DEFAULT_VALUE_FIXED64",
            FieldDescriptorProto.Types.Type.Sfixed32 => "PROTOBUF_DEFAULT_VALUE_SFIXED32",
            FieldDescriptorProto.Types.Type.Sfixed64 => "PROTOBUF_DEFAULT_VALUE_SFIXED64",
            FieldDescriptorProto.Types.Type.Bool     => "PROTOBUF_DEFAULT_VALUE_BOOL",
            FieldDescriptorProto.Types.Type.String   => "PROTOBUF_DEFAULT_VALUE_STRING",
            FieldDescriptorProto.Types.Type.Bytes    => "[]",
            FieldDescriptorProto.Types.Type.Enum     => "PROTOBUF_DEFAULT_VALUE_ENUM",
            FieldDescriptorProto.Types.Type.Message  => "PROTOBUF_DEFAULT_VALUE_MESSAGE",
            _ => throw new NotImplementedException()
        };

        /// <summary>
        /// Determines the Delphi type identifier of the Delphi type that is used to represent single protobuf field values of a specific protobuf field type,
        /// when communicating with client code.
        /// </summary>
        /// <param name="fieldType">The protobuf field descriptor's type field value</param>
        /// <param name="fieldTypeName">The protobuf field descriptor's type name field value</param>
        /// <param name="generator">Function that generates a Delphi type name for a protobuf message type or enumerated type name</param>
        /// <returns>Corresponding Delphi type identifier</returns>
        internal static string GetPublicDelphiSingleValueType(this FieldDescriptorProto.Types.Type fieldType, string fieldTypeName, Func<string, string> generator) => fieldType switch
        {
            FieldDescriptorProto.Types.Type.Double   => "Double",
            FieldDescriptorProto.Types.Type.Float    => "Single",
            FieldDescriptorProto.Types.Type.Int32    => "Int32",
            FieldDescriptorProto.Types.Type.Int64    => "Int64",
            FieldDescriptorProto.Types.Type.Uint32   => "UInt32",
            FieldDescriptorProto.Types.Type.Uint64   => "UInt64",
            FieldDescriptorProto.Types.Type.Sint32   => "Int32",
            FieldDescriptorProto.Types.Type.Sint64   => "Int64",
            FieldDescriptorProto.Types.Type.Fixed32  => "UInt32",
            FieldDescriptorProto.Types.Type.Fixed64  => "UInt64",
            FieldDescriptorProto.Types.Type.Sfixed32 => "Int32",
            FieldDescriptorProto.Types.Type.Sfixed64 => "Int64",
            FieldDescriptorProto.Types.Type.Bool     => "Boolean",
            FieldDescriptorProto.Types.Type.String   => "UnicodeString",
            FieldDescriptorProto.Types.Type.Bytes    => "TBytes",
            FieldDescriptorProto.Types.Type.Enum     => generator.Invoke(fieldTypeName),
            FieldDescriptorProto.Types.Type.Message  => generator.Invoke(fieldTypeName),
            _ => throw new NotImplementedException()
        };

        /// <summary>
        /// Determines the Delphi type identifier of the Delphi type that is used to represent single protobuf field values of a specific protobuf field type,
        /// when communicating with internal (runtime) code.
        /// </summary>
        /// <param name="fieldType">The protobuf field descriptor's type field value</param>
        /// <param name="fieldTypeName">The protobuf field descriptor's type name field value</param>
        /// <param name="generator">Function that generates a Delphi type name for a protobuf message type or enumerated type name</param>
        /// <returns>Corresponding Delphi type identifier</returns>
        internal static string GetPrivateDelphiSingleValueType(this FieldDescriptorProto.Types.Type fieldType, string fieldTypeName, Func<string, string> generator) => fieldType switch
        {
            FieldDescriptorProto.Types.Type.Double   => "Double",
            FieldDescriptorProto.Types.Type.Float    => "Single",
            FieldDescriptorProto.Types.Type.Int32    => "Int32",
            FieldDescriptorProto.Types.Type.Int64    => "Int64",
            FieldDescriptorProto.Types.Type.Uint32   => "UInt32",
            FieldDescriptorProto.Types.Type.Uint64   => "UInt64",
            FieldDescriptorProto.Types.Type.Sint32   => "Int32",
            FieldDescriptorProto.Types.Type.Sint64   => "Int64",
            FieldDescriptorProto.Types.Type.Fixed32  => "UInt32",
            FieldDescriptorProto.Types.Type.Fixed64  => "UInt64",
            FieldDescriptorProto.Types.Type.Sfixed32 => "Int32",
            FieldDescriptorProto.Types.Type.Sfixed64 => "Int64",
            FieldDescriptorProto.Types.Type.Bool     => "Boolean",
            FieldDescriptorProto.Types.Type.String   => "UnicodeString",
            FieldDescriptorProto.Types.Type.Bytes    => "TBytes",
            FieldDescriptorProto.Types.Type.Enum     => "TProtobufEnumFieldValue",
            FieldDescriptorProto.Types.Type.Message  => generator.Invoke(fieldTypeName),
            _ => throw new NotImplementedException()
        };

        /// <summary>
        /// Determines the Delphi type identifier of the Delphi type that is used to represent protobuf field values within repeated fields, of a specific protobuf field type,
        /// when communicating with client code.
        /// </summary>
        /// <param name="fieldType">The protobuf field descriptor's type field value</param>
        /// <param name="fieldTypeName">The protobuf field descriptor's type name field value</param>
        /// <param name="generator">Function that generates a Delphi type name for a protobuf message type or enumerated type name</param>
        /// <returns>Corresponding Delphi type identifier</returns>
        internal static string GetPublicDelphiElementType(this FieldDescriptorProto.Types.Type fieldType, string fieldTypeName, Func<string, string> generator) => fieldType.GetPublicDelphiSingleValueType(fieldTypeName, generator);

        /// <summary>
        /// Determines the Delphi identifier of the subtype of <c>IProtobufRepeatedFieldValues<!<![CDATA[<T>]]></c> that represents repeated fields of
        /// a specific protobuf field type.
        /// </summary>
        /// <param name="fieldType">The protobuf field descriptor's type field value</param>
        /// <param name="fieldTypeName">The protobuf field descriptor's type name field value</param>
        /// <param name="generator">Function that generates a Delphi type name for a protobuf message type or enumerated type name</param>
        /// <returns>Delphi identifier of class</returns>
        internal static string GetDelphiRepeatedFieldSubclass(this FieldDescriptorProto.Types.Type fieldType, string fieldTypeName, Func<string, string> generator) => fieldType switch
        {
            FieldDescriptorProto.Types.Type.Double   => "TProtobufRepeatedDoubleFieldValues",
            FieldDescriptorProto.Types.Type.Float    => "TProtobufRepeatedFloatFieldValues",
            FieldDescriptorProto.Types.Type.Int32    => "TProtobufRepeatedInt32FieldValues",
            FieldDescriptorProto.Types.Type.Int64    => "TProtobufRepeatedInt64FieldValues",
            FieldDescriptorProto.Types.Type.Uint32   => "TProtobufRepeatedUint32FieldValues",
            FieldDescriptorProto.Types.Type.Uint64   => "TProtobufRepeatedUint64FieldValues",
            FieldDescriptorProto.Types.Type.Sint32   => "TProtobufRepeatedSint32FieldValues",
            FieldDescriptorProto.Types.Type.Sint64   => "TProtobufRepeatedSint64FieldValues",
            FieldDescriptorProto.Types.Type.Fixed32  => "TProtobufRepeatedFixed32FieldValues",
            FieldDescriptorProto.Types.Type.Fixed64  => "TProtobufRepeatedFixed64FieldValues",
            FieldDescriptorProto.Types.Type.Sfixed32 => "TProtobufRepeatedSfixed32FieldValues",
            FieldDescriptorProto.Types.Type.Sfixed64 => "TProtobufRepeatedSfixed64FieldValues",
            FieldDescriptorProto.Types.Type.Bool     => "TProtobufRepeatedBoolFieldValues",
            FieldDescriptorProto.Types.Type.String   => "TProtobufRepeatedStringFieldValues",
            FieldDescriptorProto.Types.Type.Bytes    => "TProtobufRepeatedBytesFieldValues",
            FieldDescriptorProto.Types.Type.Enum     => $"TProtobufRepeatedEnumField<{generator.Invoke(fieldTypeName)}>",
            FieldDescriptorProto.Types.Type.Message  => $"TProtobufRepeatedMessageFieldValues<{generator.Invoke(fieldTypeName)}>",
            _ => throw new NotImplementedException()
        };
    }
}
