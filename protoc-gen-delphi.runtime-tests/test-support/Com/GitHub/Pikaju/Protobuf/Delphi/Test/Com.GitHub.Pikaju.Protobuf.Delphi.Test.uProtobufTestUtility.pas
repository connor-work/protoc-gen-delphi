unit Com.GitHub.Pikaju.Protobuf.Delphi.Test.uProtobufTestUtility;

{$INCLUDE Work.Connor.Delphi.CompilerFeatures.inc}

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  // For Exception
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.Classes,
{$ELSE}
  Classes,
{$ENDIF}
 // For TBytes
{$IFDEF WORK_CONNOR_DELPHI_COMPILER_UNIT_SCOPE_NAMES}
  System.SysUtils;
{$ELSE}
  SysUtils;
{$ENDIF}

type
  ETestFailed = class(Exception);

procedure AssertTrue(aCondition: Boolean; aMessage: String);
procedure AssertStreamEquals(aStream: TStream; aContent: TBytes; aMessage: String);
procedure AssertBytesEqual(aLeft: TBytes; aRight: TBytes; aMessage: String);

implementation

procedure AssertTrue(aCondition: Boolean; aMessage: String);
begin
  if (not aCondition) then
    raise ETestFailed.Create(aMessage);
end;

procedure AssertStreamEquals(aStream: TStream; aContent: TBytes; aMessage: String);
var
  lByte: Byte;
begin
  AssertTrue(aStream.Size = Length(aContent), aMessage);
  aStream.Seek(0, soBeginning);
  while (aStream.Position < aStream.Size) do
  begin
    aStream.ReadBuffer(lByte, 1);
    AssertTrue(aContent[aStream.Position - 1] = lByte, aMessage);
  end;
end;

procedure AssertBytesEqual(aLeft: TBytes; aRight: TBytes; aMessage: String);
var
  lIndex: Longint;
begin
  AssertTrue(Length(aLeft) = Length(aRight), aMessage);
  for lIndex := 0 to Length(aLeft) - 1 do
    AssertTrue(aLeft[lIndex] = aRight[lIndex], aMessage);
end;

end.
