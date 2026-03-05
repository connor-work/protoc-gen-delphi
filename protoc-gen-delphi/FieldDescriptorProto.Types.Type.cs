
using Google.Protobuf.Reflection;
using System;

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
namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi;

/// <summary>
/// Extensions to <see cref="FieldDescriptorProto.Types.Type"/>.
/// </summary>
internal static partial class ExtFieldDescriptorProtoTypesType
{
    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="fieldType"></param>
    /// <returns></returns>
    public static string DefaultValueDelphiConstantName(this FieldDescriptorProto.Types.Type fieldType) => fieldType switch
    {
        FieldDescriptorProto.Types.Type.Double => "PROTOBUF_DEFAULT_VALUE_DOUBLE",
        FieldDescriptorProto.Types.Type.Float => "PROTOBUF_DEFAULT_VALUE_FLOAT",
        FieldDescriptorProto.Types.Type.Int64 => "PROTOBUF_DEFAULT_VALUE_INT64",
        FieldDescriptorProto.Types.Type.Uint64 => "PROTOBUF_DEFAULT_VALUE_UINT64",
        FieldDescriptorProto.Types.Type.Int32 => "PROTOBUF_DEFAULT_VALUE_INT32",
        FieldDescriptorProto.Types.Type.Fixed64 => "PROTOBUF_DEFAULT_VALUE_FIXED64",
        FieldDescriptorProto.Types.Type.Fixed32 => "PROTOBUF_DEFAULT_VALUE_FIXED32",
        FieldDescriptorProto.Types.Type.Bool => "PROTOBUF_DEFAULT_VALUE_BOOL",
        FieldDescriptorProto.Types.Type.String => "PROTOBUF_DEFAULT_VALUE_STRING",
        FieldDescriptorProto.Types.Type.Group => throw new NotImplementedException("TODO"),
        FieldDescriptorProto.Types.Type.Message => "PROTOBUF_DEFAULT_VALUE_MESSAGE",
        FieldDescriptorProto.Types.Type.Bytes => "PROTOBUF_DEFAULT_VALUE_BYTES",
        FieldDescriptorProto.Types.Type.Uint32 => "PROTOBUF_DEFAULT_VALUE_UINT32",
        FieldDescriptorProto.Types.Type.Enum => "PROTOBUF_DEFAULT_VALUE_ENUM",
        FieldDescriptorProto.Types.Type.Sfixed32 => "PROTOBUF_DEFAULT_VALUE_SFIXED32",
        FieldDescriptorProto.Types.Type.Sfixed64 => "PROTOBUF_DEFAULT_VALUE_SFIXED64",
        FieldDescriptorProto.Types.Type.Sint32 => "PROTOBUF_DEFAULT_VALUE_SINT32",
        FieldDescriptorProto.Types.Type.Sint64 => "PROTOBUF_DEFAULT_VALUE_SINT64",
        _ => throw new InvalidOperationException("TODO")
    };

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="fieldType"></param>
    /// <returns></returns>
    public static string DelphiSingularFieldType(this FieldDescriptorProto.Types.Type fieldType) => fieldType switch
    {
        FieldDescriptorProto.Types.Type.Double => "Double",
        FieldDescriptorProto.Types.Type.Float => "Single",
        FieldDescriptorProto.Types.Type.Int64 => "Int64",
        FieldDescriptorProto.Types.Type.Uint64 => "UInt64",
        FieldDescriptorProto.Types.Type.Int32 => "Int32",
        FieldDescriptorProto.Types.Type.Fixed64 => "UInt64",
        FieldDescriptorProto.Types.Type.Fixed32 => "UInt32",
        FieldDescriptorProto.Types.Type.Bool => "Boolean",
        FieldDescriptorProto.Types.Type.String => "UnicodeString",
        FieldDescriptorProto.Types.Type.Group => throw new NotImplementedException("TODO"),
        FieldDescriptorProto.Types.Type.Message => throw new NotImplementedException("TODO"),
        FieldDescriptorProto.Types.Type.Bytes => "TByteStream",
        FieldDescriptorProto.Types.Type.Uint32 => "UInt32",
        FieldDescriptorProto.Types.Type.Enum => throw new NotImplementedException("TODO"),
        FieldDescriptorProto.Types.Type.Sfixed32 => "Int32",
        FieldDescriptorProto.Types.Type.Sfixed64 => "Int64",
        FieldDescriptorProto.Types.Type.Sint32 => "Int32",
        FieldDescriptorProto.Types.Type.Sint64 => "Int64",
        _ => throw new InvalidOperationException("TODO")
    };

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="fieldType"></param>
    /// <returns></returns>
    public static string DelphiRepeatedFieldConcreteType(this FieldDescriptorProto.Types.Type fieldType) => fieldType switch
    {
        FieldDescriptorProto.Types.Type.Double => "TProtobufRepeatedDoubleFieldValues",
        FieldDescriptorProto.Types.Type.Float => "TProtobufRepeatedFloatFieldValues",
        FieldDescriptorProto.Types.Type.Int64 => "TProtobufRepeatedInt64FieldValues",
        FieldDescriptorProto.Types.Type.Uint64 => "TProtobufRepeatedUint64FieldValues",
        FieldDescriptorProto.Types.Type.Int32 => "TProtobufRepeatedInt32FieldValues",
        FieldDescriptorProto.Types.Type.Fixed64 => "TProtobufRepeatedFixed32FieldValues",
        FieldDescriptorProto.Types.Type.Fixed32 => "TProtobufRepeatedFixed32FieldValues",
        FieldDescriptorProto.Types.Type.Bool => "TProtobufRepeatedBoolFieldValues",
        FieldDescriptorProto.Types.Type.String => "TProtobufRepeatedStringFieldValues",
        FieldDescriptorProto.Types.Type.Group => throw new NotImplementedException("TODO"),
        FieldDescriptorProto.Types.Type.Message => throw new NotImplementedException("TODO"),
        FieldDescriptorProto.Types.Type.Bytes => "TProtobufRepeatedBytesFieldValues",
        FieldDescriptorProto.Types.Type.Uint32 => "TProtobufRepeatedUint32FieldValues",
        FieldDescriptorProto.Types.Type.Enum => throw new NotImplementedException("TODO"),
        FieldDescriptorProto.Types.Type.Sfixed32 => "TProtobufRepeatedSfixed32FieldValues",
        FieldDescriptorProto.Types.Type.Sfixed64 => "TProtobufRepeatedSfixed64FieldValues",
        FieldDescriptorProto.Types.Type.Sint32 => "TProtobufRepeatedSint32FieldValues",
        FieldDescriptorProto.Types.Type.Sint64 => "TProtobufRepeatedSint64FieldValues",
        _ => throw new InvalidOperationException("TODO")
    };

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="fieldType"></param>
    /// <returns></returns>
    public static string EncodeSingularFieldDelphiMethodName(this FieldDescriptorProto.Types.Type fieldType) => fieldType switch
    {
        FieldDescriptorProto.Types.Type.Double => "EncodeProtobufDoubleField",
        FieldDescriptorProto.Types.Type.Float => "EncodeProtobufFloatField",
        FieldDescriptorProto.Types.Type.Int64 => "EncodeProtobufInt64Field",
        FieldDescriptorProto.Types.Type.Uint64 => "EncodeProtobufUint64Field",
        FieldDescriptorProto.Types.Type.Int32 => "EncodeProtobufInt32Field",
        FieldDescriptorProto.Types.Type.Fixed64 => "EncodeProtobufFixed64Field",
        FieldDescriptorProto.Types.Type.Fixed32 => "EncodeProtobufFixed32Field",
        FieldDescriptorProto.Types.Type.Bool => "EncodeProtobufBoolField",
        FieldDescriptorProto.Types.Type.String => "EncodeProtobufStringField",
        FieldDescriptorProto.Types.Type.Group => throw new NotImplementedException("TODO"),
        FieldDescriptorProto.Types.Type.Message => "EncodeProtobufMessageField",
        FieldDescriptorProto.Types.Type.Bytes => "EncodeProtobufBytesField",
        FieldDescriptorProto.Types.Type.Uint32 => "EncodeProtobufUint32Field",
        FieldDescriptorProto.Types.Type.Enum => "EncodeProtobufEnumField",
        FieldDescriptorProto.Types.Type.Sfixed32 => "EncodeProtobufSfixed32Field",
        FieldDescriptorProto.Types.Type.Sfixed64 => "EncodeProtobufSfixed64Field",
        FieldDescriptorProto.Types.Type.Sint32 => "EncodeProtobufSint32Field",
        FieldDescriptorProto.Types.Type.Sint64 => "EncodeProtobufSint64Field",
        _ => throw new InvalidOperationException("TODO")
    };

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="fieldType"></param>
    /// <returns></returns>
    public static string DecodeSingularFieldDelphiMethodName(this FieldDescriptorProto.Types.Type fieldType) => fieldType switch
    {
        FieldDescriptorProto.Types.Type.Double => "DecodeProtobufDoubleField",
        FieldDescriptorProto.Types.Type.Float => "DecodeProtobufFloatField",
        FieldDescriptorProto.Types.Type.Int64 => "DecodeProtobufInt64Field",
        FieldDescriptorProto.Types.Type.Uint64 => "DecodeProtobufUint64Field",
        FieldDescriptorProto.Types.Type.Int32 => "DecodeProtobufInt32Field",
        FieldDescriptorProto.Types.Type.Fixed64 => "DecodeProtobufFixed64Field",
        FieldDescriptorProto.Types.Type.Fixed32 => "DecodeProtobufFixed32Field",
        FieldDescriptorProto.Types.Type.Bool => "DecodeProtobufBoolField",
        FieldDescriptorProto.Types.Type.String => "DecodeProtobufStringField",
        FieldDescriptorProto.Types.Type.Group => throw new NotImplementedException("TODO"),
        FieldDescriptorProto.Types.Type.Message => throw new NotImplementedException("TODO"),
        FieldDescriptorProto.Types.Type.Bytes => "DecodeProtobufBytesField",
        FieldDescriptorProto.Types.Type.Uint32 => "DecodeProtobufUint32Field",
        FieldDescriptorProto.Types.Type.Enum => "DecodeProtobufEnumField",
        FieldDescriptorProto.Types.Type.Sfixed32 => "DecodeProtobufSfixed32Field",
        FieldDescriptorProto.Types.Type.Sfixed64 => "DecodeProtobufSfixed64Field",
        FieldDescriptorProto.Types.Type.Sint32 => "DecodeProtobufSint32Field",
        FieldDescriptorProto.Types.Type.Sint64 => "DecodeProtobufSint64Field",
        _ => throw new InvalidOperationException("TODO")
    };

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="fieldType"></param>
    /// <returns></returns>
    public static string CalculateSingularFieldSizeDelphiMethodName(this FieldDescriptorProto.Types.Type fieldType) => fieldType switch
    {
        FieldDescriptorProto.Types.Type.Double => "CalculateProtobufDoubleFieldSize",
        FieldDescriptorProto.Types.Type.Float => "CalculateProtobufFloatFieldSize",
        FieldDescriptorProto.Types.Type.Int64 => "CalculateProtobufInt64FieldSize",
        FieldDescriptorProto.Types.Type.Uint64 => "CalculateProtobufUint64FieldSize",
        FieldDescriptorProto.Types.Type.Int32 => "CalculateProtobufInt32FieldSize",
        FieldDescriptorProto.Types.Type.Fixed64 => "CalculateProtobufFixed64FieldSize",
        FieldDescriptorProto.Types.Type.Fixed32 => "CalculateProtobufFixed32FieldSize",
        FieldDescriptorProto.Types.Type.Bool => "CalculateProtobufBoolFieldSize",
        FieldDescriptorProto.Types.Type.String => "CalculateProtobufStringFieldSize",
        FieldDescriptorProto.Types.Type.Group => throw new NotImplementedException("TODO"),
        FieldDescriptorProto.Types.Type.Message => "CalculateProtobufMessageFieldSize",
        FieldDescriptorProto.Types.Type.Bytes => "CalculateProtobufBytesFieldSize",
        FieldDescriptorProto.Types.Type.Uint32 => "CalculateProtobufUint32FieldSize",
        FieldDescriptorProto.Types.Type.Enum => "CalculateProtobufEnumFieldSize",
        FieldDescriptorProto.Types.Type.Sfixed32 => "CalculateProtobufSfixed32FieldSize",
        FieldDescriptorProto.Types.Type.Sfixed64 => "CalculateProtobufSfixed64FieldSize",
        FieldDescriptorProto.Types.Type.Sint32 => "CalculateProtobufSint32FieldSize",
        FieldDescriptorProto.Types.Type.Sint64 => "CalculateProtobufSint64FieldSize",
        _ => throw new InvalidOperationException("TODO")
    };

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="fieldType"></param>
    /// <returns></returns>
    public static string CalculateRepeatedFieldSizeDelphiMethodName(this FieldDescriptorProto.Types.Type fieldType) => fieldType switch
    {
        FieldDescriptorProto.Types.Type.Double => "CalculateProtobufRepeatedDoubleFieldSize",
        FieldDescriptorProto.Types.Type.Float => "CalculateProtobufRepeatedFloatFieldSize",
        FieldDescriptorProto.Types.Type.Int64 => "CalculateProtobufRepeatedInt64FieldSize",
        FieldDescriptorProto.Types.Type.Uint64 => "CalculateProtobufRepeatedUint64FieldSize",
        FieldDescriptorProto.Types.Type.Int32 => "CalculateProtobufRepeatedInt32FieldSize",
        FieldDescriptorProto.Types.Type.Fixed64 => "CalculateProtobufRepeatedFixed64FieldSize",
        FieldDescriptorProto.Types.Type.Fixed32 => "CalculateProtobufRepeatedFixed32FieldSize",
        FieldDescriptorProto.Types.Type.Bool => "CalculateProtobufRepeatedBoolFieldSize",
        FieldDescriptorProto.Types.Type.String => "CalculateProtobufRepeatedStringFieldSize",
        FieldDescriptorProto.Types.Type.Group => throw new NotImplementedException("TODO"),
        FieldDescriptorProto.Types.Type.Message => "CalculateProtobufRepeatedMessageFieldSize",
        FieldDescriptorProto.Types.Type.Bytes => "CalculateProtobufRepeatedBytesFieldSize",
        FieldDescriptorProto.Types.Type.Uint32 => "CalculateProtobufRepeatedUint32FieldSize",
        FieldDescriptorProto.Types.Type.Enum => "CalculateProtobufRepeatedEnumFieldSize",
        FieldDescriptorProto.Types.Type.Sfixed32 => "CalculateProtobufRepeatedSfixed32FieldSize",
        FieldDescriptorProto.Types.Type.Sfixed64 => "CalculateProtobufRepeatedSfixed64FieldSize",
        FieldDescriptorProto.Types.Type.Sint32 => "CalculateProtobufRepeatedSint32FieldSize",
        FieldDescriptorProto.Types.Type.Sint64 => "CalculateProtobufRepeatedSint64FieldSize",
        _ => throw new InvalidOperationException("TODO")
    };

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="fieldType"></param>
    /// <returns></returns>
    public static string EncodeJsonSingularFieldDelphiMethodName(this FieldDescriptorProto.Types.Type fieldType) => fieldType switch
    {
        FieldDescriptorProto.Types.Type.Double => "EncodeJsonProtobufDoubleField",
        FieldDescriptorProto.Types.Type.Float => "EncodeJsonProtobufFloatField",
        FieldDescriptorProto.Types.Type.Int64 => "EncodeJsonProtobufInt64Field",
        FieldDescriptorProto.Types.Type.Uint64 => "EncodeJsonProtobufUint64Field",
        FieldDescriptorProto.Types.Type.Int32 => "EncodeJsonProtobufInt32Field",
        FieldDescriptorProto.Types.Type.Fixed64 => "EncodeJsonProtobufFixed64Field",
        FieldDescriptorProto.Types.Type.Fixed32 => "EncodeJsonProtobufFixed32Field",
        FieldDescriptorProto.Types.Type.Bool => "EncodeJsonProtobufBoolField",
        FieldDescriptorProto.Types.Type.String => "EncodeJsonProtobufStringField",
        FieldDescriptorProto.Types.Type.Group => throw new NotImplementedException("TODO"),
        FieldDescriptorProto.Types.Type.Message => "EncodeJsonProtobufMessageField",
        FieldDescriptorProto.Types.Type.Bytes => "EncodeJsonProtobufBytesField",
        FieldDescriptorProto.Types.Type.Uint32 => "EncodeJsonProtobufUint32Field",
        FieldDescriptorProto.Types.Type.Enum => "EncodeJsonProtobufEnumField",
        FieldDescriptorProto.Types.Type.Sfixed32 => "EncodeJsonProtobufSfixed32Field",
        FieldDescriptorProto.Types.Type.Sfixed64 => "EncodeJsonProtobufSfixed64Field",
        FieldDescriptorProto.Types.Type.Sint32 => "EncodeJsonProtobufSint32Field",
        FieldDescriptorProto.Types.Type.Sint64 => "EncodeJsonProtobufSint64Field",
        _ => throw new InvalidOperationException("TODO")
    };

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="fieldType"></param>
    /// <returns></returns>
    public static string EncodeJsonRepeatedFieldDelphiMethodName(this FieldDescriptorProto.Types.Type fieldType) => fieldType switch
    {
        FieldDescriptorProto.Types.Type.Double => "EncodeJsonProtobufRepeatedDoubleField",
        FieldDescriptorProto.Types.Type.Float => "EncodeJsonProtobufRepeatedFloatField",
        FieldDescriptorProto.Types.Type.Int64 => "EncodeJsonProtobufRepeatedInt64Field",
        FieldDescriptorProto.Types.Type.Uint64 => "EncodeJsonProtobufRepeatedUint64Field",
        FieldDescriptorProto.Types.Type.Int32 => "EncodeJsonProtobufRepeatedInt32Field",
        FieldDescriptorProto.Types.Type.Fixed64 => "EncodeJsonProtobufRepeatedFixed64Field",
        FieldDescriptorProto.Types.Type.Fixed32 => "EncodeJsonProtobufRepeatedFixed32Field",
        FieldDescriptorProto.Types.Type.Bool => "EncodeJsonProtobufRepeatedBoolField",
        FieldDescriptorProto.Types.Type.String => "EncodeJsonProtobufRepeatedStringField",
        FieldDescriptorProto.Types.Type.Group => throw new NotImplementedException("TODO"),
        FieldDescriptorProto.Types.Type.Message => "EncodeJsonProtobufRepeatedMessageField",
        FieldDescriptorProto.Types.Type.Bytes => "EncodeJsonProtobufRepeatedBytesField",
        FieldDescriptorProto.Types.Type.Uint32 => "EncodeJsonProtobufRepeatedUint32Field",
        FieldDescriptorProto.Types.Type.Enum => "EncodeJsonProtobufRepeatedEnumField",
        FieldDescriptorProto.Types.Type.Sfixed32 => "EncodeJsonProtobufRepeatedSfixed32Field",
        FieldDescriptorProto.Types.Type.Sfixed64 => "EncodeJsonProtobufRepeatedSfixed64Field",
        FieldDescriptorProto.Types.Type.Sint32 => "EncodeJsonProtobufRepeatedSint32Field",
        FieldDescriptorProto.Types.Type.Sint64 => "EncodeJsonProtobufRepeatedSint64Field",
        _ => throw new InvalidOperationException("TODO")
    };

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="fieldType"></param>
    /// <returns></returns>
    public static string DecodeJsonValueDelphiMethodName(this FieldDescriptorProto.Types.Type fieldType) => fieldType switch
    {
        FieldDescriptorProto.Types.Type.Double => "DecodeJsonProtobufDouble",
        FieldDescriptorProto.Types.Type.Float => "DecodeJsonProtobufFloat",
        FieldDescriptorProto.Types.Type.Int64 => "DecodeJsonProtobufInt64",
        FieldDescriptorProto.Types.Type.Uint64 => "DecodeJsonProtobufUint64",
        FieldDescriptorProto.Types.Type.Int32 => "DecodeJsonProtobufInt32",
        FieldDescriptorProto.Types.Type.Fixed64 => "DecodeJsonProtobufFixed64",
        FieldDescriptorProto.Types.Type.Fixed32 => "DecodeJsonProtobufFixed32",
        FieldDescriptorProto.Types.Type.Bool => "DecodeJsonProtobufBool",
        FieldDescriptorProto.Types.Type.String => "DecodeJsonProtobufString",
        FieldDescriptorProto.Types.Type.Group => throw new NotImplementedException("TODO"),
        FieldDescriptorProto.Types.Type.Message => throw new NotImplementedException("TODO"),
        FieldDescriptorProto.Types.Type.Bytes => "DecodeJsonProtobufBytes",
        FieldDescriptorProto.Types.Type.Uint32 => "DecodeJsonProtobufUint32",
        FieldDescriptorProto.Types.Type.Enum => "DecodeJsonProtobufEnum",
        FieldDescriptorProto.Types.Type.Sfixed32 => "DecodeJsonProtobufSfixed32",
        FieldDescriptorProto.Types.Type.Sfixed64 => "DecodeJsonProtobufSfixed64",
        FieldDescriptorProto.Types.Type.Sint32 => "DecodeJsonProtobufSint32",
        FieldDescriptorProto.Types.Type.Sint64 => "DecodeJsonProtobufSint64",
        _ => throw new InvalidOperationException("TODO")
    };
}
