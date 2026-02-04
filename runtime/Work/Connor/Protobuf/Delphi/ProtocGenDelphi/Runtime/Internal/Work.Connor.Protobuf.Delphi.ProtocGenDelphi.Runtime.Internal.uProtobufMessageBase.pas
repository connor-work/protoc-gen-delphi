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
/// Runtime support for Protobuf message types.
/// </summary>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufMessageBase;

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
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uIProtobufMessage,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufFixed32,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufWireFormat;

type
  // TODO contract
  TProtobufMessageBase = class abstract(TInterfacedPersistent, IProtobufMessage)
    private
      /// <summary>
      /// TODO contract
      /// TODO absent if no unknown fields
      /// </summary>
      FUnknownFields: TProtobufEncodedFieldsMap;

    protected
      // TODO free own fields? (nested messages, repeated fields) create own fields? (repeated fields)

      // TODO contract
      function AssignOwnFields(aSource: TProtobufMessageBase): Boolean; virtual; abstract;

      // TODO contract
      procedure ClearOwnFields; virtual; abstract;

      // TODO contract
      procedure EncodeOwnFields(aDest: TStream); virtual; abstract;

      // TODO contract
      procedure MergeFieldFrom(aSource: TStream; aTag: TProtobufTag; aRemainingLength: PUInt32); virtual; abstract;

      // TODO contract
      procedure MergeUnknownFieldFrom(aSource: TStream; aTag: TProtobufTag; aRemainingLength: PUInt32);

      // TODO contract
      function CalculateOwnFieldsSize: UInt32; virtual; abstract;

    public
      // TODO contract
      constructor Create; virtual;

      // TODO contract
      destructor Destroy; override;

    // TInterfacedPersistent implementation
    public
      /// <summary>
      /// Copies the data from another message to this one.
      /// </summary>
      /// <param name="aSource">Object to copy from</param>
      /// <remarks>
      /// The object must be another message of the same message type.
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

      // TODO contract
      procedure MergeFrom(aSource: TStream; aRemainingLength: PUInt32);

      /// <summary>
      /// Calculates the size of the message when encoded using the Protobuf binary wire format.
      /// </summary>
      /// <returns>The number of bytes that encode the message</return>
      function CalculateSize: UInt32;

      // TODO contract
      function GetTypeUrl: TProtobufTypeUrl; virtual; abstract;

      /// <summary>
      /// Encodes the message as a JSON value using the ProtoJSON format.
      /// </summary>
      /// <returns>The JSON value encoding the message</return>
      function EncodeJson: TJSONValue; virtual; abstract;

      /// <summary>
      /// Decodes the message from a JSON value using the ProtoJSON format.
      /// </summary>
      /// <param name="aSource">The JSON value encoding the message</param>
      /// <exception cref="EProtobufFormatViolation">If the JSON value did not encode a valid Protobuf message</exception>
      /// <exception cref="EProtobufSchemaViolation">If the encoded message was not compatible with this message type</exception>
      procedure DecodeJson(aSource: TJSONValue); virtual; abstract;
  end;

  // TODO contract
  TProtobufMessageType = class of TProtobufMessageBase;

implementation

// Implementation of TProtobufMessageBase

constructor TProtobufMessageBase.Create;
begin
  // TODO instead of virtual, call createownfields?
end;

destructor TProtobufMessageBase.Destroy;
begin
  // TODO instead of virtual, call destroyownfields?
  FUnknownFields.Free;
  inherited;
end;

procedure TProtobufMessageBase.MergeUnknownFieldFrom(aSource: TStream; aTag: TProtobufTag; aRemainingLength: PUInt32);
var
  lUnknownField: TProtobufEncodedField;
begin
  if (not Assigned(FUnknownFields)) then FUnknownFields := TProtobufEncodedFieldsMap.Create([doOwnsValues]);
  lUnknownField := TProtobufEncodedField.Create;
  lUnknownField.DecodePayload(aSource, aTag, aRemainingLength);
  if (not FUnknownFields.ContainsKey(aTag.FieldNumber)) then FUnknownFields.Add(aTag.FieldNumber, TObjectList<TProtobufEncodedField>.Create);
  FUnknownFields[aTag.FieldNumber].Add(lUnknownField);
end;

// TInterfacedPersistent implementation of TProtobufMessageBase
procedure TProtobufMessageBase.Assign(aSource: TPersistent);
var
  lSource: TProtobufMessageBase;
  lSourceUnknownField: TPair<TProtobufFieldNumber, TObjectList<TProtobufEncodedField>>;
  lUnknownFieldRecords: TObjectList<TProtobufEncodedField>;
  lSourceUnknownFieldRecord: TProtobufEncodedField;
  lUnknownFieldRecord: TProtobufEncodedField;
begin
  if (not (lSource is TProtobufMessageBase)) then
  begin
    inherited;
    Exit;
  end;
  lSource := TProtobufMessageBase(aSource);
  if (not AssignOwnFields(lSource)) then begin
    inherited;
    Exit;
  end;
  FUnknownFields.Free;
  if (Assigned(lSource.FUnknownFields)) then
  begin
    FUnknownFields := TProtobufEncodedFieldsMap.Create([doOwnsValues]);
    for lSourceUnknownField in lSource.FUnknownFields do
    begin
      lUnknownFieldRecords := TObjectList<TProtobufEncodedField>.Create;
      for lSourceUnknownFieldRecord in lSourceUnknownField.Value do
      begin
        lUnknownFieldRecord := TProtobufEncodedField.Create;
        lUnknownFieldRecord.Assign(lSourceUnknownFieldRecord);
        lUnknownFieldRecords.Add(lUnknownFieldRecord);
      end;
      FUnknownFields.Add(lSourceUnknownField.Key, lUnknownFieldRecords);
    end;
  end
  else FUnknownFields := nil;
end;

// IProtobufMessage implementation of TProtobufMessageBase

procedure TProtobufMessageBase.Clear;
begin
  ClearOwnFields;
  FreeAndNil(FUnknownFields);
end;

procedure TProtobufMessageBase.Encode(aDest: TStream);
begin
  EncodeOwnFields(aDest);
  if (Assigned(FUnknownFields)) then EncodeProtobufFields(aDest, FUnknownFields);
end;

procedure TProtobufMessageBase.EncodeDelimited(aDest: TStream);
begin
  EncodeProtobufFixed32(aDest, CalculateSize);
  Encode(aDest);
end;

procedure TProtobufMessageBase.Decode(aSource: TStream);
begin
  Clear;
  MergeFrom(aSource, nil);
end;

procedure TProtobufMessageBase.DecodeDelimited(aSource: TStream);
var
  lLength: UInt32;
begin
  lLength := DecodeProtobufFixed32(aSource, nil);
  MergeFrom(aSource, @lLength);
end;

procedure TProtobufMessageBase.MergeFrom(aSource: TStream; aRemainingLength: PUInt32);
var
  lTag: TProtobufTag;
begin
  while TryDecodeProtobufTag(aSource, lTag, aRemainingLength) do MergeFieldFrom(aSource, lTag, aRemainingLength);
end;

function TProtobufMessageBase.CalculateSize: UInt32;
begin
  result := CalculateOwnFieldsSize;
  if (Assigned(FUnknownFields)) then result := result + CalculateProtobufFieldsSize(FUnknownFields);
end;

end.

