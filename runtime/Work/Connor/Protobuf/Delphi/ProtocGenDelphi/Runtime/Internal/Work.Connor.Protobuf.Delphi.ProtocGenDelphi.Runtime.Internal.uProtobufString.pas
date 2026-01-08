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
/// Runtime support for the Protobuf scalar value type <c>string</c>.
/// </summary>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufString;

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
procedure EncodeProtobufStringField(aDest: TStream; aFieldNumber: TProtobufFieldNumber; aValue: UnicodeString);

// TODO
function DecodeProtobufStringField(aSource: TStream; aWireType: TProtobufWireType; aRemainingLength: PUInt32): UnicodeString;

// TODO
function CalculateProtobufStringFieldSize(aFieldNumber: TProtobufFieldNumber; aValue: UnicodeString): UInt32;

implementation

// TODO support explicit presence?
procedure EncodeProtobufStringField(aDest: TStream; aFieldNumber: TProtobufFieldNumber; aValue: UnicodeString);
var
  lBytes: TBytes;
begin
  if (aValue = PROTOBUF_DEFAULT_VALUE_STRING) then Exit;
  TProtobufTag.WithData(aFieldNumber, TProtobufWireType.Len).Encode(aDest);
  lBytes := TEncoding.UTF8.GetBytes(aValue);
  EncodeProtobufVarint(aDest, Length(lBytes));
  aDest.WriteBuffer(lBytes[0], Length(lBytes));
end;

function DecodeProtobufStringField(aSource: TStream; aWireType: TProtobufWireType; aRemainingLength: PUInt32): UnicodeString;
var
  lLength: UInt32;
  lBytes: TBytes;
begin
  if (aWireType <> TProtobufWireType.Len) then raise EProtobufSchemaViolation.Create('Protobuf bytes field has unexpected wire type: ' + IntToStr(Ord(aWireType)));
  // TODO check range?
  lLength := DecodeProtobufVarint(aSource, aRemainingLength);
  SetLength(lBytes, lLength);
  if ((aRemainingLength <> nil) and (aRemainingLength^ < lLength)) then raise EProtobufFormatViolation.Create('TODO');
  aSource.ReadBuffer(lBytes[0], lLength);
  if (aRemainingLength <> nil) then aRemainingLength^ := aRemainingLength^ - lLength;
  result := TEncoding.UTF8.GetString(lBytes);
end;

// TODO support explicit presence?
function CalculateProtobufStringFieldSize(aFieldNumber: TProtobufFieldNumber; aValue: UnicodeString): UInt32;
begin
  if (aValue = PROTOBUF_DEFAULT_VALUE_STRING) then Exit(0);
  result := TProtobufTag.WithData(aFieldNumber, TProtobufWireType.Len).CalculateSize + CalculateProtobufVarintSize(Length(aValue) * SizeOf(Char)) + Length(aValue) * SizeOf(Char);
end;

end.
