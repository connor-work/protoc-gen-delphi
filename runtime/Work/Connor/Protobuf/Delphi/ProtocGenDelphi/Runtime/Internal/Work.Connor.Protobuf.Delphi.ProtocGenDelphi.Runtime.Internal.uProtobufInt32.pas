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
/// Runtime-internal support for the protobuf type <c>int32</c>.
/// </summary>
/// <remarks>
/// Generated code needs to reference this unit in order to operate on protobuf field values of this type.
/// </remarks>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufInt32;

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
  /// protobuf fields of type <c>int32</c> from/to the protobuf binary wire format.
  /// </summary>
  gProtobufWireCodecInt32: IProtobufWireCodec<Int32>;

type
  /// <summary>
  /// Runtime library implementation of <see cref="T:IProtobufWireCodec"/> for the protobuf type <c>int32</c>.
  /// </summary>
  TProtobufInt32WireCodec = class(TProtobufVarintWireCodec<Int32>)
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

function TProtobufInt32WireCodec.FromUInt64(aValue: UInt64): Int32;
begin
  ValidateBounds(aValue, 32, True);
  result := Int32(aValue);
end;

function TProtobufInt32WireCodec.ToUInt64(aValue: Int32): UInt64;
begin
  result := UInt64(aValue);
end;

// TProtobufWireCodec<Int32> implementation

function TProtobufInt32WireCodec.GetDefault: Int32;
begin
  result := PROTOBUF_DEFAULT_VALUE_INT32;
end;

initialization
begin
  gProtobufWireCodecInt32 := TProtobufInt32WireCodec.Create;
end;

end.
