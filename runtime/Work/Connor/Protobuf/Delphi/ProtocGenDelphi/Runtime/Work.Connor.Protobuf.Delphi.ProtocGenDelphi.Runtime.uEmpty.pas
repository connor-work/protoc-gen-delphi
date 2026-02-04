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
/// Runtime support for the Protobuf well-known type <c>Empty</c>.
/// </summary>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uEmpty;

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
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.uProtobuf,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uIProtobufWellKnownTypeMessage,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufMessageBase,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufWireFormat;

type
  /// <summary>
  /// Protobuf message of the well-known type <c>Empty</c> (see Empty https://protobuf.dev/reference/protobuf/google.protobuf/#empty).
  /// Represents a generic empty message.
  /// </summary>
  /// <remarks>
  /// TODO contract
  /// The message instance carries transitive ownership of embedded objects in protobuf field values,
  /// and is responsible for their deallocation.
  /// </remarks>
  TEmpty = class sealed(TProtobufMessageBase, IProtobufWellKnownTypeMessage)
    public
      const
        /// <summary>
        /// Protobuf type URL of this message type.
        /// </summary>
        PROTOBUF_TYPE_URL = PROTOBUF_TYPE_URL_DEFAULT_PREFIX + 'google.protobuf.Empty';

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

// TProtobufMessage implementation of TEmpty

function TEmpty.AssignOwnFields(aSource: TProtobufMessageBase): Boolean;
begin
  result := aSource is TEmpty;
end;

procedure TEmpty.ClearOwnFields;
begin
end;

procedure TEmpty.EncodeOwnFields(aDest: TStream);
begin
end;

procedure TEmpty.MergeFieldFrom(aSource: TStream; aTag: TProtobufTag; aRemainingLength: PUInt32);
begin
  MergeUnknownFieldFrom(aSource, aTag, aRemainingLength);
end;

function TEmpty.CalculateOwnFieldsSize: UInt32;
begin
  result := 0;
end;

function TEmpty.GetTypeUrl: TProtobufTypeUrl;
begin
  result := PROTOBUF_TYPE_URL;
end;

function TEmpty.EncodeJson: TJSONValue;
begin
  result := TJSONObject.Create;
end;

procedure TEmpty.DecodeJson(aSource: TJSONValue);
var
  lSource: TJSONObject;
begin
  if (not (aSource is TJSONObject)) then raise EProtobufSchemaViolation.Create('google.protobuf.Empty message is not encoded as a JSON object');
  lSource := TJSONObject(aSource);
  if (not lSource.IsEmpty) then EProtobufSchemaViolation.Create('google.protobuf.Empty message is not encoded as an empty JSON object');
  Clear;
end;

end.

