{ This file was automatically created by Lazarus. Do not edit!
  This source is only used to compile and install the package.
 }

unit ProtocGenDelphiRuntime;

{$warn 5023 off : no warning about unused units}
interface

uses
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.uProtobuf, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.
  uIProtobufMessageInternal, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.
  uIProtobufRepeatedFieldValuesInternal, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.
  uIProtobufWireCodec, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufBool, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufBytes, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.
  uProtobufDouble, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufEnum, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.
  uProtobufFixed32, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.
  uProtobufFixed64, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufFloat, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufInt32, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.uProtobufInt64, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.
  uProtobufRepeatedBool, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.
  uProtobufRepeatedBytes, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.
  uProtobufRepeatedDouble, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.
  uProtobufRepeatedFixed32, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.
  uProtobufRepeatedFixed64, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.
  uProtobufRepeatedFloat, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.
  uProtobufRepeatedInt32, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.
  uProtobufRepeatedInt64, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.
  uProtobufRepeatedMessage, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.
  uProtobufRepeatedSfixed32, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.
  uProtobufRepeatedSfixed64, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.
  uProtobufRepeatedSint32, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.
  uProtobufRepeatedSint64, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.
  uProtobufRepeatedString, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.
  uProtobufRepeatedUint32, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.
  uProtobufRepeatedUint64, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.
  uProtobufSfixed32, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.
  uProtobufSfixed64, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.
  uProtobufSint32, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.
  uProtobufSint64, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.
  uProtobufString, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.
  uProtobufUint32, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.Internal.
  uProtobufUint64, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uIProtobufMessage, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.
  uIProtobufRepeatedFieldValues, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uProtobufMessage, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.StubRuntime.uProtobufMessage, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.StubRuntime.
  uProtobufRepeatedFieldValues, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.StubRuntime.uProtobufWireCodec, 
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.StubRuntime.uStubRuntime, 
  uExampleData, uBool, uDouble, uEmptySchema, uEnumField, uEnums, uFields, 
  uFloat, uInputFileImportedTypes, uInputFileImports, uMessageField, 
  uMessages, SpaceX.SpaceY.uInputFileNamespaceFromPath, uNestedEnums, uOneof, 
  uRepeatedField, uRepeatedMessageField, uString, uUint32, uImportedA, 
  uImportedB, Package1.uImportedEnumQualified, 
  Package1.uImportedMessageQualified, uImportedEnumUnqualified, 
  uImportedMessageUnqualified, LazarusPackageIntf;

implementation

procedure Register;
begin
end;

initialization
  RegisterPackage('ProtocGenDelphiRuntime', @Register);
end.
