program Decode;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

uses
  Com.GitHub.Pikaju.Protobuf.Delphi.Test.uProtobufTestUtility,
  uString,
  Classes,
  Windows;

var
  lStdIn: THandleStream;
  lMessage: TMessageX;
begin
  lStdIn := THandleStream.Create(GetStdHandle(STD_INPUT_HANDLE));

  lMessage := TMessageX.Create;
  lMessage.Decode(lStdIn);
  AssertTrue(lMessage.FieldX = 'スゲーデン so uberuhaputo', 'String is parsed correctly.');
  AssertTrue(lMessage.FieldX = 'スゲーデン so uberuhaputos', 'String is parsed correctly.');
  lMessage.Free;

  lStdIn.Free;
end.
