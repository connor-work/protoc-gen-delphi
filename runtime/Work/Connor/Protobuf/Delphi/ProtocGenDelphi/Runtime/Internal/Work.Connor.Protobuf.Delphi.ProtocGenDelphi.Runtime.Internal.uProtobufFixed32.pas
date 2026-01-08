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
/// Runtime support for the Protobuf scalar value type <c>fixed32</c>.
/// </summary>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufFixed32;

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
procedure EncodeProtobufFixed32(aDest: TStream; aValue: UInt32);

// TODO
procedure EncodeProtobufFixed32Field(aDest: TStream; aFieldNumber: TProtobufFieldNumber; aValue: UInt32);

// TODO
function DecodeProtobufFixed32(aSource: TStream; aRemainingLength: PUInt32): UInt32;

// TODO
function DecodeProtobufFixed32Field(aSource: TStream; aWireType: TProtobufWireType; aRemainingLength: PUInt32): UInt32;

// TODO
function CalculateProtobufFixed32FieldSize(aFieldNumber: TProtobufFieldNumber; aValue: UInt32): UInt32;

implementation

procedure EncodeProtobufFixed32(aDest: TStream; aValue: UInt32);
begin
  EncodeProtobufI32(aDest, aValue);
end;

// TODO support explicit presence?
procedure EncodeProtobufFixed32Field(aDest: TStream; aFieldNumber: TProtobufFieldNumber; aValue: UInt32);
begin
  if (aValue = PROTOBUF_DEFAULT_VALUE_FIXED32) then Exit;
  TProtobufTag.WithData(aFieldNumber, TProtobufWireType.I32).Encode(aDest);
  EncodeProtobufFixed32(aDest, aValue);
end;

function DecodeProtobufFixed32(aSource: TStream; aRemainingLength: PUInt32): UInt32;
begin
  result := DecodeProtobufI32(aSource, aRemainingLength);
end;

function DecodeProtobufFixed32Field(aSource: TStream; aWireType: TProtobufWireType; aRemainingLength: PUInt32): UInt32;
begin
  if (aWireType <> TProtobufWireType.I32) then raise EProtobufSchemaViolation.Create('Protobuf fixed32 field has unexpected wire type: ' + IntToStr(Ord(aWireType)));
  result := DecodeProtobufFixed32(aSource, aRemainingLength);
end;

// TODO support explicit presence?
function CalculateProtobufFixed32FieldSize(aFieldNumber: TProtobufFieldNumber; aValue: UInt32): UInt32;
begin
  if (aValue = PROTOBUF_DEFAULT_VALUE_FIXED32) then Exit(0);
  result := TProtobufTag.WithData(aFieldNumber, TProtobufWireType.I32).CalculateSize + SizeOf(aValue);
end;

end.
