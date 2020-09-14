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

type
  /// <summary>
  /// TODO (and how to document type parameter?)
  /// </summary>
  TProtobufRepeatedField<T> = class(TEnumerable<T>)

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
    property Values[Index: Integer]: T read GetValue write SetValue; default;

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="aValue"></param>
    function Add(const aValue: T): Integer;

    /// <summary>
    /// TODO
    /// </summary>
    procedure Clear;

    /// <summary>
    /// TODO
    /// </summary>
    procedure Delete(aIndex: Integer);
  end;

implementation

end.
