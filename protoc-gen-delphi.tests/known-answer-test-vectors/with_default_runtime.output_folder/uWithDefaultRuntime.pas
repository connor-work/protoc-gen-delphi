unit uWithDefaultRuntime;

interface

uses
  Com.GitHub.Pikaju.Protobuf.Delphi.uProtobufMessage;

type
  TMessageX = class(TProtobufMessage)
  public constructor Create; override;

  public destructor Destroy; override;

  public procedure Clear; override;

  public procedure Encode(aDest: TStream); override;

  public procedure Decode(aSource: TStream); override;

  private procedure ClearOwnFields;
  end;

implementation

constructor TMessageX.Create;
begin
  inherited;
  ClearOwnFields;
end;

destructor TMessageX.Destroy;
begin
  inherited;
end;

procedure TMessageX.Clear;
begin
  inherited;
  ClearOwnFields;
end;

procedure TMessageX.Encode(aDest: TStream);
begin
  inherited;
end;

procedure TMessageX.Decode(aSource: TStream);
begin
  inherited;
end;

procedure TMessageX.ClearOwnFields;
begin
end;

end.
