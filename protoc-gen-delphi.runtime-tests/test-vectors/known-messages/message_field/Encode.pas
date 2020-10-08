program Encode;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

uses
  uMessageField,
  Classes,
  Windows;

var
  lStdOut: THandleStream;
  lMessage: TMessageY;
begin
  lStdOut := THandleStream.Create(GetStdHandle(STD_OUTPUT_HANDLE));

  lMessage := TMessageY.Create;
  lMessage.FieldX := TMessageX.Create;
  lMessage.EncodeDelimited(lStdOut);
  lMessage.Free;

  lStdOut.Free;
end.
