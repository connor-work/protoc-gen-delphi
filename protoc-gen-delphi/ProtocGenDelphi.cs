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
        /// <summary>
        /// Determines the Delphi type identifier of the Delphi type that is used to represent protobuf field values of a specific protobuf field type.
        /// </summary>
        /// <param name="fieldType">The protobuf field type</param>
        /// <returns>Corresponding Delphi type identifier</returns>
        internal static string GetDelphiType(this FieldDescriptorProto.Types.Type fieldType) => fieldType switch
        {
            FieldDescriptorProto.Types.Type.String => "UnicodeString",
            FieldDescriptorProto.Types.Type.Uint32 => "UInt32",
            _ => throw new NotImplementedException()
        };

        /// <summary>
        /// Determines the Delphi identifier of the runtime instance of <c>TProtobufWireCodec<!<![CDATA[<T>]]></c> that is used for encoding
        /// and decoding values of a specific protobuf field type in the protobuf binary wire format.
        /// </summary>
        /// <param name="fieldType">The protobuf field type</param>
        /// <returns>Delphi identifier of the codec instance</returns>
        internal static string GetDelphiWireCodec(this FieldDescriptorProto.Types.Type fieldType) => fieldType switch
        {
            FieldDescriptorProto.Types.Type.String => "gProtobufWireCodecString",
            FieldDescriptorProto.Types.Type.Uint32 => "gProtobufWireCodecUint32",
            _ => throw new NotImplementedException()
        };

        /// <summary>
        /// Determines the Delphi identifier of the true constant whose value is the default value for a specific protobuf field type.
        /// </summary>
        /// <param name="fieldType">The protobuf field type</param>
        /// <returns>Delphi identifier default value constant</returns>
        internal static string GetDelphiDefaultValueConstant(this FieldDescriptorProto.Types.Type fieldType) => fieldType switch
        {
            FieldDescriptorProto.Types.Type.String => "PROTOBUF_STRING_DEFAULT_VALUE",
            FieldDescriptorProto.Types.Type.Uint32 => "PROTOBUF_UINT32_DEFAULT_VALUE",
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
            foreach (EnumDescriptorProto @enum in protoFile.EnumType) CompileEnum(@enum, delphiUnit.Interface, dependencyHandler);
            // Compile message types
            foreach (DescriptorProto messageType in protoFile.MessageType) CompileMessage(messageType, delphiUnit.Interface, delphiUnit.Implementation, dependencyHandler);
            return delphiUnit;
        }

        /// <summary>
        /// Generates a Delphi unit identifier for a protobuf schema definition.
        /// </summary>
        /// <param name="protoFile">The .proto file defining the schema</param>
        /// <returns>The unit identifier</returns>
        private UnitIdentifier GenerateUnitIdentifier(FileDescriptorProto protoFile) => new UnitIdentifier()
        {
            // Use .proto file filename without extensions as identifier
            // TODO namespaces
            Unit = $"u{protoFile.Name.Split(protoFileNamePathSeparator)[^1].Split(".")[0].ToPascalCase()}"
        };

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
        /// Compiles a protobuf enum by injecting code into a Delphi interface section.
        /// </summary>
        /// <param name="enum">The enum</param>
        /// <param name="delphiInterface">The Delphi interface section</param>
        /// <param name="dependencyHandler"> Action to perform when a new Delphi interface dependency has been detected</param>
        private void CompileEnum(EnumDescriptorProto @enum, Interface delphiInterface, Action<UnitReference> dependencyHandler)
        {
            // Add the required runtime dependency for handling compiled enums
            dependencyHandler.Invoke(runtime.GetDependencyForEnums());
            // Generate a corresponding enumerated type
            EnumDeclaration delphiEnum = GenerateEnum(@enum);
            delphiInterface.Declarations.Add(new InterfaceDeclaration()
            {
                EnumDeclaration = delphiEnum
            });
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
            Name = $"T{@enum.Name.ToPascalCase()}",
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
        /// Compiles a protobuf message type by injecting code into a Delphi interface section and a Delphi implementation section.
        /// </summary>
        /// <param name="messageType">The message type</param>
        /// <param name="delphiInterface">The Delphi interface section</param>
        /// <param name="delphiImplementation">The Delphi implementation section</param>
        /// <param name="dependencyHandler"> Action to perform when a new Delphi interface dependency has been detected</param>
        private void CompileMessage(DescriptorProto messageType, Interface delphiInterface, Implementation delphiImplementation, Action<UnitReference> dependencyHandler)
        {
            // Add the required runtime dependency for handling compiled messages
            dependencyHandler.Invoke(runtime.GetDependencyForMessages());
            // Generate a corresponding message class
            ClassDeclaration delphiClass = GenerateClass(messageType);
            delphiInterface.Declarations.Add(new InterfaceDeclaration()
            {
                ClassDeclaration = delphiClass
            });
            MessageClassSkeleton skeleton = new MessageClassSkeleton(delphiClass.Name);
            foreach (FieldDescriptorProto field in messageType.Field) CompileField(field, delphiClass, skeleton, dependencyHandler);
            skeleton.Inject(delphiClass, delphiImplementation);
        }

        /// <summary>
        /// Generates an incomplete Delphi class declaration for a protobuf message type, ignoring fields and without the message class skeleton.
        /// </summary>
        /// <param name="messageType">The message type</param>
        /// <returns> The basic Delphi class declaration</returns>
        private ClassDeclaration GenerateClass(DescriptorProto messageType) => new ClassDeclaration()
        {
            // TODO handling of absent name?
            Name = $"T{messageType.Name.ToPascalCase()}",
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
            // Add the required runtime dependency for handling protobuf fields of this specific type
            dependencyHandler.Invoke(runtime.GetDependencyForFieldType(field.Type));
            // Delphi type exposed to client code
            string publicDelphiType = field.Type.GetDelphiType(); // TODO handling of enum/message/group?
            // Delphi type used for internal representation
            string privateDelphiType = publicDelphiType;
            // Create public constants and members for the client code
            TrueConstDeclaration delphiFieldNumberConst = GenerateAndInjectFieldNumberConst(field, delphiClass);
            string delphiPropertyName = field.Name.ToPascalCase();
            FieldDeclaration delphiField = GenerateAndInjectField(field, delphiPropertyName, privateDelphiType, delphiClass);
            GenerateAndInjectProperty(field, delphiField, delphiPropertyName, publicDelphiType, delphiClass, skeleton);
            // Fill the message skeleton with the runtime field logic
            string wireCodec = field.Type.GetDelphiWireCodec();
            (_, MethodDeclaration encodeDeclaration) = skeleton.Encode;
            (_, MethodDeclaration decodeDeclaration) = skeleton.Decode;
            (_, MethodDeclaration clearOwnFieldsDeclaration) = skeleton.ClearOwnFields;
            encodeDeclaration.Statements.Add($"EncodeField<{privateDelphiType}>({delphiField.Name}, {delphiFieldNumberConst.Identifier}, {wireCodec}, aDest);");
            decodeDeclaration.Statements.Add($"{delphiField.Name} := DecodeUnknownField<{privateDelphiType}>({delphiFieldNumberConst.Identifier}, {wireCodec});");
            clearOwnFieldsDeclaration.Statements.Add($"{delphiField.Name} := {field.Type.GetDelphiDefaultValueConstant()};");
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
        /// <param name="delphiType">Delphi type of the field</param>
        /// <param name="delphiClass">The Delphi class representing the message type</param>
        /// <param name="skeleton">The message class skeleton for the Delphi class</param>
        /// <returns>The injected field declaration</returns>
        private MethodDeclaration GenerateAndInjectGetter(FieldDescriptorProto field, FieldDeclaration delphiField, string delphiPropertyName, string delphiType, ClassDeclaration delphiClass, MessageClassSkeleton skeleton)
        {
            MethodDeclaration getter = new MethodDeclaration()
            {
                Class = delphiClass.Name,
                Prototype = new Prototype()
                {
                    Name = $"Get{delphiPropertyName}",
                    Type = Prototype.Types.Type.Function,
                    ReturnType = delphiType
                },
                Statements = { $"result := {delphiField.Name};" }
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
        /// <param name="delphiType">Delphi type of the field</param>
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
            MethodDeclaration setter = new MethodDeclaration()
            {
                Class = delphiClass.Name,
                Prototype = new Prototype()
                {
                    Name = $"Set{delphiPropertyName}",
                    Type = Prototype.Types.Type.Procedure,
                    ParameterList = { setterParameter }
                },
                Statements = { $"{delphiField.Name} := {setterParameter.Name};" }
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
        /// <param name="delphiType">Delphi type of the field</param>
        /// <param name="delphiClass">The Delphi class representing the message type</param>
        /// <param name="skeleton">The message class skeleton for the Delphi class</param>
        private void GenerateAndInjectProperty(FieldDescriptorProto field, FieldDeclaration delphiField, string delphiPropertyName, string delphiType, ClassDeclaration delphiClass, MessageClassSkeleton skeleton)
        {
            MethodDeclaration getter = GenerateAndInjectGetter(field, delphiField, delphiPropertyName, delphiType, delphiClass, skeleton);
            MethodDeclaration setter = GenerateAndInjectSetter(field, delphiField, delphiPropertyName, delphiType, delphiClass, skeleton);
            PropertyDeclaration delphiProperty = new PropertyDeclaration()
            {
                Name = delphiPropertyName,
                Type = delphiType,
                ReadSpecifier = getter.Prototype.Name,
                WriteSpecifier = setter.Prototype.Name,
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
            delphiClass.MemberList.Add(new ClassMemberDeclaration()
            {
                Visibility = Visibility.Public,
                PropertyDeclaration = delphiProperty
            });
        }
    }
}
