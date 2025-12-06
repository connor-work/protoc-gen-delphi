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
/// protobuf non-message length-delimited type fields from/to the protobuf binary wire format.
/// </summary>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufDelimitedWireCodec;

{$INCLUDE Work.Connor.Delphi.CompilerFeatures.inc}

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  // To extend TProtobufWireCodec<T>
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufWireCodec,
  // IProtobufMessageInternal for IProtobufWireCodec<T> implementation
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uIProtobufMessageInternal,
  // To throw EDecodingSchemaError
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uIProtobufMessage,
  // TProtobufFieldNumber for IProtobufWireCodec<T> implementation
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.uProtobuf,
  // TStream for IProtobufWireCodec<T> implementation
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.Classes,
{$ELSE}
  Classes,
{$ENDIF}
  // TBytes to represent value data
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.SysUtils;
{$ELSE}
  SysUtils;
{$ENDIF}


type
  /// <summary>
  /// Runtime library implementation of <see cref="T:IProtobufWireCodec"/> for protobuf non-message length-delimited type types.
  /// </summary>
  /// <typeparam name="T">Delphi type representing this codec's protobuf type</typeparam>
  TProtobufDelimitedWireCodec<T> = class(TProtobufWireCodec<T>)
    // Abstract members

    public
      /// <summary>
      /// Casts a byte array to the Delphi type representing this codec's protobuf type.
      /// </summary>
      /// <param name="aValue">The data to cast</param>
      /// <returns>Value representing the protobuf data</returns>
      function FromBytes(aValue: TBytes): T; virtual; abstract;

      /// <summary>
      /// Casts a value of the Delphi type representing this codec's protobuf type to a byte array.
      /// </summary>
      /// <param name="aValue">The protobuf data</param>
      /// <returns>Corresponding byte array</returns>
      function ToBytes(aValue: T): TBytes; virtual; abstract;

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
  // For encoding and decoding of protobuf tags
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufTag,
  // For encoding and decoding of varint type lengths
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufVarint,
  // TObjectList for handling unparsed fields
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.Generics.Collections;
{$ELSE}
  Generics.Collections;
{$ENDIF}

// TProtobufWireCodec<T> implementation

procedure TProtobufDelimitedWireCodec<T>.EncodeSingularField(aValue: T; aContainer: IProtobufMessageInternal; aField: TProtobufFieldNumber; aDest: TStream);
var
  lBytes: TBytes;
begin
  TProtobufTag.WithData(aField, wtLengthDelimited).Encode(aDest);
  lBytes := ToBytes(aValue);
  EncodeVarint(Length(lBytes), aDest);
  if (Length(lBytes) > 0) then
    aDest.WriteBuffer(lBytes[0], Length(lBytes));
end;

function TProtobufDelimitedWireCodec<T>.DecodeUnknownField(aContainer: IProtobufMessageInternal; aField: TProtobufFieldNumber): T;
var
  lContainer: TProtobufMessage;
  lFields: TObjectList<TProtobufEncodedField>;
  lField: TProtobufEncodedField;
  lStream: TMemoryStream;
  lLength: UInt32;
  lBytes: TBytes;
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

        lLength := DecodeVarint(lStream);
        SetLength(lBytes, lLength);
        if (lLength > 0) then
          lStream.ReadBuffer(lBytes[0], lLength);

        result := FromBytes(lBytes);
      finally
        lStream.Free;
      end;
    end;
    lContainer.UnparsedFields.Remove(aField);
  end;
end;

end.
