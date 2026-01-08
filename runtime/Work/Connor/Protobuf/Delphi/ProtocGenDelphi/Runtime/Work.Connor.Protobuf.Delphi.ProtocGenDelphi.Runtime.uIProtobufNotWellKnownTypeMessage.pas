/// Copyright 2025 Connor Erdmann (connor.work)
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
/// Runtime support interface for Protobuf message types that are not one of the well-known types.
/// </summary>
/// <remarks>
/// This unit defines the common interface of all generated classes representing Protobuf message types (<see cref="IProtobufMessage"/>),
/// that are not one of the well-known types (<see cref="IProtobufWellKnownTypeMessage"/>).
/// </remarks>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uIProtobufNotWellKnownTypeMessage;

{$INCLUDE Work.Connor.Delphi.CompilerFeatures.inc}

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.JSON,
{$ELSE}
  JSON,
{$ENDIF}
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.JSON.Builders;
{$ELSE}
  JSON.Builders;
{$ENDIF}

type
  /// <summary>
  /// Common interface of all generated classes that represent Protobuf message types that are not one of the well-known types.
  /// </summary>
  /// <remarks>
  /// Can be used directly to handle messages of an unknown type that is known not to be one of the well-known message types.
  /// The message instance carries transitive ownership of embedded objects in Protobuf field values,
  /// and is responsible for their deallocation.
  /// </remarks>
  IProtobufNotWellKnownTypeMessage = interface(IInterface)
    ['{60876367-A0DF-4D03-A439-0C94845DBAA3}']
    /// <summary>
    /// Encodes the message as a JSON object using the ProtoJSON format and writes the key-value pairs to a <see cref="TJSONCollectionBuilder.TPairs"/>.
    /// </summary>
    /// <param name="aPairs">The <see cref="TJSONCollectionBuilder.TPairs"/> that the encoded message's key-value pairs are written to</param>
    procedure EncodeJson(aPairs: TJSONCollectionBuilder.TPairs); overload;

    /// <summary>
    /// Encodes the message as a JSON object using the ProtoJSON format and writes it to a <see cref="TJSONObjectBuilder"/>.
    /// </summary>
    /// <param name="aBuilder">The <see cref="TJSONObjectBuilder"/> that the encoded message is written to</param>
    procedure EncodeJson(aBuilder: TJSONObjectBuilder); overload;

    /// <summary>
    /// Fills the message's Protobuf fields by decoding the message from a JSON object, using the ProtoJSON format.
    /// </summary>
    /// <param name="aSource">The JSON object that the message is decoded from</param>
    /// <exception cref="EDecodingSchemaError">If the format of the JSON object was not compatible with this message type</exception>
    /// <remarks>
    /// Protobuf fields that are not present in the JSON object are rendered absent by setting them to their default values.
    /// This may cause the destruction of transitively owned objects (this is also the case when a present field overwrites a previous value).
    /// Developers must ensure that no shared ownership of current field values or further nested embedded objects is held.
    /// </remarks>
    procedure DecodeJson(aSource: TJSONObject);
  end;

implementation

end.
