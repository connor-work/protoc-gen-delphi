unit uMessages;

interface

uses
  Com.GitHub.Pikaju.Protobuf.Delphi.uMessage;

type
  TMessageX = class(TProtobufMessage)

  public
    constructor Create; override;

    procedure Clear; override;

    procedure Encode(aDest: TStream); override;

    procedure Decode(aSource: TStream); override;
  end;

type
  TMessageY = class(TProtobufMessage)

  public
    constructor Create; override;

    procedure Clear; override;

    procedure Encode(aDest: TStream); override;

    procedure Decode(aSource: TStream); override;
  end;

implementation

end.
