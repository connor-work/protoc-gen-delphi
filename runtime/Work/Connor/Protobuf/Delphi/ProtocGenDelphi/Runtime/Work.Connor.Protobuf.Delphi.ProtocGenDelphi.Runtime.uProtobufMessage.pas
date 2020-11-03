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
/// Runtime support for protobuf message types.
/// </summary>
/// <remarks>
/// This unit provides access to all runtime classes and interfaces that implement protobuf message types.
/// Client code may need to reference it in order to operate on generic protobuf messages.
/// </remarks>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uProtobufMessage;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  // To expose IProtobufMessage
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uIProtobufMessage,
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_CUSTOM_ATTRIBUTES}
  // RUNTIME-IMPL: Replace reference
  // To expose TProtobufFieldAttribute
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.StubRuntime.uProtobufFieldAttribute,
{$ENDIF}
  // RUNTIME-IMPL: Replace reference
  // To expose TProtobufMessage
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.StubRuntime.uProtobufMessage;

type
  /// <summary>
  /// Common interface of all generated classes that represent protobuf message types.
  /// </summary>
  /// <remarks>
  /// Can be used directly to handle messages of unknown type.
  /// The message instance carries transitive ownership of embedded objects in protobuf field values,
  /// and is responsible for their deallocation.
  /// </remarks>
  IProtobufMessage = Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uIProtobufMessage.IProtobufMessage;

  // RUNTIME-IMPL: Replace aliased type
  /// <summary>
  /// Common ancestor of all generated classes that represent protobuf message types.
  /// </summary>
  /// <remarks>
  /// Can be used directly to handle messages of unknown type.
  /// The message instance carries transitive ownership of embedded objects in protobuf field values,
  /// and is responsible for their deallocation.
  /// </remarks>
  TProtobufMessage = Work.Connor.Protobuf.Delphi.ProtocGenDelphi.StubRuntime.uProtobufMessage.TProtobufMessage;

{$IFDEF WORK_CONNOR_DELPHI_COMPILER_CUSTOM_ATTRIBUTES}
  // RUNTIME-IMPL: Replace aliased type
  /// <summary>
  /// RTTI attribute that annotates a property representing a protobuf field.
  /// </summary>
  ProtobufFieldAttribute = Work.Connor.Protobuf.Delphi.ProtocGenDelphi.StubRuntime.uProtobufFieldAttribute.TProtobufFieldAttribute;
{$ENDIF}

implementation

end.
