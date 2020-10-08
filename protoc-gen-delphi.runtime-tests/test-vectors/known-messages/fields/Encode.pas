program Encode;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

uses
  uFields,
  Classes,
  Windows;

var
  lStdOut: THandleStream;
  lMessage: TMessageX;
begin
  lStdOut := THandleStream.Create(GetStdHandle(STD_OUTPUT_HANDLE));

  lMessage := TMessageX.Create;
  lMessage.FieldX := 133742069;
  lMessage.FieldY := 'スゲーデン so uberuhaputo';
  lMessage.EncodeDelimited(lStdOut);
  lMessage.Free;

  lStdOut.Free;
end.
