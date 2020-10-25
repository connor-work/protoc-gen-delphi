unit uConstruct;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  uFields;

type TConstructedMessage = TMessageX;

procedure ConstructMessage(aMessage: TConstructedMessage);

implementation

procedure ConstructMessage(aMessage: TMessageX);
begin
  aMessage.FieldX := 133742069;
  aMessage.FieldY := 'スゲーデン so uberuhaputo';
end;

end.
