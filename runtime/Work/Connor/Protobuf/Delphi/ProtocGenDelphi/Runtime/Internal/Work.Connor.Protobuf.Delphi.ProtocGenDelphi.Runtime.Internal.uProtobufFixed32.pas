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
  System.JSON,
{$ELSE}
  JSON,
{$ENDIF}
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.SysUtils,
{$ELSE}
  SysUtils,
{$ENDIF}
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.uProtobuf,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufWireFormat;

// TODO contract
procedure EncodeProtobufFixed32(aDest: TStream; aValue: UInt32);

// TODO contract
procedure EncodeProtobufFixed32Field(aDest: TStream; aFieldNumber: TProtobufFieldNumber; aValue: UInt32);

// TODO contract
function DecodeProtobufFixed32(aSource: TStream; aRemainingLength: PUInt32): UInt32;

// TODO contract
function DecodeProtobufFixed32Field(aSource: TStream; aWireType: TProtobufWireType; aRemainingLength: PUInt32): UInt32;

// TODO contract
function CalculateProtobufFixed32FieldSize(aFieldNumber: TProtobufFieldNumber; aValue: UInt32): UInt32;

// TODO contract
function EncodeJsonProtobufFixed32(aValue: UInt32): TJSONNumber;

// TODO contract
procedure EncodeJsonProtobufFixed32Field(aDest: TJSONObject; aFieldJsonName: UnicodeString; aValue: UInt32);

// TODO contract
function DecodeJsonProtobufFixed32(aSource: TJSONValue): UInt32;

implementation

procedure EncodeProtobufFixed32(aDest: TStream; aValue: UInt32);
begin
  EncodeProtobufI32(aDest, aValue);
end;

// TODO explicit presence support
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

// TODO explicit presence support
function CalculateProtobufFixed32FieldSize(aFieldNumber: TProtobufFieldNumber; aValue: UInt32): UInt32;
begin
  if (aValue = PROTOBUF_DEFAULT_VALUE_FIXED32) then Exit(0);
  result := TProtobufTag.WithData(aFieldNumber, TProtobufWireType.I32).CalculateSize + SizeOf(aValue);
end;

function EncodeJsonProtobufFixed32(aValue: UInt32): TJSONNumber;
begin
  result := TJSONNumber.Create(Int64(aValue));
end;

// TODO explicit presence support
// TODO emit JSON default values
procedure EncodeJsonProtobufFixed32Field(aDest: TJSONObject; aFieldJsonName: UnicodeString; aValue: UInt32);
begin
  if (aValue = PROTOBUF_DEFAULT_VALUE_FIXED32) then Exit;
  aDest.AddPair(aFieldJsonName, EncodeJsonProtobufFixed32(aValue));
end;

function DecodeJsonProtobufFixed32(aSource: TJSONValue): UInt32;
var
  lSource: TJSONString;
begin
  lSource := aSource as TJSONString;
  if (not Assigned(lSource)) then raise EProtobufSchemaViolation.Create('Protobuf fixed32 JSON field has unexpected type: ' + aSource.ClassName);
  // TODO exponent notation support
  // TODO range check
  result := UInt32(aSource.Value.ToInt64);
end;

end.
