unit uValidate;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  Com.GitHub.Pikaju.Protobuf.Delphi.Test.uProtobufTestUtility,
  uMessageField;

type TValidatedMessage = TMessageY;

procedure ValidateMessage(aMessage: TValidatedMessage);

implementation

procedure ValidateMessage(aMessage: TMessageY);
begin
  AssertTrue(Assigned(aMessage.FieldX), 'The embedded message is not nil.');
end;

end.
