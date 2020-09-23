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
/// Runtime library support for protobuf enum types.
/// </summary>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.StubRuntime.uProtobufEnum;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  // Wire codec interface
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.StubRuntime.uProtobufWireCodec,
  // For <see cref="TProtobufEnumFieldValue"/>
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.uProtobuf;

var
  /// <summary>
  /// <i>Field codec</i> for <c>protoc-gen-delphi</c> that defines the encoding/decoding of
  /// protobuf fields of enum types from/to the protobuf binary wire format.
  /// </summary>
  gProtobufWireCodecEnum: TProtobufWireCodec<TProtobufEnumFieldValue>;

implementation

end.
