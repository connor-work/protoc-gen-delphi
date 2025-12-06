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
/// Runtime-internal support for the protobuf type <c>bytes</c>.
/// </summary>
/// <remarks>
/// Generated code needs to reference this unit in order to operate on protobuf field values of this type.
/// </remarks>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufBytes;

{$INCLUDE Work.Connor.Delphi.CompilerFeatures.inc}

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  // Runtime-internal support for the protobuf binary wire format
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uIProtobufWireCodec,
  // To represent values as TBytes
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.SysUtils,
{$ELSE}
  SysUtils,
{$ENDIF}
  // To implement TProtobufDelimitedWireCodec<TBytes>
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufDelimitedWireCodec;

var
  /// <summary>
  /// <i>Field codec</i> for <c>protoc-gen-delphi</c> that defines the encoding/decoding of
  /// protobuf fields of type <c>bytes</c> from/to the protobuf binary wire format.
  /// </summary>
  gProtobufWireCodecBytes: IProtobufWireCodec<TBytes>;

type
  /// <summary>
  /// Runtime library implementation of <see cref="T:IProtobufWireCodec"/> for the protobuf type <c>bytes</c>.
  /// </summary>
  TProtobufBytesWireCodec = class(TProtobufDelimitedWireCodec<TBytes>)
    // TProtobufDelimitedWireCodec<TBytes> implementation

    public
      function FromBytes(aValue: TBytes): TBytes; override;
      function ToBytes(aValue: TBytes): TBytes; override;

    // TProtobufWireCodec<TBytes> implementation
    
    public
      function GetDefault: TBytes; override;
      function IsDefault(aValue: TBytes): Boolean; override;
  end;

implementation

uses
  // For protobuf default values
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.uProtobuf;

// TProtobufDelimitedWireCodec<TBytes> implementation

function TProtobufBytesWireCodec.FromBytes(aValue: TBytes): TBytes;
begin
  result := aValue;
end;

function TProtobufBytesWireCodec.ToBytes(aValue: TBytes): TBytes;
begin
  result := aValue;
end;

// TProtobufWireCodec<TBytes> implementation

function TProtobufBytesWireCodec.GetDefault: TBytes;
begin
  result := PROTOBUF_DEFAULT_VALUE_BYTES;
end;

function TProtobufBytesWireCodec.IsDefault(aValue: TBytes): Boolean;
begin
  result := aValue = GetDefault;
end;

initialization
begin
  gProtobufWireCodecBytes := TProtobufBytesWireCodec.Create;
end;

end.
