unit uConstruct;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  uRepeatedField;

type TConstructedMessage = TMessageX;

procedure ConstructMessage(aMessage: TConstructedMessage);

implementation

procedure ConstructMessage(aMessage: TMessageX);
begin
  aMessage.FieldX.Add(3);
  aMessage.FieldX.Add(1);
  aMessage.FieldX.Add(4);
  aMessage.FieldX.Add(1);
  aMessage.FieldX.Add(5);
  aMessage.FieldX.Add(9);
  aMessage.FieldX.Add(2);
  aMessage.FieldX.Add(6);
  aMessage.FieldX.Add(5);
end;

end.
