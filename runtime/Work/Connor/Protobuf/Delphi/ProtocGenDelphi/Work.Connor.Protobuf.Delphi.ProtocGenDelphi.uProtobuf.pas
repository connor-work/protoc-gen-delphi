/// Copyright 2025 Connor Erdmann (connor.work)
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

/// <summary>
/// Basic definitions used by Delphi code generated from Protobuf schema definitions using <c>protoc-gen-delphi</c>,
/// and by compatible runtime library implementations.
/// </summary>
/// <remarks>
/// Client code may need to reference this unit in order to use Protobuf default values or reflection features.
/// </remarks>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.uProtobuf;

{$INCLUDE Work.Connor.Delphi.CompilerFeatures.inc}

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.SysUtils;
{$ELSE}
  SysUtils;
{$ENDIF}

type
  /// <summary>
  /// Protobuf field number.
  /// </summary>
  /// <remarks>
  /// Each field in a message definition has a unique field number that identifies it in the Protobuf binary wire format.
  /// The number should not be changed once the message type is in use (to avoid breaking compatibility).
  /// You cannot use the field numbers 19000 through 19999 (<c>PROTOBUF_FIRST_RESERVED_FIELD_NUMBER</c> through <c>PROTOBUF_LAST_RESERVED_FIELD_NUMBER</c>),
  /// as they are reserved for the Protobuf implementation.
  /// </remarks>
  TProtobufFieldNumber = 1..536870911;

  /// <summary>
  /// Type that can hold any Protobuf enum value.
  /// </summary>
  TProtobufEnumFieldValue = System.Int32;

  // TODO contract
  TProtobufTypeUrl = UnicodeString;

  /// <summary>
  /// Indicates that Protobuf decoding failed since encoded data did not comply with the applicable format specification.
  /// </summary>
  EProtobufFormatViolation = class(Exception);

  /// <summary>
  /// Indicates that Protobuf decoding failed since a encoded message was not compatible with its expected schema.
  /// </summary>
  EProtobufSchemaViolation = class(Exception);

  /// <summary>
  /// Indicates that an operation failed because it encountered an unknown Protobuf message type.
  /// </summary>
  EProtobufUnknownMessageType = class(Exception);

  /// <summary>
  /// Indicates an invalid operation on a value representing a Protobuf entity.
  /// </summary>
  EProtobufInvalidOperation = class(Exception);

const
  /// <summary>
  /// First Protobuf field number that is reserved for the Protobuf implementation.
  /// </summary>
  PROTOBUF_FIRST_RESERVED_FIELD_NUMBER = TProtobufFieldNumber(19000);

  /// <summary>
  /// Last Protobuf field number that is reserved for the Protobuf implementation
  /// </summary>
  PROTOBUF_LAST_RESERVED_FIELD_NUMBER = TProtobufFieldNumber(19999);

  /// <summary>
  /// Default value for a Protobuf field of any numeric Protobuf type.
  /// </summary>
  PROTOBUF_DEFAULT_VALUE_NUMERIC = 0;

  /// <summary>
  /// Default value for a Protobuf field of the Protobuf type <c>double</c>.
  /// </summary>
  PROTOBUF_DEFAULT_VALUE_DOUBLE = System.Double(0.0);

  /// <summary>
  /// Default value for a Protobuf field of the Protobuf type <c>float</c>.
  /// </summary>
  PROTOBUF_DEFAULT_VALUE_FLOAT = System.Single(0.0);

  /// <summary>
  /// Default value for a Protobuf field of the Protobuf type <c>int32</c>.
  /// </summary>
  PROTOBUF_DEFAULT_VALUE_INT32 = System.Int32(PROTOBUF_DEFAULT_VALUE_NUMERIC);

  /// <summary>
  /// Default value for a Protobuf field of the Protobuf type <c>int64</c>.
  /// </summary>
  PROTOBUF_DEFAULT_VALUE_INT64 = System.Int64(PROTOBUF_DEFAULT_VALUE_NUMERIC);

  /// <summary>
  /// Default value for a Protobuf field of the Protobuf type <c>uint32</c>.
  /// </summary>
  PROTOBUF_DEFAULT_VALUE_UINT32 = System.UInt32(PROTOBUF_DEFAULT_VALUE_NUMERIC);

  /// <summary>
  /// Default value for a Protobuf field of the Protobuf type <c>uint64</c>.
  /// </summary>
  PROTOBUF_DEFAULT_VALUE_UINT64 = System.UInt64(PROTOBUF_DEFAULT_VALUE_NUMERIC);

  /// <summary>
  /// Default value for a Protobuf field of the Protobuf type <c>sint32</c>.
  /// </summary>
  PROTOBUF_DEFAULT_VALUE_SINT32 = System.Int32(PROTOBUF_DEFAULT_VALUE_NUMERIC);

  /// <summary>
  /// Default value for a Protobuf field of the Protobuf type <c>sint64</c>.
  /// </summary>
  PROTOBUF_DEFAULT_VALUE_SINT64 = System.Int64(PROTOBUF_DEFAULT_VALUE_NUMERIC);

  /// <summary>
  /// Default value for a Protobuf field of the Protobuf type <c>fixed32</c>.
  /// </summary>
  PROTOBUF_DEFAULT_VALUE_FIXED32 = System.UInt32(PROTOBUF_DEFAULT_VALUE_NUMERIC);

  /// <summary>
  /// Default value for a Protobuf field of the Protobuf type <c>fixed64</c>.
  /// </summary>
  PROTOBUF_DEFAULT_VALUE_FIXED64 = System.UInt64(PROTOBUF_DEFAULT_VALUE_NUMERIC);

  /// <summary>
  /// Default value for a Protobuf field of the Protobuf type <c>sfixed32</c>.
  /// </summary>
  PROTOBUF_DEFAULT_VALUE_SFIXED32 = System.Int32(PROTOBUF_DEFAULT_VALUE_NUMERIC);

  /// <summary>
  /// Default value for a Protobuf field of the Protobuf type <c>sfixed64</c>.
  /// </summary>
  PROTOBUF_DEFAULT_VALUE_SFIXED64 = System.Int64(PROTOBUF_DEFAULT_VALUE_NUMERIC);

  /// <summary>
  /// Default value for a Protobuf field of the Protobuf type <c>bool</c>.
  /// </summary>
  PROTOBUF_DEFAULT_VALUE_BOOL = System.Boolean(False);

  /// <summary>
  /// Default value for a Protobuf field of the Protobuf type <c>string</c>.
  /// </summary>
  PROTOBUF_DEFAULT_VALUE_STRING = System.UnicodeString('');

  /// <summary>
  /// Default value for a Protobuf field of the Protobuf type <c>bytes</c>.
  /// </summary>
  PROTOBUF_DEFAULT_VALUE_BYTES = nil;

  /// <summary>
  /// Default value for a Protobuf field of a Protobuf enum type
  /// </summary>
  PROTOBUF_DEFAULT_VALUE_ENUM = TProtobufEnumFieldValue(PROTOBUF_DEFAULT_VALUE_NUMERIC);

  /// <summary>
  /// Default value for a Protobuf field of a Protobuf message type.
  /// </summary>
  PROTOBUF_DEFAULT_VALUE_MESSAGE = nil;

  /// <summary>
  /// Default prefix of Protobuf type URLs.
  /// </summary>
  PROTOBUF_TYPE_URL_DEFAULT_PREFIX = 'type.googleapis.com/';

  /// <summary>
  /// Protobuf package that contains all Protobuf message types that are one of the well-known types.
  /// </summary>
  PROTOBUF_WELL_KNOWN_TYPES_PACKAGE = 'google.protobuf';

implementation

end.
