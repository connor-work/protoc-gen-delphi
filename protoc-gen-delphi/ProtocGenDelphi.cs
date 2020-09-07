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
            FieldDescriptorProto.Types.Type.Uint32 => "gProtobufWireCodecUInt32",
            _ => throw new NotImplementedException()
        };

        /// <summary>
        /// Determines the Delphi identifier of the true constant whose value is the default value for a specific protobuf field type.
        /// </summary>
        /// <param name="fieldType">The protobuf field type</param>
        /// <returns>Delphi identifier default value constant</returns>
        internal static string GetDelphiDefaultValueConstant(this FieldDescriptorProto.Types.Type fieldType) => fieldType switch
        {
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
                Implementation = GenerateImplementation()
            };
            void dependencyHandler(UnitReference dependency) => InjectInterfaceDependency(dependency, delphiUnit.Interface);
            // Compile protobuf dependencies (imports)
            foreach (FileDescriptorProto dependency in dependencies) CompileDependency(dependency, dependencyHandler);
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
            InjectMessageClassSkeleton(skeleton, delphiClass, delphiImplementation);
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
            Ancestor = messageRootClass
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
            string delphiType = field.Type.GetDelphiType(); // TODO handling of enum/message/group?
            // Create a Delphi constant for the protobuf field number
            TrueConstDeclaration delphiFieldNumberConst = new TrueConstDeclaration()
            {
                Identifier = $"PROTOBUF_FIELD_NUMBER_{field.Name.ToScreamingSnakeCase()}",
                Value = field.Number.ToString()
            };
            delphiClass.NestedConstDeclarations.Add(new ConstDeclaration()
            {
                TrueConstDeclaration = delphiFieldNumberConst
            });
            // Create a Delphi field to hold the decoded value
            FieldDeclaration delphiField = new FieldDeclaration()
            {
                Name = $"F{field.Name.ToPascalCase()}",
                Type = delphiType
            };
            delphiClass.MemberList.Add(new ClassMemberDeclaration()
            {
                Visibility = Visibility.Private,
                FieldDeclaration = delphiField
            });
            // Create a Delphi property for access by the client
            PropertyDeclaration delphiProperty = new PropertyDeclaration()
            {
                Name = field.Name.ToPascalCase(),
                Type = delphiType,
                ReadSpecifier = delphiField.Name,
                WriteSpecifier = delphiField.Name
            };
            delphiClass.MemberList.Add(new ClassMemberDeclaration()
            {
                Visibility = Visibility.Public,
                PropertyDeclaration = delphiProperty
            });
            // Fill the message skeleton with the runtime field logic
            string wireCodec = field.Type.GetDelphiWireCodec();
            (_, MethodDeclaration encodeDeclaration) = skeleton.Encode;
            (_, MethodDeclaration decodeDeclaration) = skeleton.Decode;
            (_, MethodDeclaration clearOwnFieldsDeclaration) = skeleton.ClearOwnFields;
            encodeDeclaration.Statements.Add($"EncodeField<{delphiType}>({delphiField.Name}, {delphiFieldNumberConst.Identifier}, {wireCodec}, aDest);");
            decodeDeclaration.Statements.Add($"{delphiField.Name} := DecodeUnknownField<{delphiType}>({delphiFieldNumberConst.Identifier}, {wireCodec});");
            clearOwnFieldsDeclaration.Statements.Add($"{delphiField.Name} := {field.Type.GetDelphiDefaultValueConstant()};");
        }

        /// <summary>
        /// Injects the internal base structure of a Delphi class for a protobuf message type ("message class skeleton") into
        /// a Delphi class and a Delphi implementation section.
        /// </summary>
        /// <param name="skeleton">The message class skeleton to inject</param>
        /// <param name="delphiClass">The Delphi class</param>
        /// <param name="delphiImplementation">The Delphi implementation section</param>
        private void InjectMessageClassSkeleton(MessageClassSkeleton skeleton, ClassDeclaration delphiClass, Implementation delphiImplementation)
        {
            foreach ((ClassMemberDeclaration methodInterface, MethodDeclaration methodImplementation) in skeleton.Methods)
            {
                delphiClass.MemberList.Add(methodInterface.Clone());
                delphiImplementation.Declarations.Add(new ImplementationDeclaration()
                {
                    MethodDeclaration = methodImplementation.Clone()
                });
            }
        }

        /// <summary>
        /// Internal base structure of a Delphi class for a protobuf message type.
        /// Can be used to inject code into so-called "skeleton methods" that are present in each such class.
        /// </summary>
        private class MessageClassSkeleton
        {
            // SYNC message skeleton structure with example code in "Messages" section of the "Delphi Generated Code" guide

            /// <summary>
            /// Parameter declaration for <see cref="Encode"/>.
            /// </summary>
            private static readonly Parameter encodeDestinationParameter = new Parameter()
            {
                // SYNC with parameter declaration in stub runtime TProtobufMessage.Encode
                Name = "aDest",
                Type = "TStream"
            };

            /// <summary>
            /// Parameter declaration for <see cref="Decode"/>.
            /// </summary>
            private static readonly Parameter decodeSourceParameter = new Parameter()
            {
                // SYNC with parameter declaration in stub runtime TProtobufMessage.Decode
                Name = "aSource",
                Type = "TStream"
            };

            /// <summary>
            /// Constructs a message class skeleton for injection into a Delphi class representing a protobuf message.
            /// </summary>
            /// <param name="delphiClassName">Name of the Delphi class</param>
            public MessageClassSkeleton(string delphiClassName)
            {
                // Constructs pair of class member declaration and defining declaration for a skeleton method
#pragma warning disable S1172 // Unused method parameters should be removed -> False-positive, the parameters are used
                static (ClassMemberDeclaration, MethodDeclaration) declareMethod(Visibility visibility, MethodInterfaceDeclaration.Types.Binding binding, MethodDeclaration definingDeclaration, AnnotationComment comment) => (
#pragma warning restore S1172 // Unused method parameters should be removed
                    new ClassMemberDeclaration()
                    {
                        Visibility = visibility,
                        MethodDeclaration = new MethodInterfaceDeclaration
                        {
                            Binding = binding,
                            Prototype = definingDeclaration.Prototype.Clone(),
                            Comment = comment.Clone()
                        }
                    }, definingDeclaration);
                Create = declareMethod(Visibility.Public, MethodInterfaceDeclaration.Types.Binding.Override, new MethodDeclaration()
                {
                    Class = delphiClassName,
                    Prototype = new Prototype()
                    {
                        Name = "Create",
                        Type = Prototype.Types.Type.Constructor
                    },
                    Statements =
                    {
                        "inherited;", "ClearOwnFields;"
                    }
                }, new AnnotationComment()
                {
                    CommentLines =
                    {
                        // SYNC partially with stub runtime annotation comment for TProtobufMessage.Create
                        // SYNC partially with "Create" comment in "Messages" section of the "Delphi Generated Code" guide
                        $"<summary>",
                        $"Creates an empty <see cref=\"{delphiClassName}\"/> that can be used as a protobuf message.",
                        $"Initially, all protobuf fields are absent, meaning that they are set to their default values.",
                        $"</summary>",
                        $"<remarks>",
                        $"Protobuf's interpretation of the absence of a field may be counterintuitive for Delphi developers.",
                        $"For a detailed explanation, see https://developers.google.com/protocol-buffers/docs/proto3#default.",
                        $"</remarks>"
                    }
                });
                Destroy = declareMethod(Visibility.Public, MethodInterfaceDeclaration.Types.Binding.Override, new MethodDeclaration()
                {
                    Class = delphiClassName,
                    Prototype = new Prototype()
                    {
                        Name = "Destroy",
                        Type = Prototype.Types.Type.Destructor
                    },
                    Statements =
                    {
                        "inherited;"
                    }
                }, new AnnotationComment()
                {
                    CommentLines =
                    {
                        // SYNC partially with stub runtime annotation comment for TProtobufMessage.Destroy
                        // SYNC partially with "Destroy" comment in "Messages" section of the "Delphi Generated Code" guide
                        $"<summary>",
                        $"Destroys the instances and all objects and resources held by it, including the protobuf field values.",
                        $"</summary>",
                        $"<remarks>",
                        $"Developers must ensure that no shared ownership of current field values or further nested embedded objects is held.",
                        $"</remarks>"
                    }
                });
                Clear = declareMethod(Visibility.Public, MethodInterfaceDeclaration.Types.Binding.Override, new MethodDeclaration()
                {
                    Class = delphiClassName,
                    Prototype = new Prototype()
                    {
                        Name = "Clear",
                        Type = Prototype.Types.Type.Procedure
                    },
                    Statements =
                    {
                        "inherited;", "ClearOwnFields;"
                    }
                }, new AnnotationComment()
                {
                    CommentLines =
                    {
                        // SYNC partially with stub runtime annotation comment for TProtobufMessage.Clear
                        // SYNC partially with "Clear" comment in "Messages" section of the "Delphi Generated Code" guide
                        $"<summary>",
                        $"Renders all protobuf fields absent by setting them to their default values.",
                        $"</summary>",
                        $"<remarks>",
                        $"The resulting instance state is equivalent to a newly constructed <see cref=\"{delphiClassName}\"/>.",
                        $"For more details, see the documentation of <see cref=\"{Create.Item2.Prototype.Name}\"/>.",
                        $"This procedure may cause the destruction of transitively owned objects.",
                        $"Developers must ensure that no shared ownership of current field values or further nested embedded objects is held.",
                        $"</remarks>"
                    }
                });
                Encode = declareMethod(Visibility.Public, MethodInterfaceDeclaration.Types.Binding.Override, new MethodDeclaration()
                {
                    Class = delphiClassName,
                    Prototype = new Prototype()
                    {
                        Name = "Encode",
                        Type = Prototype.Types.Type.Procedure,
                        ParameterList = { encodeDestinationParameter.Clone() }
                    },
                    Statements =
                    {
                        "inherited;"
                    }
                }, new AnnotationComment()
                {
                    CommentLines =
                    {
                        // SYNC partially with stub runtime annotation comment for TProtobufMessage.Encode
                        // SYNC partially with "Encode" comment in "Messages" section of the "Delphi Generated Code" guide
                        $"<summary>",
                        $"Encodes the message using the protobuf binary wire format and writes it to a stream.",
                        $"</summary>",
                        $"<param name=\"{encodeDestinationParameter.Name}\">The stream that the encoded message is written to</param>"
                    }
                });
                Decode = declareMethod(Visibility.Public, MethodInterfaceDeclaration.Types.Binding.Override, new MethodDeclaration()
                {
                    Class = delphiClassName,
                    Prototype = new Prototype()
                    {
                        Name = "Decode",
                        Type = Prototype.Types.Type.Procedure,
                        ParameterList = { decodeSourceParameter.Clone() }
                    },
                    Statements =
                    {
                        "inherited;"
                    }
                }, new AnnotationComment()
                {
                    CommentLines =
                    {
                        // SYNC partially with stub runtime annotation comment for TProtobufMessage.Decode
                        // SYNC partially with "Decode" comment in "Messages" section of the "Delphi Generated Code" guide
                        $"<summary>",
                        $"Fills the message's protobuf fields by decoding the message using the protobuf binary wire format from data that is read from a stream.",
                        $"</summary>",
                        $"<param name=\"{decodeSourceParameter.Name}\">The stream that the data is read from</param>",
                        $"<remarks>",
                        $"Protobuf fields that are not present in the read data are rendered absent by setting them to their default values.",
                        $"This may cause the destruction of transitively owned objects (this is also the case when a present fields overwrites a previous value)",
                        $"Developers must ensure that no shared ownership of current field values or further nested embedded objects is held.",
                        $"</remarks>"
                    }
                });
                ClearOwnFields = declareMethod(Visibility.Private, MethodInterfaceDeclaration.Types.Binding.Static, new MethodDeclaration()
                {
                    Class = delphiClassName,
                    Prototype = new Prototype()
                    {
                        Name = "ClearOwnFields",
                        Type = Prototype.Types.Type.Procedure
                    }
                }, new AnnotationComment()
                {
                    CommentLines =
                    {
                        $"<summary>",
                        $"Renders those protobuf fields absent that belong to <see cref=\"{delphiClassName}\"/> (i.e., are not managed by an ancestor class), by setting them to their default values.",
                        $"</summary>"
                    }
                });
            }

            /// <summary>
            /// Constructor that constructs an empty message with all protobuf fields absent
            /// </summary>
            public (ClassMemberDeclaration, MethodDeclaration) Create { get; }

            /// <summary>
            /// Destructor that destroys the message and all objects and resources held by it
            /// </summary>
            public (ClassMemberDeclaration, MethodDeclaration) Destroy { get; }

            /// <summary>
            /// Procedure that renders all protobuf fields absent by setting them to their default values
            /// </summary>
            public (ClassMemberDeclaration, MethodDeclaration) Clear { get; }

            /// <summary>
            /// Procedure that encodes the message using the protobuf binary wire format and writes it to a stream
            /// </summary>
            public (ClassMemberDeclaration, MethodDeclaration) Encode { get; }

            /// <summary>
            /// Procedure that fills the message's protobuf fields by decoding the message using the protobuf binary wire format from data that is read from a stream
            /// </summary>
            public (ClassMemberDeclaration, MethodDeclaration) Decode { get; }

            /// <summary>
            /// Procedure that renders those protobuf fields absent that belong to this specific message class sub-type, by setting them to their default values
            /// </summary>
            public (ClassMemberDeclaration, MethodDeclaration) ClearOwnFields { get; }

            /// <summary>
            /// Message "skeleton methods" in intended declaration order
            /// </summary>
            public IEnumerable<(ClassMemberDeclaration, MethodDeclaration)> Methods => new (ClassMemberDeclaration, MethodDeclaration)[] { Create, Destroy, Clear, Encode, Decode, ClearOwnFields };
        }
    }
}
