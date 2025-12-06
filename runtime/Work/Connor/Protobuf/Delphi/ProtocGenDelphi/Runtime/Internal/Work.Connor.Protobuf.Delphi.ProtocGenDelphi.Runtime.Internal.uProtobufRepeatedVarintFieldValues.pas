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
/// Support code for handling of varint type values in <see cref="N:Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufRepeatedFieldValues"/>.
/// </summary>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufRepeatedVarintFieldValues;

{$INCLUDE Work.Connor.Delphi.CompilerFeatures.inc}

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  // To extend TProtobufRepeatedPrimitiveFieldValues<T>
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufRepeatedPrimitiveFieldValues,
  // TProtobufVarintWireCodec<T> for encoding and decoding of field values
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufVarintWireCodec,
  // TProtobufWireCodec<T> TProtobufRepeatedPrimitiveFieldValues<T> implementation
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufWireCodec,
  // IProtobufMessageInternal for IProtobufRepeatedFieldValuesInternal<T> implementation
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uIProtobufMessageInternal,
  // TProtobufFieldNumber for IProtobufRepeatedFieldValuesInternal<T> implementation
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.uProtobuf,
  // TStream for IProtobufRepeatedFieldValuesInternal<T> implementation
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.Classes;
{$ELSE}
  Classes;
{$ENDIF}

type
  /// <summary>
  /// Helper subclass of <see cref="T:TProtobufRepeatedPrimitiveFieldValues"/> for values of a specific varint type.
  /// </summary>
  /// <typeparam name="T">Delphi type of the field values</typeparam>
  TProtobufRepeatedVarintFieldValues<T> = class abstract(TProtobufRepeatedPrimitiveFieldValues<T>)
    // Abstract members

    protected
      /// <summary>
      /// Getter for <see cref="VarintWireCodec"/>.
      /// </summary>
      /// <returns>Field codec for the protobuf type</returns>
      function GetVarintWireCodec: TProtobufVarintWireCodec<T>; virtual; abstract;

      /// <summary>
      /// Field codec for the protobuf type.
      /// </summary>
      property VarintWireCodec: TProtobufVarintWireCodec<T> read GetVarintWireCodec;

    // TProtobufRepeatedPrimitiveFieldValues<T> implementation

    protected
      function GetWireCodec: TProtobufWireCodec<T>; override;

    // IProtobufRepeatedFieldValuesInternal<T> implementation

    public
      procedure EncodeAsRepeatedField(aContainer: IProtobufMessageInternal; aField: TProtobufFieldNumber; aDest: TStream); override;
      procedure DecodeAsUnknownRepeatedField(aContainer: IProtobufMessageInternal; aField: TProtobufFieldNumber); override;
  end;

implementation

uses
  // For handling protobuf encoded fields
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufEncodedField,
  // TProtobufMessage for message field encoding and decoding
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uProtobufMessage,
  // For encoding and decoding of protobuf tags
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufTag,
  // For encoding and decoding of varint type values
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufVarint,
  // TObjectList for handling unparsed fields
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.Generics.Collections;
{$ELSE}
  Generics.Collections;
{$ENDIF}

// TProtobufRepeatedPrimitiveFieldValues<T> implementation

function TProtobufRepeatedVarintFieldValues<T>.GetWireCodec: TProtobufWireCodec<T>;
begin
  result := VarintWireCodec;
end;


// IProtobufRepeatedFieldValuesInternal<T> implementation

procedure TProtobufRepeatedVarintFieldValues<T>.EncodeAsRepeatedField(aContainer: IProtobufMessageInternal; aField: TProtobufFieldNumber; aDest: TStream);
var
  lValue: T;
  lStream: TStream;
begin
  if (Count <> 0) then
  begin
    // Encode the values to a temporary stream first to determine their size.
    lStream := TMemoryStream.Create;
    try
      for lValue in self do EncodeVarint(VarintWireCodec.ToUInt64(lValue), lStream);
      lStream.Seek(0, soBeginning);
      TProtobufTag.WithData(aField, wtLengthDelimited).Encode(aDest);
      EncodeVarint(lStream.Size, aDest);
      if (lStream.Size > 0) then
        aDest.CopyFrom(lStream, lStream.Size);
    finally
      lStream.Free;
    end;
  end;
end;

procedure TProtobufRepeatedVarintFieldValues<T>.DecodeAsUnknownRepeatedField(aContainer: IProtobufMessageInternal; aField: TProtobufFieldNumber);
var
  lContainer: TProtobufMessage;
  lFields: TObjectList<TProtobufEncodedField>;
  lField: TProtobufEncodedField;
  lStream: TMemoryStream;
begin
  lContainer := aContainer as TProtobufMessage;

  // Default value for repeated fields is empty.
  Clear;

  lFields := nil;
  lContainer.UnparsedFields.TryGetValue(aField, lFields);
  if (Assigned(lFields)) then
  begin
    // For each field, we will decide whether to decode a packed or non-packed repeated value
    for lField in lFields do
    begin
      // Convert field to a stream for simpler processing.
      lStream := TMemoryStream.Create;
      try
        lStream.WriteBuffer(lField.Data[0], Length(lField.Data));
        lStream.Seek(0, soBeginning);

        if (lField.Tag.WireType = wtVarint) then Add(VarintWireCodec.FromUInt64(DecodeVarint(lStream)))
        else if (lField.Tag.WireType = wtLengthDelimited) then
        begin
          // Ignore the size of the field, as the stream already has the correct length.
          DecodeVarint(lStream);
          while (lStream.Position < lStream.Size) do
            Add(VarintWireCodec.FromUInt64(DecodeVarint(lStream)));
        end; // TODO: Catch invalid wire type.
      finally
        lStream.Free;
      end;
    end;
    lContainer.UnparsedFields.Remove(aField);
  end;
end;

end.
