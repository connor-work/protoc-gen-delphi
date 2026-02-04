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
/// Runtime support for the Protobuf scalar value type <c>int32</c>.
/// </summary>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufInt32;

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
procedure EncodeProtobufInt32(aDest: TStream; aValue: Int32);

// TODO contract
procedure EncodeProtobufInt32Field(aDest: TStream; aFieldNumber: TProtobufFieldNumber; aValue: Int32);

// TODO contract
function DecodeProtobufInt32(aSource: TStream; aRemainingLength: PUInt32): Int32;

// TODO contract
function DecodeProtobufInt32Field(aSource: TStream; aWireType: TProtobufWireType; aRemainingLength: PUInt32): Int32;

// TODO contract
function CalculateProtobufInt32FieldSize(aFieldNumber: TProtobufFieldNumber; aValue: Int32): UInt32;

// TODO contract
function EncodeJsonProtobufInt32(aValue: Int32): TJSONNumber;

// TODO contract
procedure EncodeJsonProtobufInt32Field(aDest: TJSONObject; aFieldJsonName: UnicodeString; aValue: Int32);

// TODO contract
function DecodeJsonProtobufInt32(aSource: TJSONValue): Int32;

implementation

procedure EncodeProtobufInt32(aDest: TStream; aValue: Int32);
begin
  // TODO correct sign handling
  EncodeProtobufVarint(aDest, UInt64(aValue));
end;

// TODO explicit presence support
procedure EncodeProtobufInt32Field(aDest: TStream; aFieldNumber: TProtobufFieldNumber; aValue: Int32);
begin
  if (aValue = PROTOBUF_DEFAULT_VALUE_INT32) then Exit;
  TProtobufTag.WithData(aFieldNumber, TProtobufWireType.Varint).Encode(aDest);
  EncodeProtobufInt32(aDest, aValue);
end;

function DecodeProtobufInt32(aSource: TStream; aRemainingLength: PUInt32): Int32;
begin
  // TODO range check
  result := Int32(DecodeProtobufVarint(aSource, aRemainingLength));
end;

function DecodeProtobufInt32Field(aSource: TStream; aWireType: TProtobufWireType; aRemainingLength: PUInt32): Int32;
begin
  if (aWireType <> TProtobufWireType.Varint) then raise EProtobufSchemaViolation.Create('Protobuf int32 field has unexpected wire type: ' + IntToStr(Ord(aWireType)));
  result := DecodeProtobufInt32(aSource, aRemainingLength);
end;

// TODO explicit presence support
function CalculateProtobufInt32FieldSize(aFieldNumber: TProtobufFieldNumber; aValue: Int32): UInt32;
begin
  if (aValue = PROTOBUF_DEFAULT_VALUE_INT32) then Exit(0);
  result := TProtobufTag.WithData(aFieldNumber, TProtobufWireType.Varint).CalculateSize;
  if aValue < 0 then result := result + PROTOBUF_MAX_VARINT_SIZE
  else result := result + CalculateProtobufVarintSize(UInt64(aValue));
end;

function EncodeJsonProtobufInt32(aValue: Int32): TJSONNumber;
begin
  result := TJSONNumber.Create(aValue);
end;

// TODO explicit presence support
// TODO emit JSON default values
procedure EncodeJsonProtobufInt32Field(aDest: TJSONObject; aFieldJsonName: UnicodeString; aValue: Int32);
begin
  if (aValue = PROTOBUF_DEFAULT_VALUE_INT32) then Exit;
  aDest.AddPair(aFieldJsonName, EncodeJsonProtobufInt32(aValue));
end;

function DecodeJsonProtobufInt32(aSource: TJSONValue): Int32;
var
  lSource: TJSONString;
begin
  // "JSON value will be a decimal string. Either numbers or strings are accepted."
  if (not (aSource is TJSONString)) then raise EProtobufSchemaViolation.Create('Protobuf int32 JSON field has unexpected type: ' + aSource.ClassName);
  lSource := TJSONString(aSource);
  // TODO exponent notation support
  // TODO range check
  result := Int32(aSource.Value.ToInt64);
end;

end.
