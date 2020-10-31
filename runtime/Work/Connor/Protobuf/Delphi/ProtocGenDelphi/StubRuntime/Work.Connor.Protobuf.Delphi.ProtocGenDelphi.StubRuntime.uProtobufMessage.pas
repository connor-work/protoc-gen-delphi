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
/// Runtime library support code for protobuf message types.
/// </summary>
/// <remarks>
/// This unit defines the common ancestor of all generated classes representing protobuf message types, <see cref="TProtobufMessage"/>.
/// Client code should reference it indirectly through <see cref="N:Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uProtobufMessage"/>.
/// </remarks>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.StubRuntime.uProtobufMessage;

interface

uses
  // To implement IProtobufMessage
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uIProtobufMessage,
  // To implement IProtobufMessageInternal
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uIProtobufMessageInternal,
  // Basic protobuf definitions like TProtobufFieldNumber
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.uProtobuf,
  // TStream for encoding and decoding of messages, TInterfacedPersistent as base class
  System.Classes,
  // Stub runtime helper code
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.StubRuntime.uStubRuntime;

type
  /// <summary>
  /// Common ancestor of all generated classes that represent protobuf message types.
  /// </summary>
  /// <remarks>
  /// Can be used directly to handle messages of unknown type.
  /// The message instance carries transitive ownership of embedded objects in protobuf field values,
  /// and is responsible for their deallocation.
  /// </remarks>
  TProtobufMessage = class(TInterfacedPersistent, IProtobufMessage, IProtobufMessageInternal)
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

    // IProtobufMessage implementation

    public
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
      /// <remarks>
      /// Since the protobuf binary wire format does not include length information for top-level messages,
      /// the recipient may not be able to detect the end of the message when reading it from a stream.
      /// If this is required, use <see cref="EncodeDelimited"/> instead.
      /// </remarks>
      procedure Encode(aDest: TStream); virtual;

      /// <summary>
      /// Encodes the message using the protobuf binary wire format and writes it to a stream, prefixed with length information.
      /// </summary>
      /// <param name="aDest">The stream that the encoded message is written to</param>
      /// <remarks>
      /// Unlike <see cref="Encode"/>, this method enables the recipient to detect the end of the message by decoding it using
      /// <see cref="DecodeDelimited"/>.
      /// </remarks>
      procedure EncodeDelimited(aDest: TStream);

      /// <summary>
      /// Fills the message's protobuf fields by decoding the message using the protobuf binary wire format from data that is read from a stream.
      /// Data is read until <see cref="TStream.Read"/> returns 0.
      /// </summary>
      /// <param name="aSource">The stream that the data is read from</param>
      /// <exception cref="EDecodingSchemaError">If the message on the stream was not compatible with this message type</exception>
      /// <remarks>
      /// Protobuf fields that are not present in the read data are rendered absent by setting them to their default values.
      /// This may cause the destruction of transitively owned objects (this is also the case when a present fields overwrites a previous value).
      /// Developers must ensure that no shared ownership of current field values or further nested embedded objects is held.
      /// This method should not be used on streams where the actual size of their contents may not be known yet (this might result in data loss).
      /// If this is required, use <see cref="DecodeDelimited"/> instead.
      /// </remarks>
      procedure Decode(aSource: TStream); virtual;

      /// <summary>
      /// Fills the message's protobuf fields by decoding the message using the protobuf binary wire format from data that is read from a stream.
      /// The data must be prefixed with message length information, as implemented by <see cref="EncodeDelimited"/>.
      /// </summary>
      /// <exception cref="EDecodingSchemaError">If the message on the stream was not compatible with this message type</exception>
      /// <param name="aSource">The stream that the data is read from</param>
      /// <remarks>
      /// See remarks on <see cref="Decode">.
      /// </remarks>
      procedure DecodeDelimited(aSource: TStream);

    // IProtobufMessageInternal implementation

    protected
      /// <summary>
      /// Tests if this message has a currently unknown protobuf field (found by <see cref="Decode"/>, but not decoded yet), with a known field number.
      /// </summary>
      /// <param name="aField">Protobuf field number of the field</param>
      /// <returns><c>true</c> if this message has a currently unknown protobuf field with the specified number</returns>
      function HasUnknownField(aField: TProtobufFieldNumber): Boolean;

      /// <summary>
      /// Encodes a protobuf singular field of a message, with this instance as value (<i>message field</i>), using the protobuf binary wire format, and writes it to a stream.
      /// </summary>
      /// <param name="aContainer">Protobuf message containing the field</param>
      /// <param name="aField">Protobuf field number of the field</param>
      /// <param name="aDest">The stream that the encoded field is written to</param>
      /// <remarks>
      /// For convenience, this method may be called on a <c>nil</c> value, since this is the representation for the default value of a protobuf message field.
      /// This should be used within an implementation of <see cref="Encode"/>, after calling the ancestor class implementation.
      /// </remarks>
      procedure EncodeAsSingularField(aContainer: IProtobufMessageInternal; aField: TProtobufFieldNumber; aDest: TStream);

      /// <summary>
      /// Decodes a previously unknown protobuf singular field of a message, which is assumed to be present, using the protobuf binary wire format, and stores the value in this instance (<i>message field</i>).
      /// If the field is present, this embedded message is filled using <see cref="Decode"/>.
      /// The field is then no longer considered unknown.
      /// If the field is present multiple times, the message values are merged, see https://developers.google.com/protocol-buffers/docs/encoding#optional.
      /// </summary>
      /// <param name="aContainer">Protobuf message containing the field</param>
      /// <param name="aField">Protobuf field number of the field</param>
      /// <exception cref="EDecodingSchemaError">If the field was absent</exception>
      /// <exception cref="EDecodingSchemaError">If the unknown field value was not compatible with this message type</exception>
      /// <remarks>
      /// This should be used within an implementation of <see cref="IProtobufMessage.Decode"/>, after calling the ancestor class implementation.
      /// This method is not idempotent. The state of the containing message is changed by the call, since decoding "consumes" the unknown field.
      /// Ownership of this message is transferred to the containing message (<i>embedded message</i>).
      /// See also remarks on destruction of transitively owned objects on <see cref="Decode"/>.
      /// </remarks>
      procedure DecodeAsUnknownSingularField(aContainer: IProtobufMessageInternal; aField: TProtobufFieldNumber);

      /// <summary>
      /// Sets the owner of the message, which is responsible for freeing it. This might be a containing message or field value collection.
      /// </summary>
      /// <param name="aOwner">The new owner of the message</param>
      procedure SetOwner(aOwner: TPersistent);

    // TInterfacedPersistent implementation

    public
      /// <summary>
      /// Copies the protobuf data from another object to this one.
      /// </summary>
      /// <param name="aSource">Object to copy from</param>
      /// <remarks>
      /// The other object must be a protobuf message of the same type.
      /// This performs a deep copy; hence, no ownership is shared.
      /// This procedure may cause the destruction of transitively owned objects in this message instance.
      /// Developers must ensure that no shared ownership of current field values or further nested embedded objects is held.
      /// </remarks>
      procedure Assign(aSource: TPersistent); override;

      function GetOwner: TPersistent; override;
  end;

implementation

constructor TProtobufMessage.Create;
begin
  raise NotImplementedInStub;
end;

destructor TProtobufMessage.Destroy;
begin
  raise NotImplementedInStub;
end;

// IProtobufMessage implementation

procedure TProtobufMessage.Clear;
begin
  raise NotImplementedInStub;
end;

procedure TProtobufMessage.Encode(aDest: TStream);
begin
  raise NotImplementedInStub;
end;

procedure TProtobufMessage.EncodeDelimited(aDest: TStream);
begin
  raise NotImplementedInStub;
end;

procedure TProtobufMessage.Decode(aSource: TStream);
begin
  raise NotImplementedInStub;
end;

procedure TProtobufMessage.DecodeDelimited(aSource: TStream);
begin
  raise NotImplementedInStub;
end;

// IProtobufMessageInternal implementation

function TProtobufMessage.HasUnknownField(aField: TProtobufFieldNumber): Boolean;
begin
  raise NotImplementedInStub;
end;

procedure TProtobufMessage.EncodeAsSingularField(aContainer: IProtobufMessageInternal; aField: TProtobufFieldNumber; aDest: TStream);
begin
  raise NotImplementedInStub;
end;

procedure TProtobufMessage.DecodeAsUnknownSingularField(aContainer: IProtobufMessageInternal; aField: TProtobufFieldNumber);
begin
  raise NotImplementedInStub;
end;

procedure TProtobufMessage.SetOwner(aOwner: TPersistent);
begin
  raise NotImplementedInStub;
end;

// TInterfacedPersistent implementation

procedure TProtobufMessage.Assign(aSource: TPersistent);
begin
  raise NotImplementedInStub;
end;

function TProtobufMessage.GetOwner: TPersistent;
begin
  raise NotImplementedInStub;
end;

end.
