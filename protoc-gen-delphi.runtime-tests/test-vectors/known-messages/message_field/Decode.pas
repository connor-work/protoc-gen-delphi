program Decode;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

uses
  Com.GitHub.Pikaju.Protobuf.Delphi.Test.uProtobufTestUtility,
  uMessageField,
  Classes,
  Windows;

var
  lStdIn: THandleStream;
  lMessage: TMessageY;
begin
  lStdIn := THandleStream.Create(GetStdHandle(STD_INPUT_HANDLE));

  lMessage := TMessageY.Create;
  AssertTrue(not Assigned(lMessage.FieldX), 'The embedded message starts off as nil.');
  lMessage.DecodeDelimited(lStdIn);
  AssertTrue(Assigned(lMessage.FieldX), 'The embedded message is not nil.');
  lMessage.Free;

  lStdIn.Free;
end.
