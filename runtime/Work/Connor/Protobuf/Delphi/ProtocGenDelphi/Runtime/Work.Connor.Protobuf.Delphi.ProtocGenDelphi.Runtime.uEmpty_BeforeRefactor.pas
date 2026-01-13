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
  System.Generics.Collections,
{$ELSE}
  Generics.Collections,
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
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uIProtobufWellKnownTypeMessage,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufFixed32,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufWireFormat;

type
  /// <summary>
  /// Protobuf message of the well-known type <c>Empty</c> (see Empty https://protobuf.dev/reference/protobuf/google.protobuf/#empty).
  /// Represents a generic empty message.
  /// </summary>
  /// <remarks>
  /// TODO
  /// The message instance carries transitive ownership of embedded objects in protobuf field values,
  /// and is responsible for their deallocation.
  /// </remarks>
  TEmpty = class sealed(TInterfacedPersistent, IProtobufWellKnownTypeMessage)
    private
      /// <summary>
      /// TODO absent if no unknown fields
      /// </summary>
      FUnknownFields: TProtobufEncodedFieldsMap;

      // TODO
      destructor Destroy; override; final;

      // TODO
      procedure MergeFieldFrom(aSource: TStream; aTag: TProtobufTag; aRemainingLength: PUInt32);

    public
      const
        /// <summary>
        /// Protobuf type URL of this message type.
        /// </summary>
        PROTOBUF_TYPE_URL = PROTOBUF_TYPE_URL_DEFAULT_PREFIX + 'google.protobuf.Empty';

    // TPersistent implementation
    public
      /// <summary>
      /// Copies the data from another <c>Empty</c> message to this one.
      /// </summary>
      /// <param name="aSource">Object to copy from</param>
      /// <remarks>
      /// The object must be another <see cref="TDuration"/>.
      /// TODO This performs a deep copy; hence, no ownership is shared.
      /// This causes the destruction of <see cref="Payload"/>; developers must ensure that no shared ownership is held.
      /// </remarks>
      procedure Assign(aSource: TPersistent); override;

    // uIProtobufMessage implementation
    public

      /// <summary>
      /// Renders all Protobuf fields absent by setting them to their default values.
      /// </summary>
      /// <remarks>
      /// The resulting instance state is equivalent to a newly constructed empty message.
      /// Protobuf's interpretation of the absence of a field may be counterintuitive for Delphi developers.
      /// For a detailed explanation, see https://developers.google.com/protocol-buffers/docs/proto3#default.
      /// This procedure may cause the destruction of transitively owned objects.
      /// Developers must ensure that no shared ownership of current field values or further nested embedded objects is held.
      /// </remarks>
      procedure Clear;

      /// <summary>
      /// Encodes the message using the Protobuf binary wire format and writes it to a stream.
      /// </summary>
      /// <param name="aDest">The stream that the encoded message is written to</param>
      /// <remarks>
      /// Since the Protobuf binary wire format does not include length information for top-level messages,
      /// the recipient may not be able to detect the end of the message when reading it from a stream (see Streaming Multiple Messages https://protobuf.dev/programming-guides/techniques/#streaming).
      /// If this is required, use <see cref="EncodeDelimited"/> instead.
      /// </remarks>
      procedure Encode(aDest: TStream);

      /// <summary>
      /// Encodes the message using the Protobuf binary wire format and writes it to a stream, prefixed with length information.
      /// </summary>
      /// <param name="aDest">The stream that the encoded message is written to</param>
      /// <remarks>
      /// Unlike <see cref="Encode"/>, this method enables the recipient to detect the end of the message by decoding it using
      /// <see cref="DecodeDelimited"/> (see Streaming Multiple Messages https://protobuf.dev/programming-guides/techniques/#streaming).
      /// The length information is encoded as a 4-byte little-endian unsigned integer equal to the number of bytes that encode the message.
      /// </remarks>
      procedure EncodeDelimited(aDest: TStream);

      /// <summary>
      /// Fills the message's Protobuf fields by decoding the message using the Protobuf binary wire format from data that is read from a stream.
      /// Data is read until <see cref="TStream.Read"/> returns 0.
      /// </summary>
      /// <param name="aSource">The stream that the data is read from</param>
      /// <exception cref="EProtobufFormatViolation">If the data on the stream did not encode a valid Protobuf message</exception>
      /// <exception cref="EProtobufSchemaViolation">If the message on the stream was not compatible with this message type</exception>
      /// <remarks>
      /// Protobuf fields that are not present in the read data are rendered absent by setting them to their default values.
      /// This may cause the destruction of transitively owned objects (this is also the case when a present fields overwrites a previous value).
      /// Developers must ensure that no shared ownership of current field values or further nested embedded objects is held.
      /// This method should not be used on a stream where the actual size of its contents may not be known yet (this might result in data loss, see Streaming Multiple Messages https://protobuf.dev/programming-guides/techniques/#streaming).
      /// If this is required, use <see cref="DecodeDelimited"/> instead.
      /// </remarks>
      procedure Decode(aSource: TStream);

      /// <summary>
      /// Fills the message's protobuf fields by decoding the message using the Protobuf binary wire format from data that is read from a stream.
      /// The data must be prefixed with message length information, as implemented by <see cref="EncodeDelimited"/>.
      /// </summary>
      /// <param name="aSource">The stream that the data is read from</param>
      /// <exception cref="EProtobufFormatViolation">If the data on the stream did not encode a valid combination of length information and a Protobuf message</exception>
      /// <exception cref="EProtobufSchemaViolation">If the message on the stream was not compatible with this message type</exception>
      /// <remarks>
      /// The length information is encoded as a 4-byte little-endian unsigned integer equal to the number of bytes that encode the message.
      /// Protobuf fields that are not present in the read data are rendered absent by setting them to their default values.
      /// This may cause the destruction of transitively owned objects (this is also the case when a present fields overwrites a previous value).
      /// Developers must ensure that no shared ownership of current field values or further nested embedded objects is held.
      /// </remarks>
      procedure DecodeDelimited(aSource: TStream);

      /// TODO
      procedure MergeFrom(aSource: TStream; aRemainingLength: PUInt32);

      /// <summary>
      /// Calculates the size of the message when encoded using the Protobuf binary wire format.
      /// </summary>
      /// <returns>The number of bytes that encode the message</return>
      function CalculateSize: UInt32;

      // TODO
      function GetTypeUrl: UnicodeString;

      /// <summary>
      /// Encodes the message as a JSON value using the ProtoJSON format.
      /// </summary>
      /// <returns>The JSON value encoding the message</return>
      function EncodeJson: TJSONValue;

      /// <summary>
      /// Decodes the message from a JSON value using the ProtoJSON format.
      /// </summary>
      /// <param name="aSource">The JSON value encoding the message</param>
      /// <exception cref="EProtobufFormatViolation">If the JSON value did not encode a valid Protobuf message</exception>
      /// <exception cref="EProtobufSchemaViolation">If the encoded message was not compatible with this message type</exception>
      procedure DecodeJson(aSource: TJSONValue);
  end;

implementation

// Implementation of TEmpty

destructor TEmpty.Destroy;
begin
  FUnknownFields.Free;
  inherited;
end;

procedure TEmpty.MergeFieldFrom(aSource: TStream; aTag: TProtobufTag; aRemainingLength: PUInt32);
var
  lUnknownField: TProtobufEncodedField;
begin
  if (not Assigned(FUnknownFields)) then FUnknownFields := TProtobufEncodedFieldsMap.Create;
  lUnknownField := TProtobufEncodedField.Create;
  lUnknownField.DecodePayload(aSource, aTag, aRemainingLength);
  if (not FUnknownFields.ContainsKey(aTag.FieldNumber)) then FUnknownFields[aTag.FieldNumber] := TObjectList<TProtobufEncodedField>.Create;
  FUnknownFields[aTag.FieldNumber].Add(lUnknownField);
end;

// TPersistent implementation of TEmpty
procedure TEmpty.Assign(aSource: TPersistent);
var
  lSource: TEmpty;
  lSourceUnknownField: TPair<TProtobufFieldNumber, TObjectList<TProtobufEncodedField>>;
  lUnknownFieldRecords: TObjectList<TProtobufEncodedField>;
  lSourceUnknownFieldRecord: TProtobufEncodedField;
  lUnknownFieldRecord: TProtobufEncodedField;
begin
  lSource := aSource as TEmpty;
  if (not(aSource is TEmpty)) then inherited
  else
  begin
    // Assign FUnknownFields.
    FUnknownFields.Free;
    if (Assigned(lSource.FUnknownFields)) then
    begin
      FUnknownFields := TProtobufEncodedFieldsMap.Create;
      for lSourceUnknownField in lSource.FUnknownFields do
      begin
        lUnknownFieldRecords := TObjectList<TProtobufEncodedField>.Create;
        for lSourceUnknownFieldRecord in lSourceUnknownField.Value do
        begin
          lUnknownFieldRecord := TProtobufEncodedField.Create;
          lUnknownFieldRecord.Assign(lSourceUnknownFieldRecord);
          lUnknownFieldRecords.Add(lUnknownFieldRecord);
        end;
        FUnknownFields[lSourceUnknownField.Key] := lUnknownFieldRecords;
      end;
    end
    else FUnknownFields := nil;
  end;
end;

// IProtobufMessage implementation of TEmpty

procedure TEmpty.Clear;
begin
  FreeAndNil(FUnknownFields);
end;

procedure TEmpty.Encode(aDest: TStream);
begin
  if (Assigned(FUnknownFields)) then EncodeProtobufFields(aDest, FUnknownFields);
end;

procedure TEmpty.EncodeDelimited(aDest: TStream);
begin
  EncodeProtobufFixed32(aDest, CalculateSize);
  Encode(aDest);
end;

procedure TEmpty.Decode(aSource: TStream);
begin
  Clear;
  MergeFrom(aSource, nil);
end;

procedure TEmpty.DecodeDelimited(aSource: TStream);
var
  lLength: UInt32;
begin
  lLength := DecodeProtobufFixed32(aSource);
  MergeFrom(aSource, @lLength);
end;

procedure TEmpty.MergeFrom(aSource: TStream; aRemainingLength: PUInt32);
var
  lTag: TProtobufTag;
begin
  while TryDecodeProtobufTag(aSource, lTag, aRemainingLength) do MergeFieldFrom(aSource, lTag, aRemainingLength);
end;

function TEmpty.CalculateSize: UInt32;
begin
  if (Assigned(FUnknownFields)) then result := CalculateProtobufFieldsSize(FUnknownFields)
  else result := 0;
end;

function TEmpty.GetTypeUrl: UnicodeString;
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
  if (aSource.ClassType <> TJSONObject) then raise EProtobufSchemaViolation.Create('google.protobuf.Empty message is not encoded as a JSON object');
  lSource := aSource as TJSONObject;
  if (not lSource.IsEmpty) then EProtobufSchemaViolation.Create('google.protobuf.Empty message is not encoded as an empty JSON object');
  Clear;
end;

end.

