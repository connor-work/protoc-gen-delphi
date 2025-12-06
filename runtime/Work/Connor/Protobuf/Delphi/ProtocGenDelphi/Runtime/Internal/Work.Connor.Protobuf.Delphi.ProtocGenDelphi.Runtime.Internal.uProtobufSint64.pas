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
/// Runtime-internal support for the protobuf type <c>sint64</c>.
/// </summary>
/// <remarks>
/// Generated code needs to reference this unit in order to operate on protobuf field values of this type.
/// </remarks>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufSint64;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  // Runtime-internal support for the protobuf binary wire format
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uIProtobufWireCodec,
  // To implement TProtobufVarintWireCodec<Int64>
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufVarintWireCodec;

var
  /// <summary>
  /// <i>Field codec</i> for <c>protoc-gen-delphi</c> that defines the encoding/decoding of
  /// protobuf fields of type <c>sint64</c> from/to the protobuf binary wire format.
  /// </summary>
  gProtobufWireCodecSint64: IProtobufWireCodec<Int64>;

type
  /// <summary>
  /// Runtime library implementation of <see cref="T:IProtobufWireCodec"/> for the protobuf type <c>sint64</c>.
  /// </summary>
  TProtobufSint64WireCodec = class(TProtobufVarintWireCodec<Int64>)
    // TProtobufVarintWireCodec<Int64> implementation

    public
      function FromUInt64(aValue: UInt64): Int64; override;
      function ToUInt64(aValue: Int64): UInt64; override;

    // TProtobufWireCodec<Int64> implementation
    
    public
      function GetDefault: Int64; override;
  end;

implementation

uses
  // For protobuf default values
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.uProtobuf;

// TProtobufVarintWireCodec<Int64> implementation

function TProtobufSint64WireCodec.FromUInt64(aValue: UInt64): Int64;
var
  lZigZagValue: UInt64;
begin
  ValidateBounds(aValue, 64, False);
  lZigZagValue := UInt64(aValue);
  result := (lZigZagValue shr 1) xor -(lZigZagValue and 1);
end;

function TProtobufSint64WireCodec.ToUInt64(aValue: Int64): UInt64;
var
  lZigZagValue: UInt64;
begin
  lZigZagValue := aValue shl 1;
  if (aValue < 0) then lZigZagValue := lZigZagValue xor -1;
  result := UInt64(lZigZagValue);
end;

// TProtobufWireCodec<Int64> implementation

function TProtobufSint64WireCodec.GetDefault: Int64;
begin
  result := PROTOBUF_DEFAULT_VALUE_SINT64;
end;

initialization
begin
  gProtobufWireCodecSint64 := TProtobufSint64WireCodec.Create;
end;

end.
