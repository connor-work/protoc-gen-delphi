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

unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.uProtobuf;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  System.SysUtils;

type
  /// <summary>
  /// Protobuf field number
  /// </summary>
  /// <remarks>
  /// Each field in a message definition has a unique field number that identifies it in the message binary format.
  /// The number should not be changed once the message type is in use (to avoid breaking compatibility).
  /// You cannot use the field numbers 19000 through 19999 (<c>PROTOBUF_FIRST_RESERVED_FIELD_NUMBER</c> through <c>PROTOBUF_LAST_RESERVED_FIELD_NUMBER</c>),
  /// as they are reserved for the protobuf implementation.
  /// </remarks>
  TProtobufFieldNumber = 1..536870911;

  /// <summary>
  /// Type that can hold any protobuf enumerated value
  /// </summary>
  TProtobufEnumFieldValue = System.Int32;

const
  /// <summary>
  /// First protobuf field number that is reserved for the protobuf implementation
  /// </summary>
  PROTOBUF_FIRST_RESERVED_FIELD_NUMBER = TProtobufFieldNumber(19000);

  /// <summary>
  /// Last protobuf field number that is reserved for the protobuf implementation
  /// </summary>
  PROTOBUF_LAST_RESERVED_FIELD_NUMBER = TProtobufFieldNumber(19999);

  /// <summary>
  /// Default value for a protobuf field of any numeric protobuf type
  /// </summary>
  PROTOBUF_DEFAULT_VALUE_NUMERIC = 0;

  /// <summary>
  /// Default value for a protobuf field of <c>double</c> protobuf type
  /// </summary>
  PROTOBUF_DEFAULT_VALUE_DOUBLE = System.Double(PROTOBUF_DEFAULT_VALUE_NUMERIC);

  /// <summary>
  /// Default value for a protobuf field of <c>float</c> protobuf type
  /// </summary>
  PROTOBUF_DEFAULT_VALUE_FLOAT = System.Single(PROTOBUF_DEFAULT_VALUE_NUMERIC);

  /// <summary>
  /// Default value for a protobuf field of <c>int32</c> protobuf type
  /// </summary>
  PROTOBUF_DEFAULT_VALUE_INT32 = System.Int32(PROTOBUF_DEFAULT_VALUE_NUMERIC);

  /// <summary>
  /// Default value for a protobuf field of <c>int64</c> protobuf type
  /// </summary>
  PROTOBUF_DEFAULT_VALUE_INT64 = System.Int64(PROTOBUF_DEFAULT_VALUE_NUMERIC);

  /// <summary>
  /// Default value for a protobuf field of <c>uint32</c> protobuf type
  /// </summary>
  PROTOBUF_DEFAULT_VALUE_UINT32 = System.UInt32(PROTOBUF_DEFAULT_VALUE_NUMERIC);

  /// <summary>
  /// Default value for a protobuf field of <c>uint64</c> protobuf type
  /// </summary>
  PROTOBUF_DEFAULT_VALUE_UINT64 = System.UInt64(PROTOBUF_DEFAULT_VALUE_NUMERIC);

  /// <summary>
  /// Default value for a protobuf field of <c>sint32</c> protobuf type
  /// </summary>
  PROTOBUF_DEFAULT_VALUE_SINT32 = System.Int32(PROTOBUF_DEFAULT_VALUE_NUMERIC);

  /// <summary>
  /// Default value for a protobuf field of <c>sint64</c> protobuf type
  /// </summary>
  PROTOBUF_DEFAULT_VALUE_SINT64 = System.Int64(PROTOBUF_DEFAULT_VALUE_NUMERIC);

  /// <summary>
  /// Default value for a protobuf field of <c>fixed32</c> protobuf type
  /// </summary>
  PROTOBUF_DEFAULT_VALUE_FIXED32 = System.UInt32(PROTOBUF_DEFAULT_VALUE_NUMERIC);

  /// <summary>
  /// Default value for a protobuf field of <c>fixed64</c> protobuf type
  /// </summary>
  PROTOBUF_DEFAULT_VALUE_FIXED64 = System.UInt64(PROTOBUF_DEFAULT_VALUE_NUMERIC);

  /// <summary>
  /// Default value for a protobuf field of <c>sfixed32</c> protobuf type
  /// </summary>
  PROTOBUF_DEFAULT_VALUE_SFIXED32 = System.Int32(PROTOBUF_DEFAULT_VALUE_NUMERIC);

  /// <summary>
  /// Default value for a protobuf field of <c>sfixed64</c> protobuf type
  /// </summary>
  PROTOBUF_DEFAULT_VALUE_SFIXED64 = System.Int64(PROTOBUF_DEFAULT_VALUE_NUMERIC);

  /// <summary>
  /// Default value for a protobuf field of <c>bool</c> protobuf type
  /// </summary>
  PROTOBUF_DEFAULT_VALUE_BOOL = System.Boolean(False);

  /// <summary>
  /// Default value for a protobuf field of <c>string</c> protobuf type
  /// </summary>
  PROTOBUF_DEFAULT_VALUE_STRING = System.UnicodeString('');

  /// <summary>
  /// Default value for a protobuf field of <c>bytes</c> protobuf type
  /// </summary>
  PROTOBUF_DEFAULT_VALUE_BYTES = System.SysUtils.TBytes([]);

  /// <summary>
  /// Default value for a protobuf field of an enum type
  /// </summary>
  PROTOBUF_DEFAULT_VALUE_ENUM = TProtobufEnumFieldValue(PROTOBUF_DEFAULT_VALUE_NUMERIC);

  /// <summary>
  /// Default value for a protobuf field of a message type
  /// </summary>
  PROTOBUF_DEFAULT_VALUE_MESSAGE = nil;

implementation

end.
