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
unit Work.Connor.Protobuf.Delphi.ProtocGenDelphi.StubRuntime.uStubRuntime;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  // To raise exceptions
  SysUtils;

/// <summary>
/// Creates an exception that signals that some functionality is not implemented in the stub runtime.
/// </summary>
function NotImplementedInStub: Exception;

implementation

function NotImplementedInStub: Exception;
begin
  result := Exception.Create('This functionality is not implemented by the stub runtime');
end;

end.
