unit uMessages;

interface

uses
  Com.GitHub.Pikaju.Protobuf.Delphi.uMessage;

type
  TMessageX = class(TProtobufMessage)
  public constructor Create; override;

  public destructor Destroy; override;

  public procedure Clear; override;

  public procedure Encode(aDest: TStream); override;

  public procedure Decode(aSource: TStream); override;
  end;

type
  TMessageY = class(TProtobufMessage)
  public constructor Create; override;

  public destructor Destroy; override;

  public procedure Clear; override;

  public procedure Encode(aDest: TStream); override;

  public procedure Decode(aSource: TStream); override;
  end;

implementation

constructor TMessageX.Create;
begin
end;

destructor TMessageX.Destroy;
begin
end;

procedure TMessageX.Clear;
begin
end;

procedure TMessageX.Encode(aDest: TStream);
begin
end;

procedure TMessageX.Decode(aSource: TStream);
begin
end;

end.
