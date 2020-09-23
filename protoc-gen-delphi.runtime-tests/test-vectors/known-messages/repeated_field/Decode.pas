program Decode;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

uses
  Com.GitHub.Pikaju.Protobuf.Delphi.Test.uProtobufTestUtility,
  uRepeatedField,
  Classes,
  Windows;

var
  lStdIn: THandleStream;
  lMessage: TMessageX;
begin
  lStdIn := THandleStream.Create(GetStdHandle(STD_INPUT_HANDLE));

  lMessage := TMessageX.Create;
  lMessage.DecodeDelimited(lStdIn);
  AssertTrue(lMessage.FieldX.Count = 9, 'The correct number of arguments is parsed.');
  AssertTrue(lMessage.FieldX[0] = 3, 'Index 0 is correctly decoded.');
  AssertTrue(lMessage.FieldX[1] = 1, 'Index 1 is correctly decoded.');
  AssertTrue(lMessage.FieldX[2] = 4, 'Index 2 is correctly decoded.');
  AssertTrue(lMessage.FieldX[3] = 1, 'Index 3 is correctly decoded.');
  AssertTrue(lMessage.FieldX[4] = 5, 'Index 4 is correctly decoded.');
  AssertTrue(lMessage.FieldX[5] = 9, 'Index 5 is correctly decoded.');
  AssertTrue(lMessage.FieldX[6] = 2, 'Index 6 is correctly decoded.');
  AssertTrue(lMessage.FieldX[7] = 6, 'Index 7 is correctly decoded.');
  AssertTrue(lMessage.FieldX[8] = 5, 'Index 8 is correctly decoded.');
  lMessage.Free;

  lStdIn.Free;
end.
