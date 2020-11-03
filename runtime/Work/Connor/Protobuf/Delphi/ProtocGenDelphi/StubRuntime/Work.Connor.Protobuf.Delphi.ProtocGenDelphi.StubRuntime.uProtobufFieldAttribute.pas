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
/// Runtime library support code for RTTI attributes on properties representing protobuf fields.
/// </summary>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.StubRuntime.uProtobufFieldAttribute;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  // To implement IProtobufFieldAttribute
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uIProtobufFieldAttribute,
  // Basic protobuf definitions like TProtobufFieldNumber
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.uProtobuf,
  // Stub runtime helper code
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.StubRuntime.uStubRuntime;

type
  /// <summary>
  /// Generic runtime library implementation of <see cref="T:IProtobufFieldAttribute"/>.
  /// </summary>
  TProtobufFieldAttribute = class(TCustomAttribute, IProtobufFieldAttribute, IInterface)
    public
      /// <summary>
      /// Constructs an RTII attribute for a property representing a protobuf field.
      /// </summary>
      /// <param name="aName">The name of the protobuf field</param>
      /// <param name="aNumber">The field number of the protobuf field</param>
      constructor Create(aName: String; aNumber: TProtobufFieldNumber);

    // IProtobufFieldAttribute implementation

    public
      /// <summary>
      /// Getter for <see cref="Name"/>.
      /// </summary>
      /// <returns>The name of the protobuf field</returns>
      function GetName: String;

      /// <summary>
      /// Name of the protobuf field.
      /// </summary>
      /// <remarks>
      /// This matches the original name of the field in the protobuf schema definition, not the derived name of the corresponding Delphi property.
      /// </remarks>
      property Name: String read GetName;

      /// <summary>
      /// Getter for <see cref="Number"/>.
      /// </summary>
      /// <returns>The field number of the protobuf field</returns>
      function GetNumber: TProtobufFieldNumber;

      /// <summary>
      /// Field number of the protobuf field.
      /// </summary>
      property Number: TProtobufFieldNumber read GetNumber;

    // IInterface implementation

    public
      function QueryInterface(const IID: TGUID; out Obj): HResult; stdcall;
      function _AddRef: Integer; stdcall;
      function _Release: Integer; stdcall;
  end;

implementation

constructor TProtobufFieldAttribute.Create(aName: String; aNumber: TProtobufFieldNumber);
begin
  raise NotImplementedInStub;
end;

// IProtobufFieldAttribute implementation

function TProtobufFieldAttribute.GetName: String;
begin
  raise NotImplementedInStub;
end;

function TProtobufFieldAttribute.GetNumber: TProtobufFieldNumber;
begin
  raise NotImplementedInStub;
end;

// IInterface implementation (no reference counting)

function TProtobufFieldAttribute.QueryInterface(const IID: TGUID; out Obj): HResult;
begin
  if GetInterface(IID, Obj) then
    Result:= S_OK
  else
    Result:= E_NOINTERFACE;
end;
 
function TProtobufFieldAttribute._AddRef: Integer;
begin
  Result:= -1;
end;
 
function TProtobufFieldAttribute._Release: Integer;
begin
  Result:= -1;
end;

end.
