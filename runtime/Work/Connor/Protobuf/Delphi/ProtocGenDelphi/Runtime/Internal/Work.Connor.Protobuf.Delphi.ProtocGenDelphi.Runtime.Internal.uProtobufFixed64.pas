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
/// Runtime-internal support for the protobuf type <c>fixed64</c>.
/// </summary>
/// <remarks>
/// Generated code needs to reference this unit in order to operate on protobuf field values of this type.
/// </remarks>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufFixed64;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  // Runtime-internal support for the protobuf binary wire format
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uIProtobufWireCodec,
  // To implement TProtobufFixedWidthWireCodec<UInt64>
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufFixedWidthWireCodec,
  // For wire type
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufTag,
  // TStream for encoding of messages
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.Classes;
{$ELSE}
  Classes;
{$ENDIF}

var
  /// <summary>
  /// <i>Field codec</i> for <c>protoc-gen-delphi</c> that defines the encoding/decoding of
  /// protobuf fields of type <c>fixed64</c> from/to the protobuf binary wire format.
  /// </summary>
  gProtobufWireCodecFixed64: IProtobufWireCodec<UInt64>;

type
  /// <summary>
  /// Runtime library implementation of <see cref="T:IProtobufWireCodec"/> for the protobuf type <c>fixed64</c>.
  /// </summary>
  TProtobufFixed64WireCodec = class(TProtobufFixedWidthWireCodec<UInt64>)
    // TProtobufFixedWidthWireCodec<UInt64> implementation

    public
      function GetWireType: TProtobufWireType; override;
      function DecodeValue(aSource: TStream): UInt64; override;
      procedure EncodeValue(aValue: UInt64; aDest: TStream); override;

    // TProtobufWireCodec<UInt64> implementation
    
    public
      function GetDefault: UInt64; override;
      function IsDefault(aValue: UInt64): Boolean; override;
  end;

implementation

uses
  // For protobuf default values
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.uProtobuf;

// TProtobufFixedWidthWireCodec<UInt64> implementation

function TProtobufFixed64WireCodec.GetWireType: TProtobufWireType;
begin
  result := wt64Bit;
end;

function TProtobufFixed64WireCodec.DecodeValue(aSource: TStream): UInt64;
begin
  aSource.Read(result, SizeOf(result));
end;

procedure TProtobufFixed64WireCodec.EncodeValue(aValue: UInt64; aDest: TStream);
begin
  aDest.Write(aValue, SizeOf(aValue));
end;

// TProtobufWireCodec<UInt64> implementation

function TProtobufFixed64WireCodec.GetDefault: UInt64;
begin
  result := PROTOBUF_DEFAULT_VALUE_FIXED64;
end;

function TProtobufFixed64WireCodec.IsDefault(aValue: UInt64): Boolean;
begin
  result := aValue = GetDefault;
end;

initialization
begin
  gProtobufWireCodecFixed64 := TProtobufFixed64WireCodec.Create;
end;

end.
