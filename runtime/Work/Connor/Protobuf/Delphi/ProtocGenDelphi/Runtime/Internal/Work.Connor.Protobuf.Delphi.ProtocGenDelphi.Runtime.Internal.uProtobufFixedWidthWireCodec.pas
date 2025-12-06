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
/// Runtime library support code for encoding/decoding of
/// protobuf fixed width scalar type fields from/to the protobuf binary wire format.
/// </summary>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufFixedWidthWireCodec;

{$INCLUDE Work.Connor.Delphi.CompilerFeatures.inc}

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  // To extend TProtobufWireCodec<T>
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufWireCodec,
  // For encoding and decoding of protobuf tags
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufTag,
  // IProtobufMessageInternal for IProtobufWireCodec<T> implementation
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uIProtobufMessageInternal,
  // To throw EDecodingSchemaError
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uIProtobufMessage,
  // TProtobufFieldNumber for IProtobufWireCodec<T> implementation
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.uProtobuf,
  // TStream for IProtobufWireCodec<T> implementation
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.Classes;
{$ELSE}
  Classes;
{$ENDIF}

type
  /// <summary>
  /// Runtime library implementation of <see cref="T:IProtobufWireCodec"/> for protobuf fixed width scalar types.
  /// </summary>
  /// <typeparam name="T">Delphi type representing this codec's protobuf fixed width scalar type</typeparam>
  TProtobufFixedWidthWireCodec<T> = class(TProtobufWireCodec<T>)
    // Abstract members

    public
      /// <summary>
      /// Determines the protobuf wire type for values of this type.
      /// </summary>
      /// <returns>The protobuf wire type</returns>
      function GetWireType: TProtobufWireType; virtual; abstract;

      /// <summary>
      /// Decodes a value from a stream.
      /// </summary>
      /// <param name="aSource">Stream containing the encoded value</param>
      /// <returns>Value representing a fixed width scalar value</returns>
      function DecodeValue(aSource: TStream): T; virtual; abstract;

      /// <summary>
      /// Encodes a value to a stream.
      /// </summary>
      /// <param name="aValue">The fixed width scalar value to encode</param>
      /// <param name="aDest">Stream to encode the value to</param>
      procedure EncodeValue(aValue: T; aDest: TStream); virtual; abstract;

    // TProtobufWireCodec<T> implementation

    public
      procedure EncodeSingularField(aValue: T; aContainer: IProtobufMessageInternal; aField: TProtobufFieldNumber; aDest: TStream); override;
      function DecodeUnknownField(aContainer: IProtobufMessageInternal; aField: TProtobufFieldNumber): T; override;
  end;

implementation

uses
  // For handling protobuf encoded fields
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufEncodedField,
  // TProtobufMessage for message field encoding and decoding
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uProtobufMessage,
  // For encoding and decoding of field lengths
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufVarint,
  // TObjectList for handling unparsed fields
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.Generics.Collections,
{$ELSE}
  Generics.Collections,
{$ENDIF}
  // For IntToStr
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.SysUtils;
{$ELSE}
  SysUtils;
{$ENDIF}

// TProtobufWireCodec<T> implementation

procedure TProtobufFixedWidthWireCodec<T>.EncodeSingularField(aValue: T; aContainer: IProtobufMessageInternal; aField: TProtobufFieldNumber; aDest: TStream);
begin
  if (not IsDefault(aValue)) then
  begin
    TProtobufTag.WithData(aField, GetWireType).Encode(aDest);
    EncodeValue(aValue, aDest);
  end;
end;

function TProtobufFixedWidthWireCodec<T>.DecodeUnknownField(aContainer: IProtobufMessageInternal; aField: TProtobufFieldNumber): T;
var
  lContainer: TProtobufMessage;
  lFields: TObjectList<TProtobufEncodedField>;
  lField: TProtobufEncodedField;
  lStream: TMemoryStream;
begin
  lContainer := aContainer as TProtobufMessage;

  result := GetDefault;

  lFields := nil;
  lContainer.UnparsedFields.TryGetValue(aField, lFields);
  if (Assigned(lFields)) then
  begin
    // https://developers.google.com/protocol-buffers/docs/encoding#optional:
    // For numeric types and strings, if the same field appears multiple times, the parser accepts the last value it sees.
    for lField in lFields do
    begin
      // Convert field to a stream for simpler processing.
      lStream := TMemoryStream.Create;
      try
        lStream.WriteBuffer(lField.Data[0], Length(lField.Data));
        lStream.Seek(0, soBeginning);

        if (lField.Tag.WireType = GetWireType) then
          result := DecodeValue(lStream)
        else if (lField.Tag.WireType = wtLengthDelimited) then
        begin
          // Ignore the size of the field, as the stream already has the correct length.
          DecodeVarint(lStream);
          while (lStream.Position < lStream.Size) do
            result := DecodeValue(lStream);
        end; // TODO: Catch invalid wire type.
      finally
        lStream.Free;
      end;
    end;
    lContainer.UnparsedFields.Remove(aField);
  end;
end;

end.
