/// <remarks>
/// This unit corresponds to the protobuf schema definition (.proto file) <c>example_data.proto</c>.
/// </remarks>
unit uExampleData;

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
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.JSON,
{$ELSE}
  JSON,
{$ENDIF}
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.JSON.Builders,
{$ELSE}
  JSON.Builders,
{$ENDIF}
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.uProtobuf,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uIProtobufMessage,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uIProtobufWellKnownTypeMessage,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufBytes,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufFixed32,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufMessageBase,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufNotWellKnownTypeMessageBase,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufString,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufUint32,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufWireFormat;

type
  /// <remarks>
  /// This class corresponds to the protobuf message type <c>MessageY</c>.
  /// </remarks>
  TMessageY = class sealed(TProtobufNotWellKnownTypeMessageBase)
    /// <summary>
    /// Protobuf type URL of this message type.
    /// </summary>
    const PROTOBUF_TYPE_URL = PROTOBUF_TYPE_URL_DEFAULT_PREFIX + 'MessageY';

    // TODO
    function AssignOwnFields(aSource: TProtobufMessageBase): Boolean; override; final;

    // TODO
    procedure ClearOwnFields; override; final;

    // TODO
    procedure EncodeOwnFields(aDest: TStream); override; final;

    // TODO
    procedure MergeFieldFrom(aSource: TStream; aTag: TProtobufTag; aRemainingLength: PUInt32); override; final;

    // TODO
    function CalculateOwnFieldsSize: UInt32; override; final;

    // TODO
    function GetTypeUrl: TProtobufTypeUrl; override; final;

    /// <summary>
    /// Encodes the message as a JSON object using the ProtoJSON format and writes the key-value pairs to a <see cref="TJSONCollectionBuilder.TPairs"/>.
    /// </summary>
    /// <param name="aDest">The <see cref="TJSONCollectionBuilder.TPairs"/> that the encoded message's key-value pairs are written to</param>
    procedure EncodeJson(aDest: TJSONCollectionBuilder.TPairs); override; final;

    // TODO
    function MergeFieldFromJson(aSource: TJSONPair): Boolean; override; final;
  end;

  /// <remarks>
  /// This class corresponds to the protobuf message type <c>MessageX</c>.
  /// </remarks>
  TMessageX = class sealed(TProtobufNotWellKnownTypeMessageBase)
    /// <summary>
    /// Protobuf type URL of this message type.
    /// </summary>
    const PROTOBUF_TYPE_URL = PROTOBUF_TYPE_URL_DEFAULT_PREFIX + 'MessageX';

    /// <summary>
    /// Protobuf field number of the Protobuf field <c>fieldX</c>.
    /// </summary>
    const PROTOBUF_FIELD_NUMBER_FIELD_X = 1;

    /// <summary>
    /// Protobuf field name of the Protobuf field <c>fieldX</c>.
    /// </summary>
    const PROTOBUF_FIELD_NAME_FIELD_X = 'fieldX';

    /// <summary>
    /// JSON object key used to encode the Protobuf field <c>fieldX</c> using the ProtoJSON format.
    /// </summary>
    const PROTOBUF_FIELD_JSON_NAME_FIELD_X = 'fieldX';

    /// <summary>
    /// Holds the decoded value of the Protobuf field <c>fieldX</c>.
    /// </summary>
    private var FFieldX: UInt32;

    /// <remarks>
    /// This property corresponds to the Protobuf field <c>fieldX</c>.
    /// </remarks>
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_CUSTOM_ATTRIBUTES}
    [ProtobufField(PROTOBUF_FIELD_NAME_FIELD_X, PROTOBUF_FIELD_NUMBER_FIELD_X)]
{$ENDIF}
    public property FieldX: UInt32 read FFieldX write FFieldX;

    /// <summary>
    /// Protobuf field number of the Protobuf field <c>fieldY</c>.
    /// </summary>
    const PROTOBUF_FIELD_NUMBER_FIELD_Y = 2;

    /// <summary>
    /// Protobuf field name of the Protobuf field <c>fieldY</c>.
    /// </summary>
    const PROTOBUF_FIELD_NAME_FIELD_Y = 'fieldY';

    /// <summary>
    /// JSON object key used to encode the Protobuf field <c>fieldY</c> using the ProtoJSON format.
    /// </summary>
    const PROTOBUF_FIELD_JSON_NAME_FIELD_Y = 'fieldY';

    /// <summary>
    /// Holds the decoded value of the Protobuf field <c>fieldY</c>.
    /// </summary>
    private var FFieldY: TMessageY;

    /// <summary>
    /// Setter for <see cref="FieldY"/>.
    /// </summary>
    /// <param name="aValue">The new value of the Protobuf field <c>fieldY</c></param>
    private procedure SetFieldY(aValue: TMessageY);

    /// <remarks>
    /// This property corresponds to the Protobuf field <c>fieldY</c>.
    /// </remarks>
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_CUSTOM_ATTRIBUTES}
    [ProtobufField(PROTOBUF_FIELD_NAME_FIELD_Y, PROTOBUF_FIELD_NUMBER_FIELD_Y)]
{$ENDIF}
    public property FieldY: TMessageY read FFieldY write SetFieldY;

//    /// <summary>
//    /// Protobuf field number of the Protobuf field <c>fieldZ</c>.
//    /// </summary>
//    const PROTOBUF_FIELD_NUMBER_FIELD_Z = 3;
//
//    /// <summary>
//    /// Protobuf field name of the Protobuf field <c>fieldZ</c>.
//    /// </summary>
//    const PROTOBUF_FIELD_NAME_FIELD_Z = 'fieldZ';
//
//    /// <summary>
//    /// JSON object key used to encode the Protobuf field <c>fieldZ</c> using the ProtoJSON format.
//    /// </summary>
//    const PROTOBUF_FIELD_JSON_NAME_FIELD_Z = 'fieldZ';
//
//    /// <summary>
//    /// Holds the decoded values of the Protobuf field <c>fieldZ</c>.
//    /// </summary>
//    private var FFieldZ: TProtobufRepeatedUint32FieldValues;
//
//    /// <summary>
//    /// Getter for <see cref="FieldZ"/>.
//    /// </summary>
//    /// <returns>The values of the Protobuf field <c>fieldZ</c></returns>
//    /// <remarks>
//    /// The returned collection is still owned by the message.
//    /// Developers must ensure that a resulting shared ownership does not lead to unexpected behavior.
//    /// </remarks>
//    private function GetFieldZ: IProtobufRepeatedFieldValues<UInt32>;
//
//    /// <summary>
//    /// Setter for <see cref="FieldZ"/>.
//    /// </summary>
//    /// <param name="aValues">The new values of the protobuf field <c>fieldZ</c></param>
//    /// <remarks>
//    /// Ownership of the inserted field value collection is transferred to the containing message.
//    /// </remarks>
//    private procedure SetFieldZ(aValues: IProtobufRepeatedFieldValues<UInt32>);
//
//    /// <remarks>
//    /// This property corresponds to the Protobuf field <c>fieldZ</c>.
//    /// The collection is always owned by the message.
//    /// Developers must ensure that a resulting shared ownership does not lead to unexpected behavior.
//    /// </remarks>
//{$IFDEF WORK_CONNOR_DELPHI_COMPILER_CUSTOM_ATTRIBUTES}
//    [ProtobufField(PROTOBUF_FIELD_NAME_FIELD_Z, PROTOBUF_FIELD_NUMBER_FIELD_Z)]
//{$ENDIF}
//    public property FieldZ: IProtobufRepeatedFieldValues<UInt32> read GetFieldZ write SetFieldZ;

      // TODO
      function AssignOwnFields(aSource: TProtobufMessageBase): Boolean; override; final;

      // TODO
      procedure ClearOwnFields; override; final;

      // TODO
      procedure EncodeOwnFields(aDest: TStream); override; final;

      // TODO
      procedure MergeFieldFrom(aSource: TStream; aTag: TProtobufTag; aRemainingLength: PUInt32); override; final;

      // TODO
      function CalculateOwnFieldsSize: UInt32; override; final;

      // TODO
      function GetTypeUrl: TProtobufTypeUrl; override; final;

      /// <summary>
      /// Encodes the message as a JSON object using the ProtoJSON format and writes the key-value pairs to a <see cref="TJSONCollectionBuilder.TPairs"/>.
      /// </summary>
      /// <param name="aDest">The <see cref="TJSONCollectionBuilder.TPairs"/> that the encoded message's key-value pairs are written to</param>
      procedure EncodeJson(aDest: TJSONCollectionBuilder.TPairs); override; final;

      // TODO
      function MergeFieldFromJson(aSource: TJSONPair): Boolean; override; final;
  end;

implementation

function TMessageY.AssignOwnFields(aSource: TProtobufMessageBase): Boolean;
var
  lSource: TMessageY;
begin
  if (not (aSource is TMessageY)) then Exit(False);
  result := True;
  lSource := TMessageY(aSource);
end;

procedure TMessageY.ClearOwnFields;
begin
end;

procedure TMessageY.EncodeOwnFields(aDest: TStream);
begin
end;

procedure TMessageY.MergeFieldFrom(aSource: TStream; aTag: TProtobufTag; aRemainingLength: PUInt32);
begin
  MergeUnknownFieldFrom(aSource, aTag, aRemainingLength);
end;

function TMessageY.CalculateOwnFieldsSize: UInt32;
begin
  result := 0;
end;

function TMessageY.GetTypeUrl: TProtobufTypeUrl;
begin
  result := PROTOBUF_TYPE_URL;
end;

procedure TMessageY.EncodeJson(aDest: TJSONCollectionBuilder.TPairs);
begin
end;

function TMessageY.MergeFieldFromJson(aSource: TJSONPair): Boolean;
begin
  // TODO
end;

procedure TMessageX.SetFieldY(aValue: TMessageY);
begin
  FFieldY.Free;
  FFieldY := aValue;
end;

//function TMessageX.GetFieldZ: IProtobufRepeatedFieldValues<UInt32>;
//begin
//  result := FFieldZ;
//end;
//
//procedure TMessageX.SetFieldZ(aValues: IProtobufRepeatedFieldValues<UInt32>);
//begin
//  FFieldZ.Free;
//  FFieldZ := aValues as TProtobufRepeatedUint32FieldValues;
//  FFieldZ.SetOwner(self);
//end;

function TMessageX.AssignOwnFields(aSource: TProtobufMessageBase): Boolean;
var
  lSource: TMessageX;
begin
  if (not (aSource is TMessageX)) then Exit(False);
  result := True;
  lSource := TMessageX(aSource);
  FFieldX := lSource.FFieldX;
  FFieldY.Free;
  FFieldY := TMessageY.Create;
  FFieldY.Assign(lSource.FFieldY);
//  (FieldZ as TInterfacedPersistent).Assign(aSource.FieldZ as TInterfacedPersistent);
end;

procedure TMessageX.ClearOwnFields;
begin
  FFieldX := PROTOBUF_DEFAULT_VALUE_UINT32;
  FFieldY.Free;
  FFieldY := PROTOBUF_DEFAULT_VALUE_MESSAGE;
//  FFieldZ.Clear;
end;

procedure TMessageX.EncodeOwnFields(aDest: TStream);
begin
  EncodeProtobufUint32Field(aDest, PROTOBUF_FIELD_NUMBER_FIELD_X, FFieldX);
  EncodeProtobufMessageField(aDest, PROTOBUF_FIELD_NUMBER_FIELD_Y, FFieldY);
//  EncodeProtobufRepeatedUint32Field(aDest, PROTOBUF_FIELD_NUMBER_Z, FFieldZ);
end;

procedure TMessageX.MergeFieldFrom(aSource: TStream; aTag: TProtobufTag; aRemainingLength: PUInt32);
begin
  // TODO is this the correct merge behavior?
  case aTag.FieldNumber of
    PROTOBUF_FIELD_NUMBER_FIELD_X: FFieldX := DecodeProtobufUint32Field(aSource, aTag.WireType, aRemainingLength);
    PROTOBUF_FIELD_NUMBER_FIELD_Y:
    begin
      FFieldY.Free;
      FFieldY := TMessageY.Create;
      FFieldY.MergeFrom(aSource, aRemainingLength);
    end;
//    PROTOBUF_FIELD_NUMBER_FIELD_Z: TODO
    else MergeUnknownFieldFrom(aSource, aTag, aRemainingLength);
  end;
end;

function TMessageX.CalculateOwnFieldsSize: UInt32;
begin
  result := 0;
  result := result + CalculateProtobufUint32FieldSize(PROTOBUF_FIELD_NUMBER_FIELD_X, FFieldX);
  result := result + CalculateProtobufMessageFieldSize(PROTOBUF_FIELD_NUMBER_FIELD_Y, FFieldY);
//  result := result + CalculateProtobufRepeatedUint32FieldSize(PROTOBUF_FIELD_NUMBER_FIELD_Z, FFieldZ);
end;

function TMessageX.GetTypeUrl: TProtobufTypeUrl;
begin
  result := PROTOBUF_TYPE_URL;
end;

procedure TMessageX.EncodeJson(aDest: TJSONCollectionBuilder.TPairs);
begin
  EncodeJsonProtobufUint32Field(aDest, PROTOBUF_FIELD_JSON_NAME_FIELD_X, FFieldX);
  EncodeJsonProtobufMessageField(aDest, PROTOBUF_FIELD_JSON_NAME_FIELD_Y, FFieldY);
//  EncodeJsonProtobufRepeatedUint32Field(aDest, PROTOBUF_FIELD_JSON_NAME_FIELD_Z, FFieldZ);
end;

function TMessageX.MergeFieldFromJson(aSource: TJSONPair): Boolean;
begin
  if ((aSource.JsonString.Value = PROTOBUF_FIELD_NAME_FIELD_X) or (aSource.JSONString.Value = PROTOBUF_FIELD_JSON_NAME_FIELD_X)) then FFieldX := DecodeJsonProtobufUint32(aSource.JSONValue);
  else if ((aSource.JsonString.Value = PROTOBUF_FIELD_NAME_FIELD_Y) or (aSource.JSONString.Value = PROTOBUF_FIELD_JSON_NAME_FIELD_Y)) then
  begin
    FFieldY.Free;
    FFieldY := TMessageY.Create;
    FFieldY.DecodeJson(aSource.JSONValue);
  end;
//  else if ((aSource.JsonString.Value = PROTOBUF_FIELD_NAME_FIELD_Z) or (aSource.JSONString.Value = PROTOBUF_FIELD_JSON_NAME_FIELD_Z)) then
//  begin
//    // TODO
//  end
  else Exit(False);
  result := True;
end;

end.
