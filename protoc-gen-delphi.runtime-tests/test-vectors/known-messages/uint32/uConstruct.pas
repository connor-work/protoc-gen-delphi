unit uConstruct;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  uUint32;

type TConstructedMessage = TMessageX;

procedure ConstructMessage(aMessage: TConstructedMessage);

implementation

procedure ConstructMessage(aMessage: TMessageX);
begin
  aMessage.FieldX := 133742069;
end;

end.
