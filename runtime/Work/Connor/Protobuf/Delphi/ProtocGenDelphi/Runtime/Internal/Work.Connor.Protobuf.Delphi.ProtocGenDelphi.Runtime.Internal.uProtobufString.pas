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
/// Runtime-internal support for the protobuf type <c>string</c>.
/// </summary>
/// <remarks>
/// Generated code needs to reference this unit in order to operate on protobuf field values of this type.
/// </remarks>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufString;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  // Runtime-internal support for the protobuf binary wire format
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uIProtobufWireCodec,
  // To implement TProtobufDelimitedWireCodec<UnicodeString>
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufDelimitedWireCodec,
  // TBytes for TProtobufDelimitedWireCodec<UnicodeString> implementation
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.SysUtils;
{$ELSE}
  SysUtils;
{$ENDIF}

var
  /// <summary>
  /// <i>Field codec</i> for <c>protoc-gen-delphi</c> that defines the encoding/decoding of
  /// protobuf fields of type <c>string</c> from/to the protobuf binary wire format.
  /// </summary>
  gProtobufWireCodecString: IProtobufWireCodec<UnicodeString>;

type
  /// <summary>
  /// Runtime library implementation of <see cref="T:IProtobufWireCodec"/> for the protobuf type <c>string</c>.
  /// </summary>
  TProtobufStringWireCodec = class(TProtobufDelimitedWireCodec<UnicodeString>)
    // TProtobufDelimitedWireCodec<UnicodeString> implementation

    public
      function FromBytes(aValue: TBytes): UnicodeString; override;
      function ToBytes(aValue: UnicodeString): TBytes; override;

    // TProtobufWireCodec<UnicodeString> implementation
    
    public
      function GetDefault: UnicodeString; override;
      function IsDefault(aValue: UnicodeString): Boolean; override;
  end;

implementation

uses
  // For protobuf default values
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.uProtobuf;

// TProtobufDelimitedWireCodec<UnicodeString> implementation

function TProtobufStringWireCodec.FromBytes(aValue: TBytes): UnicodeString;
begin
  result := TEncoding.UTF8.GetString(aValue);
end;

function TProtobufStringWireCodec.ToBytes(aValue: UnicodeString): TBytes;
begin
  result := TEncoding.UTF8.GetBytes(aValue);
end;

// TProtobufWireCodec<UnicodeString> implementation

function TProtobufStringWireCodec.GetDefault: UnicodeString;
begin
  result := PROTOBUF_DEFAULT_VALUE_STRING;
end;

function TProtobufStringWireCodec.IsDefault(aValue: UnicodeString): Boolean;
begin
  result := aValue = GetDefault;
end;

initialization
begin
  gProtobufWireCodecString := TProtobufStringWireCodec.Create;
end;

end.
