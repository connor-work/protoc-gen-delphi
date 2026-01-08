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

/// <summary>
/// Runtime support for the ProtoJSON format.
/// </summary>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtoJsonFormat;

{$INCLUDE Work.Connor.Delphi.CompilerFeatures.inc}

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.Generics.Collections,
{$ELSE}
  Generics.Collections,
{$ENDIF}
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.JSON;
{$ELSE}
  JSON;
{$ENDIF}

/// <summary>
/// Collection of <see cref="TJSONValue"/> that represent field values in the same Protobuf message that is encoded using the ProtoJSON format.
/// Field values are indexed by their field name in lowerCamelCase form, and stored in a list to support the occurence of both the lowerCamelCase form and the original field name
/// as keys in the JSON object that encodes the message.
/// </summary>
type TProtoJsonEncodedFieldValuesMap = TObjectDictionary<UnicodeString, TObjectList<TJSONValue>>;

implementation

end.

