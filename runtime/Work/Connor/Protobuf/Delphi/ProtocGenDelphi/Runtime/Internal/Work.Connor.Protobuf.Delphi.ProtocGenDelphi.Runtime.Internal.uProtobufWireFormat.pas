/// Copyright 2025 Connor Erdmann (connor.work)
/// Copyright 2020 Julien Scholz
/// Copyright 2020 Sotax AG
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
/// Runtime support for the Protobuf binary wire format.
/// </summary>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufWireFormat;

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
  System.SysUtils,
{$ELSE}
  SysUtils,
{$ENDIF}
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.uProtobuf,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uIProtobufMessage;

const
  /// <summary>
  /// Maximum number of bytes used to encode a varint using the Protobuf binary wire format.
  /// </summary>
  PROTOBUF_MAX_VARINT_SIZE = 10;

type

{$SCOPEDENUMS ON}

  /// <summary>
  /// Enumeration of all Protobuf wire types (see Message Structure https://protobuf.dev/programming-guides/encoding/#structure).
  /// </summary>
  /// <remarks>
  /// Each value's ordinal value is equal to the 3-bit value that encodes it in the Protobuf binary wire format.
  /// </remarks>
  TProtobufWireType = (
    /// <summary>
    /// Protobuf wire type <c>VARINT</c> (a varint).
    /// </summary>
    /// <remarks>
    /// Used for the Protobuf types <c>int32</c>, <c>int64</c>, <c>uint32</c>, <c>uint64</c>, <c>sint32</c>, <c>sint64</c>, and <c>bool</c> and for Protobuf enum types.
    /// </remarks>
    Varint = 0,

    /// <summary>
    /// Protobuf wire type <c>I64</c> (a fixed-width, 64-bit blob).
    /// </summary>
    /// <remarks>
    /// Used for the Protobuf types <c>fixed64</c>, <c>sfixed64</c> and <c>double</c>.
    /// </remarks>
    I64 = 1,

    /// <summary>
    /// Protobuf wire <c>LEN</c> (a length-prefixed blob).
    /// </summary>
    /// <remarks>
    /// Used for the Protobuf types <c>string</c> and <c>bytes</c>, for embedded Protobuf messages and for packed repeated fields.
    /// </remarks>
    Len = 2,

    /// <summary>
    /// TODO
    /// </summary>
    Sgroup = 3,

    /// <summary>
    /// TODO
    /// </summary>
    Egroup = 4,

    /// <summary>
    /// Protobuf wire type <c>I32</c> (a fixed-width, 32-bit blob).
    /// </summary>
    /// <remarks>
    /// Used for the Protobuf types <c>fixed32</c>, <c>sfixed32</c> and <c>float</c>.
    /// </remarks>
    I32 = 5
  );

{$SCOPEDENUMS OFF}

  /// <summary>
  /// Represents a Protobuf <i>tag</i> i.e. the part of a record, in a Protobuf message that is encoded using the Protobuf binary wire format,
  /// that encodes the Protobuf field's field number and wire type (see Message Structure https://protobuf.dev/programming-guides/encoding/#structure).
  /// </summary>
  TProtobufTag = record
    private
      /// <summary>
      /// Backing field for <see cref="FieldNumber"/>.
      /// </summary>
      FFieldNumber: TProtobufFieldNumber;

      /// <summary>
      /// Backing field for <see cref="WireType"/>.
      /// </summary>
      FWireType: TProtobufWireType;

      /// TODO
      function EncodeToVarint: UInt64;

      /// TODO
      class function DecodeFromVarint(aVarint: UInt64): TProtobufTag; static;

    public
      /// <summary>
      /// The field's field number.
      /// </summary>
      property FieldNumber: TProtobufFieldNumber read FFieldNumber;

      /// <summary>
      /// The field's wire type.
      /// </summary>
      property WireType: TProtobufWireType read FWireType;

      /// <summary>
      /// Constructs a Protobuf tag.
      /// </summary>
      /// <param name="aFieldNumber">The field's field number</param>
      /// <param name="aWireType">The field's wire type</param>
      class function WithData(aFieldNumber: TProtobufFieldNumber; aWireType: TProtobufWireType): TProtobufTag; static;

      /// <summary>
      /// Encodes the tag using the Protobuf binary wire format and writes it to a stream.
      /// </summary>
      /// <param name="aDest">The stream that the encoded tag is written to</param>
      procedure Encode(aDest: TStream);

      /// <summary>
      /// Decodes a tag using the Protobuf binary wire format from data that is read from a stream.
      /// </summary>
      /// <param name="aSource">The stream that the data is read from</param>
      /// <exception cref="EProtobufFormatViolation">If the data read from <paramref name="aSource"/> did not encode a valid Protobuf tag</exception>
      /// <returns>The decoded tag</returns>
      class function Decode(aSource: TStream): TProtobufTag; static;

      // TODO
      function CalculateSize: UInt32;
  end;

  // TODO
  TProtobufEncodedFieldValue = record
    case WireType: TProtobufWireType of
      TProtobufWireType.Varint: (VarintValue: UInt64);
      TProtobufWireType.I64: (I64Value: UInt64);
      TProtobufWireType.Len: (LenValue: TBytesStream);
      TProtobufWireType.I32: (I32Value: UInt32);
    end;

  /// <summary>
  /// Data representing a single field within a Protobuf message that is encoded using the Protobuf binary wire format.
  /// Equivalent of a Protobuf <i>record</i> in the encoded message (see Message Structure https://protobuf.dev/programming-guides/encoding/#structure).
  /// </summary>
  TProtobufEncodedField = class sealed(TPersistent)
    private
      /// <summary>
      /// Backing field for <see cref="FieldNumber"/>.
      /// </summary>
      FFieldNumber: TProtobufFieldNumber;

      /// <summary>
      /// Backing field for <see cref="Value"/>.
      /// </summary>
      FValue: TProtobufEncodedFieldValue;

    public
      /// <summary>
      /// The record's field number.
      /// </summary>
      property FieldNumber: TProtobufFieldNumber read FFieldNumber;

      /// <summary>
      /// The record's value.
      /// </summary>
      property Value: TProtobufEncodedFieldValue read FValue;

      /// <summary>
      /// Constructs a Protobuf record.
      /// </summary>
      /// <param name="aFieldNumber">The record's field number</param>
      /// <param name="aValue">The record's value</param>
      constructor CreateWithData(aFieldNumber: TProtobufFieldNumber; aValue: TProtobufEncodedFieldValue);

      // TODO
      destructor Destroy; override; final;

      /// <summary>
      /// Encodes the record using the Protobuf binary wire format and writes it to a stream.
      /// </summary>
      /// <param name="aDest">The stream that the encoded record is written to</param>
      procedure Encode(aDest: TStream);

      /// <summary>
      /// Fills the record by decoding it using the Protobuf binary wire format from data that is read from a stream.
      /// </summary>
      /// <param name="aSource">The stream that the data is read from</param>
      /// <remarks>
      /// This causes the destruction of any <see cref="TProtobufEncodedFieldValue.LenValue"/> in <see cref="Value"/>; developers must ensure that no shared ownership is held.
      /// </remarks>
      procedure Decode(aSource: TStream);

      // TODO
      procedure DecodePayload(aSource: TStream; aTag: TProtobufTag; aRemainingLength: PUInt32);

      // TODO
      function CalculateSize: UInt32;

    // TPersistent implementation

    public
      /// <summary>
      /// Copies the data from another Protobuf record to this one.
      /// </summary>
      /// <param name="aSource">Object to copy from</param>
      /// <remarks>
      /// The object must be another <see cref="TProtobufEncodedField"/>.
      /// This performs a deep copy; hence, no ownership is shared.
      /// This causes the destruction of any <see cref="TProtobufEncodedFieldValue.LenValue"/> in <see cref="Value"/>; developers must ensure that no shared ownership is held.
      /// </remarks>
      procedure Assign(aSource: TPersistent); override;
  end;

  /// <summary>
  /// Collection of <see cref="TProtobufEncodedField"/>s (Protobuf records) that represent fields in the same Protobuf message that is encoded using the Protobuf binary wire format.
  /// Records are indexed by their field number, and stored in a list to support non-packed repeated fields.
  /// </summary>
  TProtobufEncodedFieldsMap = TObjectDictionary<TProtobufFieldNumber, TObjectList<TProtobufEncodedField>>;

// TODO
procedure EncodeProtobufVarint(aDest: TStream; aVarint: UInt64);

// TODO
function DecodeProtobufVarint(aSource: TStream; aRemainingLength: PUInt32): UInt64;

// TODO
function TryDecodeProtobufVarint(aSource: TStream; out aVarint: UInt64; aRemainingLength: PUInt32): Boolean;

// TODO
function CalculateProtobufVarintSize(aVarint: UInt64): UInt32;

// TODO
procedure EncodeProtobufI64(aDest: TStream; aI64: UInt64);

// TODO
function DecodeProtobufI64(aSource: TStream; aRemainingLength: PUInt32): UInt64;

// TODO
procedure EncodeProtobufI32(aDest: TStream; aI32: UInt32);

// TODO
function DecodeProtobufI32(aSource: TStream; aRemainingLength: PUInt32): UInt32;

// TODO
function TryDecodeProtobufTag(aSource: TStream; out aTag: TProtobufTag; aRemainingLength: PUInt32): Boolean;

// TODO
procedure EncodeProtobufMessageField(aDest: TStream; aFieldNumber: TProtobufFieldNumber; aValue: IProtobufMessage);

// TODO
function CalculateProtobufMessageFieldSize(aFieldNumber: TProtobufFieldNumber; aValue: IProtobufMessage): UInt32;

// TODO
procedure EncodeProtobufFields(aDest: TStream; aFields: TProtobufEncodedFieldsMap);

// TODO
function CalculateProtobufFieldsSize(aFields: TProtobufEncodedFieldsMap): UInt32;

implementation

// Implementation of TProtobufTag

function TProtobufTag.EncodeToVarint: UInt64;
begin
  result := (FFieldNumber shl 3) or Ord(FWireType);
end;

class function TProtobufTag.DecodeFromVarint(aVarint: UInt64): TProtobufTag;
begin
  result.FFieldNumber := aVarint shr 3;
  result.FWireType := TProtobufWireType(aVarint and $7);
  // TODO check valid wire type?
end;

class function TProtobufTag.WithData(aFieldNumber: TProtobufFieldNumber; aWireType: TProtobufWireType): TProtobufTag;
begin
  result.FFieldNumber := aFieldNumber;
  result.FWireType := aWireType;
end;

procedure TProtobufTag.Encode(aDest: TStream);
begin
  EncodeProtobufVarint(aDest, EncodeToVarint);
end;

class function TProtobufTag.Decode(aSource: TStream): TProtobufTag;
begin
  result := DecodeFromVarint(DecodeProtobufVarint(aSource, nil));
end;

function TProtobufTag.CalculateSize: UInt32;
begin
  result := CalculateProtobufVarintSize(EncodeToVarint);
end;

// Implementation of TProtobufEncodedField

constructor TProtobufEncodedField.CreateWithData(aFieldNumber: TProtobufFieldNumber; aValue: TProtobufEncodedFieldValue);
begin
  FFieldNumber := aFieldNumber;
  FValue := aValue;
end;

destructor TProtobufEncodedField.Destroy;
begin
  if (FValue.WireType = TProtobufWireType.Len) then FValue.LenValue.Free;
end;

// TODO default values?
procedure TProtobufEncodedField.Encode(aDest: TStream);
begin
  TProtobufTag.WithData(FFieldNumber, FValue.WireType).Encode(aDest);
  case FValue.WireType of
    TProtobufWireType.Varint: EncodeProtobufVarint(aDest, FValue.VarintValue);
    TProtobufWireType.I64: EncodeProtobufI64(aDest, FValue.I64Value);
    TProtobufWireType.Len:
    begin
      EncodeProtobufVarint(aDest, FValue.LenValue.Size);
      aDest.CopyFrom(FValue.LenValue, 0);
      FValue.LenValue.Position := 0;
    end;
    TProtobufWireType.I32: EncodeProtobufI32(aDest, FValue.I32Value);
  else raise EProtobufFormatViolation.Create('TODO');
  end;
end;

procedure TProtobufEncodedField.Decode(aSource: TStream);
var
  lTag: TProtobufTag;
  lLength: UInt32;
begin
  lTag.Decode(aSource);
  DecodePayload(aSource, lTag, nil);
end;

procedure TProtobufEncodedField.DecodePayload(aSource: TStream; aTag: TProtobufTag; aRemainingLength: PUInt32);
var
  lLength: UInt32;
begin
  FFieldNumber := aTag.FFieldNumber;
  if (FValue.WireType = TProtobufWireType.Len) then FValue.LenValue.Free;
  FValue.WireType := aTag.FWireType;
  case aTag.FWireType of
    TProtobufWireType.Varint: FValue.VarintValue := DecodeProtobufVarint(aSource, aRemainingLength);
    TProtobufWireType.I64: FValue.I64Value := DecodeProtobufI64(aSource, aRemainingLength);
    TProtobufWireType.Len:
    begin
      lLength := DecodeProtobufVarint(aSource, aRemainingLength);
      FValue.LenValue := TBytesStream.Create;
      FValue.LenValue.CopyFrom(aSource, lLength);
      if (aRemainingLength <> nil) then aRemainingLength^ := aRemainingLength^ - lLength;
      FValue.LenValue.Position := 0;
    end;
    TProtobufWireType.I32: FValue.I32Value := DecodeProtobufI32(aSource, aRemainingLength);
  else raise EProtobufFormatViolation.Create('TODO')
  end;
end;

function TProtobufEncodedField.CalculateSize: UInt32;
begin
  result := TProtobufTag.WithData(FFieldNumber, FValue.WireType).CalculateSize;
  case FValue.WireType of
    TProtobufWireType.Varint: result := result + CalculateProtobufVarintSize(FValue.VarintValue);
    TProtobufWireType.I64: result := result + SizeOf(FValue.I64Value);
    TProtobufWireType.Len: result := result + CalculateProtobufVarintSize(FValue.LenValue.Size) + FValue.LenValue.Size;
    TProtobufWireType.I32: result := result + SizeOf(FValue.I32Value);
  else raise EProtobufFormatViolation.Create('TODO')
  end;
end;

// TPersistent implementation of TProtobufEncodedField

procedure TProtobufEncodedField.Assign(aSource: TPersistent);
var
  lSource: TProtobufEncodedField;
begin
  if (not (aSource is TProtobufEncodedField)) then
  begin
    inherited;
    Exit;
  end;
  lSource := aSource as TProtobufEncodedField;
  FFieldNumber := lSource.FFieldNumber;
  if (FValue.WireType = TProtobufWireType.Len) then FValue.LenValue.Free;
  FValue := lSource.FValue;
  if (lSource.FValue.WireType = TProtobufWireType.Len) then
  begin
    FValue.LenValue := TBytesStream.Create;
    FValue.LenValue.CopyFrom(lSource.FValue.LenValue, 0);
    lSource.FValue.LenValue.Position := 0;
    FValue.LenValue.Position := 0;
  end;
end;

// Implementation of global functions

procedure EncodeProtobufVarint(aDest: TStream; aVarint: UInt64);
var
  lByte: Byte;
begin
  repeat
    // Read the smallest 7 bits of the varint.
    lByte := aVarint and $7F;
    aVarint := aVarint shr 7;
    // Set the most significant bit of the byte only if we are done after this iteration.
    if (aVarint <> 0) then lByte := lByte or $80;
    // Write the constructed byte to the stream.
    aDest.WriteBuffer(lByte, 1);
  until (aVarint = 0);
end;

function DecodeProtobufVarint(aSource: TStream; aRemainingLength: PUInt32): UInt64;
var
  lByte: Byte;
  lCurrentOffset: NativeUint;
begin
  lByte := 0;
  // Read 7 bit-segments into the correct position within the result, until the most significant bit is set.
  result := 0;
  lCurrentOffset := 0;
  repeat
    if ((aRemainingLength <> nil) and (aRemainingLength^ = 0)) then raise EProtobufFormatViolation.Create('TODO');
    aSource.ReadBuffer(lByte, 1);
    if (aRemainingLength <> nil) then aRemainingLength^ := aRemainingLength^ - 1;
    result := result or (UInt64(lByte and $7f) shl lCurrentOffset);
    lCurrentOffset := lCurrentOffset + 7;
  until (lByte and $80 = 0);
end;

function TryDecodeProtobufVarint(aSource: TStream; out aVarint: UInt64; aRemainingLength: PUInt32): Boolean;
var
  lByte: Byte;
  lResult: Uint64;
  lCurrentOffset: NativeUint;
begin
  if ((aRemainingLength <> nil) and (aRemainingLength^ = 0)) then Exit(False);
  lByte := 0;
  // Read 7 bit-segments into the correct position within the result, until the most significant bit is set.
  lResult := 0;
  lCurrentOffset := 0;
  repeat
    if (aRemainingLength <> nil) then
    begin
      if (aRemainingLength^ = 0) then raise EProtobufFormatViolation.Create('TODO');
      aSource.ReadBuffer(lByte, 1);
      aRemainingLength^ := aRemainingLength^ - 1;
    end
    else
    begin
      if (lCurrentOffset = 0) then begin
         if (aSource.Read(lByte, 1) = 0) then Exit(False);
      end
      else aSource.ReadBuffer(lByte, 1);
    end;
    lResult := lResult or (UInt64(lByte and $7f) shl lCurrentOffset);
    lCurrentOffset := lCurrentOffset + 7;
  until (lByte and $80 = 0);
  aVarint := lResult;
  result := True;
end;

function CalculateProtobufVarintSize(aVarint: UInt64): UInt32;
begin
  result := 0;
  repeat
    result := result + 1;
  until ((aVarint shr (result * 7)) = 0);
end;

procedure EncodeProtobufI64(aDest: TStream; aI64: UInt64);
begin
  aDest.Write(aI64, SizeOf(aI64));
end;

function DecodeProtobufI64(aSource: TStream; aRemainingLength: PUInt32): UInt64;
begin
  if ((aRemainingLength <> nil) and (aRemainingLength^ < SizeOf(result))) then raise EProtobufFormatViolation.Create('TODO');
  aSource.Read(result, SizeOf(result));
  if (aRemainingLength <> nil) then aRemainingLength^ := aRemainingLength^ - SizeOf(result);
end;

procedure EncodeProtobufI32(aDest: TStream; aI32: UInt32);
begin
  aDest.Write(aI32, SizeOf(aI32));
end;

function DecodeProtobufI32(aSource: TStream; aRemainingLength: PUInt32): UInt32;
begin
  if ((aRemainingLength <> nil) and (aRemainingLength^ < SizeOf(result))) then raise EProtobufFormatViolation.Create('TODO');
  aSource.Read(result, SizeOf(result));
  if (aRemainingLength <> nil) then aRemainingLength^ := aRemainingLength^ - SizeOf(result);
end;

function TryDecodeProtobufTag(aSource: TStream; out aTag: TProtobufTag; aRemainingLength: PUInt32): Boolean;
var
  lVarint: UInt64;
begin
  result := TryDecodeProtobufVarint(aSource, lVarint, aRemainingLength);
  if (result) then aTag := TProtobufTag.DecodeFromVarint(lVarint);
end;

procedure EncodeProtobufMessageField(aDest: TStream; aFieldNumber: TProtobufFieldNumber; aValue: IProtobufMessage);
begin
  if (aValue = PROTOBUF_DEFAULT_VALUE_MESSAGE) then Exit;
  TProtobufTag.WithData(aFieldNumber, TProtobufWireType.Len).Encode(aDest);
  EncodeProtobufVarint(aDest, aValue.CalculateSize);
  aValue.Encode(aDest);
end;

function CalculateProtobufMessageFieldSize(aFieldNumber: TProtobufFieldNumber; aValue: IProtobufMessage): UInt32;
begin
  if (aValue = PROTOBUF_DEFAULT_VALUE_MESSAGE) then Exit(0);
  result := TProtobufTag.WithData(aFieldNumber, TProtobufWireType.Len).CalculateSize + CalculateProtobufVarintSize(aValue.CalculateSize) + aValue.CalculateSize;
end;

// TODO default values?
procedure EncodeProtobufFields(aDest: TStream; aFields: TProtobufEncodedFieldsMap);
var
  lRecords: TObjectList<TProtobufEncodedField>;
  lRecord: TProtobufEncodedField;
begin
  for lRecords in aFields.Values do
  begin
    for lRecord in lRecords do lRecord.Encode(aDest);
  end;
end;

function CalculateProtobufFieldsSize(aFields: TProtobufEncodedFieldsMap): UInt32;
var
  lRecords: TObjectList<TProtobufEncodedField>;
  lRecord: TProtobufEncodedField;
begin
  result := 0;
  for lRecords in aFields.Values do
  begin
    for lRecord in lRecords do result := result + lRecord.CalculateSize;
  end;
end;

end.
