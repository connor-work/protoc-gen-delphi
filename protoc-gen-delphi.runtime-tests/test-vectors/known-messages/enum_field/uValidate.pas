unit uValidate;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  Com.GitHub.Pikaju.Protobuf.Delphi.Test.uProtobufTestUtility,
  uEnumField;

type TValidatedMessage = TMessageX;

procedure ValidateMessage(aMessage: TValidatedMessage);

implementation

procedure ValidateMessage(aMessage: TMessageX);
begin
  AssertTrue(aMessage.FieldX = EnumXValueX, 'Enum value is parsed correctly.');
end;

end.
