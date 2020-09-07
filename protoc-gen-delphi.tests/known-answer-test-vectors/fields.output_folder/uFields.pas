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

  /// <summary>
  /// Creates an empty <see cref="TMessageX"/> that can be used as a protobuf message.
  /// Initially, all protobuf fields are absent, meaning that they are set to their default values.
  /// </summary>
  /// <remarks>
  /// Protobuf's interpretation of the absence of a field may be counterintuitive for Delphi developers.
  /// For a detailed explanation, see https://developers.google.com/protocol-buffers/docs/proto3#default.
  /// </remarks>
  public constructor Create; override;

  /// <summary>
  /// Destroys the instances and all objects and resources held by it, including the protobuf field values.
  /// </summary>
  /// <remarks>
  /// Developers must ensure that no shared ownership of current field values or further nested embedded objects is held.
  /// </remarks>
  public destructor Destroy; override;

  /// <summary>
  /// Renders all protobuf fields absent by setting them to their default values.
  /// </summary>
  /// <remarks>
  /// The resulting instance state is equivalent to a newly constructed <see cref="TMessageX"/>.
  /// For more details, see the documentation of <see cref="Create"/>.
  /// This procedure may cause the destruction of transitively owned objects.
  /// Developers must ensure that no shared ownership of current field values or further nested embedded objects is held.
  /// </remarks>
  public procedure Clear; override;

  /// <summary>
  /// Encodes the message using the protobuf binary wire format and writes it to a stream.
  /// </summary>
  /// <param name="aDest">The stream that the encoded message is written to</param>
  public procedure Encode(aDest: TStream); override;

  /// <summary>
  /// Fills the message's protobuf fields by decoding the message using the protobuf binary wire format from data that is read from a stream.
  /// </summary>
  /// <param name="aSource">The stream that the data is read from</param>
  /// <remarks>
  /// Protobuf fields that are not present in the read data are rendered absent by setting them to their default values.
  /// This may cause the destruction of transitively owned objects (this is also the case when a present fields overwrites a previous value)
  /// Developers must ensure that no shared ownership of current field values or further nested embedded objects is held.
  /// </remarks>
  public procedure Decode(aSource: TStream); override;

  /// <summary>
  /// Renders those protobuf fields absent that belong to <see cref="TMessageX"/> (i.e., are not managed by an ancestor class), by setting them to their default values.
  /// </summary>
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
