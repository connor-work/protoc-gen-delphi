unit uExampleData;

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
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.JSON,
{$ELSE}
  JSON,
{$ENDIF}
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.uProtobuf,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uIProtobufNotWellKnownTypeMessage,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uIProtobufRepeatedFieldValues,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufMessageBase,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufNotWellKnownTypeMessageBase,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufUint32,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufWireFormat;

type
  /// <summary>
  /// TODO contract
  /// </summary>
  /// <remarks>
  /// This interface corresponds to the Protobuf message type <c>MessageX</c>.
  /// </remarks>
  IMessageX = interface(IProtobufNotWellKnownTypeMessage)
    ['{FB021EC7-09EB-47CB-9B1D-F868224DDBE5}']
    /// TODO contract
    function AssignOwnFields(aSource: TProtobufMessageBase): Boolean;

    /// TODO contract
    procedure ClearOwnFields;

    /// TODO contract
    procedure EncodeOwnFields(aDest: TStream);

    /// TODO contract
    procedure MergeFieldFrom(aSource: TStream; aTag: TProtobufTag; aRemainingLength: PUInt32);

    /// TODO contract
    function CalculateOwnFieldsSize: UInt32;

    /// TODO contract
    function GetTypeUrl: TProtobufTypeUrl;

    /// <summary>
    /// Encodes the message as a JSON object using the ProtoJSON format and writes it to a <see cref="TJSONObject"/>.
    /// </summary>
    /// <param name="aDest">The <see cref="TJSONObject"/> that the encoded message is written to</param>
    procedure EncodeJson(aDest: TJSONObject);

    /// TODO contract
    function MergeFieldFromJson(aSource: TJSONPair): Boolean;

    /// <summary>
    /// Getter for <see cref="FieldX"/>.
    /// </summary>
    /// <returns>The value of <see cref="FieldX"/></param>
    function GetFieldX: UInt32;

    /// <summary>
    /// Setter for <see cref="FieldX"/>.
    /// </summary>
    /// <param name="aValue">The new value of the Protobuf field <c>fieldX</c></param>
    procedure SetFieldX(aValue: UInt32);

    /// <summary>
    /// Getter for <see cref="FieldZ"/>.
    /// </summary>
    /// <returns>The value of <see cref="FieldZ"/></param>
    function GetFieldZ: IProtobufRepeatedFieldValues<UInt32>;
    /// TODO
    /// <remarks>
    /// This property corresponds to the Protobuf field <c>fieldX</c>.
    /// </remarks>
    property FieldX: UInt32 read GetFieldX write SetFieldX;

    /// TODO
    /// <remarks>
    /// This property corresponds to the Protobuf field <c>fieldZ</c>.
    /// </remarks>
    property FieldZ: IProtobufRepeatedFieldValues<UInt32> read GetFieldZ;
  end;

type
  /// <summary>
  /// TODO contract
  /// </summary>
  /// <remarks>
  /// This class corresponds to the Protobuf message type <c>MessageX</c>.
  /// </remarks>
  TMessageX = class sealed(TProtobufNotWellKnownTypeMessageBase)
    /// <summary>
    /// Protobuf type URL of this message type.
    /// </summary>
    public const PROTOBUF_TYPE_URL = PROTOBUF_TYPE_URL_DEFAULT_PREFIX + 'MessageX';

    /// TODO contract
    public constructor Create; override; final;

    /// TODO contract
    public function AssignOwnFields(aSource: TProtobufMessageBase): Boolean; override; final;

    /// TODO contract
    public procedure ClearOwnFields; override; final;

    /// TODO contract
    public procedure EncodeOwnFields(aDest: TStream); override; final;

    /// TODO contract
    public procedure MergeFieldFrom(aSource: TStream; aTag: TProtobufTag; aRemainingLength: PUInt32); override; final;

    /// TODO contract
    public function CalculateOwnFieldsSize: UInt32; override; final;

    /// TODO contract
    public function GetTypeUrl: TProtobufTypeUrl; override; final;

    /// <summary>
    /// Encodes the message as a JSON object using the ProtoJSON format and writes it to a <see cref="TJSONObject"/>.
    /// </summary>
    /// <param name="aDest">The <see cref="TJSONObject"/> that the encoded message is written to</param>
    public procedure EncodeJson(aDest: TJSONObject); override; final;

    /// TODO contract
    public function MergeFieldFromJson(aSource: TJSONPair): Boolean; override; final;

    /// <summary>
    /// Getter for <see cref="FieldX"/>.
    /// </summary>
    /// <returns>The value of <see cref="FieldX"/></param>
    public function GetFieldX: UInt32;

    /// <summary>
    /// Setter for <see cref="FieldX"/>.
    /// </summary>
    /// <param name="aValue">The new value of the Protobuf field <c>fieldX</c></param>
    public procedure SetFieldX(aValue: UInt32);

    /// <summary>
    /// Getter for <see cref="FieldZ"/>.
    /// </summary>
    /// <returns>The value of <see cref="FieldZ"/></param>
    public function GetFieldZ: TProtobufRepeatedUint32FieldValues;

    /// TODO
    /// <remarks>
    /// This property corresponds to the Protobuf field <c>fieldX</c>.
    /// </remarks>
    public property FieldX: UInt32 read GetFieldX write SetFieldX;

    /// TODO
    /// <remarks>
    /// This property corresponds to the Protobuf field <c>fieldZ</c>.
    /// </remarks>
    public property FieldZ: TProtobufRepeatedUint32FieldValues read GetFieldZ;
  end;

implementation

constructor TMessageX.Create;
begin
end;

function TMessageX.AssignOwnFields(aSource: TProtobufMessageBase): Boolean;
var
  lSource: TMessageX;
begin
  lSource := aSource as TMessageX;
  if (not Assigned(lSource)) then Exit(False);
  result := True;
  FFieldX := lSource.FFieldX;
  FFieldZ.Assign(lSource.FFieldZ);
end;

procedure TMessageX.ClearOwnFields;
begin
  FFieldX := PROTOBUF_DEFAULT_VALUE_UINT32;
  FFieldZ.Clear;
end;

procedure TMessageX.EncodeOwnFields(aDest: TStream);
begin
  EncodeProtobufUint32Field(aDest, PROTOBUF_FIELD_NUMBER_FIELD_X, FFieldX);
  FFieldZ.EncodeField(aDest, PROTOBUF_FIELD_NUMBER_FIELD_Z);
end;

procedure TMessageX.MergeFieldFrom(aSource: TStream; aTag: TProtobufTag; aRemainingLength: PUInt32);
begin
  // TODO is this the correct merge behavior?
  case aTag.FieldNumber of
    PROTOBUF_FIELD_NUMBER_FIELD_X:
    begin
      FFieldX := DecodeProtobufUint32Field(aSource, aTag.WireType, aRemainingLength);
    end;
    PROTOBUF_FIELD_NUMBER_FIELD_Z:
    begin
      FFieldZ.MergeFromField(aSource, aTag.WireType, aRemainingLength);
    end;
    else MergeUnknownFieldFrom(aSource, aTag, aRemainingLength)
  end;
end;

function TMessageX.CalculateOwnFieldsSize: UInt32;
begin
  result := 0;
  result := result + CalculateProtobufUint32FieldSize(PROTOBUF_FIELD_NUMBER_FIELD_X, FFieldX);
  result := result + CalculateProtobufRepeatedUint32FieldSize(PROTOBUF_FIELD_NUMBER_FIELD_Z, FFieldZ);
end;

function TMessageX.GetTypeUrl: TProtobufTypeUrl;
begin
  result := PROTOBUF_TYPE_URL;
end;

procedure TMessageX.EncodeJson(aDest: TJSONObject);
begin
  EncodeJsonProtobufUint32Field(aDest, PROTOBUF_FIELD_NAME_FIELD_X, FFieldX);
  EncodeJsonProtobufRepeatedUint32Field(aDest, PROTOBUF_FIELD_NAME_FIELD_Z, FFieldZ);
end;

function TMessageX.MergeFieldFromJson(aSource: TJSONPair): Boolean;
begin
  if ((aSource.JsonString.Value = PROTOBUF_FIELD_NAME_FIELD_X) or (aSource.JsonString.Value = PROTOBUF_FIELD_NUMBER_FIELD_X)) then
  begin
    FFieldX := DecodeJsonProtobufUint32(aSource.JsonValue);
  end
  else
  if ((aSource.JsonString.Value = PROTOBUF_FIELD_NAME_FIELD_Z) or (aSource.JsonString.Value = PROTOBUF_FIELD_NUMBER_FIELD_Z)) then
  begin
    // TODO
  end
  else Exit(False);
  result := True;
end;

function TMessageX.GetFieldX: UInt32;
begin
  result := FFieldX;
end;

procedure TMessageX.SetFieldX(aValue: UInt32);
begin
  FFieldX := aValue;
end;

function TMessageX.GetFieldZ: TProtobufRepeatedUint32FieldValues;
begin
  result := FFieldZ;
end;

initialization

TProtobufTypeRegistry.Global.RegisterNotWellKnownType(TMessageX.PROTOBUF_TYPE_URL, TMessageX);

end.

