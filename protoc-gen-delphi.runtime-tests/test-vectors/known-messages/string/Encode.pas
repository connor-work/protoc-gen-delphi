program Encode;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

uses
  uString,
  Classes,
  SysUtils,
  Windows;

var
  lStdOut: THandleStream;
  lMessage: TMessageX;
begin
  lStdOut := THandleStream.Create(GetStdHandle(STD_OUTPUT_HANDLE));

  lMessage := TMessageX.Create;
  lMessage.FieldX := 'スゲーデン so uberuhaputo';
  lMessage.EncodeDelimited(lStdOut);
  lMessage.Free;

  lStdOut.Free;
end.
