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
/// Runtime library implementation of support code for handling protobuf repeated fields 
/// in generated Delphi code.
/// </summary>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.StubRuntime.uProtobufRepeatedBasicField;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  // For TEnumerable implementation
  Generics.Collections,
  // Super class of TProtobufRepeatedBasicField
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.StubRuntime.uProtobufRepeatedField,
  // Helper code for the stub runtime library, not required by functional implementations of the runtime library
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.StubRuntime.uStub;

type
  /// <summary>
  /// TODO
  /// </summary>
  /// <typeparam name="T">TODO</typeparam>
  TProtobufRepeatedBasicField<T> = class(TProtobufRepeatedField<T>)
  private
    FStorage: TList<T>;

  protected
    function GetCount: Integer; override;
    procedure SetCount(aCount: Integer); override;
    function GetValue(aIndex: Integer): T; override;
    procedure SetValue(aIndex: Integer; aValue: T); override;
    function DoGetEnumerator: TList<T>.TEnumerator; override;

  public
    constructor Create; override;
    destructor Destroy; override;

    function Add(const aValue: T): Integer; override;
    function EmplaceAdd: T; override;

    procedure Clear; override;
    procedure Delete(aIndex: Integer); override;
    function ExtractAt(aIndex: Integer): T; override;
  end;

implementation

constructor TProtobufRepeatedBasicField<T>.Create;
begin
  NotImplementedInStub;
end;

destructor TProtobufRepeatedBasicField<T>.Destroy;
begin
  NotImplementedInStub;
end;

function TProtobufRepeatedBasicField<T>.GetCount;
begin
  NotImplementedInStub;
end;

procedure TProtobufRepeatedBasicField<T>.SetCount(aCount: Integer);
begin
  NotImplementedInStub;
end;

function TProtobufRepeatedBasicField<T>.GetValue(aIndex: Integer): T;
begin
  NotImplementedInStub;
end;

procedure TProtobufRepeatedBasicField<T>.SetValue(aIndex: Integer; aValue: T);
begin
  NotImplementedInStub;
end;

function TProtobufRepeatedBasicField<T>.DoGetEnumerator: TList<T>.TEnumerator;
begin
  NotImplementedInStub;
end;

function TProtobufRepeatedBasicField<T>.Add(const aValue: T): Integer;
begin
  NotImplementedInStub;
end;

function TProtobufRepeatedBasicField<T>.EmplaceAdd: T;
begin
  NotImplementedInStub;
end;

procedure TProtobufRepeatedBasicField<T>.Clear;
begin
  NotImplementedInStub;
end;

procedure TProtobufRepeatedBasicField<T>.Delete(aIndex: Integer);
begin
  NotImplementedInStub;
end;

function TProtobufRepeatedBasicField<T>.ExtractAt(aIndex: Integer): T;
begin
  NotImplementedInStub;
end;

end.
