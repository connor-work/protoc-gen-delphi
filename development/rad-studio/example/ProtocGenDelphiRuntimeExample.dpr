program ProtocGenDelphiRuntimeExample;

{$APPTYPE CONSOLE}

{$R *.res}

uses
  System.SysUtils,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Example.uExample in '..\..\..\runtime\Work\Connor\Protobuf\Delphi\ProtocGenDelphi\Example\Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Example.uExample.pas',
  uExampleData in '..\..\..\example\output\uExampleData.pas';

begin
  try
    RunExample;
  except
    on E: Exception do
      Writeln(E.ClassName, ': ', E.Message);
  end;
end.

