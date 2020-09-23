program Encode;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

uses
  uEnumField,
  Classes,
  Windows;

var
  lStdOut: THandleStream;
  lMessage: TMessageX;
begin
  lStdOut := THandleStream.Create(GetStdHandle(STD_OUTPUT_HANDLE));

  lMessage := TMessageX.Create;
  lMessage.FieldX := EnumXValueX;
  lMessage.EncodeDelimited(lStdOut);
  lMessage.Free;

  lStdOut.Free;
end.
