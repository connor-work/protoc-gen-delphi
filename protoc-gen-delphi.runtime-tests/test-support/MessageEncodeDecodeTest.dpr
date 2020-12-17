/// Copyright 2020 Connor Roehricht (connor.work)
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
/// Tests whether encoding and decoding a known protobuf message correctly preserves the message.
/// <see cref="uConstructor" /> must contain a function <see cref="ConstructMessage" /> that constructs the message contents.
/// <see cref="uValidate" /> must contain a function <see cref="ValidateMessage" /> that validates the decoded message.
/// </summary>
program MessageEncodeDecodeTest;

{$INCLUDE Work.Connor.Delphi.CompilerFeatures.inc}

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  // For TStream
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.Classes,
{$ELSE}
  Classes,
{$ENDIF}
  // Message constructor
  uConstruct,
  // Message validator
  uValidate;

var
  lTempStream: TStream;
  lConstructedMessage: TConstructedMessage;
  lValidatedMessage: TValidatedMessage;
begin
  lTempStream := TMemoryStream.Create;
  lConstructedMessage := TConstructedMessage.Create;
  ConstructMessage(lConstructedMessage);
  lConstructedMessage.Encode(lTempStream);
  lConstructedMessage.Free;
  lTempStream.Seek(0, soBeginning);
  lValidatedMessage := TValidatedMessage.Create;
  lValidatedMessage.Decode(lTempStream);
  lTempStream.Free;
  ValidateMessage(lValidatedMessage);
  lValidatedMessage.Free;
end.
