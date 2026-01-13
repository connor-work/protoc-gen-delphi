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
/// Runtime support for the Protobuf well-known type <c>Timestamp</c>.
/// </summary>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uTimestamp;

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
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uIProtobufWellKnownTypeMessage,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufMessageBase,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufWireFormat;

type
  /// <summary>
  /// Protobuf message of the well-known type <c>Timestamp</c> (see Timestamp https://protobuf.dev/reference/protobuf/google.protobuf/#timestamp).
  /// Represents a point in time independent of any time zone or calendar, represented as seconds and fractions of seconds at nanosecond resolution in UTC Epoch time.
  /// </summary>
  /// <remarks>
  /// TODO contract
  /// The message instance carries transitive ownership of embedded objects in protobuf field values,
  /// and is responsible for their deallocation.
  /// </remarks>
  TTimestamp = class sealed(TProtobufMessageBase, IProtobufWellKnownTypeMessage)
    private
      /// <summary>
      /// <see cref="System.SysUtils.TTimestamp"/> that represents the same point in time, rounded down to the nearest millisecond.
      /// </summary>
      FRtlTimestamp: System.SysUtils.TTimestamp;

      /// <summary>
      /// Number of nanoseconds that have passed at this point in time since <see cref="FRtlTimestamp"/>.
      /// </summary>
      FSubMilliNanoseconds: UInt32;

      // TODO contract
      procedure SetRtlTimestamp(aValue: System.SysUtils.TTimestamp);

      // TODO contract
      function GetDateTime: TDateTime;

      // TODO contract
      procedure SetDateTime(aValue: TDateTime);

    public
      const
        /// <summary>
        /// Protobuf type URL of this message type.
        /// </summary>
        PROTOBUF_TYPE_URL = PROTOBUF_TYPE_URL_DEFAULT_PREFIX + 'google.protobuf.Timestamp';

        /// <summary>
        /// Protobuf field number of the Protobuf field <c>seconds</c>.
        /// </summary>
        PROTOBUF_FIELD_NUMBER_SECONDS = 1;

        /// <summary>
        /// Protobuf field name of the Protobuf field <c>seconds</c>.
        /// </summary>
        PROTOBUF_FIELD_NAME_SECONDS = 'seconds';

        /// <summary>
        /// Protobuf field number of the Protobuf field <c>nanos</c>.
        /// </summary>
        PROTOBUF_FIELD_NUMBER_NANOS = 2;

        /// <summary>
        /// Protobuf field name of the Protobuf field <c>nanos</c>.
        /// </summary>
        PROTOBUF_FIELD_NAME_NANOS = 'nanos';

      // TODO contract
      property RtlTimestamp: System.SysUtils.TTimestamp read FRtlTimestamp write SetRtlTimestamp;

      /// <summary>
      /// Number of nanoseconds that have passed at this point in time since <see cref="RtlTimestamp"/>.
      /// </summary>
      property SubMilliNanoseconds: UInt32 read FSubMilliNanoseconds write FSubMilliNanoseconds;

      // TODO contract
      property DateTime: TDateTime read GetDateTime write SetDateTime;

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

// Implementation of TTimestamp

procedure TTimestamp.SetRtlTimestamp(aValue: System.SysUtils.TTimestamp);
begin
  FRtlTimestamp := aValue;
  FSubMilliNanoseconds := 0;
end;

function TTimestamp.GetDateTime: TDateTime;
begin
  result := TimeStampToDateTime(FRtlTimestamp);
end;

procedure TTimestamp.SetDateTime(aValue: TDateTime);
begin
  SetRtlTimestamp(DateTimeToTimeStamp(aValue));
end;

// TProtobufMessageBase implementation of TTimestamp

function TTimestamp.AssignOwnFields(aSource: TProtobufMessageBase): Boolean;
var
  lSource: TTimestamp;
begin
  lSource := aSource as TTimestamp;
  if (not Assigned(lSource)) then Exit(False);
  result := True;
  FRtlTimestamp := lSource.FRtlTimestamp;
  FSubMilliNanoseconds := lSource.FSubMilliNanoseconds;
end;

procedure TTimestamp.ClearOwnFields;
begin
  // TODO we need a constant for Unix epoch 0
end;

procedure TTimestamp.EncodeOwnFields(aDest: TStream);
begin
  // TODO implementation
end;

procedure TTimestamp.MergeFieldFrom(aSource: TStream; aTag: TProtobufTag; aRemainingLength: PUInt32);
begin
  // TODO if we encounter a known field, first convert to seconds/nanos and then back
  case aTag.FieldNumber of
    PROTOBUF_FIELD_NUMBER_SECONDS:
    begin
      // TODO implementation
    end;
    PROTOBUF_FIELD_NUMBER_NANOS:
    begin
      // TODO implementation
    end;
    else MergeUnknownFieldFrom(aSource, aTag, aRemainingLength);
  end;
end;

function TTimestamp.CalculateOwnFieldsSize: UInt32;
begin
  // TODO implementation
end;

function TTimestamp.GetTypeUrl: TProtobufTypeUrl;
begin
  result := PROTOBUF_TYPE_URL;
end;

function TTimestamp.EncodeJson: TJSONValue;
begin
  // TODO implementation
end;

procedure TTimestamp.DecodeJson(aSource: TJSONValue);
begin
  // TODO implementation
end;

end.

