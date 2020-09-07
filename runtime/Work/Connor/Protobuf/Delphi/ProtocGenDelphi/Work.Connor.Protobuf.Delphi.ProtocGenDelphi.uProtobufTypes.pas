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

unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.uProtobufTypes;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

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

const
  /// <summary>
  /// First protobuf field number that is reserved for the protobuf implementation
  /// </summary>
  PROTOBUF_FIRST_RESERVED_FIELD_NUMBER: TProtobufFieldNumber = 19000;

  /// <summary>
  /// Last protobuf field number that is reserved for the protobuf implementation
  /// </summary>
  PROTOBUF_LAST_RESERVED_FIELD_NUMBER: TProtobufFieldNumber = 19999;

  /// <summary>
  /// Default value for a protobuf field of <c>uint32</c> protobuf type
  /// </summary>
  PROTOBUF_UINT32_DEFAULT_VALUE: UInt32 = 0;

  /// <summary>
  /// Default value for a protobuf field of <c>string</c> protobuf type
  /// </summary>
  PROTOBUF_STRING_DEFAULT_VALUE: UnicodeString = '';

implementation

end.
