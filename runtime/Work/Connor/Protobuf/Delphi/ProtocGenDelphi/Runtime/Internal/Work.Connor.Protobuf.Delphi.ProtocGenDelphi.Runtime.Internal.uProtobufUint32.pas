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
/// Runtime-internal support for the protobuf type <c>uint32</c>.
/// </summary>
/// <remarks>
/// Generated code needs to reference this unit in order to operate on protobuf field values of this type.
/// </remarks>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufUint32;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  // Runtime-internal support for the protobuf binary wire format
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uIProtobufWireCodec,
  // To implement TProtobufVarintWireCodec<UInt32>
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufVarintWireCodec;

var
  /// <summary>
  /// <i>Field codec</i> for <c>protoc-gen-delphi</c> that defines the encoding/decoding of
  /// protobuf fields of type <c>uint32</c> from/to the protobuf binary wire format.
  /// </summary>
  gProtobufWireCodecUint32: IProtobufWireCodec<UInt32>;

type
  /// <summary>
  /// Runtime library implementation of <see cref="T:IProtobufWireCodec"/> for the protobuf type <c>uint32</c>.
  /// </summary>
  TProtobufUint32WireCodec = class(TProtobufVarintWireCodec<UInt32>)
    // TProtobufVarintWireCodec<UInt32> implementation

    public
      function FromUInt64(aValue: UInt64): UInt32; override;
      function ToUInt64(aValue: UInt32): UInt64; override;

    // TProtobufWireCodec<UInt32> implementation
    
    public
      function GetDefault: UInt32; override;
  end;

implementation

uses
  // For protobuf default values
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.uProtobuf;

// TProtobufVarintWireCodec<UInt32> implementation

function TProtobufUint32WireCodec.FromUInt64(aValue: UInt64): UInt32;
begin
  ValidateBounds(aValue, 32, False);
  result := UInt32(aValue);
end;

function TProtobufUint32WireCodec.ToUInt64(aValue: UInt32): UInt64;
begin
  result := UInt64(aValue);
end;

// TProtobufWireCodec<UInt32> implementation

function TProtobufUint32WireCodec.GetDefault: UInt32;
begin
  result := PROTOBUF_DEFAULT_VALUE_UINT32;
end;

initialization
begin
  gProtobufWireCodecUint32 := TProtobufUint32WireCodec.Create;
end;

end.
