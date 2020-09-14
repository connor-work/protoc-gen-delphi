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

unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.StubRuntime.uProtobufMessage;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  SysUtils,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.uProtobufTypes,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.StubRuntime.uProtobufWireCodec,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.StubRuntime.uProtobufRepeatedField;

type
  /// <summary>
  /// Common ancestor of all generated classes that represent protobuf message types.
  /// </summary>
  /// <remarks>
  /// Can be used directly to handle message of unknown type.
  /// The message instance carries transitive ownership of embedded objects in protobuf field values,
  /// and is responsible for their deallocation.
  /// </remarks>
  TProtobufMessage = class

  public
    /// <summary>
    /// Constructs an empty message with all protobuf fields absent, meaning that they are set to their default values.
    /// </summary>
    /// <remarks>
    /// Protobuf's interpretation of the absence of a field may be counterintuitive for Delphi developers.
    /// For a detailed explanation, see https://developers.google.com/protocol-buffers/docs/proto3#default.
    /// </remarks>
    constructor Create; virtual;

    /// <summary>
    /// Destroys the message and all objects and resources held by it, including the protobuf field values.
    /// </summary>
    /// <remarks>
    /// Developers must ensure that no shared ownership of current field values or further nested embedded objects is held.
    /// </remarks>
    destructor Destroy; override;

    /// <summary>
    /// Renders all protobuf fields absent by setting them to their default values.
    /// </summary>
    /// <remarks>
    /// The resulting instance state is equivalent to a newly constructed empty message.
    /// For more details, see the documentation of <see cref="Create"/>.
    /// This procedure may cause the destruction of transitively owned objects.
    /// Developers must ensure that no shared ownership of current field values or further nested embedded objects is held.
    /// </remarks>
    procedure Clear; virtual;

    /// <summary>
    /// Encodes the message using the protobuf binary wire format and writes it to a stream.
    /// </summary>
    /// <param name="aDest">The stream that the encoded message is written to</param>
    procedure Encode(aDest: TStream); virtual;

    /// <summary>
    /// Fills the message's protobuf fields by decoding the message using the protobuf binary wire format from data that is read from a stream.
    /// </summary>
    /// <param name="aSource">The stream that the data is read from</param>
    /// <remarks>
    /// Protobuf fields that are not present in the read data are rendered absent by setting them to their default values.
    /// This may cause the destruction of transitively owned objects (this is also the case when a present fields overwrites a previous value).
    /// Developers must ensure that no shared ownership of current field values or further nested embedded objects is held.
    /// </remarks>
    procedure Decode(aSource: TStream); virtual;

  protected
    /// <summary>
    /// TODO doc
    /// </summary>
    procedure EncodeField<T>(aValue: T; aField: TProtobufFieldNumber; aCodec: TProtobufWireCodec<T>; aDest: TStream);

    /// <summary>
    /// Encodes a protobuf field with a specific protobuf message type (<i>message field</i>) using the protobuf binary wire format and writes it to a stream.
    /// </summary>
    /// <param name="T">Delphi type representing the protobuf message type of the field</param>
    /// <param name="aField">Protobuf field number of the field</param>
    /// <param name="aDest">The stream that the encoded field is written to</param>
    /// <remarks>
    /// This should be used within an implementation of <see cref="Encode"/>, after calling the ancestor class implementation.
    /// </remarks>
    procedure EncodeMessageField<T: TMessage>(aValue: T; aField: TProtobufFieldNumber; aDest: TStream);

    /// <summary>
    /// TODO doc
    /// </summary>
    procedure EncodeRepeatedField<T>(aSource: TProtobufRepeatedField<T>; aField: TProtobufFieldNumber; aCodec: TProtobufWireCodec<T>; aDest: TStream);

    /// <summary>
    /// TODO doc
    /// </summary>
    function DecodeUnknownField<T>(aField: TProtobufFieldNumber; aCodec: TProtobufWireCodec<T>): T;

    /// <summary>
    /// Decodes a previously unknown protobuf field with a specific protobuf message type (<i>message field</i>).
    /// If the field is present, an instance representing the embedded message is constructed and filled using <see cref="Create"/> and <see cref="Decode"/>.
    /// The field is then no longer considered unknown.
    /// If the field is present multiple times, the last value is used, see https://developers.google.com/protocol-buffers/docs/encoding#optional.
    /// If the field is absent, <c>nil</c> is returned (which is the representation of the default value).
    /// </summary>
    /// <param name="T">Delphi type representing the protobuf message type of the field</param>
    /// <param name="aField">Protobuf field number of the field</param>
    /// <returns>The decoded field value</returns>
    /// <remarks>
    /// This should be used within an implementation of <see cref="Decode"/>, after calling the ancestor class implementation.
    /// This method is not idempotent. The state of this instance is changed by the call, since decoding "consumes" the unknown field.
    /// Ownership of the returned object, if one is allocated, is transferred to the caller (which should be an instance of a descendant class).
    /// </remarks>
    function DecodeUnknownMessageField<T: TMessage>(aField: TProtobufFieldNumber): T;
    
    /// <summary>
    /// TODO doc
    /// </summary>
    procedure DecodeUnknownRepeatedField<T>(aField: TProtobufFieldNumber; aCodec: TProtobufWireCodec<T>, aDest: TProtobufRepeatedField<T>);
  end;

implementation

procedure NotImplementedInStub;
begin
  raise Exception.Create('This functionality is not implemented by the stub runtime');
end;

constructor TProtobufMessage.Create;
begin
  NotImplementedInStub;
end;

destructor TProtobufMessage.Destroy;
begin
  NotImplementedInStub;
end;

procedure TProtobufMessage.Clear;
begin
  NotImplementedInStub;
end;

procedure TProtobufMessage.Encode(aDest: TStream);
begin
  NotImplementedInStub;
end;

procedure TProtobufMessage.Decode(aSource: TStream);
begin
  NotImplementedInStub;
end;

end.
