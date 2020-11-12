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
using Type = Google.Protobuf.Reflection.FieldDescriptorProto.Types.Type;

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi
{
    /// <summary>
    /// Extensions to protobuf types for Delphi source code production.
    /// </summary>
    public static partial class ProtobufTypeExtensions
    {
        /// <summary>
        /// Determines the Delphi identifier of the runtime instance of <c>TProtobufWireCodec<!<![CDATA[<T>]]></c> that is used for encoding
        /// and decoding values of a specific protobuf field type in the protobuf binary wire format.
        /// </summary>
        /// <param name="fieldType">The protobuf field type</param>
        /// <returns>Delphi identifier of the codec instance</returns>
        /// <remarks>This is not used for protobuf message types</remarks>
        public static string GetDelphiWireCodec(this Type fieldType) => fieldType switch
        {
            Type.Double   => "gProtobufWireCodecDouble",
            Type.Float    => "gProtobufWireCodecFloat",
            Type.Int32    => "gProtobufWireCodecInt32",
            Type.Int64    => "gProtobufWireCodecInt64",
            Type.Uint32   => "gProtobufWireCodecUint32",
            Type.Uint64   => "gProtobufWireCodecUint64",
            Type.Sint32   => "gProtobufWireCodecSint32",
            Type.Sint64   => "gProtobufWireCodecSint64",
            Type.Fixed32  => "gProtobufWireCodecFixed32",
            Type.Fixed64  => "gProtobufWireCodecFixed64",
            Type.Sfixed32 => "gProtobufWireCodecSfixed32",
            Type.Sfixed64 => "gProtobufWireCodecSfixed64",
            Type.Bool     => "gProtobufWireCodecBool",
            Type.String   => "gProtobufWireCodecString",
            Type.Bytes    => "gProtobufWireCodecBytes",
            Type.Enum     => "gProtobufWireCodecEnum",
            _ => throw new NotImplementedException()
        };

        /// <summary>
        /// Determines the Delphi expression that can be used to set the default value for a specific protobuf field type.
        /// </summary>
        /// <param name="fieldType">The protobuf field type</param>
        /// <returns>Delphi default value expression</returns>
        public static string GetDelphiDefaultValueExpression(this Type fieldType) => fieldType switch
        {
            Type.Double   => "PROTOBUF_DEFAULT_VALUE_DOUBLE",
            Type.Float    => "PROTOBUF_DEFAULT_VALUE_FLOAT",
            Type.Int32    => "PROTOBUF_DEFAULT_VALUE_INT32",
            Type.Int64    => "PROTOBUF_DEFAULT_VALUE_INT64",
            Type.Uint32   => "PROTOBUF_DEFAULT_VALUE_UINT32",
            Type.Uint64   => "PROTOBUF_DEFAULT_VALUE_UINT64",
            Type.Sint32   => "PROTOBUF_DEFAULT_VALUE_SINT32",
            Type.Sint64   => "PROTOBUF_DEFAULT_VALUE_SINT64",
            Type.Fixed32  => "PROTOBUF_DEFAULT_VALUE_FIXED32",
            Type.Fixed64  => "PROTOBUF_DEFAULT_VALUE_FIXED64",
            Type.Sfixed32 => "PROTOBUF_DEFAULT_VALUE_SFIXED32",
            Type.Sfixed64 => "PROTOBUF_DEFAULT_VALUE_SFIXED64",
            Type.Bool     => "PROTOBUF_DEFAULT_VALUE_BOOL",
            Type.String   => "PROTOBUF_DEFAULT_VALUE_STRING",
            Type.Bytes    => "[]",
            Type.Enum     => "PROTOBUF_DEFAULT_VALUE_ENUM",
            Type.Message  => "PROTOBUF_DEFAULT_VALUE_MESSAGE",
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
        public static string GetPublicDelphiSingleValueType(this Type fieldType, string fieldTypeName, Func<string, string> generator) => fieldType switch
        {
            Type.Double   => "Double",
            Type.Float    => "Single",
            Type.Int32    => "Int32",
            Type.Int64    => "Int64",
            Type.Uint32   => "UInt32",
            Type.Uint64   => "UInt64",
            Type.Sint32   => "Int32",
            Type.Sint64   => "Int64",
            Type.Fixed32  => "UInt32",
            Type.Fixed64  => "UInt64",
            Type.Sfixed32 => "Int32",
            Type.Sfixed64 => "Int64",
            Type.Bool     => "Boolean",
            Type.String   => "UnicodeString",
            Type.Bytes    => "TBytes",
            Type.Enum     => generator.Invoke(fieldTypeName),
            Type.Message  => generator.Invoke(fieldTypeName),
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
        public static string GetPrivateDelphiSingleValueType(this Type fieldType, string fieldTypeName, Func<string, string> generator) => fieldType switch
        {
            Type.Double   => "Double",
            Type.Float    => "Single",
            Type.Int32    => "Int32",
            Type.Int64    => "Int64",
            Type.Uint32   => "UInt32",
            Type.Uint64   => "UInt64",
            Type.Sint32   => "Int32",
            Type.Sint64   => "Int64",
            Type.Fixed32  => "UInt32",
            Type.Fixed64  => "UInt64",
            Type.Sfixed32 => "Int32",
            Type.Sfixed64 => "Int64",
            Type.Bool     => "Boolean",
            Type.String   => "UnicodeString",
            Type.Bytes    => "TBytes",
            Type.Enum     => "TProtobufEnumFieldValue",
            Type.Message  => generator.Invoke(fieldTypeName),
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
        public static string GetPublicDelphiElementType(this Type fieldType, string fieldTypeName, Func<string, string> generator) => fieldType.GetPublicDelphiSingleValueType(fieldTypeName, generator);

        /// <summary>
        /// Determines the Delphi identifier of the subtype of <c>IProtobufRepeatedFieldValues<!<![CDATA[<T>]]></c> that represents repeated fields of
        /// a specific protobuf field type.
        /// </summary>
        /// <param name="fieldType">The protobuf field descriptor's type field value</param>
        /// <param name="fieldTypeName">The protobuf field descriptor's type name field value</param>
        /// <param name="generator">Function that generates a Delphi type name for a protobuf message type or enumerated type name</param>
        /// <returns>Delphi identifier of class</returns>
        public static string GetDelphiRepeatedFieldSubclass(this Type fieldType, string fieldTypeName, Func<string, string> generator) => fieldType switch
        {
            Type.Double   => "TProtobufRepeatedDoubleFieldValues",
            Type.Float    => "TProtobufRepeatedFloatFieldValues",
            Type.Int32    => "TProtobufRepeatedInt32FieldValues",
            Type.Int64    => "TProtobufRepeatedInt64FieldValues",
            Type.Uint32   => "TProtobufRepeatedUint32FieldValues",
            Type.Uint64   => "TProtobufRepeatedUint64FieldValues",
            Type.Sint32   => "TProtobufRepeatedSint32FieldValues",
            Type.Sint64   => "TProtobufRepeatedSint64FieldValues",
            Type.Fixed32  => "TProtobufRepeatedFixed32FieldValues",
            Type.Fixed64  => "TProtobufRepeatedFixed64FieldValues",
            Type.Sfixed32 => "TProtobufRepeatedSfixed32FieldValues",
            Type.Sfixed64 => "TProtobufRepeatedSfixed64FieldValues",
            Type.Bool     => "TProtobufRepeatedBoolFieldValues",
            Type.String   => "TProtobufRepeatedStringFieldValues",
            Type.Bytes    => "TProtobufRepeatedBytesFieldValues",
            Type.Enum     => $"TProtobufRepeatedEnumField<{generator.Invoke(fieldTypeName)}>",
            Type.Message  => $"TProtobufRepeatedMessageFieldValues<{generator.Invoke(fieldTypeName)}>",
            _ => throw new NotImplementedException()
        };
    }
}
