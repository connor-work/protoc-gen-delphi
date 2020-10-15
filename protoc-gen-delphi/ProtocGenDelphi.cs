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

using Google.Protobuf;
using Google.Protobuf.Compiler;
using Google.Protobuf.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Work.Connor.Delphi;
using Work.Connor.Delphi.CodeWriter;

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi
{
    /// <summary>
    /// Extensions to protobuf types for Delphi source code production.
    /// </summary>
    public static partial class ProtobufExtensions
    {

#pragma warning disable S4136 // Method overloads should be grouped together -> Method order is optimized for easier reading

        /// <summary>
        /// Determines the Delphi type identifier of the Delphi type that is used to represent single protobuf field values of a specific protobuf field type,
        /// when communicating with client code.
        /// </summary>
        /// <param name="fieldType">The protobuf field descriptor's type field value</param>
        /// <param name="fieldTypeName">The protobuf field descriptor's type name field value</param>
        /// <param name="generator">Function that generates a Delphi type name for a protobuf message type or enumerated type name</param>
        /// <returns>Corresponding Delphi type identifier</returns>
        internal static string GetPublicDelphiSingleValueType(this FieldDescriptorProto.Types.Type fieldType, string fieldTypeName, Func<string, string> generator) => fieldType switch
        {
            FieldDescriptorProto.Types.Type.String => "UnicodeString",
            FieldDescriptorProto.Types.Type.Float => "Single",
            FieldDescriptorProto.Types.Type.Double => "Double",
            FieldDescriptorProto.Types.Type.Uint32 => "UInt32",
            FieldDescriptorProto.Types.Type.Bool => "Boolean",
            FieldDescriptorProto.Types.Type.Enum => generator.Invoke(fieldTypeName),
            FieldDescriptorProto.Types.Type.Message => generator.Invoke(fieldTypeName),
            _ => throw new NotImplementedException()
        };

        /// <summary>
        /// Determines the Delphi type identifier of the Delphi type that is used to represent single protobuf field values of a specific protobuf field type,
        /// when communicating with internal (runtime) code.
        /// </summary>
        /// <param name="fieldType">The protobuf field descriptor's type field value</param>
        /// <param name="fieldTypeName">The protobuf field descriptor's type name field value</param>
        /// <param name="generator">Function that generates a Delphi type name for a protobuf message type or enumerated type name</param>
        /// <returns>Corresponding Delphi type identifier</returns>
        internal static string GetPrivateDelphiSingleValueType(this FieldDescriptorProto.Types.Type fieldType, string fieldTypeName, Func<string, string> generator) => fieldType switch
        {
            FieldDescriptorProto.Types.Type.String => "UnicodeString",
            FieldDescriptorProto.Types.Type.Float => "Single",
            FieldDescriptorProto.Types.Type.Double => "Double",
            FieldDescriptorProto.Types.Type.Uint32 => "UInt32",
            FieldDescriptorProto.Types.Type.Bool => "Boolean",
            FieldDescriptorProto.Types.Type.Enum => "TProtobufEnumFieldValue",
            FieldDescriptorProto.Types.Type.Message => generator.Invoke(fieldTypeName),
            _ => throw new NotImplementedException()
        };

        /// <summary>
        /// Determines the Delphi type identifier of the Delphi type that is used to represent protobuf field values within repeated fields, of a specific protobuf field type,
        /// when communicating with client code.
        /// </summary>
        /// <param name="fieldType">The protobuf field descriptor's type field value</param>
        /// <param name="fieldTypeName">The protobuf field descriptor's type name field value</param>
        /// <param name="generator">Function that generates a Delphi type name for a protobuf message type or enumerated type name</param>
        /// <returns>Corresponding Delphi type identifier</returns>
        internal static string GetPublicDelphiElementType(this FieldDescriptorProto.Types.Type fieldType, string fieldTypeName, Func<string, string> generator) => fieldType.GetPublicDelphiSingleValueType(fieldTypeName, generator);

        /// <summary>
        /// Determines the Delphi type identifier of the Delphi type that is used to represent the protobuf field contents of a specific protobuf field,
        /// when communicating with client code.
        /// </summary>
        /// <param name="field">The protobuf field</param>
        /// <param name="generator">Function that generates a Delphi type name for a protobuf message type or enumerated type name</param>
        /// <returns>Corresponding Delphi type identifier</returns>
        internal static string GetPublicDelphiType(this FieldDescriptorProto field, Func<string, string> generator) => field.Label switch
        {
            FieldDescriptorProto.Types.Label.Optional => GetPublicDelphiSingleValueType(field.Type, field.TypeName, generator),
            FieldDescriptorProto.Types.Label.Repeated => $"TProtobufRepeatedField<{GetPublicDelphiElementType(field.Type, field.TypeName, generator)}>",
            _ => throw new NotImplementedException()
        };

        /// <summary>
        /// Determines the Delphi type identifier of the Delphi type that is used to represent the protobuf field contents of a specific protobuf field,
        /// when communicating with internal (runtime) code.
        /// </summary>
        /// <param name="field">The protobuf field</param>
        /// <param name="generator">Function that generates a Delphi type name for a protobuf message type or enumerated type name</param>
        /// <returns>Corresponding Delphi type identifier</returns>
        internal static string GetPrivateDelphiType(this FieldDescriptorProto field, Func<string, string> generator) => field.Label switch
        {
            FieldDescriptorProto.Types.Label.Optional => GetPrivateDelphiSingleValueType(field.Type, field.TypeName, generator),
            FieldDescriptorProto.Types.Label.Repeated => GetDelphiRepeatedFieldSubclass(field.Type, field.TypeName, generator),
            _ => throw new NotImplementedException()
        };

#pragma warning disable S4136 // Method overloads should be grouped together

        /// <summary>
        /// Determines the Delphi identifier of the runtime instance of <c>TProtobufWireCodec<!<![CDATA[<T>]]></c> that is used for encoding
        /// and decoding values of a specific protobuf field type in the protobuf binary wire format.
        /// </summary>
        /// <param name="fieldType">The protobuf field type</param>
        /// <returns>Delphi identifier of the codec instance</returns>
        /// <remarks>This is not used for protobuf message types</remarks>
        internal static string GetDelphiWireCodec(this FieldDescriptorProto.Types.Type fieldType) => fieldType switch
        {
            FieldDescriptorProto.Types.Type.String => "gProtobufWireCodecString",
            FieldDescriptorProto.Types.Type.Float => "gProtobufWireCodecFloat",
            FieldDescriptorProto.Types.Type.Double => "gProtobufWireCodecDouble",
            FieldDescriptorProto.Types.Type.Uint32 => "gProtobufWireCodecUint32",
            FieldDescriptorProto.Types.Type.Bool => "gProtobufWireCodecBool",
            FieldDescriptorProto.Types.Type.Enum => "gProtobufWireCodecEnum",
            _ => throw new NotImplementedException()
        };

        /// <summary>
        /// Determines the Delphi identifier of the subclass of <c>TProtobufRepeatedField<!<![CDATA[<T>]]></c> that represents repeated fields of
        /// a specific protobuf field type.
        /// </summary>
        /// <param name="fieldType">The protobuf field descriptor's type field value</param>
        /// <param name="fieldTypeName">The protobuf field descriptor's type name field value</param>
        /// <param name="generator">Function that generates a Delphi type name for a protobuf message type or enumerated type name</param>
        /// <returns>Delphi identifier of class</returns>
        internal static string GetDelphiRepeatedFieldSubclass(this FieldDescriptorProto.Types.Type fieldType, string fieldTypeName, Func<string, string> generator) => fieldType switch
        {
            FieldDescriptorProto.Types.Type.String => "TProtobufRepeatedStringField",
            FieldDescriptorProto.Types.Type.Float => "TProtobufRepeatedFloatField",
            FieldDescriptorProto.Types.Type.Double => "TProtobufRepeatedDoubleField",
            FieldDescriptorProto.Types.Type.Uint32 => "TProtobufRepeatedUint32Field",
            FieldDescriptorProto.Types.Type.Bool => "TProtobufRepeatedBoolField",
            FieldDescriptorProto.Types.Type.Enum => $"TProtobufRepeatedEnumField<{generator.Invoke(fieldTypeName)}>",
            FieldDescriptorProto.Types.Type.Message => $"TProtobufRepeatedMessageField<{generator.Invoke(fieldTypeName)}>",
            _ => throw new NotImplementedException()
        };

        /// <summary>
        /// Determines the Delphi identifier of the true constant whose value is the default value for a specific protobuf field type.
        /// </summary>
        /// <param name="fieldType">The protobuf field type</param>
        /// <returns>Delphi identifier default value constant</returns>
        internal static string GetDelphiDefaultValueConstant(this FieldDescriptorProto.Types.Type fieldType) => fieldType switch
        {
            FieldDescriptorProto.Types.Type.String => "PROTOBUF_DEFAULT_VALUE_STRING",
            FieldDescriptorProto.Types.Type.Float => "PROTOBUF_DEFAULT_VALUE_FLOAT",
            FieldDescriptorProto.Types.Type.Double => "PROTOBUF_DEFAULT_VALUE_DOUBLE",
            FieldDescriptorProto.Types.Type.Uint32 => "PROTOBUF_DEFAULT_VALUE_UINT32",
            FieldDescriptorProto.Types.Type.Bool => "PROTOBUF_DEFAULT_VALUE_BOOL",
            FieldDescriptorProto.Types.Type.Enum => "PROTOBUF_DEFAULT_VALUE_ENUM",
            FieldDescriptorProto.Types.Type.Message => "PROTOBUF_DEFAULT_VALUE_MESSAGE",
            _ => throw new NotImplementedException()
        };
    }

    /// <summary>
    /// Plug-in for the protobuf compiler <c>protoc</c> that generates Delphi unit source code files for protobuf schema definitions.
    /// </summary>
    public class ProtocGenDelphi
    {
        /// <summary>
        /// Optional plug-in option whose value is the Delphi namespace identifier of a custom runtime library to use.
        /// The custom runtime library needs to follow the structure of the reference stub runtime library.
        /// </summary>
        public const string customRuntimeOption = "runtime";

        /// <summary>
        /// File name extension (without leading dot) for protobuf schema definitions
        /// </summary>
        public static readonly string protoFileExtension = "proto";

        /// <summary>
        /// File name extension (without leading dot) for generated Delphi unit source files
        /// </summary>
        public static readonly string unitSourceFileExtension = "pas";

        /// <summary>
        /// Path separator used in protobuf file names
        /// </summary>
        public static readonly string protoFileNamePathSeparator = "/";

        /// <summary>
        /// Name of the root base class of all generated Delphi classes for protobuf messages
        /// </summary>
        private static readonly string messageRootClass = "TProtobufMessage";

        /// <summary>
        /// Required unit reference for using runtime-independent support definitions for generated files (support code)
        /// </summary>
        private static readonly UnitReference supportCodeReference = new UnitReference()
        {
            Unit = new UnitIdentifier()
            {
                Unit = "uProtobuf",
                Namespace = { "Work.Connor.Protobuf.Delphi.ProtocGenDelphi".Split(".") }
            }
        };

        /// <summary>
        /// Required unit reference for using Delphi classes
        /// </summary>
        private static readonly UnitReference classesReference = new UnitReference() { Unit = new UnitIdentifier() { Unit = "Classes" } };

        /// <summary>
        /// Support definition for the targetted protobuf runtime
        /// </summary>
        private IRuntimeSupport runtime = IRuntimeSupport.Default;

        static void Main(string[] args)
        {
            if (args.Length != 0) throw new ArgumentException("protoc-gen-delphi does not expect program arguments");
            // protoc communicates with the plug-in through stdin and stdout
            using Stream input = Console.OpenStandardInput();
            using Stream output = Console.OpenStandardOutput();
            CodeGeneratorRequest request = CodeGeneratorRequest.Parser.ParseFrom(input);
            ProtocGenDelphi plugIn = new ProtocGenDelphi();
            CodeGeneratorResponse response = plugIn.HandleRequest(request);
            response.WriteTo(output);
        }

        /// <summary>
        /// Handles a request from code generation that <c>protoc</c> passed to the plug-in,
        /// by generating a response message.
        /// </summary>
        /// <param name="request">The request from <c>protoc</c></param>
        /// <returns>The response to <c>protoc</c></returns>
        public CodeGeneratorResponse HandleRequest(CodeGeneratorRequest request)
        {
            if (request.Parameter.Length != 0) ApplyOptions(request.Parameter.Split(","));
            CodeGeneratorResponse response = new CodeGeneratorResponse();
            FileDescriptorProto lookupProtoFile(string name) => request.ProtoFile.First(file => file.Name == name);
            // Generate one source code file for each .proto file
            foreach (string protoFileName in request.FileToGenerate) response.File.Add(GenerateSourceFile(lookupProtoFile(protoFileName), lookupProtoFile));
            return response;
        }

        /// <summary>
        /// Applies custom plug-in options passed through <c>protoc</c>.
        /// </summary>
        /// <param name="options">Sequence of option strings</param>
        private void ApplyOptions(IEnumerable<string> options)
        {
            foreach (string option in options)
            {
                string[] optionSegments = option.Split("=", 2);
                ApplyOption(optionSegments[0], optionSegments.Length > 0 ? optionSegments[1] : null);
            }
        }

        /// <summary>
        /// Applies a custom plug-in option passed through <c>protoc</c>.
        /// </summary>
        /// <param name="optionKey">Key of the option</param>
        /// <param name="optionValue">Optional value of the option</param>
        private void ApplyOption(string optionKey, string? optionValue)
        {
            switch (optionKey)
            {
                case customRuntimeOption:
                    if (string.IsNullOrWhiteSpace(optionValue)) throw new ArgumentException("Missing plug-in option value", optionKey);
                    runtime = new IRuntimeSupport.ReferenceRuntimeSupport(optionValue);
                    break;
                default: throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Generates a Delphi source code file for a protobuf schema definition.
        /// </summary>
        /// <param name="protoFile">The .proto file defining the schema</param>
        /// <returns>The source code file in the format expected by <c>protoc</c></returns>
        private CodeGeneratorResponse.Types.File GenerateSourceFile(FileDescriptorProto protoFile, Func<string, FileDescriptorProto> lookupProtoFile)
        {
            IEnumerable<FileDescriptorProto> dependencies()
            {
                foreach (string dependency in protoFile.Dependency) yield return lookupProtoFile.Invoke(dependency);
            }
            // Generate a new Delphi unit
            Unit unit = GenerateUnit(protoFile, dependencies());
            return new CodeGeneratorResponse.Types.File()
            {
                Name = string.Join(protoFileNamePathSeparator, unit.ToSourceFilePath()),
                Content = unit.ToSourceCode()
            };
        }

        /// <summary>
        /// Constructs a Delphi type name for a type that represents a protobuf message type or enum type.
        /// </summary>
        /// <param name="typeName">The protobuf type's name</param>
        /// <returns>The Delphi type name</returns>
        private string ConstructDelphiTypeName(string typeName)
        {
            if (!typeName.StartsWith(".")) return $"T{typeName.ToPascalCase()}";
            string[] delphiTypeNameSegments = typeName.Split(".", StringSplitOptions.RemoveEmptyEntries).Select(segment => segment.ToPascalCase()).ToArray();
            string unqualifiedName = ConstructDelphiTypeName(delphiTypeNameSegments[^1]);
            if (delphiTypeNameSegments.Length < 2) return unqualifiedName;
            return $"{ConstructUnitIdentifier(delphiTypeNameSegments[0..^2], delphiTypeNameSegments[^2]).ToSourceCode()}.{unqualifiedName}";
        }

        /// <summary>
        /// Constructs a Delphi unit identifier that corresponds to a protobuf qualified schema name.
        /// </summary>
        /// <param name="nameSpaceSegments">Segments in the protobuf namespace string</param>
        /// <param name="fileName">Unqualified schema name (should be .proto file name without extension)</param>
        /// <returns>The unit identifier</returns>
        private UnitIdentifier ConstructUnitIdentifier(string[] nameSpaceSegments, string fileName) => new UnitIdentifier()
        {
            // Use .proto file filename without extensions as identifier
            Unit = $"u{fileName.Split(".")[0].ToPascalCase()}",
            Namespace = { nameSpaceSegments.Select(segment => segment.ToPascalCase()) }
        };

        /// <summary>
        /// Generates a Delphi unit for a protobuf schema definition.
        /// </summary>
        /// <param name="protoFile">The .proto file defining the schema</param>
        /// <returns>The unit</returns>
        private Unit GenerateUnit(FileDescriptorProto protoFile, IEnumerable<FileDescriptorProto> dependencies)
        {
            Unit delphiUnit = new Unit()
            {
                Heading = GenerateUnitIdentifier(protoFile),
                Interface = GenerateInterface(),
                Implementation = GenerateImplementation(),
                Comment = new AnnotationComment()
                {
                    CommentLines =
                    {
                        // TODO transfer protobuf comment
                        $"<remarks>",
                        $"This unit corresponds to the protobuf schema definition (.proto file) <c>{protoFile.Name}</c>.",
                        $"</remarks>"
                    }
                }
            };
            void dependencyHandler(UnitReference dependency) => InjectInterfaceDependency(dependency, delphiUnit.Interface);
            // Compile protobuf dependencies (imports)
            foreach (FileDescriptorProto dependency in dependencies) CompileDependency(dependency, dependencyHandler);
            // Compile enums
            Action<EnumDeclaration> enumInjection = @enum => delphiUnit.Interface.Declarations.Add(new InterfaceDeclaration() { EnumDeclaration = @enum });
            foreach (EnumDescriptorProto @enum in protoFile.EnumType) CompileEnum(@enum, enumInjection, dependencyHandler);
            // Compile message types
            Action<ClassDeclaration> classInjection = @class => delphiUnit.Interface.Declarations.Add(new InterfaceDeclaration() { ClassDeclaration = @class });
            foreach (DescriptorProto messageType in protoFile.MessageType) CompileMessage(messageType, classInjection, delphiUnit.Implementation, dependencyHandler);
            return delphiUnit;
        }

        /// <summary>
        /// Generates a Delphi unit identifier for a protobuf schema definition.
        /// </summary>
        /// <param name="protoFile">The .proto file defining the schema</param>
        /// <returns>The unit identifier</returns>
        private UnitIdentifier GenerateUnitIdentifier(FileDescriptorProto protoFile)
        {
            string[] fileNameSegments = protoFile.Name.Split(protoFileNamePathSeparator);
            // Use namespace that matches the package, if no package is set, match the path
            string[] nameSpaceSegments = protoFile.HasPackage ? protoFile.Package.Split(".")
                                                              : fileNameSegments[0..^1];
            // Split off extension from file name
            return ConstructUnitIdentifier(nameSpaceSegments, fileNameSegments[^1].Split(".")[0].ToPascalCase());
        }

        /// <summary>
        /// Generates an incomplete Delphi interface section for a protobuf schema definition, ignoring schema types.
        /// </summary>
        /// <returns>The basic Delphi interface section</returns>
        private Interface GenerateInterface() => new Interface();

        /// <summary>
        /// Generates an incomplete Delphi implementation section for a protobuf schema definition, ignoring schema types.
        /// </summary>
        /// <returns>The basic Delphi implementation section</returns>
        private Implementation GenerateImplementation() => new Implementation();

        /// <summary>
        /// Injects a Delphi interface dependency into a Delphi interface section.
        /// </summary>
        /// <param name="delphiUnitReference">Unit reference for the dependency</param>
        /// <param name="delphiInterface">The Delphi interface section</param>
        private void InjectInterfaceDependency(UnitReference delphiUnitReference, Interface delphiInterface)
        {
            if (delphiInterface.UsesClause.Any(existingReference => existingReference.Unit.Equals(delphiUnitReference.Unit))) return;
            delphiInterface.UsesClause.Add(delphiUnitReference);
        }

        /// <summary>
        /// Compiles a protobuf dependency by creating Delphi interface dependencies.
        /// </summary>
        /// <param name="dependency">The .proto file of the protobuf dependency</param>
        /// <param name="dependencyHandler"> Action to perform when a new Delphi interface dependency has been detected</param>
        private void CompileDependency(FileDescriptorProto dependency, Action<UnitReference> dependencyHandler) => dependencyHandler.Invoke(new UnitReference()
        {
            Unit = GenerateUnitIdentifier(dependency)
        });

        /// <summary>
        /// Compiles a protobuf enum by injecting code into a Delphi interface section or a surrounding class.
        /// </summary>
        /// <param name="enum">The enum</param>
        /// <param name="interfaceInjection">Action that injects the interface part of the enum into an interface section or surrounding class</param>
        /// <param name="dependencyHandler"> Action to perform when a new Delphi interface dependency has been detected</param>
        private void CompileEnum(EnumDescriptorProto @enum, Action<EnumDeclaration> interfaceInjection, Action<UnitReference> dependencyHandler)
        {
            // Add the required runtime dependency for handling compiled enums
            dependencyHandler.Invoke(runtime.GetDependencyForEnums());
            // Generate a corresponding enumerated type
            EnumDeclaration delphiEnum = GenerateEnum(@enum);
            interfaceInjection.Invoke(delphiEnum);
            foreach (EnumValueDescriptorProto value in @enum.Value) CompileEnumValue(value, @enum.Name.ToPascalCase(), delphiEnum);
        }

        /// <summary>
        /// Generates an incomplete Delphi enumerated type declaration for a protobuf enum, ignoring enum constants.
        /// </summary>
        /// <param name="enum">The enum</param>
        /// <returns> The basic Delphi enumerated type declaration</returns>
        private EnumDeclaration GenerateEnum(EnumDescriptorProto @enum) => new EnumDeclaration()
        {
            // TODO handling of absent name?
            Name = ConstructDelphiTypeName(@enum.Name),
            Comment = new AnnotationComment()
            {
                CommentLines =
                {
                    // TODO transfer protobuf comment
                    $"<remarks>",
                    $"This enumerated type corresponds to the protobuf enum <c>{@enum.Name}</c>.",
                    $"</remarks>"
                }
            }
        };

        /// <summary>
        /// Compiles a protobuf enum constant by injecting code into a Delphi enumerated type declaration for the enumerated type representing the containing enum.
        /// </summary>
        /// <param name="value">The protobuf enum constant</param>
        /// <param name="prefix">Optional string that is prepended to the name of the generated Delphi enumerated value</param>
        /// <param name="delphiEnum">The Delphi enumerated type representing the enum</param>
        private void CompileEnumValue(EnumValueDescriptorProto value, string? prefix, EnumDeclaration delphiEnum)
        {
            delphiEnum.Values.Add(new EnumValueDeclaration()
            {
                Name = $"{prefix ?? ""}{value.Name.ToPascalCase()}",
                Ordinality = value.Number,
                Comment = new AnnotationComment()
                {
                    CommentLines =
                    {
                        // TODO transfer protobuf comment
                        $"<remarks>",
                        $"This enumerated value corresponds to the protobuf enum constant <c>{value.Name}</c>.",
                        $"</remarks>"
                    }
                }
            });
        }

        /// <summary>
        /// Compiles a protobuf message type by injecting code into a Delphi interface section or surrounding class and a Delphi implementation section.
        /// </summary>
        /// <param name="messageType">The message type</param>
        /// <param name="interfaceInjection">Action that injects the interface part of the class into an interface section or surrounding class</param>
        /// <param name="delphiImplementation">The Delphi implementation section</param>
        /// <param name="dependencyHandler"> Action to perform when a new Delphi interface dependency has been detected</param>
        private void CompileMessage(DescriptorProto messageType, Action<ClassDeclaration> interfaceInjection, Implementation delphiImplementation, Action<UnitReference> dependencyHandler)
        {
            // Add the required dependencies for handling compiled messages
            dependencyHandler.Invoke(runtime.GetDependencyForMessages());
            dependencyHandler.Invoke(classesReference);
            // Generate a corresponding message class
            ClassDeclaration delphiClass = GenerateClass(messageType);
            interfaceInjection.Invoke(delphiClass);
            MessageClassSkeleton skeleton = new MessageClassSkeleton(delphiClass.Name);
            foreach (FieldDescriptorProto field in messageType.Field) CompileField(field, delphiClass, skeleton, dependencyHandler);
            skeleton.Inject(delphiClass, delphiImplementation);
            Action<ClassDeclaration> nestedClassInjection = nestedClass => delphiClass.NestedTypeDeclarations.Add(new NestedTypeDeclaration() { ClassDeclaration = nestedClass });
            foreach (DescriptorProto nestedMessageType in messageType.NestedType) CompileMessage(nestedMessageType, nestedClassInjection, delphiImplementation, dependencyHandler);            
            Action<EnumDeclaration> nestedEnumInjection = nestedEnum => delphiClass.NestedTypeDeclarations.Add(new NestedTypeDeclaration() { EnumDeclaration = nestedEnum });
            foreach (EnumDescriptorProto nestedEnumType in messageType.EnumType) CompileEnum(nestedEnumType, nestedEnumInjection, dependencyHandler);
        }

        /// <summary>
        /// Generates an incomplete Delphi class declaration for a protobuf message type, ignoring fields and without the message class skeleton.
        /// </summary>
        /// <param name="messageType">The message type</param>
        /// <returns> The basic Delphi class declaration</returns>
        private ClassDeclaration GenerateClass(DescriptorProto messageType) => new ClassDeclaration()
        {
            // TODO handling of absent name?
            Name = ConstructDelphiTypeName(messageType.Name),
            Ancestor = messageRootClass,
            Comment = new AnnotationComment()
            {
                CommentLines =
                {
                    // TODO transfer protobuf comment
                    $"<remarks>",
                    $"This class corresponds to the protobuf message type <c>{messageType.Name}</c>.",
                    $"</remarks>"
                }
            }
        };

        /// <summary>
        /// Compiles a protobuf field by injecting code into a Delphi class declaration for the message class representing the containing message type,
        /// and into its message class skeleton (containing the "message skeleton methods").
        /// </summary>
        /// <param name="field">The protobuf field</param>
        /// <param name="delphiClass">The Delphi class representing the message type</param>
        /// <param name="skeleton">The message class skeleton for the Delphi class</param>
        /// <param name="dependencyHandler"> Action to perform when a new Delphi interface dependency has been detected</param>
        private void CompileField(FieldDescriptorProto field, ClassDeclaration delphiClass, MessageClassSkeleton skeleton, Action<UnitReference> dependencyHandler)
        {
            // Add the required runtime dependencies for handling protobuf fields of this specific type
            if (field.Label == FieldDescriptorProto.Types.Label.Repeated)
            {
                foreach (UnitReference dependency in runtime.GetDependenciesForRepeatedFieldType(field.Type)) dependencyHandler.Invoke(dependency);
            }
            else dependencyHandler.Invoke(runtime.GetDependencyForSingularFieldType(field.Type));
            // Delphi type exposed to client code
            string publicDelphiType = field.GetPublicDelphiType(name => ConstructDelphiTypeName(name));
            // Delphi type used for internal representation
            string privateDelphiType = field.GetPrivateDelphiType(name => ConstructDelphiTypeName(name));
            // Create public constants and members for the client code
            TrueConstDeclaration delphiFieldNumberConst = GenerateAndInjectFieldNumberConst(field, delphiClass);
            string delphiPropertyName = field.Name.ToPascalCase();
            FieldDeclaration delphiField = GenerateAndInjectField(field, delphiPropertyName, privateDelphiType, delphiClass);
            GenerateAndInjectProperty(field, delphiField, delphiPropertyName, publicDelphiType, delphiClass, skeleton);
            // Fill the message skeleton with the runtime field logic
            (_, MethodDeclaration createDeclaration) = skeleton.Create;
            (_, MethodDeclaration destroyDeclaration) = skeleton.Destroy;
            (_, MethodDeclaration encodeDeclaration) = skeleton.Encode;
            (_, MethodDeclaration decodeDeclaration) = skeleton.Decode;
            (_, MethodDeclaration clearOwnFieldsDeclaration) = skeleton.ClearOwnFields;
            // TODO handling of absent type (unknown if message or enum)
            if (field.Type == FieldDescriptorProto.Types.Type.Message)
            {
                destroyDeclaration.Statements.Insert(destroyDeclaration.Statements.Count - 1, $"{delphiField.Name}.Free;");
                encodeDeclaration.Statements.Add($"EncodeMessageField<{privateDelphiType}>({delphiField.Name}, {delphiFieldNumberConst.Identifier}, aDest);");
                decodeDeclaration.Statements.Add($"{delphiField.Name}.Free;");
                decodeDeclaration.Statements.Add($"{delphiField.Name} := DecodeUnknownMessageField<{privateDelphiType}>({delphiFieldNumberConst.Identifier});");
                clearOwnFieldsDeclaration.Statements.Add($"{delphiField.Name}.Free;");
            }
            else
            {
                string wireCodec = field.Type.GetDelphiWireCodec();
                if (field.Label == FieldDescriptorProto.Types.Label.Repeated)
                {
                    string delphiElementType = field.Type.GetPublicDelphiElementType(field.TypeName, name => ConstructDelphiTypeName(name));
                    createDeclaration.Statements.Insert(createDeclaration.Statements.Count - 1, $"{delphiField.Name} := {privateDelphiType}.Create;");
                    destroyDeclaration.Statements.Insert(destroyDeclaration.Statements.Count - 1, $"{delphiField.Name}.Free;");
                    encodeDeclaration.Statements.Add($"EncodeRepeatedField<{delphiElementType}>({delphiField.Name}, {delphiFieldNumberConst.Identifier}, {wireCodec}, aDest);");
                    decodeDeclaration.Statements.Add($"{delphiField.Name}.Clear;");
                    decodeDeclaration.Statements.Add($"DecodeUnknownRepeatedField<{delphiElementType}>({delphiFieldNumberConst.Identifier}, {wireCodec}, {delphiField.Name});");
                }
                else
                {
                    encodeDeclaration.Statements.Add($"EncodeField<{privateDelphiType}>({delphiField.Name}, {delphiFieldNumberConst.Identifier}, {wireCodec}, aDest);");
                    decodeDeclaration.Statements.Add($"{delphiField.Name} := DecodeUnknownField<{privateDelphiType}>({delphiFieldNumberConst.Identifier}, {wireCodec});");
                }
            }
            if (field.Label == FieldDescriptorProto.Types.Label.Repeated) clearOwnFieldsDeclaration.Statements.Add($"{delphiField.Name}.Clear;");
            else
            {
                dependencyHandler.Invoke(supportCodeReference); // Required for default value constant
                clearOwnFieldsDeclaration.Statements.Add($"{delphiField.Name} := {field.Type.GetDelphiDefaultValueConstant()};");
            }
        }

        /// <summary>
        /// Generates a Delphi true constant to hold a protobuf field number, and injects it into the Delphi class declaration for the message class representing the containing message type.
        /// </summary>
        /// <param name="field">The protobuf field</param>
        /// <param name="delphiClass">The Delphi class representing the message type</param>
        /// <returns>The injected constant declaration</returns>
        private TrueConstDeclaration GenerateAndInjectFieldNumberConst(FieldDescriptorProto field, ClassDeclaration delphiClass)
        {
            TrueConstDeclaration delphiFieldNumberConst = new TrueConstDeclaration()
            {
                Identifier = $"PROTOBUF_FIELD_NUMBER_{field.Name.ToScreamingSnakeCase()}",
                Value = field.Number.ToString(),
                Comment = new AnnotationComment()
                {
                    CommentLines =
                    {
                        $"<summary>",
                        $"Protobuf field number of the protobuf field <c>{field.Name}</c>.",
                        $"</summary>"
                    }
                }
            };
            delphiClass.NestedConstDeclarations.Add(new ConstDeclaration() { TrueConstDeclaration = delphiFieldNumberConst });
            return delphiFieldNumberConst;
        }

        /// <summary>
        /// Generates a Delphi field to hold the internal representation of a protobuf field value,
        /// and injects it into the Delphi class declaration for the message class representing the containing message type.
        /// </summary>
        /// <param name="field">The protobuf field</param>
        /// <param name="delphiPropertyName">Name of the Delphi property providing client access to the field</param>
        /// <param name="delphiType">Delphi type of the field</param>
        /// <param name="delphiClass">The Delphi class representing the message type</param>
        /// <returns>The injected field declaration</returns>
        private FieldDeclaration GenerateAndInjectField(FieldDescriptorProto field, string delphiPropertyName, string delphiType, ClassDeclaration delphiClass)
        {
            FieldDeclaration delphiField = new FieldDeclaration()
            {
                Name = $"F{delphiPropertyName}",
                Type = delphiType,
                Comment = new AnnotationComment()
                {
                    CommentLines =
                    {
                        $"<summary>",
                        $"Holds the decoded value of the protobuf field <c>{field.Name}</c>.",
                        $"</summary>"
                    }
                }
            };
            delphiClass.MemberList.Add(new ClassMemberDeclaration()
            {
                Visibility = Visibility.Private,
                FieldDeclaration = delphiField
            });
            return delphiField;
        }

        /// <summary>
        /// Generates a Delphi getter that backs a property representing a protobuf field,
        /// and injects it into the Delphi class declaration for the message class representing the containing message type,
        /// and into its message class skeleton (containing the "message skeleton methods").
        /// </summary>
        /// <param name="field">The protobuf field</param>
        /// <param name="delphiField">The Delphi field holding the internal representation of the protobuf field value</param>
        /// <param name="delphiPropertyName">Name of the Delphi property providing client access to the field</param>
        /// <param name="delphiType">Delphi type of the property</param>
        /// <param name="delphiClass">The Delphi class representing the message type</param>
        /// <param name="skeleton">The message class skeleton for the Delphi class</param>
        /// <returns>The injected field declaration</returns>
        private MethodDeclaration GenerateAndInjectGetter(FieldDescriptorProto field, FieldDeclaration delphiField, string delphiPropertyName, string delphiType, ClassDeclaration delphiClass, MessageClassSkeleton skeleton)
        {
            // TODO handling of absent type (unknown if message or enum)
            string valueExpression = field.Type == FieldDescriptorProto.Types.Type.Enum ? $"{ConstructDelphiTypeName(field.TypeName)}({delphiField.Name})"
                                                                                        : delphiField.Name;
            MethodDeclaration getter = new MethodDeclaration()
            {
                Class = delphiClass.Name,
                Prototype = new Prototype()
                {
                    Name = $"Get{delphiPropertyName}",
                    Type = Prototype.Types.Type.Function,
                    ReturnType = delphiType
                },
                Statements = { $"result := {valueExpression};" }
            };
            skeleton.PropertyAccessors.Add(getter);
            delphiClass.MemberList.Add(new ClassMemberDeclaration()
            {
                Visibility = Visibility.Protected,
                MethodDeclaration = new MethodInterfaceDeclaration()
                {
                    Prototype = getter.Prototype.Clone(),
                    Binding = MethodInterfaceDeclaration.Types.Binding.Virtual,
                    Comment = new AnnotationComment()
                    {
                        CommentLines =
                        {
                            $"<summary>",
                            $"Getter for <see cref=\"{delphiPropertyName}\"/>.",
                            $"</summary>",
                            $"<returns>The value of the protobuf field <c>{field.Name}</c></returns>",
                            $"<remarks>",
                            $"May be overridden. Overriders shall only add side-effects and must call the ancestor implementation.",
                            $"</remarks>"
                        },
                    }
                }
            });
            return getter;
        }

        /// <summary>
        /// Generates a Delphi setter that backs a property representing a protobuf field,
        /// and injects it into the Delphi class declaration for the message class representing the containing message type,
        /// and into its message class skeleton (containing the "message skeleton methods").
        /// </summary>
        /// <param name="field">The protobuf field</param>
        /// <param name="delphiField">The Delphi field holding the internal representation of the protobuf field value</param>
        /// <param name="delphiPropertyName">Name of the Delphi property providing client access to the field</param>
        /// <param name="delphiType">Delphi type of the property</param>
        /// <param name="delphiClass">The Delphi class representing the message type</param>
        /// <param name="skeleton">The message class skeleton for the Delphi class</param>
        /// <returns>The injected field declaration</returns>
        private MethodDeclaration GenerateAndInjectSetter(FieldDescriptorProto field, FieldDeclaration delphiField, string delphiPropertyName, string delphiType, ClassDeclaration delphiClass, MessageClassSkeleton skeleton)
        {
            Parameter setterParameter = new Parameter()
            {
                Name = "aValue",
                Type = delphiType
            };
            // TODO handling of absent type (unknown if message or enum)
            List<string> statements = new List<string>();
            if (field.Type == FieldDescriptorProto.Types.Type.Message) statements.Add($"{delphiField.Name}.Free;");
            string valueExpression = field.Type == FieldDescriptorProto.Types.Type.Enum ? $"Ord({setterParameter.Name})"
                                                                                        : setterParameter.Name;
            statements.Add($"{delphiField.Name} := {valueExpression};");
            MethodDeclaration setter = new MethodDeclaration()
            {
                Class = delphiClass.Name,
                Prototype = new Prototype()
                {
                    Name = $"Set{delphiPropertyName}",
                    Type = Prototype.Types.Type.Procedure,
                    ParameterList = { setterParameter }
                },
                Statements = { statements }
            };
            skeleton.PropertyAccessors.Add(setter);
            delphiClass.MemberList.Add(new ClassMemberDeclaration()
            {
                Visibility = Visibility.Protected,
                MethodDeclaration = new MethodInterfaceDeclaration()
                {
                    Prototype = setter.Prototype.Clone(),
                    Binding = MethodInterfaceDeclaration.Types.Binding.Virtual,
                    Comment = new AnnotationComment()
                    {
                        CommentLines =
                        {
                            $"<summary>",
                            $"Setter for <see cref=\"{delphiPropertyName}\"/>.",
                            $"</summary>",
                            $"<param name=\"{setterParameter.Name}\">The new value of the protobuf field <c>{field.Name}</c></param>",
                            $"<remarks>",
                            $"May be overridden. Overriders shall only add side-effects and must call the ancestor implementation.",
                            $"</remarks>"
                        },
                    }
                }
            });
            return setter;
        }

        /// <summary>
        /// Generates a Delphi property representing a protobuf field,
        /// and injects it (including getter and setter code) into the Delphi class declaration for the message class representing the containing message type,
        /// and into its message class skeleton (containing the "message skeleton methods").
        /// </summary>
        /// <param name="field">The protobuf field</param>
        /// <param name="delphiField">The Delphi field holding the internal representation of the protobuf field value</param>
        /// <param name="delphiPropertyName">Name of the Delphi property</param>
        /// <param name="delphiType">Delphi type of the property</param>
        /// <param name="delphiClass">The Delphi class representing the message type</param>
        /// <param name="skeleton">The message class skeleton for the Delphi class</param>
        private void GenerateAndInjectProperty(FieldDescriptorProto field, FieldDeclaration delphiField, string delphiPropertyName, string delphiType, ClassDeclaration delphiClass, MessageClassSkeleton skeleton)
        {
            MethodDeclaration getter = GenerateAndInjectGetter(field, delphiField, delphiPropertyName, delphiType, delphiClass, skeleton);
            PropertyDeclaration delphiProperty = new PropertyDeclaration()
            {
                Name = delphiPropertyName,
                Type = delphiType,
                ReadSpecifier = getter.Prototype.Name,
                Comment = new AnnotationComment()
                {
                    CommentLines =
                    {
                        // TODO transfer protobuf comment
                        $"<remarks>",
                        $"This property corresponds to the protobuf field <c>{field.Name}</c>.",
                        $"</remarks>"
                    }
                }
            };
            // Do not generate a setter for repeated fields, the client code shall mutate the existing object
            if (field.Label != FieldDescriptorProto.Types.Label.Repeated)
            {
                MethodDeclaration setter = GenerateAndInjectSetter(field, delphiField, delphiPropertyName, delphiType, delphiClass, skeleton);
                delphiProperty.WriteSpecifier = setter.Prototype.Name;
            }
            delphiClass.MemberList.Add(new ClassMemberDeclaration()
            {
                Visibility = Visibility.Public,
                PropertyDeclaration = delphiProperty
            });
        }
    }
}
