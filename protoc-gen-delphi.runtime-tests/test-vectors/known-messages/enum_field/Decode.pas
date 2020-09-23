program Decode;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

uses
  Com.GitHub.Pikaju.Protobuf.Delphi.Test.uProtobufTestUtility,
  uEnumField,
  Classes,
  Windows;

var
  lStdIn: THandleStream;
  lMessage: TMessageX;
begin
  lStdIn := THandleStream.Create(GetStdHandle(STD_INPUT_HANDLE));

  lMessage := TMessageX.Create;
  lMessage.DecodeDelimited(lStdIn);
  AssertTrue(lMessage.FieldX = EnumXFieldX, 'Enum value is parsed correctly.');
  lMessage.Free;

  lStdIn.Free;
end.
