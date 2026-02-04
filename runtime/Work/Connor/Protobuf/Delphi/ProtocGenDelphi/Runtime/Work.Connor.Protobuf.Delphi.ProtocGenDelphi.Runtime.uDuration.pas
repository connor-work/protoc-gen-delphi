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
/// Runtime support for the Protobuf well-known type <c>Duration</c>.
/// </summary>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uDuration;

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
  System.RegularExpressions,
{$ELSE}
  RegularExpressions,
{$ENDIF}
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.SysUtils,
{$ELSE}
  SysUtils,
{$ENDIF}
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.uProtobuf,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uIProtobufWellKnownTypeMessage,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufInt32,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufInt64,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufMessageBase,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufWireFormat;

type
  /// <summary>
  /// Protobuf message of the well-known type <c>Duration</c> (see Duration https://protobuf.dev/reference/protobuf/google.protobuf/#duration).
  /// Represents a signed, fixed-length span of time represented as a count of seconds and fractions of seconds at nanosecond resolution,
  /// independent of any calendar and concepts like "day" or "month".
  /// </summary>
  /// <remarks>
  /// TODO contract
  /// The message instance carries transitive ownership of embedded objects in protobuf field values,
  /// and is responsible for their deallocation.
  /// </remarks>
  TDuration = class sealed(TProtobufMessageBase, IProtobufWellKnownTypeMessage)
    private
      /// <summary>
      /// The duration, rounded towards zero to the nearest whole second, in seconds.
      /// </summary>
      FSeconds: Int64;

      /// <summary>
      /// The remaining signed duration to add to <see cref="FSeconds"/> to arrive at the exact duration, in nanoseconds.
      /// </summary>
      FSubSecondNanoseconds: Int32;

      // TODO contract
      class var JsonRegex: TRegEx;

    public
      const
        /// <summary>
        /// Protobuf type URL of this message type.
        /// </summary>
        PROTOBUF_TYPE_URL = PROTOBUF_TYPE_URL_DEFAULT_PREFIX + 'google.protobuf.Duration';

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

      /// <summary>
      /// The duration, rounded towards zero to the nearest whole second, in seconds.
      /// </summary>
      property Seconds: Int64 read FSeconds write FSeconds;

      /// <summary>
      /// The remaining signed duration to add to <see cref="Seconds"/> to arrive at the exact duration, in nanoseconds.
      /// </summary>
      property SubSecondNanoseconds: Int32 read FSubSecondNanoseconds write FSubSecondNanoseconds;

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

// TProtobufMessageBase implementation of TDuration

function TDuration.AssignOwnFields(aSource: TProtobufMessageBase): Boolean;
var
  lSource: TDuration;
begin
  if (not (aSource is TDuration)) then Exit(False);
  lSource := TDuration(aSource);
  result := True;
  FSeconds := lSource.FSeconds;
  FSubSecondNanoseconds := lSource.FSubSecondNanoseconds;
end;

procedure TDuration.ClearOwnFields;
begin
  FSeconds := 0;
  FSubSecondNanoseconds := 0;
end;

procedure TDuration.EncodeOwnFields(aDest: TStream);
begin
  EncodeProtobufInt64Field(aDest, PROTOBUF_FIELD_NUMBER_SECONDS, FSeconds);
  EncodeProtobufInt32Field(aDest, PROTOBUF_FIELD_NUMBER_NANOS, FSubSecondNanoseconds);
end;

procedure TDuration.MergeFieldFrom(aSource: TStream; aTag: TProtobufTag; aRemainingLength: PUInt32);
begin
  case aTag.FieldNumber of
    PROTOBUF_FIELD_NUMBER_SECONDS: FSeconds := DecodeProtobufInt64Field(aSource, aTag.WireType, aRemainingLength);
    PROTOBUF_FIELD_NUMBER_NANOS: FSubSecondNanoseconds := DecodeProtobufInt32Field(aSource, aTag.WireType, aRemainingLength);
    else MergeUnknownFieldFrom(aSource, aTag, aRemainingLength);
  end;
end;

function TDuration.CalculateOwnFieldsSize: UInt32;
begin
  result := CalculateProtobufInt64FieldSize(PROTOBUF_FIELD_NUMBER_SECONDS, FSeconds) + CalculateProtobufInt32FieldSize(PROTOBUF_FIELD_NUMBER_NANOS, FSubSecondNanoseconds);
end;

function TDuration.GetTypeUrl: TProtobufTypeUrl;
begin
  result := PROTOBUF_TYPE_URL;
end;

function TDuration.EncodeJson: TJSONValue;
var
  lStringBuilder: TStringBuilder;
  lAbsoluteNanoSeconds: Int64;
begin
  lStringBuilder := TStringBuilder.Create(24);
  if ((FSeconds = 0) and (FSubSecondNanoseconds < 0)) then lStringBuilder.Append('-');
  lStringBuilder.Append(FSeconds);
  // "Generated output always contains 0, 3, 6, or 9 fractional digits, depending on required precision, [...]"
  if (FSubSecondNanoseconds <> 0) then
  begin
    lAbsoluteNanoSeconds := Abs(FSubSecondNanoseconds);
    lStringBuilder.Append('.');
    if (lAbsoluteNanoSeconds mod 1000000 = 0) then lStringBuilder.AppendFormat('%.3d', [lAbsoluteNanoSeconds div 1000000])
    else if (lAbsoluteNanoSeconds mod 1000 = 0) then lStringBuilder.AppendFormat('%.6d', [lAbsoluteNanoSeconds div 1000])
    else lStringBuilder.AppendFormat('%.9d', [lAbsoluteNanoSeconds]);
  end;
  // "[...] followed by the suffix 's'."
  lStringBuilder.Append('s');
  result := TJSONString.Create(lStringBuilder.ToString);
  lStringBuilder.Free;
end;

procedure TDuration.DecodeJson(aSource: TJSONValue);
var
  lSource: TJSONString;
  lRegexMatch: TMatch;
  lSecondsFractionGroup: TGroup;
begin
  if (not (aSource is TJSONString)) then raise EProtobufSchemaViolation.Create('TODO');
  lSource := TJSONString(aSource);
  lRegexMatch := JsonRegex.Match(lSource.Value);
  if (not lRegexMatch.Success) then raise EProtobufSchemaViolation.Create('TODO');
  // TODO range check
  FSeconds := StrToInt64(lRegexMatch.Groups['whole_seconds'].Value);
  lSecondsFractionGroup := lRegexMatch.Groups['seconds_fraction'];
  if (lSecondsFractionGroup.Success) then
  begin
    FSubSecondNanoseconds := StrToInt(lSecondsFractionGroup.Value.PadRight(9, '0'));
    // NOTE Even if the group is not matched, TGroup.Success returns True for unclear reasons.
    if (lRegexMatch.Groups['negative_sign'].Length <> 0) then FSubSecondNanoseconds := -FSubSecondNanoseconds;
  end
  else FSubSecondNanoseconds := 0;
end;

initialization

TDuration.JsonRegex.Create('^(?<whole_seconds>(?<negative_sign>-)?[0-9]{1,12})(?:\.(?<seconds_fraction>[0-9]{1,9}))?s$', [roCompiled])

end.

