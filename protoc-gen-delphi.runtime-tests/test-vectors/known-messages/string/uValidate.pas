unit uValidate;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  Com.GitHub.Pikaju.Protobuf.Delphi.Test.uProtobufTestUtility,
  uString;

type TValidatedMessage = TMessageX;

procedure ValidateMessage(aMessage: TValidatedMessage);

implementation

procedure ValidateMessage(aMessage: TMessageX);
begin
  AssertTrue(aMessage.FieldX = 'スゲーデン so uberuhaputo', 'String is parsed correctly.');
end;

end.
