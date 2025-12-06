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
/// Support code for handling of primitive values in <see cref="N:Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufRepeatedFieldValues"/>.
/// </summary>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufRepeatedPrimitiveFieldValues;

{$INCLUDE Work.Connor.Delphi.CompilerFeatures.inc}

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  // To extend TProtobufRepeatedFieldValues<T>
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufRepeatedFieldValues,
  // TProtobufWireCodec to construct new elements with default values
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufWireCodec,
  // IProtobufRepeatedFieldValues<T> for IProtobufRepeatedFieldValues<T> implementation
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uIProtobufRepeatedFieldValues,
  // TList for TProtobufRepeatedFieldValues<T> implementation
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.Generics.Collections;
{$ELSE}
  Generics.Collections;
{$ENDIF}

type
  /// <summary>
  /// Helper subclass of <see cref="T:TProtobufRepeatedFieldValues"/> for type parameters that are neither object types nor enumerated types.
  /// </summary>
  /// <typeparam name="T">Delphi type of the field values</typeparam>
  TProtobufRepeatedPrimitiveFieldValues<T> = class abstract(TProtobufRepeatedFieldValues<T>)
    private
      /// <summary>
      /// Backing storage for <see cref="GetStorage"/> using primitive values.
      /// </summary>
      FStorage: TList<T>;

    // Abstract members

    protected
      /// <summary>
      /// Getter for <see cref="WireCodec"/>.
      /// </summary>
      /// <returns>Field codec for the protobuf type</returns>
      function GetWireCodec: TProtobufWireCodec<T>; virtual; abstract;

      /// <summary>
      /// Field codec for the protobuf type.
      /// </summary>
      property WireCodec: TProtobufWireCodec<T> read GetWireCodec;

    // TProtobufRepeatedFieldValues<T> implementation

    public
      constructor Create; override;
      destructor Destroy; override;

    protected
      function GetStorage: TList<T>; override;
      function ConstructElement: T; override;
      procedure AssignFieldValues(aSource: TProtobufRepeatedFieldValues<T>); override;

    // IProtobufRepeatedFieldValues<T> implementation

    public
      procedure MergeFrom(aSource: IProtobufRepeatedFieldValues<T>); override;
    end;

implementation

// TProtobufRepeatedFieldValues<T> implementation

constructor TProtobufRepeatedPrimitiveFieldValues<T>.Create;
begin
  FStorage := TList<T>.Create;
end;

destructor TProtobufRepeatedPrimitiveFieldValues<T>.Destroy;
begin
  FStorage.Free;
end;

function TProtobufRepeatedPrimitiveFieldValues<T>.GetStorage: TList<T>;
begin
  result := FStorage;
end;

function TProtobufRepeatedPrimitiveFieldValues<T>.ConstructElement: T;
begin
  result := WireCodec.GetDefault;
end;

procedure  TProtobufRepeatedPrimitiveFieldValues<T>.AssignFieldValues(aSource: TProtobufRepeatedFieldValues<T>);
var
  lSource: TProtobufRepeatedPrimitiveFieldValues<T>;
begin
  lSource := aSource as TProtobufRepeatedPrimitiveFieldValues<T>;
  FStorage.Clear;
  FStorage.InsertRange(0, lSource.FStorage);
end;

// IProtobufRepeatedFieldValues<T> implementation

procedure TProtobufRepeatedPrimitiveFieldValues<T>.MergeFrom(aSource: IProtobufRepeatedFieldValues<T>);
var
  lSource: TProtobufRepeatedPrimitiveFieldValues<T>;
begin
  lSource := aSource as TProtobufRepeatedPrimitiveFieldValues<T>;
  FStorage.AddRange(lSource.FStorage);
end;

end.
