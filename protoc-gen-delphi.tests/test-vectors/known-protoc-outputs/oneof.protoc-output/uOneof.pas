/// <remarks>
/// This unit corresponds to the protobuf schema definition (.proto file) <c>oneof.proto</c>.
/// </remarks>
unit uOneof;

{$INCLUDE Work.Connor.Delphi.CompilerFeatures.inc}

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.Classes,
{$ELSE}
  Classes,
{$ENDIF}
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufString,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uProtobufMessage,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.uProtobuf;

type
  /// <remarks>
  /// This class corresponds to the protobuf message type <c>MessageX</c>.
  /// </remarks>
  TMessageX = class(TProtobufMessage)
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
    /// This may cause the destruction of transitively owned objects (this is also the case when a present fields overwrites a previous value).
    /// Developers must ensure that no shared ownership of current field values or further nested embedded objects is held.
    /// </remarks>
    public procedure Decode(aSource: TStream); override;

    /// <summary>
    /// Merges the given message (source) into this one (destination).
    /// All singular present (non-default) scalar fields in the source replace those in the destination.
    /// All singular embedded messages are merged recursively.
    /// All repeated fields are concatenated, with the source field values being appended to the destination field.
    /// If this causes a new message object to be added, a copy is created to preserve ownership.
    /// </summary>
    /// <param name="aSource">Message to merge into this one</param>
    /// <remarks>
    /// The source message must be a protobuf message of the same type.
    /// This procedure does not cause the destruction of any transitively owned objects in this message instance (append-only).
    /// </remarks>
    public procedure MergeFrom(aSource: IProtobufMessage); override;

    /// <summary>
    /// Copies the protobuf data from another object to this one.
    /// </summary>
    /// <param name="aSource">Object to copy from</param>
    /// <remarks>
    /// The other object must be a protobuf message of the same type.
    /// This performs a deep copy; hence, no ownership is shared.
    /// This procedure may cause the destruction of transitively owned objects in this message instance.
    /// Developers must ensure that no shared ownership of current field values or further nested embedded objects is held.
    /// </remarks>
    public procedure Assign(aSource: TPersistent); override;

    /// <summary>
    /// Renders those protobuf fields absent that belong to <see cref="TMessageX"/> (i.e., are not managed by an ancestor class), by setting them to their default values.
    /// </summary>
    private procedure ClearOwnFields;

    /// <summary>
    /// Merges those protobuf fields that belong to <see cref="TMessageX"/> (i.e., are not managed by an ancestor class), during a call to <see cref="MergeFrom"/>.
    /// </summary>
    /// <param name="aSource">Message to merge into this one</param>
    private procedure MergeFromOwnFields(aSource: TMessageX);

    /// <summary>
    /// Copies those protobuf fields that belong to <see cref="TMessageX"/> (i.e., are not managed by an ancestor class), during a call to <see cref="TInterfacedPersistent.Assign"/>.
    /// </summary>
    /// <param name="aSource">Source message to copy from</param>
    private procedure AssignOwnFields(aSource: TMessageX);
  end;

type
  /// <remarks>
  /// This class corresponds to the protobuf message type <c>MessageY</c>.
  /// </remarks>
  TMessageY = class(TProtobufMessage)
    /// <summary>
    /// Protobuf field number of the protobuf field <c>fieldX</c>.
    /// </summary>
    public const PROTOBUF_FIELD_NUMBER_FIELD_X = 1;

    /// <summary>
    /// Protobuf field name of the protobuf field <c>fieldX</c>.
    /// </summary>
    public const PROTOBUF_FIELD_NAME_FIELD_X = 'fieldX';

    /// <summary>
    /// Holds the decoded value of the protobuf field <c>fieldX</c>.
    /// </summary>
    private var FFieldX: TMessageX;

    /// <summary>
    /// Getter for <see cref="FieldX"/>.
    /// </summary>
    /// <returns>The value of the protobuf field <c>fieldX</c></returns>
    /// <remarks>
    /// May be overridden. Overriders shall only add side-effects and must call the ancestor implementation.
    /// </remarks>
    protected function GetFieldX: TMessageX; virtual;

    /// <summary>
    /// Setter for <see cref="FieldX"/>.
    /// </summary>
    /// <param name="aValue">The new value of the protobuf field <c>fieldX</c></param>
    /// <remarks>
    /// Ownership of the inserted message is transferred to the containing message.
    /// May be overridden. Overriders shall only add side-effects and must call the ancestor implementation.
    /// </remarks>
    protected procedure SetFieldX(aValue: TMessageX); virtual;

    /// <remarks>
    /// This property corresponds to the protobuf field <c>fieldX</c>.
    /// When written, ownership of the inserted message is transferred to the containing message.
    /// </remarks>
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_CUSTOM_ATTRIBUTES}
    [ProtobufField(PROTOBUF_FIELD_NAME_FIELD_X, PROTOBUF_FIELD_NUMBER_FIELD_X)]
{$ENDIF}
    public property FieldX: TMessageX read GetFieldX write SetFieldX;

    /// <summary>
    /// Getter for <see cref="HasFieldX"/>.
    /// </summary>
    /// <returns><c>true</c> if the protobuf field <c>fieldX</c> is present</returns>
    /// <remarks>
    /// For details on presence semantics, see <see cref="HasFieldX"/>.
    /// </remarks>
    protected function GetHasFieldX: Boolean;

    /// <summary>
    /// Setter for <see cref="HasFieldX"/>.
    /// </summary>
    /// <param name="aPresent"><c>true</c> if the protobuf field <c>fieldX</c> shall be present, <c>false</c> if absent</param>
    /// <remarks>
    /// For details on presence semantics, see <see cref="HasFieldX"/>
    /// </remarks>
    protected procedure SetHasFieldX(aPresent: Boolean);

    /// <summary>
    /// Indicates if the protobuf field <c>fieldX</c> is present in this message.
    /// If present, setting it to absent will set the case of its containing protobuf oneof <c>oneofX</c> (<see cref="OneofXCase"/>) to absent (<see cref="OneofXCaseNone"/>).
    /// If absent, setting it to present will set it to a newly created empty message.
    /// </summary>
    /// <remarks>
    /// The field (represented by <see cref="FieldX"/>) is a protobuf 3 field within the protobuf oneof <c>oneofX</c> with the <i>explicit presence</i> serialization discipline.
    /// This means that it is considered present when the oneof's case (<see cref="OneofXCase"/>) equals the corresponding presence case (<see cref="OneofXCaseFieldX"/>).
    /// </remarks>
    public property HasFieldX: Boolean read GetHasFieldX write SetHasFieldX;

    /// <summary>
    /// Protobuf field number of the protobuf field <c>fieldY</c>.
    /// </summary>
    public const PROTOBUF_FIELD_NUMBER_FIELD_Y = 2;

    /// <summary>
    /// Protobuf field name of the protobuf field <c>fieldY</c>.
    /// </summary>
    public const PROTOBUF_FIELD_NAME_FIELD_Y = 'fieldY';

    /// <summary>
    /// Holds the decoded value of the protobuf field <c>fieldY</c>.
    /// </summary>
    private var FFieldY: UnicodeString;

    /// <summary>
    /// Getter for <see cref="FieldY"/>.
    /// </summary>
    /// <returns>The value of the protobuf field <c>fieldY</c></returns>
    /// <remarks>
    /// May be overridden. Overriders shall only add side-effects and must call the ancestor implementation.
    /// </remarks>
    protected function GetFieldY: UnicodeString; virtual;

    /// <summary>
    /// Setter for <see cref="FieldY"/>.
    /// </summary>
    /// <param name="aValue">The new value of the protobuf field <c>fieldY</c></param>
    /// <remarks>
    /// May be overridden. Overriders shall only add side-effects and must call the ancestor implementation.
    /// </remarks>
    protected procedure SetFieldY(aValue: UnicodeString); virtual;

    /// <remarks>
    /// This property corresponds to the protobuf field <c>fieldY</c>.
    /// </remarks>
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_CUSTOM_ATTRIBUTES}
    [ProtobufField(PROTOBUF_FIELD_NAME_FIELD_Y, PROTOBUF_FIELD_NUMBER_FIELD_Y)]
{$ENDIF}
    public property FieldY: UnicodeString read GetFieldY write SetFieldY;

    /// <summary>
    /// Getter for <see cref="HasFieldY"/>.
    /// </summary>
    /// <returns><c>true</c> if the protobuf field <c>fieldY</c> is present</returns>
    /// <remarks>
    /// For details on presence semantics, see <see cref="HasFieldY"/>.
    /// </remarks>
    protected function GetHasFieldY: Boolean;

    /// <summary>
    /// Setter for <see cref="HasFieldY"/>.
    /// </summary>
    /// <param name="aPresent"><c>true</c> if the protobuf field <c>fieldY</c> shall be present, <c>false</c> if absent</param>
    /// <remarks>
    /// For details on presence semantics, see <see cref="HasFieldY"/>
    /// </remarks>
    protected procedure SetHasFieldY(aPresent: Boolean);

    /// <summary>
    /// Indicates if the protobuf field <c>fieldY</c> is present in this message.
    /// If present, setting it to absent will set the case of its containing protobuf oneof <c>oneofX</c> (<see cref="OneofXCase"/>) to absent (<see cref="OneofXCaseNone"/>).
    /// If absent, setting it to present will set it to its default value <see cref="PROTOBUF_DEFAULT_VALUE_STRING"/>.
    /// </summary>
    /// <remarks>
    /// The field (represented by <see cref="FieldY"/>) is a protobuf 3 field within the protobuf oneof <c>oneofX</c> with the <i>explicit presence</i> serialization discipline.
    /// This means that it is considered present when the oneof's case (<see cref="OneofXCase"/>) equals the corresponding presence case (<see cref="OneofXCaseFieldY"/>).
    /// </remarks>
    public property HasFieldY: Boolean read GetHasFieldY write SetHasFieldY;

    public type
      /// <remarks>
      /// This enumerated type represents the cases of the protobuf oneof <c>oneofX</c>.
      /// </remarks>
      TOneofXCase = (
        /// <summary>
        /// Indicates absence of the protobuf oneof <c>oneofX</c>.
        /// </summary>
        OneofXCaseNone = 0,

        /// <summary>
        /// Indicates presence of the protobuf field <c>fieldX</c> in the protobuf oneof <c>oneofX</c>.
        /// </summary>
        OneofXCaseFieldX = 1,

        /// <summary>
        /// Indicates presence of the protobuf field <c>fieldY</c> in the protobuf oneof <c>oneofX</c>.
        /// </summary>
        OneofXCaseFieldY = 2
      );

    /// <summary>
    /// Holds the case of the protobuf oneof <c>oneofX</c>.
    /// </summary>
    private var FOneofXCase: TOneofXCase;

    /// <summary>
    /// Getter for <see cref="OneofXCase"/>.
    /// </summary>
    /// <returns>The case of the protobuf oneof <c>oneofX</c></returns>
    /// <remarks>
    /// May be overridden. Overriders shall only add side-effects and must call the ancestor implementation.
    /// </remarks>
    protected function GetOneofXCase: TOneofXCase; virtual;

    /// <summary>
    /// Setter for <see cref="OneofXCase"/>.
    /// </summary>
    /// <param name="aCase">The new case of the protobuf oneof <c>oneofX</c></param>
    /// <remarks>
    /// May be overridden. Overriders shall only add side-effects and must call the ancestor implementation.
    /// </remarks>
    protected procedure SetOneofXCase(aCase: TOneofXCase); virtual;

    /// <remarks>
    /// This property corresponds to the case of the protobuf oneof <c>oneofX</c>.
    /// </remarks>
    public property OneofXCase: TOneofXCase read GetOneofXCase write SetOneofXCase;

    /// <summary>
    /// Creates an empty <see cref="TMessageY"/> that can be used as a protobuf message.
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
    /// The resulting instance state is equivalent to a newly constructed <see cref="TMessageY"/>.
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
    /// This may cause the destruction of transitively owned objects (this is also the case when a present fields overwrites a previous value).
    /// Developers must ensure that no shared ownership of current field values or further nested embedded objects is held.
    /// </remarks>
    public procedure Decode(aSource: TStream); override;

    /// <summary>
    /// Merges the given message (source) into this one (destination).
    /// All singular present (non-default) scalar fields in the source replace those in the destination.
    /// All singular embedded messages are merged recursively.
    /// All repeated fields are concatenated, with the source field values being appended to the destination field.
    /// If this causes a new message object to be added, a copy is created to preserve ownership.
    /// </summary>
    /// <param name="aSource">Message to merge into this one</param>
    /// <remarks>
    /// The source message must be a protobuf message of the same type.
    /// This procedure does not cause the destruction of any transitively owned objects in this message instance (append-only).
    /// </remarks>
    public procedure MergeFrom(aSource: IProtobufMessage); override;

    /// <summary>
    /// Copies the protobuf data from another object to this one.
    /// </summary>
    /// <param name="aSource">Object to copy from</param>
    /// <remarks>
    /// The other object must be a protobuf message of the same type.
    /// This performs a deep copy; hence, no ownership is shared.
    /// This procedure may cause the destruction of transitively owned objects in this message instance.
    /// Developers must ensure that no shared ownership of current field values or further nested embedded objects is held.
    /// </remarks>
    public procedure Assign(aSource: TPersistent); override;

    /// <summary>
    /// Renders those protobuf fields absent that belong to <see cref="TMessageY"/> (i.e., are not managed by an ancestor class), by setting them to their default values.
    /// </summary>
    private procedure ClearOwnFields;

    /// <summary>
    /// Merges those protobuf fields that belong to <see cref="TMessageY"/> (i.e., are not managed by an ancestor class), during a call to <see cref="MergeFrom"/>.
    /// </summary>
    /// <param name="aSource">Message to merge into this one</param>
    private procedure MergeFromOwnFields(aSource: TMessageY);

    /// <summary>
    /// Copies those protobuf fields that belong to <see cref="TMessageY"/> (i.e., are not managed by an ancestor class), during a call to <see cref="TInterfacedPersistent.Assign"/>.
    /// </summary>
    /// <param name="aSource">Source message to copy from</param>
    private procedure AssignOwnFields(aSource: TMessageY);
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

procedure TMessageX.MergeFrom(aSource: IProtobufMessage);
var
  lSource: TMessageX;
begin
  lSource := aSource as TMessageX;
  inherited MergeFrom(lSource);
  if (Assigned(lSource)) then MergeFromOwnFields(lSource);
end;

procedure TMessageX.Assign(aSource: TPersistent);
var
  lSource: TMessageX;
begin
  lSource := aSource as TMessageX;
  inherited Assign(lSource);
  AssignOwnFields(lSource);
end;

procedure TMessageX.ClearOwnFields;
begin
end;

procedure TMessageX.MergeFromOwnFields(aSource: TMessageX);
begin
end;

procedure TMessageX.AssignOwnFields(aSource: TMessageX);
begin
end;

constructor TMessageY.Create;
begin
  inherited;
  FOneofXCase := OneofXCaseNone;
  ClearOwnFields;
end;

destructor TMessageY.Destroy;
begin
  FFieldX.Free;
  inherited;
end;

procedure TMessageY.Clear;
begin
  inherited;
  ClearOwnFields;
end;

procedure TMessageY.Encode(aDest: TStream);
begin
  inherited;
  FieldX.EncodeAsSingularField(self, PROTOBUF_FIELD_NUMBER_FIELD_X, aDest);
  gProtobufWireCodecString.EncodeSingularField(FieldY, self, PROTOBUF_FIELD_NUMBER_FIELD_Y, aDest);
end;

procedure TMessageY.Decode(aSource: TStream);
begin
  inherited;
  if HasUnknownField(PROTOBUF_FIELD_NUMBER_FIELD_X) then
  begin
    FieldX := TMessageX.Create;
    FieldX.DecodeAsUnknownSingularField(self, PROTOBUF_FIELD_NUMBER_FIELD_X);
  end
  else HasFieldX := False;
  FieldY := gProtobufWireCodecString.DecodeUnknownField(self, PROTOBUF_FIELD_NUMBER_FIELD_Y);
end;

procedure TMessageY.MergeFrom(aSource: IProtobufMessage);
var
  lSource: TMessageY;
begin
  lSource := aSource as TMessageY;
  inherited MergeFrom(lSource);
  if (Assigned(lSource)) then MergeFromOwnFields(lSource);
end;

procedure TMessageY.Assign(aSource: TPersistent);
var
  lSource: TMessageY;
begin
  lSource := aSource as TMessageY;
  inherited Assign(lSource);
  AssignOwnFields(lSource);
end;

procedure TMessageY.ClearOwnFields;
begin
  HasFieldX := False;
  HasFieldY := False;
end;

procedure TMessageY.MergeFromOwnFields(aSource: TMessageY);
var
  lFieldX: TMessageX;
begin
  if (aSource.HasFieldX) then
  begin
    if (HasFieldX) then FieldX.MergeFrom(aSource.FieldX)
    else
    begin
      lFieldX := TMessageX.Create;
      lFieldX.Assign(aSource.FieldX);
      FieldX := lFieldX;
    end;
  end;
  if (aSource.HasFieldY) then
  begin
    FieldY := aSource.FieldY;
  end;
end;

procedure TMessageY.AssignOwnFields(aSource: TMessageY);
var
  lFieldX: TMessageX;
begin
  if (aSource.HasFieldX) then
  begin
    lFieldX := TMessageX.Create;
    lFieldX.Assign(aSource.FieldX);
    FieldX := lFieldX;
  end
  else HasFieldX := False;
  if (aSource.HasFieldY) then
  begin
    FieldY := aSource.FieldY;
  end
  else HasFieldY := False;
end;

function TMessageY.GetFieldX: TMessageX;
begin
  result := FFieldX;
end;

procedure TMessageY.SetFieldX(aValue: TMessageX);
begin
  if (Assigned(FFieldX)) then FFieldX.Free;
  FFieldX := aValue;
  if (Assigned(FFieldX)) then FFieldX.SetOwner(self);
  if (FFieldX <> PROTOBUF_DEFAULT_VALUE_MESSAGE) then OneofXCase := OneofXCaseFieldX
  else if (HasFieldX) then OneofXCase := OneofXCaseNone;
end;

function TMessageY.GetHasFieldX: Boolean;
begin
  result := (OneofXCase = OneofXCaseFieldX);
end;

procedure TMessageY.SetHasFieldX(aPresent: Boolean);
begin
  if (aPresent and (not HasFieldX)) then FieldX := TMessageX.Create
  else if (not aPresent) then
  begin
    if (Assigned(FFieldX)) then FFieldX.Free;
    FFieldX := PROTOBUF_DEFAULT_VALUE_MESSAGE;
    if (HasFieldX) then OneofXCase := OneofXCaseNone;
  end;
end;

function TMessageY.GetFieldY: UnicodeString;
begin
  result := FFieldY;
end;

procedure TMessageY.SetFieldY(aValue: UnicodeString);
begin
  FFieldY := aValue;
  OneofXCase := OneofXCaseFieldY;
end;

function TMessageY.GetHasFieldY: Boolean;
begin
  result := (OneofXCase = OneofXCaseFieldY);
end;

procedure TMessageY.SetHasFieldY(aPresent: Boolean);
begin
  if (aPresent and (not HasFieldY)) then FieldY := PROTOBUF_DEFAULT_VALUE_STRING
  else if (not aPresent) then
  begin
    FFieldY := PROTOBUF_DEFAULT_VALUE_STRING;
    if (HasFieldY) then OneofXCase := OneofXCaseNone;
  end;
end;

function TMessageY.GetOneofXCase: TOneofXCase;
begin
  result := FOneofXCase;
end;

procedure TMessageY.SetOneofXCase(aCase: TOneofXCase);
var
  lCase: TOneofXCase;
begin
  if (aCase <> FOneofXCase) then
  begin
    lCase := FOneofXCase;
    FOneofXCase := aCase;
    case lCase of
      OneofXCaseFieldX: HasFieldX := False;
      OneofXCaseFieldY: HasFieldY := False;
    end;
    case FOneofXCase of
      OneofXCaseFieldX: HasFieldX := True;
      OneofXCaseFieldY: HasFieldY := True;
    end;
  end;
end;

end.
