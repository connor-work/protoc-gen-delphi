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
/// "Stub runtime" <c>protobuf-delphi</c> runtime library implementation of support code for handling protobuf repeated fields 
/// in generated Delphi code.
/// </summary>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.StubRuntime.uProtobufRepeatedField;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  Generics.Collections, // For TEnumerable implementation
  SysUtils; // For Exception in stubs

type
  /// <summary>
  /// TODO
  /// </summary>
  /// <typeparam name="T">TODO</typeparam>
  TProtobufRepeatedField<T> = class(TEnumerable<T>)

  private
    /// <summary>
    /// TODO
    /// </summary>
    function GetCount: Integer;

    /// <summary>
    /// TODO
    /// </summary>
    procedure SetCount(aCount: Integer);

    /// <summary>
    /// TODO
    /// </summary>
    function GetValue(aIndex: Integer): T;

    /// <summary>
    /// TODO
    /// </summary>
    procedure SetValue(aIndex: Integer; aValue: T);

  public
    /// <summary>
    /// TODO
    /// </summary>
    constructor Create; virtual;

    /// <summary>
    /// TODO destroys transitively held resources, meaning field values of message type and their nested embedded objects
    /// </summary>
    destructor Destroy; override;

    /// <summary>
    /// TODO
    /// </summary>
    property Count: Integer read GetCount write SetCount;

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="aIndex">TODO</param>
    property Values[aIndex: Integer]: T read GetValue write SetValue; default;

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="aValue">TODO</param>
    /// <returns>TODO</return>
    function Add(const aValue: T): Integer;

    /// <summary>
    /// TODO
    /// </summary>
    /// <returns>TODO</return>
    function EmplaceAdd: T;

    /// <summary>
    /// TODO
    /// </summary>
    procedure Clear;

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="aIndex">TODO</param>
    procedure Delete(aIndex: Integer);

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="aIndex">TODO</param>
    /// <returns>TODO</returns>
    function ExtractAt(aIndex: Integer): T;

  private
    /// <summary>
    /// Called by the stub runtime to signal that some functionality is not implemented.
    /// </summary>
    class procedure NotImplementedInStub;
  end;

implementation

constructor TProtobufRepeatedField<T>.Create;
begin
  NotImplementedInStub;
end;

destructor TProtobufRepeatedField<T>.Destroy;
begin
  NotImplementedInStub;
end;

function TProtobufRepeatedField<T>.GetCount;
begin
  NotImplementedInStub;
end;

procedure TProtobufRepeatedField<T>.SetCount(aCount: Integer);
begin
  NotImplementedInStub;
end;

function TProtobufRepeatedField<T>.GetValue(aIndex: Integer): T;
begin
  NotImplementedInStub;
end;

procedure TProtobufRepeatedField<T>.SetValue(aIndex: Integer; aValue: T);
begin
  NotImplementedInStub;
end;

function TProtobufRepeatedField<T>.Add(const aValue: T): Integer;
begin
  NotImplementedInStub;
end;

procedure TProtobufRepeatedField<T>.Clear;
begin
  NotImplementedInStub;
end;

procedure TProtobufRepeatedField<T>.Delete(aIndex: Integer);
begin
  NotImplementedInStub;
end;

class procedure TProtobufRepeatedField<T>.NotImplementedInStub;
begin
  raise Exception.Create('This functionality is not implemented by the stub runtime');
end;

end.
