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
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufRepeatedFieldValuesBase;

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
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uIProtobufRepeatedFieldValues,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufWireFormat;

type
  // TODO contract
  TProtobufRepeatedFieldValuesBase<TValue> = class abstract(TInterfacedPersistent, IProtobufRepeatedFieldValues<TValue>)
    protected
      /// <summary>
      /// Getter for <see cref="Storage"/>.
      /// </summary>
      /// <returns>The internal backing storage</returns>
      function GetStorage: TList<TValue>; virtual; abstract;

      /// <summary>
      /// Constructs a new default field value for insertion into the backing storage.
      /// </summary>
      /// <returns>The new field value</returns>
      /// <remarks>
      /// The default value for a non-message field type is the Protobuf default value for the type.
      /// The default value for a message field type is an empty instance of the message type.
      /// </remarks>
      function ConstructElement: TValue; virtual; abstract;

    private
      /// <summary>
      /// Internal list as backing storage for field values.
      /// </summary>
      property Storage: TList<TValue> read GetStorage;

    public
      // TODO contract
      procedure EncodeField(aDest: TStream; aFieldNumber: TProtobufFieldNumber); virtual; abstract;

      // TODO contract
      procedure MergeFromField(aSource: TStream; aWireType: TProtobufWireType; aRemainingLength: PUInt32 = nil); virtual; abstract;

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
      procedure Assign(aSource: TPersistent); override; abstract;

    // IProtobufRepeatedFieldValues<TValue> implementation
    public
      /// <summary>
      /// Returns an enumerator for the Protobuf field values, which enables usage of <c>for..in</c> loops.
      /// </summary>
      /// <returns>Enumerator for the Protobuf field values, in order</returns>
      function GetEnumerator: TEnumerator<TValue>;

      /// <summary>
      /// Getter for <see cref="Count"/>.
      /// </summary>
      /// <returns>The number of field values</returns>
      function GetCount: Integer;

      /// <summary>
      /// Setter for <see cref="Count"/>.
      /// </summary>
      /// <param name="aCount">The new number of field values</param>
      procedure SetCount(aCount: Integer);

      /// <summary>
      /// Indexed getter for <see cref="Values"/>.
      /// </summary>
      /// <param name="aIndex">The index to read from</param>
      /// <returns>The field value at the specified index</returns>
      function GetValue(aIndex: Integer): TValue;

      /// <summary>
      /// Indexed setter for <see cref="Values"/>.
      /// </summary>
      /// <param name="aIndex">The index to write to</param>
      /// <param name="aValue">The new field value at the specified index</param>
      procedure SetValue(aIndex: Integer; aValue: TValue);

      /// <summary>
      /// Adds a field value to the end of the sequence.
      /// </summary>
      /// <param name="aValue">The field value to add</param>
      /// <returns>The index of the new field value</return>
      /// <remarks>
      /// This operation transfers ownership of the added field value to the repeated field.
      /// </remarks>
      function Add(const aValue: TValue): Integer;

      /// <summary>
      /// Adds a default field value to the end of the sequence.
      /// </summary>
      /// <returns>The new field value</return>
      /// <remarks>
      /// The default value for a non-message field type is the Protobuf default value for the type.
      /// The default value for a message field type is an empty instance of the message type.
      /// </remarks>
      function EmplaceAdd: TValue;

      /// <summary>
      /// Removes all field values.
      /// </summary>
      /// <remarks>
      /// Developers must ensure that no shared ownership of the destroyed field values or further nested embedded objects is held.
      /// </remarks>
      procedure Clear;

      /// <summary>
      /// Removes and destroys a field value at the specified index.
      /// </summary>
      /// <param name="aIndex">The index to delete at</param>
      /// <remarks>
      /// Developers must ensure that no shared ownership of the destroyed field value or further nested embedded objects is held.
      /// </remarks>
      procedure Delete(aIndex: Integer);

      /// <summary>
      /// Removes a field value at the specified index and transfers ownership to the caller.
      /// </summary>
      /// <param name="aIndex">The index to extract a value from</param>
      /// <remarks>
      /// Unlike <see cref="Delete"/>, this method does not destroy the value. The caller is reponsible for managing (and eventually releasing)
      /// all resources held by the value.
      /// </remarks>
      function ExtractAt(aIndex: Integer): TValue;
  end;

implementation

// IProtobufRepeatedFieldValues<TValue> implementation of TProtobufRepeatedFieldValuesBase<TValue>

function TProtobufRepeatedFieldValuesBase<TValue>.GetEnumerator: TEnumerator<TValue>;
begin
  result := Storage.GetEnumerator;
end;

function TProtobufRepeatedFieldValuesBase<TValue>.GetCount: Integer;
begin
  result := Storage.Count;
end;

procedure TProtobufRepeatedFieldValuesBase<TValue>.SetCount(aCount: Integer);
var
  lOldCount: Integer;
  lIndex: Integer;
begin
  lOldCount := Storage.Count;
  Storage.Count := aCount;
  for lIndex := lOldCount to aCount - 1 do Storage[lIndex] := ConstructElement;
end;

function TProtobufRepeatedFieldValuesBase<TValue>.GetValue(aIndex: Integer): TValue;
begin
  result := Storage[aIndex];
end;

procedure TProtobufRepeatedFieldValuesBase<TValue>.SetValue(aIndex: Integer; aValue: TValue);
begin
  Storage[aIndex] := aValue;
end;

function TProtobufRepeatedFieldValuesBase<TValue>.Add(const aValue: TValue): Integer;
begin
  Storage.Add(aValue);
end;

function TProtobufRepeatedFieldValuesBase<TValue>.EmplaceAdd: TValue;
begin
  SetCount(Storage.Count + 1);
  result := Storage.Last;
end;

procedure TProtobufRepeatedFieldValuesBase<TValue>.Clear;
begin
  // TODO move this to implementation so we can clear if not nil?
  Storage.Clear;
end;

procedure TProtobufRepeatedFieldValuesBase<TValue>.Delete(aIndex: Integer);
begin
  Storage.Delete(aIndex);
end;

function TProtobufRepeatedFieldValuesBase<TValue>.ExtractAt(aIndex: Integer): TValue;
begin
{$IFDEF FPC}
  result := Storage.ExtractIndex(aIndex);
{$ELSE}
  result := Storage.ExtractAt(aIndex);
{$ENDIF}
end;

end.

