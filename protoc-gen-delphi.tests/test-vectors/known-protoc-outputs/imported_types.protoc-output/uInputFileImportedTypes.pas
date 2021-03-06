/// <remarks>
/// This unit corresponds to the protobuf schema definition (.proto file) <c>input_file_imported_types.proto</c>.
/// </remarks>
unit uInputFileImportedTypes;

{$INCLUDE Work.Connor.Delphi.CompilerFeatures.inc}

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  Package1.uImportedEnumQualified,
  Package1.uImportedMessageQualified,
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.Classes,
{$ELSE}
  Classes,
{$ENDIF}
  uImportedEnumUnqualified,
  uImportedMessageUnqualified,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufEnum,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uProtobufMessage,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.uProtobuf;

type
  /// <remarks>
  /// This class corresponds to the protobuf message type <c>MessageX</c>.
  /// </remarks>
  TMessageX = class(TProtobufMessage)
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
    private var FFieldX: uImportedMessageUnqualified.TImportedMessageX;

    /// <summary>
    /// Getter for <see cref="FieldX"/>.
    /// </summary>
    /// <returns>The value of the protobuf field <c>fieldX</c></returns>
    /// <remarks>
    /// May be overridden. Overriders shall only add side-effects and must call the ancestor implementation.
    /// </remarks>
    protected function GetFieldX: uImportedMessageUnqualified.TImportedMessageX; virtual;

    /// <summary>
    /// Setter for <see cref="FieldX"/>.
    /// </summary>
    /// <param name="aValue">The new value of the protobuf field <c>fieldX</c></param>
    /// <remarks>
    /// Ownership of the inserted message is transferred to the containing message.
    /// May be overridden. Overriders shall only add side-effects and must call the ancestor implementation.
    /// </remarks>
    protected procedure SetFieldX(aValue: uImportedMessageUnqualified.TImportedMessageX); virtual;

    /// <remarks>
    /// This property corresponds to the protobuf field <c>fieldX</c>.
    /// When written, ownership of the inserted message is transferred to the containing message.
    /// </remarks>
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_CUSTOM_ATTRIBUTES}
    [ProtobufField(PROTOBUF_FIELD_NAME_FIELD_X, PROTOBUF_FIELD_NUMBER_FIELD_X)]
{$ENDIF}
    public property FieldX: uImportedMessageUnqualified.TImportedMessageX read GetFieldX write SetFieldX;

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
    /// <exception cref="EProtobufInvalidOperation">If the field was absent and set to present</exception>
    /// <remarks>
    /// For details on presence semantics, see <see cref="HasFieldX"/>
    /// </remarks>
    protected procedure SetHasFieldX(aPresent: Boolean);

    /// <summary>
    /// Indicates if the protobuf field <c>fieldX</c> is present in this message.
    /// If present, setting it to absent sets it to its default value <see cref="PROTOBUF_DEFAULT_VALUE_MESSAGE"/>.
    /// If absent, it cannot be set to present using this property, attempting to do so will raise an <exception cref="EProtobufInvalidOperation">.
    /// </summary>
    /// <remarks>
    /// The field (represented by <see cref="FieldX"/>) is a protobuf 3 field with the <i>no presence</i> serialization discipline.
    /// This means that it is considered present when its value does not equal the default value <see cref="PROTOBUF_DEFAULT_VALUE_MESSAGE"/>.
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
    private var FFieldY: Package1.uImportedMessageQualified.TImportedMessageX;

    /// <summary>
    /// Getter for <see cref="FieldY"/>.
    /// </summary>
    /// <returns>The value of the protobuf field <c>fieldY</c></returns>
    /// <remarks>
    /// May be overridden. Overriders shall only add side-effects and must call the ancestor implementation.
    /// </remarks>
    protected function GetFieldY: Package1.uImportedMessageQualified.TImportedMessageX; virtual;

    /// <summary>
    /// Setter for <see cref="FieldY"/>.
    /// </summary>
    /// <param name="aValue">The new value of the protobuf field <c>fieldY</c></param>
    /// <remarks>
    /// Ownership of the inserted message is transferred to the containing message.
    /// May be overridden. Overriders shall only add side-effects and must call the ancestor implementation.
    /// </remarks>
    protected procedure SetFieldY(aValue: Package1.uImportedMessageQualified.TImportedMessageX); virtual;

    /// <remarks>
    /// This property corresponds to the protobuf field <c>fieldY</c>.
    /// When written, ownership of the inserted message is transferred to the containing message.
    /// </remarks>
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_CUSTOM_ATTRIBUTES}
    [ProtobufField(PROTOBUF_FIELD_NAME_FIELD_Y, PROTOBUF_FIELD_NUMBER_FIELD_Y)]
{$ENDIF}
    public property FieldY: Package1.uImportedMessageQualified.TImportedMessageX read GetFieldY write SetFieldY;

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
    /// <exception cref="EProtobufInvalidOperation">If the field was absent and set to present</exception>
    /// <remarks>
    /// For details on presence semantics, see <see cref="HasFieldY"/>
    /// </remarks>
    protected procedure SetHasFieldY(aPresent: Boolean);

    /// <summary>
    /// Indicates if the protobuf field <c>fieldY</c> is present in this message.
    /// If present, setting it to absent sets it to its default value <see cref="PROTOBUF_DEFAULT_VALUE_MESSAGE"/>.
    /// If absent, it cannot be set to present using this property, attempting to do so will raise an <exception cref="EProtobufInvalidOperation">.
    /// </summary>
    /// <remarks>
    /// The field (represented by <see cref="FieldY"/>) is a protobuf 3 field with the <i>no presence</i> serialization discipline.
    /// This means that it is considered present when its value does not equal the default value <see cref="PROTOBUF_DEFAULT_VALUE_MESSAGE"/>.
    /// </remarks>
    public property HasFieldY: Boolean read GetHasFieldY write SetHasFieldY;

    /// <summary>
    /// Protobuf field number of the protobuf field <c>fieldZ</c>.
    /// </summary>
    public const PROTOBUF_FIELD_NUMBER_FIELD_Z = 3;

    /// <summary>
    /// Protobuf field name of the protobuf field <c>fieldZ</c>.
    /// </summary>
    public const PROTOBUF_FIELD_NAME_FIELD_Z = 'fieldZ';

    /// <summary>
    /// Holds the decoded value of the protobuf field <c>fieldZ</c>.
    /// </summary>
    private var FFieldZ: TProtobufEnumFieldValue;

    /// <summary>
    /// Getter for <see cref="FieldZ"/>.
    /// </summary>
    /// <returns>The value of the protobuf field <c>fieldZ</c></returns>
    /// <remarks>
    /// May be overridden. Overriders shall only add side-effects and must call the ancestor implementation.
    /// </remarks>
    protected function GetFieldZ: uImportedEnumUnqualified.TImportedEnumX; virtual;

    /// <summary>
    /// Setter for <see cref="FieldZ"/>.
    /// </summary>
    /// <param name="aValue">The new value of the protobuf field <c>fieldZ</c></param>
    /// <remarks>
    /// May be overridden. Overriders shall only add side-effects and must call the ancestor implementation.
    /// </remarks>
    protected procedure SetFieldZ(aValue: uImportedEnumUnqualified.TImportedEnumX); virtual;

    /// <remarks>
    /// This property corresponds to the protobuf field <c>fieldZ</c>.
    /// </remarks>
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_CUSTOM_ATTRIBUTES}
    [ProtobufField(PROTOBUF_FIELD_NAME_FIELD_Z, PROTOBUF_FIELD_NUMBER_FIELD_Z)]
{$ENDIF}
    public property FieldZ: uImportedEnumUnqualified.TImportedEnumX read GetFieldZ write SetFieldZ;

    /// <summary>
    /// Getter for <see cref="HasFieldZ"/>.
    /// </summary>
    /// <returns><c>true</c> if the protobuf field <c>fieldZ</c> is present</returns>
    /// <remarks>
    /// For details on presence semantics, see <see cref="HasFieldZ"/>.
    /// </remarks>
    protected function GetHasFieldZ: Boolean;

    /// <summary>
    /// Setter for <see cref="HasFieldZ"/>.
    /// </summary>
    /// <param name="aPresent"><c>true</c> if the protobuf field <c>fieldZ</c> shall be present, <c>false</c> if absent</param>
    /// <exception cref="EProtobufInvalidOperation">If the field was absent and set to present</exception>
    /// <remarks>
    /// For details on presence semantics, see <see cref="HasFieldZ"/>
    /// </remarks>
    protected procedure SetHasFieldZ(aPresent: Boolean);

    /// <summary>
    /// Indicates if the protobuf field <c>fieldZ</c> is present in this message.
    /// If present, setting it to absent sets it to its default value <see cref="PROTOBUF_DEFAULT_VALUE_ENUM"/>.
    /// If absent, it cannot be set to present using this property, attempting to do so will raise an <exception cref="EProtobufInvalidOperation">.
    /// </summary>
    /// <remarks>
    /// The field (represented by <see cref="FieldZ"/>) is a protobuf 3 field with the <i>no presence</i> serialization discipline.
    /// This means that it is considered present when its value does not equal the default value <see cref="PROTOBUF_DEFAULT_VALUE_ENUM"/>.
    /// </remarks>
    public property HasFieldZ: Boolean read GetHasFieldZ write SetHasFieldZ;

    /// <summary>
    /// Protobuf field number of the protobuf field <c>fieldA</c>.
    /// </summary>
    public const PROTOBUF_FIELD_NUMBER_FIELD_A = 4;

    /// <summary>
    /// Protobuf field name of the protobuf field <c>fieldA</c>.
    /// </summary>
    public const PROTOBUF_FIELD_NAME_FIELD_A = 'fieldA';

    /// <summary>
    /// Holds the decoded value of the protobuf field <c>fieldA</c>.
    /// </summary>
    private var FFieldA: TProtobufEnumFieldValue;

    /// <summary>
    /// Getter for <see cref="FieldA"/>.
    /// </summary>
    /// <returns>The value of the protobuf field <c>fieldA</c></returns>
    /// <remarks>
    /// May be overridden. Overriders shall only add side-effects and must call the ancestor implementation.
    /// </remarks>
    protected function GetFieldA: Package1.uImportedEnumQualified.TImportedEnumX; virtual;

    /// <summary>
    /// Setter for <see cref="FieldA"/>.
    /// </summary>
    /// <param name="aValue">The new value of the protobuf field <c>fieldA</c></param>
    /// <remarks>
    /// May be overridden. Overriders shall only add side-effects and must call the ancestor implementation.
    /// </remarks>
    protected procedure SetFieldA(aValue: Package1.uImportedEnumQualified.TImportedEnumX); virtual;

    /// <remarks>
    /// This property corresponds to the protobuf field <c>fieldA</c>.
    /// </remarks>
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_CUSTOM_ATTRIBUTES}
    [ProtobufField(PROTOBUF_FIELD_NAME_FIELD_A, PROTOBUF_FIELD_NUMBER_FIELD_A)]
{$ENDIF}
    public property FieldA: Package1.uImportedEnumQualified.TImportedEnumX read GetFieldA write SetFieldA;

    /// <summary>
    /// Getter for <see cref="HasFieldA"/>.
    /// </summary>
    /// <returns><c>true</c> if the protobuf field <c>fieldA</c> is present</returns>
    /// <remarks>
    /// For details on presence semantics, see <see cref="HasFieldA"/>.
    /// </remarks>
    protected function GetHasFieldA: Boolean;

    /// <summary>
    /// Setter for <see cref="HasFieldA"/>.
    /// </summary>
    /// <param name="aPresent"><c>true</c> if the protobuf field <c>fieldA</c> shall be present, <c>false</c> if absent</param>
    /// <exception cref="EProtobufInvalidOperation">If the field was absent and set to present</exception>
    /// <remarks>
    /// For details on presence semantics, see <see cref="HasFieldA"/>
    /// </remarks>
    protected procedure SetHasFieldA(aPresent: Boolean);

    /// <summary>
    /// Indicates if the protobuf field <c>fieldA</c> is present in this message.
    /// If present, setting it to absent sets it to its default value <see cref="PROTOBUF_DEFAULT_VALUE_ENUM"/>.
    /// If absent, it cannot be set to present using this property, attempting to do so will raise an <exception cref="EProtobufInvalidOperation">.
    /// </summary>
    /// <remarks>
    /// The field (represented by <see cref="FieldA"/>) is a protobuf 3 field with the <i>no presence</i> serialization discipline.
    /// This means that it is considered present when its value does not equal the default value <see cref="PROTOBUF_DEFAULT_VALUE_ENUM"/>.
    /// </remarks>
    public property HasFieldA: Boolean read GetHasFieldA write SetHasFieldA;

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

implementation

constructor TMessageX.Create;
begin
  inherited;
  ClearOwnFields;
end;

destructor TMessageX.Destroy;
begin
  FFieldY.Free;
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
  FieldX.EncodeAsSingularField(self, PROTOBUF_FIELD_NUMBER_FIELD_X, aDest);
  FieldY.EncodeAsSingularField(self, PROTOBUF_FIELD_NUMBER_FIELD_Y, aDest);
  gProtobufWireCodecEnum.EncodeSingularField(Ord(FieldZ), self, PROTOBUF_FIELD_NUMBER_FIELD_Z, aDest);
  gProtobufWireCodecEnum.EncodeSingularField(Ord(FieldA), self, PROTOBUF_FIELD_NUMBER_FIELD_A, aDest);
end;

procedure TMessageX.Decode(aSource: TStream);
begin
  inherited;
  if HasUnknownField(PROTOBUF_FIELD_NUMBER_FIELD_X) then
  begin
    FieldX := uImportedMessageUnqualified.TImportedMessageX.Create;
    FieldX.DecodeAsUnknownSingularField(self, PROTOBUF_FIELD_NUMBER_FIELD_X);
  end
  else HasFieldX := False;
  if HasUnknownField(PROTOBUF_FIELD_NUMBER_FIELD_Y) then
  begin
    FieldY := Package1.uImportedMessageQualified.TImportedMessageX.Create;
    FieldY.DecodeAsUnknownSingularField(self, PROTOBUF_FIELD_NUMBER_FIELD_Y);
  end
  else HasFieldY := False;
  FieldZ := uImportedEnumUnqualified.TImportedEnumX(gProtobufWireCodecEnum.DecodeUnknownField(self, PROTOBUF_FIELD_NUMBER_FIELD_Z));
  FieldA := Package1.uImportedEnumQualified.TImportedEnumX(gProtobufWireCodecEnum.DecodeUnknownField(self, PROTOBUF_FIELD_NUMBER_FIELD_A));
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
  HasFieldX := False;
  HasFieldY := False;
  HasFieldZ := False;
  HasFieldA := False;
end;

procedure TMessageX.MergeFromOwnFields(aSource: TMessageX);
var
  lFieldX: uImportedMessageUnqualified.TImportedMessageX;
  lFieldY: Package1.uImportedMessageQualified.TImportedMessageX;
begin
  if (aSource.HasFieldX) then
  begin
    if (HasFieldX) then FieldX.MergeFrom(aSource.FieldX)
    else
    begin
      lFieldX := uImportedMessageUnqualified.TImportedMessageX.Create;
      lFieldX.Assign(aSource.FieldX);
      FieldX := lFieldX;
    end;
  end;
  if (aSource.HasFieldY) then
  begin
    if (HasFieldY) then FieldY.MergeFrom(aSource.FieldY)
    else
    begin
      lFieldY := Package1.uImportedMessageQualified.TImportedMessageX.Create;
      lFieldY.Assign(aSource.FieldY);
      FieldY := lFieldY;
    end;
  end;
  if (aSource.HasFieldZ) then
  begin
    FieldZ := aSource.FieldZ;
  end;
  if (aSource.HasFieldA) then
  begin
    FieldA := aSource.FieldA;
  end;
end;

procedure TMessageX.AssignOwnFields(aSource: TMessageX);
var
  lFieldX: uImportedMessageUnqualified.TImportedMessageX;
  lFieldY: Package1.uImportedMessageQualified.TImportedMessageX;
begin
  if (aSource.HasFieldX) then
  begin
    lFieldX := uImportedMessageUnqualified.TImportedMessageX.Create;
    lFieldX.Assign(aSource.FieldX);
    FieldX := lFieldX;
  end
  else HasFieldX := False;
  if (aSource.HasFieldY) then
  begin
    lFieldY := Package1.uImportedMessageQualified.TImportedMessageX.Create;
    lFieldY.Assign(aSource.FieldY);
    FieldY := lFieldY;
  end
  else HasFieldY := False;
  if (aSource.HasFieldZ) then
  begin
    FieldZ := aSource.FieldZ;
  end
  else HasFieldZ := False;
  if (aSource.HasFieldA) then
  begin
    FieldA := aSource.FieldA;
  end
  else HasFieldA := False;
end;

function TMessageX.GetFieldX: uImportedMessageUnqualified.TImportedMessageX;
begin
  result := FFieldX;
end;

procedure TMessageX.SetFieldX(aValue: uImportedMessageUnqualified.TImportedMessageX);
begin
  if (Assigned(FFieldX)) then FFieldX.Free;
  FFieldX := aValue;
  if (Assigned(FFieldX)) then FFieldX.SetOwner(self);
end;

function TMessageX.GetHasFieldX: Boolean;
begin
  result := (FieldX = PROTOBUF_DEFAULT_VALUE_MESSAGE);
end;

procedure TMessageX.SetHasFieldX(aPresent: Boolean);
begin
  if (aPresent and (not HasFieldX)) then raise EProtobufInvalidOperation.Create('Attempted to set a protobuf field to present without defining a value')
  else if (not aPresent) then
  begin
    if (HasFieldX) then FieldX := PROTOBUF_DEFAULT_VALUE_MESSAGE;
  end;
end;

function TMessageX.GetFieldY: Package1.uImportedMessageQualified.TImportedMessageX;
begin
  result := FFieldY;
end;

procedure TMessageX.SetFieldY(aValue: Package1.uImportedMessageQualified.TImportedMessageX);
begin
  if (Assigned(FFieldY)) then FFieldY.Free;
  FFieldY := aValue;
  if (Assigned(FFieldY)) then FFieldY.SetOwner(self);
end;

function TMessageX.GetHasFieldY: Boolean;
begin
  result := (FieldY = PROTOBUF_DEFAULT_VALUE_MESSAGE);
end;

procedure TMessageX.SetHasFieldY(aPresent: Boolean);
begin
  if (aPresent and (not HasFieldY)) then raise EProtobufInvalidOperation.Create('Attempted to set a protobuf field to present without defining a value')
  else if (not aPresent) then
  begin
    if (HasFieldY) then FieldY := PROTOBUF_DEFAULT_VALUE_MESSAGE;
  end;
end;

function TMessageX.GetFieldZ: uImportedEnumUnqualified.TImportedEnumX;
begin
  result := uImportedEnumUnqualified.TImportedEnumX(FFieldZ);
end;

procedure TMessageX.SetFieldZ(aValue: uImportedEnumUnqualified.TImportedEnumX);
begin
  FFieldZ := Ord(aValue);
end;

function TMessageX.GetHasFieldZ: Boolean;
begin
  result := (Ord(FieldZ) = PROTOBUF_DEFAULT_VALUE_ENUM);
end;

procedure TMessageX.SetHasFieldZ(aPresent: Boolean);
begin
  if (aPresent and (not HasFieldZ)) then raise EProtobufInvalidOperation.Create('Attempted to set a protobuf field to present without defining a value')
  else if (not aPresent) then
  begin
    if (HasFieldZ) then FieldZ := uImportedEnumUnqualified.TImportedEnumX(PROTOBUF_DEFAULT_VALUE_ENUM);
  end;
end;

function TMessageX.GetFieldA: Package1.uImportedEnumQualified.TImportedEnumX;
begin
  result := Package1.uImportedEnumQualified.TImportedEnumX(FFieldA);
end;

procedure TMessageX.SetFieldA(aValue: Package1.uImportedEnumQualified.TImportedEnumX);
begin
  FFieldA := Ord(aValue);
end;

function TMessageX.GetHasFieldA: Boolean;
begin
  result := (Ord(FieldA) = PROTOBUF_DEFAULT_VALUE_ENUM);
end;

procedure TMessageX.SetHasFieldA(aPresent: Boolean);
begin
  if (aPresent and (not HasFieldA)) then raise EProtobufInvalidOperation.Create('Attempted to set a protobuf field to present without defining a value')
  else if (not aPresent) then
  begin
    if (HasFieldA) then FieldA := Package1.uImportedEnumQualified.TImportedEnumX(PROTOBUF_DEFAULT_VALUE_ENUM);
  end;
end;

end.
