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

// TODO contract
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufRepeatedMessageFieldValues;

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
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufMessageBase,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufWireFormat;

type
  // TODO contract
  TProtobufRepeatedMessageFieldValues<TMessage : TProtobufMessageBase, constructor> = class sealed(TProtobufRepeatedFieldValuesBase<TMessage>)
    private
      /// <summary>
      /// Backing storage for <see cref="GetStorage"/>.
      /// </summary>
      FStorage: TObjectList<TMessage>;

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

    // TProtobufRepeatedFieldValuesBase<T> implementation
    protected
      /// <summary>
      /// Getter for <see cref="Storage"/>.
      /// </summary>
      /// <returns>The internal backing storage</returns>
      function GetStorage: TList<TMessage>; override; final;

      /// <summary>
      /// Constructs a new default field value for insertion into the backing storage.
      /// </summary>
      /// <returns>The new field value</returns>
      /// <remarks>
      /// The default value for a message field type is an empty instance of the message type.
      /// </remarks>
      function ConstructElement: TMessage; override; final;

    public
      // TODO contract
      procedure EncodeField(aDest: TStream; aFieldNumber: TProtobufFieldNumber); override; final;

      // TODO contract
      procedure MergeFromField(aSource: TStream; aWireType: TProtobufWireType; aRemainingLength: PUInt32 = nil); override; final;
  end;

implementation

destructor TProtobufRepeatedMessageFieldValues<TMessage>.Destroy;
begin
  FStorage.Free;
  inherited;
end;

// TInterfacedPersistent implementation of TProtobufRepeatedMessageFieldValues<TMessage>

procedure TProtobufRepeatedMessageFieldValues<TMessage>.Assign(aSource: TPersistent);
var
  lSource: TProtobufRepeatedMessageFieldValues<TMessage>;
  lValue: TMessage;
  lValueCopy: TMessage;
begin
  if (not (aSource is TProtobufRepeatedMessageFieldValues<TMessage>)) then
  begin
    inherited;
    exit;
  end;
  lSource := TProtobufRepeatedMessageFieldValues<TMessage>(aSource);
  FStorage.Clear;
  for lValue in lSource.FStorage do
  begin
    lValueCopy := EmplaceAdd;
    lValueCopy.Assign(lValue);
  end;
end;

// TProtobufRepeatedFieldValuesBase<TMessage> implementation of TProtobufRepeatedMessageFieldValues<TMessage>

function TProtobufRepeatedMessageFieldValues<TMessage>.GetStorage: TList<TMessage>;
begin
  if (not(Assigned(FStorage))) then FStorage := TObjectList<TMessage>.Create;
  result := FStorage;
end;

function TProtobufRepeatedMessageFieldValues<TMessage>.ConstructElement: TMessage;
begin
  result := TMessage.Create;
end;

procedure TProtobufRepeatedMessageFieldValues<TMessage>.EncodeField(aDest: TStream; aFieldNumber: TProtobufFieldNumber);
begin
  // TODO implementation
end;

procedure TProtobufRepeatedMessageFieldValues<TMessage>.MergeFromField(aSource: TStream; aWireType: TProtobufWireType; aRemainingLength: PUInt32 = nil);
var
  lValue: TMessage;
begin
  lValue := EmplaceAdd;
  // TODO problem with ARC? will message be freed by upcast to IProtobufMessage?
  // TODO implementation MergeFromProtobufMessageField()
end;

end.
