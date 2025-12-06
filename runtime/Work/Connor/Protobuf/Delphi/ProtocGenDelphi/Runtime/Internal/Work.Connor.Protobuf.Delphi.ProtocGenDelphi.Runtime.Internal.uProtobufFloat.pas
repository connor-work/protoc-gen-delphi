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
/// Runtime-internal support for the protobuf type <c>float</c>.
/// </summary>
/// <remarks>
/// Generated code needs to reference this unit in order to operate on protobuf field values of this type.
/// </remarks>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufFloat;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  // Runtime-internal support for the protobuf binary wire format
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uIProtobufWireCodec,
  // To implement TProtobufFixedWidthWireCodec<Single>
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufFixedWidthWireCodec,
  // For wire type
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufTag,
  // TStream for encoding of messages
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.Classes;
{$ELSE}
  Classes;
{$ENDIF}

var
  /// <summary>
  /// <i>Field codec</i> for <c>protoc-gen-delphi</c> that defines the encoding/decoding of
  /// protobuf fields of type <c>float</c> from/to the protobuf binary wire format.
  /// </summary>
  gProtobufWireCodecFloat: IProtobufWireCodec<Single>;

type
  /// <summary>
  /// Runtime library implementation of <see cref="T:IProtobufWireCodec"/> for the protobuf type <c>float</c>.
  /// </summary>
  TProtobufFloatWireCodec = class(TProtobufFixedWidthWireCodec<Single>)
    // TProtobufFixedWidthWireCodec<Single> implementation

    public
      function GetWireType: TProtobufWireType; override;
      function DecodeValue(aSource: TStream): Single; override;
      procedure EncodeValue(aValue: Single; aDest: TStream); override;

    // TProtobufWireCodec<Single> implementation
    
    public
      function GetDefault: Single; override;
      function IsDefault(aValue: Single): Boolean; override;
  end;

implementation

uses
  // For protobuf default values
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.uProtobuf;

// TProtobufFixedWidthWireCodec<Single> implementation

function TProtobufFloatWireCodec.GetWireType: TProtobufWireType;
begin
  result := wt32Bit;
end;

function TProtobufFloatWireCodec.DecodeValue(aSource: TStream): Single;
begin
  aSource.Read(result, SizeOf(result));
end;

procedure TProtobufFloatWireCodec.EncodeValue(aValue: Single; aDest: TStream);
begin
  aDest.Write(aValue, SizeOf(aValue));
end;

// TProtobufWireCodec<Single> implementation

function TProtobufFloatWireCodec.GetDefault: Single;
begin
  result := PROTOBUF_DEFAULT_VALUE_FLOAT;
end;

function TProtobufFloatWireCodec.IsDefault(aValue: Single): Boolean;
begin
  result := aValue = GetDefault;
end;

initialization
begin
  gProtobufWireCodecFloat := TProtobufFloatWireCodec.Create;
end;

end.
