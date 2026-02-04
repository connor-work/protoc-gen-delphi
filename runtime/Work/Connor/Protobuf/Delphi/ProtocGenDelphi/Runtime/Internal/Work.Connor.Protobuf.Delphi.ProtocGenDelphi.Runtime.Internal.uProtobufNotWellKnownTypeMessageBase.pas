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
/// Runtime support for Protobuf message types that are not one of the well-known types.
/// </summary>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufNotWellKnownTypeMessageBase;

{$INCLUDE Work.Connor.Delphi.CompilerFeatures.inc}

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.JSON,
{$ELSE}
  JSON,
{$ENDIF}
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.uProtobuf,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uIProtobufMessage,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufMessageBase,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtoJsonFormat;

type
  // TODO contract
  TProtobufNotWellKnownTypeMessageBase = class abstract(TProtobufMessageBase, IProtobufMessageWithGenericJsonRepresentation)
    protected
      // TODO free own fields? (nested messages, repeated fields) create own fields? (repeated fields)

    // TProtobufMessageBase implementation
    public
      /// <summary>
      /// Encodes the message as a JSON value using the ProtoJSON format.
      /// </summary>
      /// <returns>The JSON value encoding the message</return>
      function EncodeJson: TJSONValue; overload; override; final;

      /// <summary>
      /// Decodes the message from a JSON value using the ProtoJSON format.
      /// </summary>
      /// <param name="aSource">The JSON value encoding the message</param>
      /// <exception cref="EProtobufFormatViolation">If the JSON value did not encode a valid Protobuf message</exception>
      /// <exception cref="EProtobufSchemaViolation">If the encoded message was not compatible with this message type</exception>
      procedure DecodeJson(aSource: TJSONValue); overload; override; final;

    // IProtobufMessageWithGenericJsonRepresentation implementation
    public
      /// <summary>
      /// Encodes the message as a JSON object using the ProtoJSON format.
      /// </summary>
      /// <returns>TODO contract</returns>
      function EncodeJsonObject: TJSONObject;

      /// <summary>
      /// Encodes the message as a JSON object using the ProtoJSON format and writes it to a <see cref="TJSONObject"/>.
      /// </summary>
      /// <param name="aDest">The <see cref="TJSONObject"/> that the encoded message is written to</param>
      procedure EncodeJson(aDest: TJSONObject); overload; virtual; abstract;

      /// <summary>
      /// Fills the message's Protobuf fields by decoding the message from a JSON object, using the ProtoJSON format.
      /// </summary>
      /// <param name="aSource">The JSON object that the message is decoded from</param>
      /// <param name="aSkipType">TODO contract</param>
      /// <exception cref="EDecodingSchemaError">If the format of the JSON object was not compatible with this message type</exception>
      /// <remarks>
      /// Protobuf fields that are not present in the JSON object are rendered absent by setting them to their default values.
      /// This may cause the destruction of transitively owned objects (this is also the case when a present field overwrites a previous value).
      /// Developers must ensure that no shared ownership of current field values or further nested embedded objects is held.
      /// </remarks>
      procedure DecodeJson(aSource: TJSONObject; aSkipType: Boolean = False); overload;

      // TODO contract
      function MergeFieldFromJson(aSource: TJSONPair): Boolean; virtual; abstract;
  end;

implementation

// TProtobufMessageBase implementation of TProtobufNotWellKnownTypeMessageBase

function TProtobufNotWellKnownTypeMessageBase.EncodeJson: TJSONValue;
begin
  result := EncodeJsonObject;
end;

procedure TProtobufNotWellKnownTypeMessageBase.DecodeJson(aSource: TJSONValue);
var
  lSource: TJSONObject;
begin
  if (not (aSource is TJSONObject)) then raise EProtobufSchemaViolation.Create('TODO');
  lSource := TJSONObject(aSource); 
  DecodeJson(lSource);
end;

// IProtobufMessageWithGenericJsonRepresentation implementation of TProtobufNotWellKnownTypeMessageBase

function TProtobufNotWellKnownTypeMessageBase.EncodeJsonObject: TJSONObject;
begin
  result := TJSONObject.Create;
  EncodeJson(result);
end;

procedure TProtobufNotWellKnownTypeMessageBase.DecodeJson(aSource: TJSONObject; aSkipType: Boolean = False);
var
  lPair: TJSONPair;
begin
  for lPair in aSource do
  begin
    if (aSkipType and (lPair.JsonString.Value = '@type')) then Continue;
    // TODO support JSON unknown fields
    if (not MergeFieldFromJson(lPair)) then raise EProtobufSchemaViolation.Create('TODO');
  end;
end;

end.

