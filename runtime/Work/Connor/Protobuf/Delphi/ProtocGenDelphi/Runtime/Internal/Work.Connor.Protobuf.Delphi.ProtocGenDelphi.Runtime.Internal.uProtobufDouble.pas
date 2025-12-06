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
/// Runtime-internal support for the protobuf type <c>double</c>.
/// </summary>
/// <remarks>
/// Generated code needs to reference this unit in order to operate on protobuf field values of this type.
/// </remarks>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufDouble;

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
  /// protobuf fields of type <c>double</c> from/to the protobuf binary wire format.
  /// </summary>
  gProtobufWireCodecDouble: IProtobufWireCodec<Double>;

type
  /// <summary>
  /// Runtime library implementation of <see cref="T:IProtobufWireCodec"/> for the protobuf type <c>double</c>.
  /// </summary>
  TProtobufDoubleWireCodec = class(TProtobufFixedWidthWireCodec<Double>)
    // TProtobufFixedWidthWireCodec<Double> implementation

    public
      function GetWireType: TProtobufWireType; override;
      function DecodeValue(aSource: TStream): Double; override;
      procedure EncodeValue(aValue: Double; aDest: TStream); override;

    // TProtobufWireCodec<Double> implementation
    
    public
      function GetDefault: Double; override;
      function IsDefault(aValue: Double): Boolean; override;
  end;

implementation

uses
  // For protobuf default values
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.uProtobuf;

// TProtobufFixedWidthWireCodec<Double> implementation

function TProtobufDoubleWireCodec.GetWireType: TProtobufWireType;
begin
  result := wt64Bit;
end;

function TProtobufDoubleWireCodec.DecodeValue(aSource: TStream): Double;
begin
  aSource.Read(result, SizeOf(result));
end;

procedure TProtobufDoubleWireCodec.EncodeValue(aValue: Double; aDest: TStream);
begin
  aDest.Write(aValue, SizeOf(aValue));
end;

// TProtobufWireCodec<Double> implementation

function TProtobufDoubleWireCodec.GetDefault: Double;
begin
  result := PROTOBUF_DEFAULT_VALUE_DOUBLE;
end;

function TProtobufDoubleWireCodec.IsDefault(aValue: Double): Boolean;
begin
  result := aValue = GetDefault;
end;

initialization
begin
  gProtobufWireCodecDouble := TProtobufDoubleWireCodec.Create;
end;

end.
