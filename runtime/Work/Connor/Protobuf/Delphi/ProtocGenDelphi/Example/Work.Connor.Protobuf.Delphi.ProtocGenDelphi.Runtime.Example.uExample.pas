/// Copyright 2025 Connor Erdmann (connor.work)
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
/// Example for protoc-gen-delphi and its runtime library support code.
/// </summary>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Example.uExample;

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
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uAny,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uDuration,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uIProtobufMessage,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufMessageBase;
  // TODO // TProtobufMessage for generically handling protobuf messages
  // TODO Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uProtobufMessage,
  // TODO uExampleData;

/// <summary>
/// Runs the example, writing documentation to stdout.
/// </summary>
procedure RunExample();

implementation

procedure WriteStreamContent(aStream: TMemoryStream);
var
  lByte: Byte;
begin
  aStream.Seek(0, soBeginning);
  while (aStream.Position < aStream.Size) do
  begin
    aStream.ReadBuffer(lByte, 1);
    Write(lByte.ToHexString);
    Write(' ');
  end;
  Writeln;
end;

procedure WriteWireFormatMessage(aMessage: IProtobufMessage);
var
  lStream: TMemoryStream;
begin
  lStream := TMemoryStream.Create;
  try
    aMessage.Encode(lStream);
    WriteStreamContent(lStream);
  finally
    lStream.Free;
  end;
end;

// TODO clone method?

procedure TestWireFormatRoundtrip(aMessage: IProtobufMessage);
var
  lStream: TMemoryStream;
  lDecodedMessage: IProtobufMessage;
begin
  Writeln('The example message, encoded using the protobuf binary wire format:');
  WriteWireFormatMessage(aMessage);
  lStream := TMemoryStream.Create;
  try
    aMessage.Encode(lStream);
    lStream.Seek(0, soBeginning);
    lDecodedMessage := TProtobufMessageBase(TProtobufMessageBase(aMessage).ClassType.Create);
    lDecodedMessage.Decode(lStream);
    Writeln('The example message, encoded using the protobuf binary wire format, after a wire format roundtrip:');
    WriteWireFormatMessage(lDecodedMessage);
  finally
    lStream.Free;
  end;
end;

procedure RunExample();
var
  lDuration: TDuration;
  lAny: TAny;
begin
  try
    lDuration := TDuration.Create;
    lDuration.Seconds := 42;
    lDuration.SubSecondNanoseconds := 123456789;
    TestWireFormatRoundtrip(lDuration);
    lAny := TAny.Create;
    lAny.Message := lDuration;
    TestWireFormatRoundtrip(lAny);
  finally
    lAny.Free;
    lDuration.Free;
  end;
end;

end.
