unit uValidate;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  Com.GitHub.Pikaju.Protobuf.Delphi.Test.uProtobufTestUtility,
  uRepeatedField;

type TValidatedMessage = TMessageX;

procedure ValidateMessage(aMessage: TValidatedMessage);

implementation

procedure ValidateMessage(aMessage: TMessageX);
begin
  AssertTrue(aMessage.FieldX.Count = 9, 'The correct number of arguments is parsed.');
  AssertTrue(aMessage.FieldX[0] = 3, 'Index 0 is correctly decoded.');
  AssertTrue(aMessage.FieldX[1] = 1, 'Index 1 is correctly decoded.');
  AssertTrue(aMessage.FieldX[2] = 4, 'Index 2 is correctly decoded.');
  AssertTrue(aMessage.FieldX[3] = 1, 'Index 3 is correctly decoded.');
  AssertTrue(aMessage.FieldX[4] = 5, 'Index 4 is correctly decoded.');
  AssertTrue(aMessage.FieldX[5] = 9, 'Index 5 is correctly decoded.');
  AssertTrue(aMessage.FieldX[6] = 2, 'Index 6 is correctly decoded.');
  AssertTrue(aMessage.FieldX[7] = 6, 'Index 7 is correctly decoded.');
  AssertTrue(aMessage.FieldX[8] = 5, 'Index 8 is correctly decoded.');
end;

end.
