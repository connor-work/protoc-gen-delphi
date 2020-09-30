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
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.StubRuntime.uProtobufRepeatedField;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  // For TEnumerable implementation
  Generics.Collections;

type
  /// <summary>
  /// TODO
  /// </summary>
  /// <typeparam name="T">TODO</typeparam>
  TProtobufRepeatedField<T> = class(TEnumerable<T>)
  protected
    /// <summary>
    /// TODO
    /// </summary>
    function GetCount: Integer; virtual; abstract;

    /// <summary>
    /// TODO
    /// </summary>
    procedure SetCount(aCount: Integer); virtual; abstract;

    /// <summary>
    /// TODO
    /// </summary>
    function GetValue(aIndex: Integer): T; virtual; abstract;

    /// <summary>
    /// TODO
    /// </summary>
    procedure SetValue(aIndex: Integer; aValue: T); virtual; abstract;

    /// <summary>
    /// TODO
    /// </summary>
    function DoGetEnumerator: TList<T>.TEnumerator; override; abstract;

  public
    /// <summary>
    /// TODO
    /// </summary>
    constructor Create; virtual; abstract;

    /// <summary>
    /// TODO destroys transitively held resources, meaning field values of message type and their nested embedded objects
    /// </summary>
    destructor Destroy; override; abstract;

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
    function Add(const aValue: T): Integer; virtual; abstract;

    /// <summary>
    /// TODO
    /// </summary>
    /// <returns>TODO</return>
    function EmplaceAdd: T; virtual; abstract;

    /// <summary>
    /// TODO
    /// </summary>
    procedure Clear; virtual; abstract;

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="aIndex">TODO</param>
    procedure Delete(aIndex: Integer); virtual; abstract;

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="aIndex">TODO</param>
    /// <returns>TODO</returns>
    function ExtractAt(aIndex: Integer): T; virtual; abstract;
  end;

implementation

end.
