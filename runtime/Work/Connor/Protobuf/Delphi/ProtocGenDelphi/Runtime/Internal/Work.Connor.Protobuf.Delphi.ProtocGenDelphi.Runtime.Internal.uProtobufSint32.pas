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
/// Runtime-internal support for the protobuf type <c>sint32</c>.
/// </summary>
/// <remarks>
/// Generated code needs to reference this unit in order to operate on protobuf field values of this type.
/// </remarks>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufSint32;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  // Runtime-internal support for the protobuf binary wire format
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uIProtobufWireCodec,
  // To implement TProtobufVarintWireCodec<Int32>
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufVarintWireCodec;

var
  /// <summary>
  /// <i>Field codec</i> for <c>protoc-gen-delphi</c> that defines the encoding/decoding of
  /// protobuf fields of type <c>sint32</c> from/to the protobuf binary wire format.
  /// </summary>
  gProtobufWireCodecSint32: IProtobufWireCodec<Int32>;

type
  /// <summary>
  /// Runtime library implementation of <see cref="T:IProtobufWireCodec"/> for the protobuf type <c>sint32</c>.
  /// </summary>
  TProtobufSint32WireCodec = class(TProtobufVarintWireCodec<Int32>)
    // TProtobufVarintWireCodec<Int32> implementation

    public
      function FromUInt64(aValue: UInt64): Int32; override;
      function ToUInt64(aValue: Int32): UInt64; override;

    // TProtobufWireCodec<Int32> implementation
    
    public
      function GetDefault: Int32; override;
  end;

implementation

uses
  // For protobuf default values
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.uProtobuf;

// TProtobufVarintWireCodec<Int32> implementation

function TProtobufSint32WireCodec.FromUInt64(aValue: UInt64): Int32;
var
  lZigZagValue: UInt32;
begin
  ValidateBounds(aValue, 32, False);
  lZigZagValue := UInt32(aValue);
  result := (lZigZagValue shr 1) xor -(lZigZagValue and 1);
end;

function TProtobufSint32WireCodec.ToUInt64(aValue: Int32): UInt64;
var
  lZigZagValue: UInt32;
begin
  lZigZagValue := aValue shl 1;
  if (aValue < 0) then lZigZagValue := lZigZagValue xor -1;
  result := UInt64(lZigZagValue);
end;

// TProtobufWireCodec<Int32> implementation

function TProtobufSint32WireCodec.GetDefault: Int32;
begin
  result := PROTOBUF_DEFAULT_VALUE_SINT32;
end;

initialization
begin
  // RUNTIME-IMPL: Replace constructed class
  gProtobufWireCodecSint32 := TProtobufSint32WireCodec.Create;
end;

end.
