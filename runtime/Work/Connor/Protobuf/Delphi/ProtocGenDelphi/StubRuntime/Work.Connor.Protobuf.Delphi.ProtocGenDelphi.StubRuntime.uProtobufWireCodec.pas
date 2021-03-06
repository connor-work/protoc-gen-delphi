/// Copyright 2020 Connor Roehricht (connor.work)
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
/// protobuf fields from/to the protobuf binary wire format.
/// </summary>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.StubRuntime.uProtobufWireCodec;

{$INCLUDE Work.Connor.Delphi.CompilerFeatures.inc}

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  // To implement IProtobufWireCodec
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uIProtobufWireCodec,
  // Runtime-internal support for message types
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uIProtobufMessageInternal,
  // Basic protobuf definitions like TProtobufFieldNumber
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.uProtobuf,
  // TStream for encoding and decoding of messages
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.Classes,
{$ELSE}
  Classes,
{$ENDIF}
  // Stub runtime helper code
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.StubRuntime.uStubRuntime;

type
  /// <summary>
  /// Generic runtime library implementation of <see cref="T:IProtobufWireCodec"/>.
  /// </summary>
  TProtobufWireCodec<T> = class(TInterfacedObject, IProtobufWireCodec<T>)
    // IProtobufWireCodec<T> implementation

    public
      /// <summary>
      /// Encodes a protobuf singular field of a message using the protobuf binary wire format and writes it to a stream.
      /// </summary>
      /// <param name="aValue">Value of the field</param>
      /// <param name="aContainer">Protobuf message containing the field</param>
      /// <param name="aField">Protobuf field number of the field</param>
      /// <param name="aDest">The stream that the encoded field is written to</param>
      /// <remarks>
      /// This method is not used for message fields, see <see cref="IProtobufMessageInternal.EncodeAsSingularField"/>.
      /// This should be used within an implementation of <see cref="IProtobufMessage.Encode"/>, after calling the ancestor class implementation.
      /// </remarks>
      procedure EncodeSingularField(aValue: T; aContainer: IProtobufMessageInternal; aField: TProtobufFieldNumber; aDest: TStream);

      /// <summary>
      /// Decodes a previously unknown protobuf singular field of a message.
      /// The field is then no longer considered unknown.
      /// If the field is present multiple times, the last value is used, see https://developers.google.com/protocol-buffers/docs/encoding#optional.
      /// If the field is absent, the default value for the protobuf type is returned.
      /// </summary>
      /// <param name="aContainer">Protobuf message containing the field</param>
      /// <param name="aField">Protobuf field number of the field</param>
      /// <returns>The decoded field value</returns>
      /// <exception cref="EDecodingSchemaError">If the unknown field value was not compatible with the expected type</exception>
      /// <remarks>
      /// This method is not used for message fields, see <see cref="IProtobufMessageInternal.DecodeAsUnknownSingularField"/>.
      /// This should be used within an implementation of <see cref="IProtobufMessage.Decode"/>, after calling the ancestor class implementation.
      /// This method is not idempotent. The state of the message is changed by the call, since decoding "consumes" the unknown field.
      /// </remarks>
      function DecodeUnknownField(aContainer: IProtobufMessageInternal; aField: TProtobufFieldNumber): T;
  end;

implementation

// IProtobufWireCodec implementation

procedure TProtobufWireCodec<T>.EncodeSingularField(aValue: T; aContainer: IProtobufMessageInternal; aField: TProtobufFieldNumber; aDest: TStream);
begin
  raise NotImplementedInStub;
end;

function TProtobufWireCodec<T>.DecodeUnknownField(aContainer: IProtobufMessageInternal; aField: TProtobufFieldNumber): T;
begin
  raise NotImplementedInStub;
end;

end.
