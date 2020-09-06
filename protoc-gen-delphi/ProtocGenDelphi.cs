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
    /// Plug-in for the protobuf compiler <c>protoc</c> that generates Delphi unit source code files for protobuf schema definitions.
    /// </summary>
    public class ProtocGenDelphi
    {
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
        /// Support definition for the targetted protobuf runtime
        /// </summary>
        private static readonly IRuntimeSupport runtime = new IRuntimeSupport.Default();

        /// <summary>
        /// Name of the root base class of all generated Delphi classes for protobuf messages
        /// </summary>
        private static readonly string messageRootClass = "TProtobufMessage";

        static void Main(string[] args)
        {
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
            CodeGeneratorResponse response = new CodeGeneratorResponse();
            FileDescriptorProto lookupProtoFile(string name) => request.ProtoFile.First(file => file.Name == name);
            // Generate one source code file for each .proto file
            foreach (string protoFileName in request.FileToGenerate) response.File.Add(GenerateSourceFile(lookupProtoFile(protoFileName), lookupProtoFile));
            return response;
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
            dependencyHandler.Invoke(runtime.GetMessageDependency());
            ClassDeclaration delphiClass = GenerateClass(messageType);
            delphiInterface.Declarations.Add(new InterfaceDeclaration()
            {
                ClassDeclaration = delphiClass
            });
            MessageClassSkeleton skeleton = new MessageClassSkeleton(delphiClass.Name);
            InjectMessageClassSkeleton(skeleton, delphiClass, delphiImplementation);
            // TODO fields
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
            /// <summary>
            /// Constructs a message class skeleton for injection into a Delphi class representing a protobuf message.
            /// </summary>
            /// <param name="delphiClassName">Name of the Delphi class</param>
            public MessageClassSkeleton(string delphiClassName)
            {
                // Constructs pair of class member declaration and defining declaration for a skeleton method
                (ClassMemberDeclaration, MethodDeclaration) declareMethod(Visibility visibility, MethodInterfaceDeclaration.Types.Binding binding, MethodDeclaration definingDeclaration) => (
                    new ClassMemberDeclaration()
                    {
                        Visibility = visibility,
                        MethodDeclaration = new MethodInterfaceDeclaration
                        {
                            Binding = binding,
                            Prototype = definingDeclaration.Prototype.Clone()
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
                });
                Encode = declareMethod(Visibility.Public, MethodInterfaceDeclaration.Types.Binding.Override, new MethodDeclaration()
                {
                    Class = delphiClassName,
                    Prototype = new Prototype()
                    {
                        Name = "Encode",
                        Type = Prototype.Types.Type.Procedure,
                        ParameterList =
                        {
                            new Parameter()
                            {
                                Name = "aDest",
                                Type = "TStream"
                            }
                        }
                    },
                    Statements =
                    {
                        "inherited;"
                    }
                });
                Decode = declareMethod(Visibility.Public, MethodInterfaceDeclaration.Types.Binding.Override, new MethodDeclaration()
                {
                    Class = delphiClassName,
                    Prototype = new Prototype()
                    {
                        Name = "Decode",
                        Type = Prototype.Types.Type.Procedure,
                        ParameterList =
                        {
                            new Parameter()
                            {
                                Name = "aSource",
                                Type = "TStream"
                            }
                        }
                    },
                    Statements =
                    {
                        "inherited;"
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
