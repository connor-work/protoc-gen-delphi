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
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtoJsonFormat,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uIProtobufMessage,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uIProtobufWellKnownTypeMessage;

type
  /// <summary>
  /// Common interface of all generated classes that represent Protobuf message types that are not one of the well-known types.
  /// </summary>
  /// <remarks>
  /// Can be used directly to handle messages of an unknown type that is known not to be one of the well-known message types.
  /// The message instance carries transitive ownership of embedded objects in Protobuf field values,
  /// and is responsible for their deallocation.
  /// </remarks>
  IProtobufNotWellKnownTypeMessage = interface(IProtobufMessage, IProtobuf)
    ['{60876367-A0DF-4D03-A439-0C94845DBAA3}']
  end;

implementation

end.
