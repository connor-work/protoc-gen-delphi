unit uConstruct;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  uString;

type TConstructedMessage = TMessageX;

procedure ConstructMessage(aMessage: TConstructedMessage);

implementation

procedure ConstructMessage(aMessage: TMessageX);
begin
  aMessage.FieldX := 'スゲーデン so uberuhaputo';
end;

end.
