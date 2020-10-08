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
/// Helper code used only by the stub runtime, not required for functional implementations of the runtime library for <c>protoc-gen-delphi</c>.
/// </summary>
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.StubRuntime.uStub;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  // Raising of exceptions
  SysUtils;

/// <summary>
/// Called by the stub runtime to signal that some functionality is not implemented.
/// </summary>
procedure NotImplementedInStub;

implementation

procedure NotImplementedInStub;
begin
  raise Exception.Create('This functionality is not implemented by the stub runtime');
end;

end.
