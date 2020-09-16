program Encode;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

uses
  uString;

var
  lStdOut: THandleStream;
  lMessage: TMessageX;
begin
  lStdOut := THandleStream.Create(GetStdHandle(STD_OUTPUT_HANDLE));

  lMessage := TMessageX.Create;
  lMessage.FieldX := 'スゲーデン so uberuhaputo';
  lMessage.Encode(lStdOut);
  lMessage.Free;

  lStdOut.Free;
end.
