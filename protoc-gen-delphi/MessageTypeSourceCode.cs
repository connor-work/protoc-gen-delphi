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

using Google.Protobuf.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using Work.Connor.Delphi;
using Binding = Work.Connor.Delphi.MethodInterfaceDeclaration.Types.Binding;
using Visibility = Work.Connor.Delphi.Visibility;

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi
{
    /// <summary>
    /// Aggregation of Delphi source code elements that represent a protobuf message type.
    /// </summary>
    /// <remarks>
    /// The protobuf message type is mapped to a Delphi class (<i>message class</i>).
    /// </remarks>
    internal sealed partial class MessageTypeSourceCode : TypeSourceCode
    {
		/// <summary>
		/// Name of the constant that contains the Protobuf type URL of the message type.
		/// </summary>
		private static string TypeUrlConstantName => "PROTOBUF_TYPE_URL";

		/// <summary>
		/// Name of the constant that contains the default prefix of Protobuf type URLs.
		/// </summary>
		private static string TypeUrlDefaultPrefixConstantName => "PROTOBUF_TYPE_URL_DEFAULT_PREFIX";

		/// <summary>
		/// Declaration of the constant that contains the Protobuf type URL of the message type.
		/// </summary>
		private TrueConstDeclaration TypeUrlConstant => new()
        {
            Identifier = TypeUrlConstantName,
            Value = $"{TypeUrlDefaultPrefixConstantName} + {TypeName}",
            Comment = """
                <summary>
                Protobuf type URL of this message type.
                </summary>
                """.AnnotationComment(),
        };
        
        // TODO fields






		/// <summary>
		/// Required unit reference for using Delphi classes
		/// </summary>
		private static UnitReference ClassesReference => new() { Unit = new() { Unit = "Classes", Namespace = { "System" } } };

        /// <summary>
        /// Name of the Delphi root base class of all generated Delphi classes for protobuf messages
        /// </summary>
        private static readonly string messageRootClass = "TProtobufMessage";

        /// <summary>
        /// Name of the Delphi interface of all generated Delphi classes for protobuf messages that contains the public API
        /// </summary>
        private static readonly string messagePublicInterface = "IProtobufMessage";

        /// <summary>
        /// Protobuf message type to generate code for
        /// </summary>
        public DescriptorProto MessageType;

        /// <summary>
        /// Constructs Delphi source code representing a protobuf message type.
        /// </summary>
        /// <param name="messageType">Protobuf message type to generate code for</param>
        /// <param name="schema">Protobuf schema definition that this message type is part of</param>
        /// <param name="containerType">Representation of the message type that this type is nested in, absent if this is not a nested type</param>
        public MessageTypeSourceCode(DescriptorProto messageType, SchemaSourceCode schema, MessageTypeSourceCode? containerType) : base(schema, containerType) => MessageType = messageType;

        public override string TypeName => MessageType.Name; // TODO handling of absent name?

        public override InterfaceDeclaration InterfaceDeclaration => new() { ClassDeclaration = DelphiClass };

        public override NestedTypeDeclaration NestedTypeDeclaration => new() { ClassDeclaration = DelphiClass };

        /// <summary>
        /// Delphi source code representations of the directly nested protobuf enums
        /// </summary>
        private IEnumerable<EnumSourceCode> NestedEnums => MessageType.EnumType.Select(@enum => new EnumSourceCode(@enum, Schema, this));

        /// <summary>
        /// Delphi source code representations of the transitively nested protobuf enums
        /// </summary>
        public IEnumerable<EnumSourceCode> TransitivelyNestedEnums => NestedEnums.Concat(NestedMessageTypes.SelectMany(messageType => messageType.TransitivelyNestedEnums));

        /// <summary>
        /// Delphi source code representations of the directly nested protobuf message types
        /// </summary>
        private IEnumerable<MessageTypeSourceCode> NestedMessageTypes => MessageType.NestedType.Select(nestedMessageType => new MessageTypeSourceCode(nestedMessageType, Schema, this));

        /// <summary>
        /// Delphi source code representations of the transivitely nested protobuf message types (including this message type)
        /// </summary>
        public IEnumerable<MessageTypeSourceCode> TransitivelyNestedMessages => NestedMessageTypes.SelectMany(messageType => messageType.TransitivelyNestedMessages).Prepend(this);

        /// <summary>
        /// Delphi source code representations of the protobuf fields of the message type
        /// </summary>
        public IEnumerable<FieldSourceCode> Fields => MessageType.Field.Select(field => new FieldSourceCode(field, Schema, field.HasOneofIndex ? Oneofs.ElementAt(field.OneofIndex)
                                                                                                                                               : null));

        /// <summary>
        /// Delphi source code representations of the protobuf oneofs of the message type
        /// </summary>
        public IEnumerable<OneofSourceCode> Oneofs => MessageType.OneofDecl.Select((oneof, i) => new OneofSourceCode(oneof, Fields.Where(field => field.field.HasOneofIndex
                                                                                                                                               && field.field.OneofIndex == i)));

        /// <summary>
        /// Determines the required unit references for handling this protobuf message.
        /// </summary>
        /// <param name="runtime">Support definition for the targetted protobuf runtime</param>
        /// <returns>Sequence of required unit references</returns>
        public IEnumerable<UnitReference> Dependencies(IRuntimeSupport runtime)
        {
            IEnumerable<UnitReference> baseDependencies()
            {
                yield return runtime.GetDependencyForMessages();
                yield return ClassesReference;
            }
            return baseDependencies().Concat(Fields.SelectMany(field => field.Dependencies(runtime))).Concat(NestedMessageTypes.SelectMany(messageType => messageType.Dependencies(runtime))).Distinct();
        }

        /// <summary>
        /// Generated Delphi class (message class)
        /// </summary>
        private ClassDeclaration DelphiClass => new()
        {
            Name = DelphiTypeName,
            Ancestor = messageRootClass,
            NestedDeclarations = { ClassNestedDeclarations },
            Comment = new() { CommentLines = { ClassComment } }
            // TODO annotate
        };

        /// <summary>
        /// XML documentation comment for the generated Delphi class
        /// </summary>
        public IEnumerable<string> ClassComment => // TODO transfer protobuf comment
$@"<remarks>
This class corresponds to the protobuf message type <c>{TypeName}</c>.
</remarks>".Lines();

        /// <summary>
        /// Nested declarations in the message class
        /// </summary>
        private IEnumerable<ClassDeclarationNestedDeclaration> ClassNestedDeclarations
        {
            get
            {
                yield return TypeUrlConstant.InClass(Visibility.Public);
                foreach (TypeSourceCode nestedType in NestedEnums.Concat<TypeSourceCode>(NestedMessageTypes)) yield return new()
                {
                    Visibility = Visibility.Public,
                    NestedTypeDeclaration = nestedType.NestedTypeDeclaration
                };
                foreach (ClassDeclarationNestedDeclaration declaration in Fields.SelectMany(field => field.ClassNestedDeclarations)) yield return declaration;
                foreach (ClassDeclarationNestedDeclaration declaration in Oneofs.SelectMany(oneof => oneof.ClassNestedDeclarations)) yield return declaration;
                yield return new()
                {
                    Visibility = Visibility.Public,
                    Member = new() { MethodDeclaration = CreateInterface }
                };
                yield return new()
                {
                    Visibility = Visibility.Public,
                    Member = new() { MethodDeclaration = DestroyInterface }
                };
                yield return new()
                {
                    Visibility = Visibility.Public,
                    Member = new() { MethodDeclaration = ClearInterface }
                };
                yield return new()
                {
                    Visibility = Visibility.Public,
                    Member = new() { MethodDeclaration = EncodeInterface }
                };
                yield return new()
                {
                    Visibility = Visibility.Public,
                    Member = new() { MethodDeclaration = DecodeInterface }
                };
                yield return new()
                {
                    Visibility = Visibility.Public,
                    Member = new() { MethodDeclaration = MergeFromInterface }
                };
                yield return new()
                {
                    Visibility = Visibility.Public,
                    Member = new() { MethodDeclaration = AssignInterface }
                };
                yield return new()
                {
                    Visibility = Visibility.Private,
                    Member = new() { MethodDeclaration = ClearOwnFieldsInterface }
                };
                yield return new()
                {
                    Visibility = Visibility.Private,
                    Member = new() { MethodDeclaration = MergeFromOwnFieldsInterface }
                };
                yield return new()
                {
                    Visibility = Visibility.Private,
                    Member = new() { MethodDeclaration = AssignOwnFieldsInterface }
                };
            }
        }

        /// <summary>
        /// Method declarations of the message class
        /// </summary>
        public IEnumerable<MethodDeclaration> MethodDeclarations
        {
            get
            {
                yield return Create;
                yield return Destroy;
                yield return Clear;
                yield return Encode;
                yield return Decode;
                yield return MergeFrom;
                yield return Assign;
                yield return ClearOwnFields;
                yield return MergeFromOwnFields;
                yield return AssignOwnFields;
                foreach ((MessageTypeSourceCode type, MethodDeclaration method) in Fields.SelectMany(field => field.MethodDeclarations.Select(method => (this, method)))
                                                                           .Concat(Oneofs.SelectMany(oneof => oneof.MethodDeclarations.Select(method => (this, method))))
                                                                           .Concat(NestedMessageTypes.SelectMany(messageType => messageType.MethodDeclarations.Select(method => (messageType, method)))))
                {
                    method.Class = type.ContainerQualifiedDelphiTypeName;
                    yield return method;
                }
            }
        }

        /// <summary>
        /// Interface declaration of the generated <c>Create</c> method
        /// </summary>
        private MethodInterfaceDeclaration CreateInterface => new()
        {
            Binding = Binding.Override,
            Prototype = CreatePrototype,
            Comment = new() { CommentLines = { CreateComment } }
        };

        /// <summary>
        /// Prototype of the generated <c>Create</c> method
        /// </summary>
        private Prototype CreatePrototype => new()
        {
            Name = "Create",
            Type = Prototype.Types.Type.Constructor
        };

        /// <summary>
        /// XML documentation comment for the generated <c>Create</c> method
        /// </summary>
        private IEnumerable<string> CreateComment =>
$@"<summary>
Creates an empty <see cref=""{DelphiTypeName}""/> that can be used as a protobuf message.
Initially, all protobuf fields are absent, meaning that they are set to their default values.
</summary>
<remarks>
Protobuf's interpretation of the absence of a field may be counterintuitive for Delphi developers.
For a detailed explanation, see https://developers.google.com/protocol-buffers/docs/proto3#default.
</remarks>".Lines();

        /// <summary>
        /// Method declaration of the generated <c>Create</c> method
        /// </summary>
        private MethodDeclaration Create => new()
        {
            Class = DelphiTypeName,
            Prototype = CreatePrototype,
            Statements = { CreateStatements }
        };

        /// <summary>
        /// Source code lines within the generated <c>Create</c> method's statement block
        /// </summary>
        private IEnumerable<string> CreateStatements
        {
            get
            {
                yield return "inherited;";
                foreach (string statement in Oneofs.SelectMany(oneof => oneof.CreateStatements)) yield return statement;
                foreach (string statement in Fields.SelectMany(field => field.CreateStatements)) yield return statement;
                yield return "ClearOwnFields;";
            }
        }

        /// <summary>
        /// Interface declaration of the generated <c>Destroy</c> method
        /// </summary>
        private MethodInterfaceDeclaration DestroyInterface => new()
        {
            Binding = Binding.Override,
            Prototype = DestroyPrototype,
            Comment = new() { CommentLines = { DestroyComment } }
        };

        /// <summary>
        /// Prototype of the generated <c>Destroy</c> method
        /// </summary>
        private Prototype DestroyPrototype => new()
        {
            Name = "Destroy",
            Type = Prototype.Types.Type.Destructor
        };

        /// <summary>
        /// XML documentation comment for the generated <c>Destroy</c> method
        /// </summary>
        private IEnumerable<string> DestroyComment =>
$@"<summary>
Destroys the instances and all objects and resources held by it, including the protobuf field values.
</summary>
<remarks>
Developers must ensure that no shared ownership of current field values or further nested embedded objects is held.
</remarks>".Lines();

        /// <summary>
        /// Method declaration of the generated <c>Destroy</c> method
        /// </summary>
        private MethodDeclaration Destroy => new()
        {
            Class = DelphiTypeName,
            Prototype = DestroyPrototype,
            Statements = { DestroyStatements }
        };

        /// <summary>
        /// Source code lines within the generated <c>Destroy</c> method's statement block
        /// </summary>
        private IEnumerable<string> DestroyStatements
        {
            get
            {
                foreach (string statement in Fields.Reverse().SelectMany(field => field.DestroyStatements)) yield return statement;
                yield return "inherited;";
            }
        }

        /// <summary>
        /// Interface declaration of the generated <c>Clear</c> method
        /// </summary>
        private MethodInterfaceDeclaration ClearInterface => new()
        {
            Binding = Binding.Override,
            Prototype = ClearPrototype,
            Comment = new() { CommentLines = { ClearComment } }
        };

        /// <summary>
        /// Prototype of the generated <c>Clear</c> method
        /// </summary>
        private Prototype ClearPrototype => new()
        {
            Name = "Clear",
            Type = Prototype.Types.Type.Procedure
        };

        /// <summary>
        /// XML documentation comment for the generated <c>Clear</c> method
        /// </summary>
        private IEnumerable<string> ClearComment =>
$@"<summary>
Renders all protobuf fields absent by setting them to their default values.
</summary>
<remarks>
The resulting instance state is equivalent to a newly constructed <see cref=""{DelphiTypeName}""/>.
For more details, see the documentation of <see cref=""Create""/>.
This procedure may cause the destruction of transitively owned objects.
Developers must ensure that no shared ownership of current field values or further nested embedded objects is held.
</remarks>".Lines();

        /// <summary>
        /// Method declaration of the generated <c>Clear</c> method
        /// </summary>
        private MethodDeclaration Clear => new()
        {
            Class = DelphiTypeName,
            Prototype = ClearPrototype,
            Statements = { ClearStatements }
        };

        /// <summary>
        /// Source code lines within the generated <c>Clear</c> method's statement block
        /// </summary>
        private IEnumerable<string> ClearStatements =>
$@"inherited;
ClearOwnFields;".Lines();

        /// <summary>
        /// Interface declaration of the generated <c>Encode</c> method
        /// </summary>
        private MethodInterfaceDeclaration EncodeInterface => new()
        {
            Binding = Binding.Override,
            Prototype = EncodePrototype,
            Comment = new() { CommentLines = { EncodeComment } }
        };

        /// <summary>
        /// Prototype of the generated <c>Encode</c> method
        /// </summary>
        private Prototype EncodePrototype => new()
        {
            Name = "Encode",
            Type = Prototype.Types.Type.Procedure,
            ParameterList = { EncodeDestinationParameter }
        };

        /// <summary>
        /// Destination stream parameter of the generated <c>Encode</c> method
        /// </summary>
        public static Parameter EncodeDestinationParameter => new()
        {
            Name = "aDest",
            Type = "TStream"
        };

        /// <summary>
        /// XML documentation comment for the generated <c>Encode</c> method
        /// </summary>
        private IEnumerable<string> EncodeComment =>
$@"<summary>
Encodes the message using the protobuf binary wire format and writes it to a stream.
</summary>
<param name=""{EncodeDestinationParameter.Name}"">The stream that the encoded message is written to</param>".Lines();

        /// <summary>
        /// Method declaration of the generated <c>Encode</c> method
        /// </summary>
        private MethodDeclaration Encode => new()
        {
            Class = DelphiTypeName,
            Prototype = EncodePrototype,
            Statements = { EncodeStatements }
        };

        /// <summary>
        /// Source code lines within the generated <c>Encode</c> method's statement block
        /// </summary>
        private IEnumerable<string> EncodeStatements
        {
            get
            {
                yield return "inherited;";
                foreach (string statement in Fields.SelectMany(field => field.EncodeStatements)) yield return statement;
            }
        }

        /// <summary>
        /// Interface declaration of the generated <c>Decode</c> method
        /// </summary>
        private MethodInterfaceDeclaration DecodeInterface => new()
        {
            Binding = Binding.Override,
            Prototype = DecodePrototype,
            Comment = new() { CommentLines = { DecodeComment } }
        };

        /// <summary>
        /// Prototype of the generated <c>Decode</c> method
        /// </summary>
        private Prototype DecodePrototype => new()
        {
            Name = "Decode",
            Type = Prototype.Types.Type.Procedure,
            ParameterList = { DecodeSourceParameter }
        };

        /// <summary>
        /// Source stream parameter of the generated <c>Decode</c> method
        /// </summary>
        private Parameter DecodeSourceParameter => new()
        {
            Name = "aSource",
            Type = "TStream"
        };

        /// <summary>
        /// XML documentation comment for the generated <c>Decode</c> method
        /// </summary>
        private IEnumerable<string> DecodeComment =>
$@"<summary>
Fills the message's protobuf fields by decoding the message using the protobuf binary wire format from data that is read from a stream.
</summary>
<param name=""{DecodeSourceParameter.Name}"">The stream that the data is read from</param>
<remarks>
Protobuf fields that are not present in the read data are rendered absent by setting them to their default values.
This may cause the destruction of transitively owned objects (this is also the case when a present fields overwrites a previous value).
Developers must ensure that no shared ownership of current field values or further nested embedded objects is held.
</remarks>".Lines();

        /// <summary>
        /// Method declaration of the generated <c>Decode</c> method
        /// </summary>
        private MethodDeclaration Decode => new()
        {
            Class = DelphiTypeName,
            Prototype = DecodePrototype,
            Statements = { DecodeStatements }
        };

        /// <summary>
        /// Source code lines within the generated <c>Decode</c> method's statement block
        /// </summary>
        private IEnumerable<string> DecodeStatements
        {
            get
            {
                yield return "inherited;";
                foreach (string statement in Fields.SelectMany(field => field.DecodeStatements)) yield return statement;
            }
        }

        /// <summary>
        /// Interface declaration of the generated <c>MergeFrom</c> method
        /// </summary>
        private MethodInterfaceDeclaration MergeFromInterface => new()
        {
            Binding = Binding.Override,
            Prototype = MergeFromPrototype,
            Comment = new() { CommentLines = { MergeFromComment } }
        };

        /// <summary>
        /// Prototype of the generated <c>MergeFrom</c> method
        /// </summary>
        private Prototype MergeFromPrototype => new()
        {
            Name = "MergeFrom",
            Type = Prototype.Types.Type.Procedure,
            ParameterList = { MergeFromSourceParameter }
        };

        /// <summary>
        /// Source object parameter of the generated <c>MergeFrom</c> method
        /// </summary>
        private Parameter MergeFromSourceParameter => new()
        {
            Name = "aSource",
            Type = messagePublicInterface
        };

        /// <summary>
        /// XML documentation comment for the generated <c>MergeFrom</c> method
        /// </summary>
        private IEnumerable<string> MergeFromComment =>
$@"<summary>
Merges the given message (source) into this one (destination).
All singular present (non-default) scalar fields in the source replace those in the destination.
All singular embedded messages are merged recursively.
All repeated fields are concatenated, with the source field values being appended to the destination field.
If this causes a new message object to be added, a copy is created to preserve ownership.
</summary>
<param name=""{MergeFromSourceParameter.Name}"">Message to merge into this one</param>
<remarks>
The source message must be a protobuf message of the same type.
This procedure does not cause the destruction of any transitively owned objects in this message instance (append-only).
</remarks>".Lines();

        /// <summary>
        /// Method declaration of the generated <c>MergeFrom</c> method
        /// </summary>
        private MethodDeclaration MergeFrom => new()
        {
            Class = DelphiTypeName,
            Prototype = MergeFromPrototype,
            LocalDeclarations = { $"{MergeFromScratchVariableName}: {DelphiTypeName};" },
            Statements = { MergeFromStatements }
        };

        /// <summary>
        /// Name of the local variable of the <c>MergeFrom</c> method, that is used for casting to the message class's type
        /// </summary>
        private string MergeFromScratchVariableName => "lSource";

        /// <summary>
        /// Source code lines within the generated <c>MergeFrom</c> method's statement block
        /// </summary>
        private IEnumerable<string> MergeFromStatements =>
$@"{MergeFromScratchVariableName} := {MergeFromSourceParameter.Name} as {DelphiTypeName};
inherited MergeFrom({MergeFromScratchVariableName});
if (Assigned({MergeFromScratchVariableName})) then MergeFromOwnFields({MergeFromScratchVariableName});".Lines();

        /// <summary>
        /// Interface declaration of the generated <c>Assign</c> method
        /// </summary>
        private MethodInterfaceDeclaration AssignInterface => new()
        {
            Binding = Binding.Override,
            Prototype = AssignPrototype,
            Comment = new() { CommentLines = { AssignComment } }
        };

        /// <summary>
        /// Prototype of the generated <c>Assign</c> method
        /// </summary>
        private Prototype AssignPrototype => new()
        {
            Name = "Assign",
            Type = Prototype.Types.Type.Procedure,
            ParameterList = { AssignSourceParameter }
        };

        /// <summary>
        /// Source object parameter of the generated <c>Assign</c> method
        /// </summary>
        private Parameter AssignSourceParameter => new()
        {
            Name = "aSource",
            Type = "TPersistent"
        };

        /// <summary>
        /// XML documentation comment for the generated <c>Assign</c> method
        /// </summary>
        private IEnumerable<string> AssignComment =>
$@"<summary>
Copies the protobuf data from another object to this one.
</summary>
<param name=""{AssignSourceParameter.Name}"">Object to copy from</param>
<remarks>
The other object must be a protobuf message of the same type.
This performs a deep copy; hence, no ownership is shared.
This procedure may cause the destruction of transitively owned objects in this message instance.
Developers must ensure that no shared ownership of current field values or further nested embedded objects is held.
</remarks>".Lines();

        /// <summary>
        /// Method declaration of the generated <c>Assign</c> method
        /// </summary>
        private MethodDeclaration Assign => new()
        {
            Class = DelphiTypeName,
            Prototype = AssignPrototype,
            LocalDeclarations = { $"{AssignScratchVariableName}: {DelphiTypeName};" },
            Statements = { AssignStatements }
        };

        /// <summary>
        /// Name of the local variable of the <c>Assign</c> method, that is used for casting to the message class's type
        /// </summary>
        private string AssignScratchVariableName => "lSource";

        /// <summary>
        /// Source code lines within the generated <c>Assign</c> method's statement block
        /// </summary>
        private IEnumerable<string> AssignStatements =>
$@"{AssignScratchVariableName} := {AssignSourceParameter.Name} as {DelphiTypeName};
inherited Assign({AssignScratchVariableName});
AssignOwnFields({AssignScratchVariableName});".Lines();

        /// <summary>
        /// Interface declaration of the generated <c>ClearOwnFields</c> method
        /// </summary>
        private MethodInterfaceDeclaration ClearOwnFieldsInterface => new()
        {
            Binding = Binding.Static,
            Prototype = ClearOwnFieldsPrototype,
            Comment = new() { CommentLines = { ClearOwnFieldsComment } }
        };

        /// <summary>
        /// Prototype for the generated <c>ClearOwnFields</c> method
        /// </summary>
        private Prototype ClearOwnFieldsPrototype => new()
        {
            Name = "ClearOwnFields",
            Type = Prototype.Types.Type.Procedure
        };

        /// <summary>
        /// XML documentation comment for the generated <c>ClearOwnFields</c> method
        /// </summary>
        private IEnumerable<string> ClearOwnFieldsComment =>
$@"<summary>
Renders those protobuf fields absent that belong to <see cref=""{DelphiTypeName}""/> (i.e., are not managed by an ancestor class), by setting them to their default values.
</summary>".Lines();

        /// <summary>
        /// Method declaration of the generated <c>ClearOwnFields</c> method
        /// </summary>
        private MethodDeclaration ClearOwnFields => new()
        {
            Class = DelphiTypeName,
            Prototype = ClearOwnFieldsPrototype,
            Statements = { ClearOwnFieldsStatements }
        };

        /// <summary>
        /// Source code lines within the generated <c>ClearOwnFields</c> method's statement block
        /// </summary>
        private IEnumerable<string> ClearOwnFieldsStatements
        {
            get
            {
                foreach (string statement in Fields.SelectMany(field => field.ClearOwnFieldsStatements)) yield return statement;
            }
        }

        /// <summary>
        /// Interface declaration of the generated <c>MergeFromOwnFields</c> method
        /// </summary>
        private MethodInterfaceDeclaration MergeFromOwnFieldsInterface => new()
        {
            Binding = Binding.Static,
            Prototype = MergeFromOwnFieldsPrototype,
            Comment = new() { CommentLines = { MergeFromOwnFieldsComment } }
        };

        /// <summary>
        /// Prototype of the generated <c>MergeFromOwnFields</c> method
        /// </summary>
        private Prototype MergeFromOwnFieldsPrototype => new()
        {
            Name = "MergeFromOwnFields",
            Type = Prototype.Types.Type.Procedure,
            ParameterList = { MergeFromOwnFieldsSourceParameter }
        };

        /// <summary>
        /// Name of the source object parameter of the generated <c>MergeFromOwnFields</c> method
        /// </summary>
        public static string MergeFromOwnFieldsSourceParameterName => "aSource";

        /// <summary>
        /// Source object parameter of the generated <c>MergeFromOwnFields</c> method
        /// </summary>
        private Parameter MergeFromOwnFieldsSourceParameter => new()
        {
            Name = MergeFromOwnFieldsSourceParameterName,
            Type = DelphiTypeName
        };

        /// <summary>
        /// XML documentation comment for the generated <c>MergeFromOwnFields</c> method
        /// </summary>
        private IEnumerable<string> MergeFromOwnFieldsComment =>
$@"<summary>
Merges those protobuf fields that belong to <see cref=""{DelphiTypeName}""/> (i.e., are not managed by an ancestor class), during a call to <see cref=""MergeFrom""/>.
</summary>
<param name=""{MergeFromOwnFieldsSourceParameter.Name}"">Message to merge into this one</param>".Lines();

        /// <summary>
        /// Method declaration of the generated <c>MergeFromOwnFields</c> method
        /// </summary>
        private MethodDeclaration MergeFromOwnFields => new()
        {
            Class = DelphiTypeName,
            Prototype = MergeFromOwnFieldsPrototype,
            LocalDeclarations = { MergeFromOwnFieldsLocalDeclarations },
            Statements = { MergeFromOwnFieldsStatements }
        };

        /// <summary>
        /// Source code lines within the generated <c>MergeFromOwnFields</c> method's local declaration section
        /// </summary>
        private IEnumerable<string> MergeFromOwnFieldsLocalDeclarations
        {
            get
            {
                foreach (string declaration in Fields.SelectMany(field => field.MergeFromOwnFieldsLocalDeclarations)) yield return declaration;
            }
        }

        /// <summary>
        /// Source code lines within the generated <c>MergeFromOwnFields</c> method's statement block
        /// </summary>
        private IEnumerable<string> MergeFromOwnFieldsStatements
        {
            get
            {
                foreach (string statement in Fields.SelectMany(field => field.MergeFromOwnFieldsStatements)) yield return statement;
            }
        }

        /// <summary>
        /// Interface declaration of the generated <c>AssignOwnFields</c> method
        /// </summary>
        private MethodInterfaceDeclaration AssignOwnFieldsInterface => new()
        {
            Binding = Binding.Static,
            Prototype = AssignOwnFieldsPrototype,
            Comment = new() { CommentLines = { AssignOwnFieldsComment } }
        };

        /// <summary>
        /// Prototype of the generated <c>AssignOwnFields</c> method
        /// </summary>
        private Prototype AssignOwnFieldsPrototype => new()
        {
            Name = "AssignOwnFields",
            Type = Prototype.Types.Type.Procedure,
            ParameterList = { AssignOwnFieldsSourceParameter }
        };

        /// <summary>
        /// Name of the source object parameter of the generated <c>AssignOwnFields</c> method
        /// </summary>
        public static string AssignOwnFieldsSourceParameterName => "aSource";

        /// <summary>
        /// Source object parameter of the generated <c>AssignOwnFields</c> method
        /// </summary>
        private Parameter AssignOwnFieldsSourceParameter => new()
        {
            Name = AssignOwnFieldsSourceParameterName,
            Type = DelphiTypeName
        };

        /// <summary>
        /// XML documentation comment for the generated <c>AssignOwnFields</c> method
        /// </summary>
        private IEnumerable<string> AssignOwnFieldsComment =>
$@"<summary>
Copies those protobuf fields that belong to <see cref=""{DelphiTypeName}""/> (i.e., are not managed by an ancestor class), during a call to <see cref=""TInterfacedPersistent.Assign""/>.
</summary>
<param name=""{AssignOwnFieldsSourceParameter.Name}"">Source message to copy from</param>".Lines();

        /// <summary>
        /// Method declaration of the generated <c>AssignOwnFields</c> method
        /// </summary>
        private MethodDeclaration AssignOwnFields => new()
        {
            Class = DelphiTypeName,
            Prototype = AssignOwnFieldsPrototype,
            LocalDeclarations = { AssignOwnFieldsLocalDeclarations },
            Statements = { AssignOwnFieldsStatements }
        };

        /// <summary>
        /// Source code lines within the generated <c>AssignOwnFields</c> method's local declaration section
        /// </summary>
        private IEnumerable<string> AssignOwnFieldsLocalDeclarations
        {
            get
            {
                foreach (string declaration in Fields.SelectMany(field => field.AssignOwnFieldsLocalDeclarations)) yield return declaration;
            }
        }

        /// <summary>
        /// Source code lines within the generated <c>AssignOwnFields</c> method's statement block
        /// </summary>
        private IEnumerable<string> AssignOwnFieldsStatements
        {
            get
            {
                foreach (string statement in Fields.SelectMany(field => field.AssignOwnFieldsStatements)) yield return statement;
            }
        }


		/// <summary>
		/// Source code lines of the generated <c>MergeFieldFromJson(TJSONPair)</c> instance method's statement block.
		/// </summary>
		private static IEnumerable<string> GetTypeUrlStatements
			=>
			"""
            result := PROTOBUF_TYPE_URL;
            """.Lines();

		/// <summary>
		/// Source code lines of the generated <c>MergeFieldFromJson(TJSONPair)</c> instance method's statement block.
		/// </summary>
		private IEnumerable<string> MergeFieldFromJsonStatements
            => throw new System.NotImplementedException();
	}
}
