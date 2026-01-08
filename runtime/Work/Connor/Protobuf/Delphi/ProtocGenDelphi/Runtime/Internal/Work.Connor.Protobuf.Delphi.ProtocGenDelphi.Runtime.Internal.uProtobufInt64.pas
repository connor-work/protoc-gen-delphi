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
/// Runtime support for the Protobuf scalar value type <c>int64</c>.
/// </summary>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufInt64;

{$INCLUDE Work.Connor.Delphi.CompilerFeatures.inc}

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.Classes,
{$ELSE}
  Classes,
{$ENDIF}
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.SysUtils,
{$ELSE}
  SysUtils,
{$ENDIF}
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.uProtobuf,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufWireFormat;

// TODO
procedure EncodeProtobufInt64(aDest: TStream; aValue: Int64);

// TODO
procedure EncodeProtobufInt64Field(aDest: TStream; aFieldNumber: TProtobufFieldNumber; aValue: Int64);

// TODO
function DecodeProtobufInt64(aSource: TStream; aRemainingLength: PUInt32): Int64;

// TODO
function DecodeProtobufInt64Field(aSource: TStream; aWireType: TProtobufWireType; aRemainingLength: PUInt32): Int64;

// TODO
function CalculateProtobufInt64FieldSize(aFieldNumber: TProtobufFieldNumber; aValue: Int64): UInt32;

implementation

procedure EncodeProtobufInt64(aDest: TStream; aValue: Int64);
begin
  // TODO correct sign handling?
  EncodeProtobufVarint(aDest, UInt64(aValue));
end;

// TODO support explicit presence?
procedure EncodeProtobufInt64Field(aDest: TStream; aFieldNumber: TProtobufFieldNumber; aValue: Int64);
begin
  if (aValue = PROTOBUF_DEFAULT_VALUE_INT64) then Exit;
  TProtobufTag.WithData(aFieldNumber, TProtobufWireType.Varint).Encode(aDest);
  EncodeProtobufInt64(aDest, aValue);
end;

function DecodeProtobufInt64(aSource: TStream; aRemainingLength: PUInt32): Int64;
begin
  // TODO check range?
  result := Int64(DecodeProtobufVarint(aSource, aRemainingLength));
end;

function DecodeProtobufInt64Field(aSource: TStream; aWireType: TProtobufWireType; aRemainingLength: PUInt32): Int64;
begin
  if (aWireType <> TProtobufWireType.Varint) then raise EProtobufSchemaViolation.Create('Protobuf int64 field has unexpected wire type: ' + IntToStr(Ord(aWireType)));
  result := DecodeProtobufInt64(aSource, aRemainingLength);
end;

// TODO support explicit presence?
function CalculateProtobufInt64FieldSize(aFieldNumber: TProtobufFieldNumber; aValue: Int64): UInt32;
begin
  if (aValue = PROTOBUF_DEFAULT_VALUE_INT64) then Exit(0);
  // TODO correct sign handling?
  result := TProtobufTag.WithData(aFieldNumber, TProtobufWireType.Varint).CalculateSize + CalculateProtobufVarintSize(UInt64(aValue));
end;

end.
