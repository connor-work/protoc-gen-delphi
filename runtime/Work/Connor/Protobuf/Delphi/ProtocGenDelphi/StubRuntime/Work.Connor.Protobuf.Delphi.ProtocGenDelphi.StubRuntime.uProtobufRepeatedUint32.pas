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
/// Runtime library support for protobuf repeated fields of the protobuf type <c>uint32</c>.
/// </summary>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.StubRuntime.uProtobufRepeatedUint32;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  Classes,
  // Support code for handling protobuf repeated fields
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.StubRuntime.uProtobufRepeatedField;

type
  /// <summary>
  /// Concrete subclass of <see cref="TProtobufRepeatedField"/> for protobuf repeated fields of the protobuf type <c>uint32</c>.
  /// </summary>
  TProtobufRepeatedUint32Field = class(TProtobufRepeatedField<UInt32>)
  end;

implementation

end.
