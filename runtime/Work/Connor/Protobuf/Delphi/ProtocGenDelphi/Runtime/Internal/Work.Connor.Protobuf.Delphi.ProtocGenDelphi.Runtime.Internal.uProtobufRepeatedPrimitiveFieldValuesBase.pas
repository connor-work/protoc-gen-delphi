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

// TODO contract
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufRepeatedPrimitiveFieldValuesBase;

{$INCLUDE Work.Connor.Delphi.CompilerFeatures.inc}

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.Classes,
{$ELSE}
  Classes,
{$ENDIF}
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.Generics.Collections,
{$ELSE}
  Generics.Collections,
{$ENDIF}
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.uProtobuf,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufRepeatedFieldValuesBase,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufWireFormat;

type
  // TODO contract
  TProtobufRepeatedPrimitiveFieldValuesBase<TValue> = class abstract(TProtobufRepeatedFieldValuesBase<TValue>)
    private
      /// <summary>
      /// Backing storage for <see cref="GetStorage"/>.
      /// </summary>
      FStorage: TList<TValue>;

    protected
      // TODO contract
      procedure EncodeSingularField(aDest: TStream; aFieldNumber: TProtobufFieldNumber; aValue: TValue); virtual; abstract;

      // TODO contract
      function DecodeSingularField(aSource: TStream; aWireType: TProtobufWireType; aRemainingLength: PUInt32): TValue; virtual; abstract;

    public
      // TODO contract
      destructor Destroy;

    // TInterfacedPersistent implementation
    public
      /// <summary>
      /// Copies the data from another repeated field to this one.
      /// </summary>
      /// <param name="aSource">Object to copy from</param>
      /// <remarks>
      /// The object must be another repeated field of the same value type.
      /// TODO This performs a deep copy; hence, no ownership is shared.
      /// This causes the destruction of <see cref="Payload"/>; developers must ensure that no shared ownership is held.
      /// </remarks>
      procedure Assign(aSource: TPersistent); override; final;

    // TProtobufRepeatedFieldValuesBase<TValue> implementation
    protected
      /// <summary>
      /// Getter for <see cref="Storage"/>.
      /// </summary>
      /// <returns>The internal backing storage</returns>
      function GetStorage: TList<TValue>; override; final;

    public
      // TODO contract
      procedure EncodeField(aDest: TStream; aFieldNumber: TProtobufFieldNumber); override; final;

      // TODO contract
      procedure MergeFromField(aSource: TStream; aWireType: TProtobufWireType; aRemainingLength: PUInt32 = nil); override; final;
  end;

implementation

destructor TProtobufRepeatedPrimitiveFieldValuesBase<TValue>.Destroy;
begin
  FStorage.Free;
  inherited;
end;

// TInterfacedPersistent implementation of TProtobufRepeatedPrimitiveFieldValuesBase<TValue>

procedure TProtobufRepeatedPrimitiveFieldValuesBase<TValue>.Assign(aSource: TPersistent);
var
  lSource: TProtobufRepeatedPrimitiveFieldValuesBase<TValue>;
begin
  if (not (aSource is TProtobufRepeatedPrimitiveFieldValuesBase<TValue>)) then
  begin
    inherited;
    exit;
  end;
  lSource := TProtobufRepeatedPrimitiveFieldValuesBase<TValue>(aSource);
  FStorage.Clear;
  FStorage.InsertRange(0, lSource.FStorage);
end;

// TProtobufRepeatedFieldValuesBase<TValue> implementation of TProtobufRepeatedPrimitiveFieldValuesBase<TValue>

function TProtobufRepeatedPrimitiveFieldValuesBase<TValue>.GetStorage: TList<TValue>;
begin
  if (not(Assigned(FStorage))) then FStorage := TList<TValue>.Create;
  result := FStorage;
end;

// TODO packed support
procedure TProtobufRepeatedPrimitiveFieldValuesBase<TValue>.EncodeField(aDest: TStream; aFieldNumber: TProtobufFieldNumber);
var
  lValue: TValue;
begin
  for lValue in GetStorage do EncodeSingularField(aDest, aFieldNumber, lValue);
end;

// TODO packed support
procedure TProtobufRepeatedPrimitiveFieldValuesBase<TValue>.MergeFromField(aSource: TStream; aWireType: TProtobufWireType; aRemainingLength: PUInt32 = nil);
var
  lValue: TValue;
begin
  lValue := DecodeSingularField(aSource, aWireType, aRemainingLength);
  GetStorage.Add(lValue);
end;

end.
