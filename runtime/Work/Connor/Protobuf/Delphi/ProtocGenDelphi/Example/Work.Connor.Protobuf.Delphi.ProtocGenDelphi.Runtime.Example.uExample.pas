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
  System.JSON,
{$ELSE}
  JSON,
{$ENDIF}
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.SysUtils,
{$ELSE}
  SysUtils,
{$ENDIF}
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uAny,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uDuration,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uIProtobufMessage,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufMessageBase,
  // TODO // TProtobufMessage for generically handling protobuf messages
  // TODO Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uProtobufMessage,
  uExampleData;

/// <summary>
/// Runs the example, writing documentation to stdout.
/// </summary>
procedure RunExample;

type
  ITestInterface1 = interface(IInterface)
    ['{EEE81F48-0172-4128-85EC-4274242DDE21}']
  end;

  TTestClass1 = class(TInterfacedPersistent, ITestInterface1)
    public destructor Destroy();
  end;

  TTestClass2 = class(TPersistent)
    var FClass1: ITestInterface1;

    public constructor Create();
  end;

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

procedure WriteJsonValue(aJsonValue: TJSONValue);
begin
  Write(aJsonValue.Format);
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

procedure WriteProtoJsonMessage(aMessage: IProtobufMessage);
var
  lJsonValue: TJSONValue;
begin
  lJsonValue := aMessage.EncodeJson;
  try
    WriteJsonValue(lJsonValue);
  finally
    lJsonValue.Free;
  end;
end;

// TODO clone method?

procedure TestWireFormatRoundtrip(aMessage: IProtobufMessage);
var
  lStream: TMemoryStream;
  lDecodedMessage: IProtobufMessage;
begin
  Writeln('The example message, encoded using the Protobuf binary wire format:');
  WriteWireFormatMessage(aMessage);
  lStream := TMemoryStream.Create;
  try
    aMessage.Encode(lStream);
    lStream.Seek(0, soBeginning);
    lDecodedMessage := TProtobufMessageBase(TProtobufMessageType(TProtobufMessageBase(aMessage).ClassType).Create);
    lDecodedMessage.Decode(lStream);
    Writeln('The example message, encoded using the Protobuf binary wire format, after a wire format roundtrip:');
    WriteWireFormatMessage(lDecodedMessage);
  finally
    lStream.Free;
  end;
end;

procedure TestProtoJsonRoundtrip(aMessage: IProtobufMessage);
var
  lJsonValue: TJSONValue;
  lDecodedMessage: IProtobufMessage;
begin
  Writeln('The example message, encoded using the ProtoJSON format:');
  WriteProtoJsonMessage(aMessage);
  lJsonValue := aMessage.EncodeJson;
  try
    lDecodedMessage := TProtobufMessageBase(TProtobufMessageBase(aMessage).ClassType.Create);
    lDecodedMessage.DecodeJson(lJsonValue);
    Writeln('The example message, encoded using the ProtoJSON format, after a ProtoJSON roundtrip:');
    WriteProtoJsonMessage(lDecodedMessage);
  finally
    lJsonValue.Free;
  end;
end;

procedure RunRoundtripExample(aMessage: IProtobufMessage);
var
  lAny: TAny;
begin
  TestWireFormatRoundtrip(aMessage);
  TestProtoJsonRoundtrip(aMessage);
  lAny := TAny.Create;
  try
    lAny.Message := aMessage;
    TestWireFormatRoundtrip(lAny);
    TestProtoJsonRoundtrip(lAny);
  finally
    lAny.Free;
  end;
end;

procedure RunDurationExample;
var
  lDuration: TDuration;
begin
  lDuration := TDuration.Create;
  try
    lDuration.Seconds := -42;
    lDuration.SubSecondNanoseconds := 123456789;
    RunRoundtripExample(lDuration);
  finally
    lDuration.Free;
  end;
end;

constructor TTestClass2.Create;
begin
  FClass1 := TTestClass1.Create;
end;

destructor TTestClass1.Destroy;
begin
  // TODO
end;

procedure RunExample;
var
  lMessageX: TMessageX;
  lTest: UInt64;
  lInterface1: ITestInterface1;
  lClass2: TTestClass2;
begin
  lInterface1 := TTestClass1.Create;
  lClass2 := TTestClass2.Create;
  lClass2.Free;
  lTest := 18446744073709551574;
  //lTest := lTest shr 63;
  lTest := (lTest shr 63) shr 7;
  lTest := 1 shr 7;
  lTest := 1 shr 1;
  lMessageX := TMessageX.Create;
  try
    lMessageX.FieldX := 42;
    lMessageX.FieldY := TMessageY.Create;
    // TODO encode empty message!
    RunRoundtripExample(lMessageX);
    RunDurationExample;
  finally
    lMessageX.Free;
  end;
end;

end.
