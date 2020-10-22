unit uValidate;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  Com.GitHub.Pikaju.Protobuf.Delphi.Test.uProtobufTestUtility,
  uFields;

type TValidatedMessage = TMessageX;

procedure ValidateMessage(aMessage: TValidatedMessage);

implementation

procedure ValidateMessage(aMessage: TMessageX);
begin
  AssertTrue(aMessage.FieldX = 133742069, 'Int32 is parsed correctly.');
  AssertTrue(aMessage.FieldY = 'スゲーデン so uberuhaputo', 'String is parsed correctly.');
end;

end.
