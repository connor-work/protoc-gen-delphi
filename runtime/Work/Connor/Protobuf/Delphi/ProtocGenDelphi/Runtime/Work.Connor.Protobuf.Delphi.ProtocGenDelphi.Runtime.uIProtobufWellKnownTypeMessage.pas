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
/// Runtime support interface for Protobuf message types that are one of the well-known types.
/// </summary>
/// <remarks>
/// This unit defines the common interface of all classes representing Protobuf message types that are one of the well-known types (<see cref="IProtobufWellKnownTypeMessage"/>).
/// Client code should reference it indirectly through one of the classes that represent a well-known type.
/// </remarks>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uIProtobufWellKnownTypeMessage;

{$INCLUDE Work.Connor.Delphi.CompilerFeatures.inc}

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uIProtobufMessage;

type
  /// <summary>
  /// Common interface of all classes that represent Protobuf message types that are one of the well-known types.
  /// </summary>
  /// <remarks>
  /// The message instance carries transitive ownership of embedded objects that represent Protobuf field values,
  /// and is responsible for their deallocation.
  /// </remarks>
  IProtobufWellKnownTypeMessage = interface(IProtobufMessage)
    ['{65AC9E1A-8395-4772-9890-B0206B5694C2}']
  end;

implementation

end.
