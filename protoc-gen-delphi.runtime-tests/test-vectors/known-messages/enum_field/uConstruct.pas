unit uConstruct;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  uEnumField;

type TConstructedMessage = TMessageX;

procedure ConstructMessage(aMessage: TConstructedMessage);

implementation

procedure ConstructMessage(aMessage: TMessageX);
begin
  aMessage.FieldX := EnumXValueX;
end;

end.
