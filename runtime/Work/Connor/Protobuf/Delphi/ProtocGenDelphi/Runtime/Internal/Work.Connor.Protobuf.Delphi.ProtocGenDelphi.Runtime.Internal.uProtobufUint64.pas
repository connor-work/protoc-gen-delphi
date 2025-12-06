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
/// Runtime-internal support for the protobuf type <c>uint64</c>.
/// </summary>
/// <remarks>
/// Generated code needs to reference this unit in order to operate on protobuf field values of this type.
/// </remarks>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufUint64;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  // Runtime-internal support for the protobuf binary wire format
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uIProtobufWireCodec,
  // To implement TProtobufVarintWireCodec<UInt64>
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufVarintWireCodec;

var
  /// <summary>
  /// <i>Field codec</i> for <c>protoc-gen-delphi</c> that defines the encoding/decoding of
  /// protobuf fields of type <c>uint64</c> from/to the protobuf binary wire format.
  /// </summary>
  gProtobufWireCodecUint64: IProtobufWireCodec<UInt64>;

type
  /// <summary>
  /// Runtime library implementation of <see cref="T:IProtobufWireCodec"/> for the protobuf type <c>uint64</c>.
  /// </summary>
  TProtobufUint64WireCodec = class(TProtobufVarintWireCodec<UInt64>)
    // TProtobufVarintWireCodec<UInt64> implementation

    public
      function FromUInt64(aValue: UInt64): UInt64; override;
      function ToUInt64(aValue: UInt64): UInt64; override;

    // TProtobufWireCodec<UInt64> implementation
    
    public
      function GetDefault: UInt64; override;
  end;

implementation

uses
  // For protobuf default values
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.uProtobuf;

// TProtobufVarintWireCodec<UInt64> implementation

function TProtobufUint64WireCodec.FromUInt64(aValue: UInt64): UInt64;
begin
  ValidateBounds(aValue, 64, False);
  result := UInt64(aValue);
end;

function TProtobufUint64WireCodec.ToUInt64(aValue: UInt64): UInt64;
begin
  result := UInt64(aValue);
end;

// TProtobufWireCodec<UInt64> implementation

function TProtobufUint64WireCodec.GetDefault: UInt64;
begin
  result := PROTOBUF_DEFAULT_VALUE_UINT64;
end;

initialization
begin
  gProtobufWireCodecUint64 := TProtobufUint64WireCodec.Create;
end;

end.
