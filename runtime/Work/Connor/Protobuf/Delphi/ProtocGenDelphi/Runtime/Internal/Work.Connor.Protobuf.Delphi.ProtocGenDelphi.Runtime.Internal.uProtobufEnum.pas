/// Copyright 2025 Connor Erdmann (connor.work)
/// Copyright 2020 Julien Scholz
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
/// Runtime-internal support for the protobuf enum types.
/// </summary>
/// <remarks>
/// Generated code needs to reference this unit in order to operate on protobuf field values of enum types.
/// </remarks>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufEnum;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  // Runtime-internal support for the protobuf binary wire format
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uIProtobufWireCodec,
  // Definition of TProtobufEnumFieldValue
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.uProtobuf,
  // To implement TProtobufVarintWireCodec<TProtobufEnumFieldValue>
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufVarintWireCodec;

var
  /// <summary>
  /// <i>Field codec</i> for <c>protoc-gen-delphi</c> that defines the encoding/decoding of
  /// protobuf fields of enum types from/to the protobuf binary wire format.
  /// </summary>
  gProtobufWireCodecEnum: IProtobufWireCodec<TProtobufEnumFieldValue>;

type
  /// <summary>
  /// Runtime library implementation of <see cref="T:IProtobufWireCodec"/> for protobuf enum types.
  /// </summary>
  TProtobufEnumWireCodec = class(TProtobufVarintWireCodec<TProtobufEnumFieldValue>)
    // TProtobufVarintWireCodec<TProtobufEnumFieldValue> implementation

    public
      function FromUInt64(aValue: UInt64): TProtobufEnumFieldValue; override;
      function ToUInt64(aValue: TProtobufEnumFieldValue): UInt64; override;

    // TProtobufWireCodec<T> implementation
    
    public
      function GetDefault: TProtobufEnumFieldValue; override;
  end;

implementation

// TProtobufVarintWireCodec<TProtobufEnumFieldValue> implementation

function TProtobufEnumWireCodec.FromUInt64(aValue: UInt64): TProtobufEnumFieldValue;
begin
  // "Enumerator constants must be in the range of a 32-bit integer. Since enum values
  // use varint encoding on the wire, negative values are inefficient and thus not recommended."
  // See: https://developers.google.com/protocol-buffers/docs/proto3#enum
  ValidateBounds(aValue, 32, True);
  result := TProtobufEnumFieldValue(aValue);
end;

function TProtobufEnumWireCodec.ToUInt64(aValue: TProtobufEnumFieldValue): UInt64;
begin
  result := UInt64(aValue);
end;

// TProtobufWireCodec<T> implementation

function TProtobufEnumWireCodec.GetDefault: TProtobufEnumFieldValue;
begin
  result := PROTOBUF_DEFAULT_VALUE_ENUM;
end;

initialization
begin
  gProtobufWireCodecEnum := TProtobufEnumWireCodec.Create;
end;

end.
