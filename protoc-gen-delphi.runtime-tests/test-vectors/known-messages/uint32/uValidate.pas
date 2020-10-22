unit uValidate;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  Com.GitHub.Pikaju.Protobuf.Delphi.Test.uProtobufTestUtility,
  uUint32;

type TValidatedMessage = TMessageX;

procedure ValidateMessage(aMessage: TValidatedMessage);

implementation

procedure ValidateMessage(aMessage: TMessageX);
begin
  AssertTrue(aMessage.FieldX = 133742069, 'Uint32 is parsed correctly.');
end;

end.
