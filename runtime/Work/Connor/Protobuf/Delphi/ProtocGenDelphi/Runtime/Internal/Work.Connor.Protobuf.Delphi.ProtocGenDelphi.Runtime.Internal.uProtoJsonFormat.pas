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
/// Runtime support for the ProtoJSON format.
/// </summary>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtoJsonFormat;

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
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uIProtobufMessage;

type
  // TODO contract
  IProtobufMessageWithGenericJsonRepresentation = interface(IProtobufMessage)
    ['{7184DB5F-EC3D-4206-B56D-7E75B3BF8498}']
    /// <summary>
    /// Encodes the message as a JSON object using the ProtoJSON format.
    /// </summary>
    /// <returns>TODO contract</returns>
    function EncodeJsonObject: TJSONObject;

    /// <summary>
    /// Encodes the message as a JSON object using the ProtoJSON format and writes it to a <see cref="TJSONObject"/>.
    /// </summary>
    /// <param name="aDest">The <see cref="TJSONObject"/> that the encoded message is written to</param>
    procedure EncodeJson(aDest: TJSONObject);

    /// <summary>
    /// Fills the message's Protobuf fields by decoding the message from a JSON object, using the ProtoJSON format.
    /// </summary>
    /// <param name="aSource">The JSON object that the message is decoded from</param>
    /// <param name="aSkipType">TODO contract</param>
    /// <exception cref="EDecodingSchemaError">If the format of the JSON object was not compatible with this message type</exception>
    /// <remarks>
    /// Protobuf fields that are not present in the JSON object are rendered absent by setting them to their default values.
    /// This may cause the destruction of transitively owned objects (this is also the case when a present field overwrites a previous value).
    /// Developers must ensure that no shared ownership of current field values or further nested embedded objects is held.
    /// </remarks>
    procedure DecodeJson(aSource: TJSONObject; aSkipType: Boolean = False);

    // TODO also support readers/writers framework with TObjectBuilder and TJSONIterator? would have to figure out how to abstract MergeFieldFromJson parameter
  end;

// TODO contract
procedure EncodeJsonProtobufMessageField(aDest: TJSONObject; aFieldJsonName: UnicodeString; aValue: IProtobufMessage);

implementation

procedure EncodeJsonProtobufMessageField(aDest: TJSONObject; aFieldJsonName: UnicodeString; aValue: IProtobufMessage);
begin
  aDest.AddPair(aFieldJsonName, aValue.EncodeJson);
end;

end.

