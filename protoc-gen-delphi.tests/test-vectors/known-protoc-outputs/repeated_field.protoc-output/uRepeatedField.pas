/// <remarks>
/// This unit corresponds to the protobuf schema definition (.proto file) <c>repeated_field.proto</c>.
/// </remarks>
unit uRepeatedField;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.StubRuntime.uProtobufMessage,
  Classes,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.StubRuntime.uProtobufRepeatedField,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.StubRuntime.uProtobufUint32;

type
  /// <remarks>
  /// This class corresponds to the protobuf message type <c>MessageX</c>.
  /// </remarks>
  TMessageX = class(TProtobufMessage)
  /// <summary>
  /// Protobuf field number of the protobuf field <c>fieldX</c>.
  /// </summary>
  const PROTOBUF_FIELD_NUMBER_FIELD_X = 1;

  /// <summary>
  /// Holds the decoded value of the protobuf field <c>fieldX</c>.
  /// </summary>
  private var FFieldX: TProtobufRepeatedField<UInt32>;

  /// <summary>
  /// Getter for <see cref="FieldX"/>.
  /// </summary>
  /// <returns>The value of the protobuf field <c>fieldX</c></returns>
  /// <remarks>
  /// May be overridden. Overriders shall only add side-effects and must call the ancestor implementation.
  /// </remarks>
  protected function GetFieldX: TProtobufRepeatedField<UInt32>; virtual;

  /// <remarks>
  /// This property corresponds to the protobuf field <c>fieldX</c>.
  /// </remarks>
  public property FieldX: TProtobufRepeatedField<UInt32> read GetFieldX;

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
  FFieldX = TProtobufRepeatedField<UInt32>.Create;
  ClearOwnFields;
end;

destructor TMessageX.Destroy;
begin
  FFieldX.Free;
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
  EncodeRepeatedField<UInt32>(FFieldX, PROTOBUF_FIELD_NUMBER_FIELD_X, gProtobufWireCodecUint32, aDest);
end;

procedure TMessageX.Decode(aSource: TStream);
begin
  inherited;
  FFieldX.Clear;
  DecodeUnknownRepeatedField<UInt32>(PROTOBUF_FIELD_NUMBER_FIELD_X, gProtobufWireCodecUint32, FFieldX);
end;

procedure TMessageX.ClearOwnFields;
begin
  FFieldX.Clear;
end;

function TMessageX.GetFieldX: TProtobufRepeatedField<UInt32>;
begin
  result := FFieldX;
end;

end.
