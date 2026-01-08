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
/// Runtime support for the Protobuf scalar value type <c>bytes</c>.
/// </summary>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufBytes;

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
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uIProtobufMessage,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufWireFormat;

// TODO
procedure EncodeProtobufBytesField(aDest: TStream; aFieldNumber: TProtobufFieldNumber; aValue: TBytesStream);

// TODO
function DecodeProtobufBytesField(aSource: TStream; aWireType: TProtobufWireType; aRemainingLength: PUInt32): TBytes;

// TODO
function CalculateProtobufBytesFieldSize(aFieldNumber: TProtobufFieldNumber; aValue: TBytesStream): UInt32;

// TODO
function CalculateProtobufMessageBytesFieldSize(aFieldNumber: TProtobufFieldNumber; aMessage: IProtobufMessage): UInt32;

implementation

// TODO support explicit presence?
procedure EncodeProtobufBytesField(aDest: TStream; aFieldNumber: TProtobufFieldNumber; aValue: TBytesStream);
begin
  if (aValue.Size = 0) then Exit;
  TProtobufTag.WithData(aFieldNumber, TProtobufWireType.Len).Encode(aDest);
  EncodeProtobufVarint(aDest, aValue.Size);
  aDest.CopyFrom(aValue);
  aValue.Position := 0;
end;

function DecodeProtobufBytesField(aSource: TStream; aWireType: TProtobufWireType; aRemainingLength: PUInt32): TBytes;
var
  lLength: UInt32;
begin
  if (aWireType <> TProtobufWireType.Len) then raise EProtobufSchemaViolation.Create('Protobuf bytes field has unexpected wire type: ' + IntToStr(Ord(aWireType)));
  // TODO check range?
  lLength := DecodeProtobufVarint(aSource, aRemainingLength);
  SetLength(result, lLength);
  if ((aRemainingLength <> nil) and (aRemainingLength^ < lLength)) then raise EProtobufFormatViolation.Create('TODO');
  aSource.ReadBuffer(result[0], lLength);
  if (aRemainingLength <> nil) then aRemainingLength^ := aRemainingLength^ - lLength;
end;

// TODO support explicit presence?
function CalculateProtobufBytesFieldSize(aFieldNumber: TProtobufFieldNumber; aValue: TBytesStream): UInt32;
begin
  if (aValue.Size = 0) then Exit(0);
  result := TProtobufTag.WithData(aFieldNumber, TProtobufWireType.Len).CalculateSize + CalculateProtobufVarintSize(aValue.Size) + aValue.Size;
end;

function CalculateProtobufMessageBytesFieldSize(aFieldNumber: TProtobufFieldNumber; aMessage: IProtobufMessage): UInt32;
begin
  result := TProtobufTag.WithData(aFieldNumber, TProtobufWireType.Len).CalculateSize + CalculateProtobufVarintSize(aMessage.CalculateSize) + aMessage.CalculateSize;
end;

end.
