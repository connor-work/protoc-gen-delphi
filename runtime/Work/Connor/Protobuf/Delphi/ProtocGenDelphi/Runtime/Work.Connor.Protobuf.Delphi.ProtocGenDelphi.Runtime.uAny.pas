/// Copyright 2025 Connor Erdmann (connor.work)
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
/// 
///     http://www.apache.org/licenses/LICENSE-2.0
/// 
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.

/// <summary>
/// Runtime support for the Protobuf well-known type <c>Any</c>.
/// </summary>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uAny;

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
  System.SysUtils,
{$ELSE}
  SysUtils,
{$ENDIF}
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.uProtobuf,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uIProtobufMessage,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uIProtobufWellKnownTypeMessage,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufBytes,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufMessageBase,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufString,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufWireFormat,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtoJsonFormat;

type
  /// <summary>
  /// Protobuf message of the well-known type <c>Any</c> (see Any https://protobuf.dev/reference/protobuf/google.protobuf/#any).
  /// Contains an arbitrary serialized message along with a URL that describes the type of the serialized message.
  /// </summary>
  /// <remarks>
  /// TODO contract
  /// The message instance carries transitive ownership of embedded objects in protobuf field values,
  /// and is responsible for their deallocation.
  /// </remarks>
  TAny = class sealed(TProtobufMessageBase, IProtobufWellKnownTypeMessage)
    private
      /// <summary>
      /// TODO contract
      /// </summary>
      FInnerTypeUrl: TProtobufTypeUrl;

      /// <summary>
      /// TODO contract
      /// TODO position always at 0
      /// </summary>
      FValue: TBytesStream;

      /// <summary>
      /// TODO contract
      /// </summary>
      FMessage: IProtobufMessage;

      // TODO contract
      function GetInnerTypeUrl: TProtobufTypeUrl;

      // TODO contract
      procedure SetInnerTypeUrl(aValue: TProtobufTypeUrl);

      // TODO contract
      function GetValue: TBytes;

      // TODO contract
      procedure SetValue(aValue: TBytes);

      // TODO contract
      function GetMessage: IProtobufMessage;

      // TODO contract
      procedure SetMessage(aValue: IProtobufMessage);

      // TODO contract
      procedure PackTypeUrl;

      // TODO contract
      procedure PackValue;

      // TODO contract
      procedure UnpackMessage;

    public
      const
        /// <summary>
        /// Protobuf type URL of this message type.
        /// </summary>
        PROTOBUF_TYPE_URL = PROTOBUF_TYPE_URL_DEFAULT_PREFIX + 'google.protobuf.Any';

        /// <summary>
        /// Protobuf field number of the Protobuf field <c>type_url</c>.
        /// </summary>
        PROTOBUF_FIELD_NUMBER_TYPE_URL = 1;

        /// <summary>
        /// Protobuf field name of the Protobuf field <c>type_url</c>.
        /// </summary>
        PROTOBUF_FIELD_NAME_TYPE_URL = 'type_url';

        /// <summary>
        /// Protobuf field number of the Protobuf field <c>value</c>.
        /// </summary>
        PROTOBUF_FIELD_NUMBER_VALUE = 2;

        /// <summary>
        /// Protobuf field name of the Protobuf field <c>value</c>.
        /// </summary>
        PROTOBUF_FIELD_NAME_VALUE = 'value';

      // TODO contract
      property InnerTypeUrl: TProtobufTypeUrl read GetInnerTypeUrl write SetInnerTypeUrl;

      // TODO contract
      property Value: TBytes read GetValue write SetValue;

      // TODO contract
      property Message: IProtobufMessage read GetMessage write SetMessage;

      // TODO unpacking with type parameter

    // TProtobufMessageBase implementation
    public
      // TODO contract
      function AssignOwnFields(aSource: TProtobufMessageBase): Boolean; override; final;

      // TODO contract
      procedure ClearOwnFields; override; final;

      // TODO contract
      procedure EncodeOwnFields(aDest: TStream); override; final;

      // TODO contract
      procedure MergeFieldFrom(aSource: TStream; aTag: TProtobufTag; aRemainingLength: PUInt32); override; final;

      // TODO contract
      function CalculateOwnFieldsSize: UInt32; override; final;

      // TODO contract
      function GetTypeUrl: TProtobufTypeUrl; override; final;

      /// <summary>
      /// Encodes the message as a JSON value using the ProtoJSON format.
      /// </summary>
      /// <returns>The JSON value encoding the message</return>
      function EncodeJson: TJSONValue; override; final;

      /// <summary>
      /// Decodes the message from a JSON value using the ProtoJSON format.
      /// </summary>
      /// <param name="aSource">The JSON value encoding the message</param>
      /// <exception cref="EProtobufFormatViolation">If the JSON value did not encode a valid Protobuf message</exception>
      /// <exception cref="EProtobufSchemaViolation">If the encoded message was not compatible with this message type</exception>
      procedure DecodeJson(aSource: TJSONValue); override; final;
  end;

implementation

uses
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uTypeRegistry;

// Implementation of TAny

function TAny.GetInnerTypeUrl: TProtobufTypeUrl;
begin
  if (FInnerTypeUrl = '') then PackTypeUrl;
  result := FInnerTypeUrl;
end;

procedure TAny.SetInnerTypeUrl(aValue: TProtobufTypeUrl);
begin
  FMessage := nil;
  FInnerTypeUrl := aValue;
end;

function TAny.GetValue: TBytes;
begin
  if (not Assigned(FValue)) then PackValue;
  result := FValue.Bytes;
end;

procedure TAny.SetValue(aValue: TBytes);
begin
  FMessage := nil;
  FValue.Free;
  FValue := TBytesStream.Create(aValue);
end;

function TAny.GetMessage: IProtobufMessage;
begin
  if (not Assigned(FMessage)) then UnpackMessage;
  result := FMessage;
end;

procedure TAny.SetMessage(aValue: IProtobufMessage);
begin
  FInnerTypeUrl := '';
  FreeAndNil(FValue);
  FMessage := aValue;
end;

procedure TAny.PackTypeUrl;
begin
  if (Assigned(FMessage)) then
  begin
    FInnerTypeUrl := FMessage.TypeUrl;
  end
  else FInnerTypeUrl := '';
end;

procedure TAny.PackValue;
begin
  FValue.Free;
  if (Assigned(FMessage)) then
  begin
    FMessage.Encode(FValue);
    FValue.Position := 0;
  end
  else FValue := nil;
end;

procedure TAny.UnpackMessage;
var
  lMessageClass: TProtobufMessageType;
begin
  if ((FInnerTypeUrl <> '') and Assigned(FValue)) then
  begin
    lMessageClass := TProtobufTypeRegistry.Global.GetType(FInnerTypeUrl);
    if (not Assigned(lMessageClass)) then raise EProtobufUnknownMessageType.Create('Unknown message type: ' + FInnerTypeUrl);
    FMessage := lMessageClass.Create as IProtobufMessage;
    FMessage.Decode(FValue);
    FValue.Position := 0;
  end
  else if ((FInnerTypeUrl = '') and not Assigned(FValue)) then FMessage := nil
  else raise Exception.Create('Both TAny.InnerTypeUrl and TAny.Value need to be assigned to represent a message');
end;

// TProtobufMessageBase implementation of TAny

function TAny.AssignOwnFields(aSource: TProtobufMessageBase): Boolean;
var
  lSource: TAny;
begin
  lSource := aSource as TAny;
  if (not Assigned(lSource)) then Exit(False);
  result := True;
  // Assign FInnerTypeUrl.
  FInnerTypeUrl := lSource.FInnerTypeUrl;
  // Assign FValue.
  FValue.Free;
  if (Assigned(lSource.FValue)) then
  begin
    FValue := TBytesStream.Create;
    FValue.CopyFrom(lSource.FValue, 0);
    lSource.FValue.Position := 0;
    FValue.Position := 0;
  end
  else FValue := nil;
  // Assign FMessage.
  if (Assigned(lSource.FMessage)) then FMessage := IProtobufMessage(TProtobufMessageBase((TProtobufMessageBase(lSource.Message).ClassType).Create))
  else FMessage := nil;
end;

procedure TAny.ClearOwnFields;
begin
  FInnerTypeUrl := '';
  FreeAndNil(FValue);
  FMessage := nil;
end;

procedure TAny.EncodeOwnFields(aDest: TStream);
begin
  if (FInnerTypeUrl = '') then PackTypeUrl;
  EncodeProtobufStringField(aDest, PROTOBUF_FIELD_NUMBER_TYPE_URL, FInnerTypeUrl);
  if (Assigned(FValue)) then EncodeProtobufBytesField(aDest, PROTOBUF_FIELD_NUMBER_VALUE, FValue)
  else if Assigned(FMessage) then EncodeProtobufMessageField(aDest, PROTOBUF_FIELD_NUMBER_VALUE, FMessage);
end;

procedure TAny.MergeFieldFrom(aSource: TStream; aTag: TProtobufTag; aRemainingLength: PUInt32);
begin
  case aTag.FieldNumber of
    PROTOBUF_FIELD_NUMBER_TYPE_URL: FInnerTypeUrl := DecodeProtobufStringField(aSource, aTag.WireType, aRemainingLength);
    PROTOBUF_FIELD_NUMBER_VALUE:
    begin
      FValue.Free;
      FValue := TBytesStream.Create(DecodeProtobufBytesField(aSource, aTag.WireType, aRemainingLength));
    end;
    else MergeUnknownFieldFrom(aSource, aTag, aRemainingLength);
  end;
end;

function TAny.CalculateOwnFieldsSize: UInt32;
begin
  if ((FInnerTypeUrl <> '') and Assigned(FValue)) then result := CalculateProtobufStringFieldSize(PROTOBUF_FIELD_NUMBER_TYPE_URL, FInnerTypeUrl) + CalculateProtobufBytesFieldSize(PROTOBUF_FIELD_NUMBER_VALUE, FValue)
  else if ((FInnerTypeUrl = '') and not Assigned(FValue)) then
  begin
    if (Assigned(FMessage)) then result := CalculateProtobufStringFieldSize(PROTOBUF_FIELD_NUMBER_TYPE_URL, FMessage.TypeUrl) + CalculateProtobufMessageBytesFieldSize(PROTOBUF_FIELD_NUMBER_VALUE, FMessage)
    else result := 0;
  end
  else raise Exception.Create('Both TAny.InnerTypeUrl and TAny.Value need to be assigned to represent a message');
end;

function TAny.GetTypeUrl: TProtobufTypeUrl;
begin
  result := PROTOBUF_TYPE_URL;
end;

// TODO document that handling Anys with unknown type URL as JSON is not supported? It is the same for C#, might not be that useful to support it
// TODO generally: Make sure we dont leak object allocations when we throw a checked exception
function TAny.EncodeJson: TJSONValue;
var
  lJsonObject: TJSONObject;
  lMessageWithGenericJsonRepresentation: IProtobufMessageWithGenericJsonRepresentation;
begin
  if (not Assigned(FMessage)) then UnpackMessage;
  lJsonObject := TJSONObject.Create;
  lJsonObject.AddPair('@type', FMessage.TypeUrl);
  lMessageWithGenericJsonRepresentation := FMessage as IProtobufMessageWithGenericJsonRepresentation;
  if (Assigned(lMessageWithGenericJsonRepresentation)) then lMessageWithGenericJsonRepresentation.EncodeJson(lJsonObject)
  else lJsonObject.AddPair('value', FMessage.EncodeJson);
  result := lJsonObject;
end;

procedure TAny.DecodeJson(aSource: TJSONValue);
var
  lSource: TJSONObject;
  lTypeUrl: TJSONString;
  lMessageClass: TProtobufMessageType;
  lMessageWithGenericJsonRepresentation: IProtobufMessageWithGenericJsonRepresentation;
  lValue: TJSONValue;
begin
  lSource := aSource as TJSONObject;
  if (not Assigned(lSource)) then raise EProtobufSchemaViolation.Create('TODO');
  lTypeUrl := lSource.GetValue('@type') as TJSONString;
  if (not Assigned(lTypeUrl)) then raise EProtobufSchemaViolation.Create('TODO');
  FInnerTypeUrl := lTypeUrl.Value;
  lMessageClass := TProtobufTypeRegistry.Global.GetType(FInnerTypeUrl);
  if (not Assigned(lMessageClass)) then raise EProtobufUnknownMessageType.Create('Unknown message type: ' + FInnerTypeUrl);
  FMessage := lMessageClass.Create as IProtobufMessage;
  lMessageWithGenericJsonRepresentation := FMessage as IProtobufMessageWithGenericJsonRepresentation;
  if (Assigned(lMessageWithGenericJsonRepresentation)) then lMessageWithGenericJsonRepresentation.DecodeJson(lSource, True)
  else
  begin
    lValue := lSource.GetValue('value');
    if (not Assigned(lValue)) then raise EProtobufSchemaViolation.Create('TODO');
    FMessage.DecodeJson(lValue);
  end;
end;

end.

