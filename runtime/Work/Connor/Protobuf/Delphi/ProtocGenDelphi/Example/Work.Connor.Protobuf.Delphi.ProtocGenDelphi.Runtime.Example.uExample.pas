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
  // TStream for encoding and decoding of messages in the protobuf binary wire format
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.Classes,
{$ELSE}
  Classes,
{$ENDIF}
  // TByteHelper to print bytes as their hexadecimal string representation
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.SysUtils,
{$ELSE}
  SysUtils,
{$ENDIF}
  // TProtobufMessage for generically handling protobuf messages
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uProtobufMessage,
  uExampleData;

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

procedure WriteWireFormatMessage(aMessage: TProtobufMessage);
var
  lStream: TMemoryStream;
begin
  lStream := TMemoryStream.Create;
  try
    aMessage.Encode(lStream);
    WriteStreamContent(lStream);
    lStream.Seek(0, soBeginning);
  finally
    lStream.Free;
  end;
end;

procedure RunExample();
var
  lMessageX: TMessageX;
  lStream: TMemoryStream;
begin
  lMessageX := TMessageX.Create;
  try
    lMessageX.FieldX := 42;
    lMessageX.FieldY := TMessageY.Create;
    lMessageX.FieldZ.EmplaceAdd;
    lMessageX.FieldZ.Add(21);
    Writeln('The example message, encoded using the protobuf binary wire format:');
    WriteWireFormatMessage(lMessageX);
    lStream := TMemoryStream.Create;
    try
      lMessageX.Encode(lStream);
      lStream.Seek(0, soBeginning);
      FreeAndNil(lMessageX);
      lMessageX := TMessageX.Create;
      lMessageX.Decode(lStream);
      Writeln('The example message, encoded using the protobuf binary wire format, after a wire format roundtrip:');
      WriteWireFormatMessage(lMessageX);
    finally
      lStream.Free;
    end;
  finally
    lMessageX.Free;
  end;
end;

end.
