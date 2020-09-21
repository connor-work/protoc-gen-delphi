program Decode;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

uses
  Com.GitHub.Pikaju.Protobuf.Delphi.Test.uProtobufTestUtility,
  uUint32,
  Classes,
  Windows;

var
  lStdIn: THandleStream;
  lMessage: TMessageX;
begin
  lStdIn := THandleStream.Create(GetStdHandle(STD_INPUT_HANDLE));

  lMessage := TMessageX.Create;
  lMessage.Decode(lStdIn);
  AssertTrue(lMessage.FieldX = 133742069, 'Uint32 is parsed correctly.');
  lMessage.Free;

  lStdIn.Free;
end.
