unit uFields;

interface

uses
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.StubRuntime.uProtobufMessage,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.StubRuntime.uProtobufUint32;

type
  TMessageX = class(TProtobufMessage)
  const PROTOBUF_FIELD_NUMBER_FIELD_X = 1;

  const PROTOBUF_FIELD_NUMBER_FIELD_Y = 3;

  private var FFieldX: UInt32;

  public property FieldX: UInt32 read FFieldX write FFieldX;

  private var FFieldY: UInt32;

  public property FieldY: UInt32 read FFieldY write FFieldY;

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
  EncodeField<UInt32>(FFieldX, PROTOBUF_FIELD_NUMBER_FIELD_X, gProtobufWireCodecUInt32, aDest);
  EncodeField<UInt32>(FFieldY, PROTOBUF_FIELD_NUMBER_FIELD_Y, gProtobufWireCodecUInt32, aDest);
end;

procedure TMessageX.Decode(aSource: TStream);
begin
  inherited;
  FFieldX := DecodeUnknownField<UInt32>(PROTOBUF_FIELD_NUMBER_FIELD_X, gProtobufWireCodecUInt32);
  FFieldY := DecodeUnknownField<UInt32>(PROTOBUF_FIELD_NUMBER_FIELD_Y, gProtobufWireCodecUInt32);
end;

procedure TMessageX.ClearOwnFields;
begin
  FFieldX := PROTOBUF_UINT32_DEFAULT_VALUE;
  FFieldY := PROTOBUF_UINT32_DEFAULT_VALUE;
end;

end.
