program Decode;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

uses
  Com.GitHub.Pikaju.Protobuf.Delphi.Test.uProtobufTestUtility,
  uFields,
  Classes,
  Windows;

var
  lStdIn: THandleStream;
  lMessage: TMessageX;
begin
  lStdIn := THandleStream.Create(GetStdHandle(STD_INPUT_HANDLE));

  lMessage := TMessageX.Create;
  lMessage.DecodeDelimited(lStdIn);
  AssertTrue(lMessage.FieldX = 133742069, 'Int32 is parsed correctly.');
  AssertTrue(lMessage.FieldY = 'スゲーデン so uberuhaputo', 'String is parsed correctly.');
  lMessage.Free;

  lStdIn.Free;
end.
