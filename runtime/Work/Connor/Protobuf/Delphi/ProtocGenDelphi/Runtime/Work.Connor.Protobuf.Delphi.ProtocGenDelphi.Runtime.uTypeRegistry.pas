/// Copyright 2025 Connor Erdmann (connor.work)
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
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uTypeRegistry;

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
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.SysUtils,
{$ELSE}
  SysUtils,
{$ENDIF}
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.uProtobuf,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufMessageBase;

type
  // TODO contract
  TProtobufMessageType = class of TProtobufMessageBase;

  // TODO contract
  TProtobufTypeRegistry = class sealed(TPersistent)
    private
      // TODO contract
      FNotWellKnownTypes: TDictionary<TProtobufTypeUrl, TProtobufMessageType>;

      // TODO contract
      class var FGlobal: TProtobufTypeRegistry;

      // TODO contract
      class function GetGlobal: TProtobufTypeRegistry; static;

    public
      // TODO contract
      class property Global: TProtobufTypeRegistry read GetGlobal;

      // TODO contract
      constructor Create;

      // TODO contract
      destructor Destroy; override; final;

      // TODO contract
      function GetType(aTypeUrl: TProtobufTypeUrl): TProtobufMessageType;

      // TODO contract
      procedure RegisterNotWellKnownType(aTypeUrl: TProtobufTypeUrl; aType: TProtobufMessageType);

    // TPersistent implementation
    public
      /// <summary>
      /// Copies the data from another type registry to this one.
      /// </summary>
      /// <param name="aSource">Object to copy from</param>
      /// <remarks>
      /// The object must be another <see cref="TProtobufTypeRegistry"/>.
      /// TODO contract This performs a deep copy; hence, no ownership is shared.
      /// This causes the destruction of <see cref="Payload"/>; developers must ensure that no shared ownership is held.
      /// </remarks>
      procedure Assign(aSource: TPersistent); override; final;
  end;

implementation

uses
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uAny,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uDuration,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uEmpty,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uTimestamp;

// TProtobufTypeRegistry implementation

class function TProtobufTypeRegistry.GetGlobal: TProtobufTypeRegistry;
begin
  if (not Assigned(FGlobal)) then FGlobal := TProtobufTypeRegistry.Create;
  result := FGlobal;
end;

constructor TProtobufTypeRegistry.Create;
begin
  FNotWellKnownTypes := TDictionary<TProtobufTypeUrl, TProtobufMessageType>.Create;
end;

destructor TProtobufTypeRegistry.Destroy;
begin
  FNotWellKnownTypes.Free;
  inherited;
end;

function TProtobufTypeRegistry.GetType(aTypeUrl: TProtobufTypeUrl): TProtobufMessageType;
begin
  if (aTypeUrl.StartsWith(PROTOBUF_TYPE_URL_DEFAULT_PREFIX + PROTOBUF_WELL_KNOWN_TYPES_PACKAGE)) then
  begin
    if aTypeUrl = TAny.PROTOBUF_TYPE_URL then Exit(TAny)
    else if aTypeUrl = TDuration.PROTOBUF_TYPE_URL then Exit(TDuration)
    else if aTypeUrl = TEmpty.PROTOBUF_TYPE_URL then Exit(TEmpty)
    else if aTypeUrl = TTimestamp.PROTOBUF_TYPE_URL then Exit(TTimestamp);
  end;
  FNotWellKnownTypes.TryGetValue(aTypeUrl, result);
end;

procedure TProtobufTypeRegistry.RegisterNotWellKnownType(aTypeUrl: TProtobufTypeUrl; aType: TProtobufMessageType);
var
  lExistingType: TProtobufMessageType;
begin
  if (FNotWellKnownTypes.TryGetValue(aTypeUrl, lExistingType)) then
  begin
    if (lExistingType <> aType) then raise Exception.Create('Duplicate registration of different types for type URL: ' + aTypeUrl);
  end
  else FNotWellKnownTypes.Add(aTypeUrl, aType);
end;

// TPersistent implementation of TProtobufTypeRegistry

procedure TProtobufTypeRegistry.Assign(aSource: TPersistent);
var
  lSource: TProtobufTypeRegistry;
  lSourceNotWellKnownType: TPair<TProtobufTypeUrl, TProtobufMessageType>;
begin
  lSource := aSource as TProtobufTypeRegistry;
  if (not Assigned(lSource)) then
  begin
    inherited;
    Exit;
  end;
  FNotWellKnownTypes.Free;
  FNotWellKnownTypes := TDictionary<TProtobufTypeUrl, TProtobufMessageType>.Create(lSource.FNotWellKnownTypes);
end;

initialization

if (not Assigned(TProtobufTypeRegistry.FGlobal)) then TProtobufTypeRegistry.FGlobal := TProtobufTypeRegistry.Create;

finalization

FreeAndNil(TProtobufTypeRegistry.FGlobal);

end.

