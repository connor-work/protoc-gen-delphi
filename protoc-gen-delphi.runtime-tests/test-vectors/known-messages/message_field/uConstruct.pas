unit uConstruct;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  uMessageField;

type TConstructedMessage = TMessageY;

procedure ConstructMessage(aMessage: TConstructedMessage);

implementation

procedure ConstructMessage(aMessage: TMessageY);
begin
  aMessage.FieldX := TMessageX.Create;
end;

end.
