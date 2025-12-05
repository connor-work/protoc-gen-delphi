/// Copyright 2025 Connor Erdmann (connor.work)
/// Copyright 2020 Julien Scholz
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
/// Runtime-internal support for the protobuf repeated fields of the protobuf type <c>uint32</c>.
/// </summary>
/// <remarks>
/// Generated code needs to reference this unit in order to operate on protobuf repeated field values of this type.
/// </remarks>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufRepeatedUint32;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  // To extend TProtobufRepeatedVarintFieldValues<UInt32>
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufRepeatedVarintFieldValues,
  // TProtobufVarintWireCodec for TProtobufRepeatedVarintFieldValues<UInt32> implementation
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufVarintWireCodec;

type
  /// <summary>
  /// Concrete subclass of <see cref="T:TProtobufRepeatedVarintFieldValues"/> for protobuf repeated fields of the protobuf type <c>uint32</c>.
  /// </summary>
  TProtobufRepeatedUint32FieldValues = class(TProtobufRepeatedVarintFieldValues<UInt32>)
    // TProtobufRepeatedVarintFieldValues<UInt32> implementation

    protected
      function GetVarintWireCodec: TProtobufVarintWireCodec<UInt32>; override;
  end;

implementation

uses
  // TProtobufUint32WireCodec as field codec
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufUint32;

// TProtobufRepeatedVarintFieldValues<UInt32> implementation

function TProtobufRepeatedUint32FieldValues.GetVarintWireCodec: TProtobufVarintWireCodec<UInt32>;
begin
  result := gProtobufWireCodecUint32 as TProtobufUint32WireCodec;
end;

end.
