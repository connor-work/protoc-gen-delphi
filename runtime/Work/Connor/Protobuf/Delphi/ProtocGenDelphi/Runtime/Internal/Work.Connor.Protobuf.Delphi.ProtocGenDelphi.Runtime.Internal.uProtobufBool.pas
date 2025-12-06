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
/// Runtime-internal support for the protobuf type <c>bool</c>.
/// </summary>
/// <remarks>
/// Generated code needs to reference this unit in order to operate on protobuf field values of this type.
/// </remarks>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufBool;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  // Runtime-internal support for the protobuf binary wire format
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uIProtobufWireCodec,
  // To implement TProtobufVarintWireCodec<Boolean>
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufVarintWireCodec;

var
  /// <summary>
  /// <i>Field codec</i> for <c>protoc-gen-delphi</c> that defines the encoding/decoding of
  /// protobuf fields of type <c>bool</c> from/to the protobuf binary wire format.
  /// </summary>
  gProtobufWireCodecBool: IProtobufWireCodec<Boolean>;

type
  /// <summary>
  /// Runtime library implementation of <see cref="T:IProtobufWireCodec"/> for the protobuf type <c>bool</c>.
  /// </summary>
  TProtobufBoolWireCodec = class(TProtobufVarintWireCodec<Boolean>)
    // TProtobufVarintWireCodec<Boolean> implementation

    public
      function FromUInt64(aValue: UInt64): Boolean; override;
      function ToUInt64(aValue: Boolean): UInt64; override;

    // TProtobufWireCodec<Boolean> implementation
    
    public
      function GetDefault: Boolean; override;
  end;

implementation

uses
  // For protobuf default values
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.uProtobuf;

// TProtobufVarintWireCodec<Boolean> implementation

function TProtobufBoolWireCodec.FromUInt64(aValue: UInt64): Boolean;
begin
  ValidateBounds(aValue, 1, False);
  result := Boolean(aValue);
end;

function TProtobufBoolWireCodec.ToUInt64(aValue: Boolean): UInt64;
begin
  result := UInt64(aValue);
end;

// TProtobufWireCodec<Boolean> implementation

function TProtobufBoolWireCodec.GetDefault: Boolean;
begin
  result := PROTOBUF_DEFAULT_VALUE_BOOL;
end;

initialization
begin
  gProtobufWireCodecBool := TProtobufBoolWireCodec.Create;
end;

end.
