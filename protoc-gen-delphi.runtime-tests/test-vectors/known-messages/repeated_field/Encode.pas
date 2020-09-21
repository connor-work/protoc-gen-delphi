program Encode;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

uses
  uRepeatedField,
  Classes,
  Windows;

var
  lStdOut: THandleStream;
  lMessage: TMessageX;
begin
  lStdOut := THandleStream.Create(GetStdHandle(STD_OUTPUT_HANDLE));

  lMessage := TMessageX.Create;
  lMessage.FieldX.Add(3);
  lMessage.FieldX.Add(1);
  lMessage.FieldX.Add(4);
  lMessage.FieldX.Add(1);
  lMessage.FieldX.Add(5);
  lMessage.FieldX.Add(9);
  lMessage.FieldX.Add(2);
  lMessage.FieldX.Add(6);
  lMessage.FieldX.Add(5);
  lMessage.Encode(lStdOut);
  lMessage.Free;

  lStdOut.Free;
end.
