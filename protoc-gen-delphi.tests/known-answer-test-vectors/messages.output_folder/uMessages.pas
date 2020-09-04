unit uMessages;

interface

uses
  Com.GitHub.Pikaju.Protobuf.Delphi.uMessage;

type
  TMessageX = class(TProtobufMessage)
  public constructor Create; override;

  public procedure Clear; override;

  public procedure Encode(aDest: TStream); override;

  public procedure Decode(aSource: TStream); override;
  end;

type
  TMessageY = class(TProtobufMessage)
  public constructor Create; override;

  public procedure Clear; override;

  public procedure Encode(aDest: TStream); override;

  public procedure Decode(aSource: TStream); override;
  end;

implementation

end.
