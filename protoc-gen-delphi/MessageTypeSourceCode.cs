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

using System.Collections.Generic;
using System.Linq;
using Google.Protobuf.Reflection;
using Work.Connor.Delphi;
using Visibility = Work.Connor.Delphi.Visibility;
using Binding = Work.Connor.Delphi.MethodInterfaceDeclaration.Types.Binding;

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi
{
    /// <summary>
	/// Aggregation of Delphi source code elements that represent a protobuf message type.
    /// </summary>
    /// <remarks>
    /// The protobuf message type is mapped to a Delphi class (<i>message class</i>).
    /// </remarks>
    internal class MessageTypeSourceCode
    {
        /// <summary>
        /// Required unit reference for using Delphi classes
        /// </summary>
        private static UnitReference ClassesReference => new UnitReference() { Unit = new UnitIdentifier() { Unit = "Classes", Namespace = { "System" } } };

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
        private readonly DescriptorProto messageType;

        /// <summary>
        /// Constructs Delphi source code representing a protobuf message type.
        /// </summary>
        /// <param name="messageType">Protobuf message type to generate code for</param>
        public MessageTypeSourceCode(DescriptorProto messageType) => this.messageType = messageType;

        /// <summary>
        /// Name of the generated Delphi class
        /// </summary>
        private string DelphiClassName => ProtocGenDelphi.ConstructDelphiTypeName(messageType.Name); // TODO handling of absent name?

        /// <summary>
        /// Delphi source code representations of the nested protobuf enums
        /// </summary>
        private IEnumerable<EnumSourceCode> NestedEnums => messageType.EnumType.Select(@enum => new EnumSourceCode(@enum));

        /// <summary>
        /// Delphi source code representations of the nested protobuf message types
        /// </summary>
        private IEnumerable<MessageTypeSourceCode> NestedMessageTypes => messageType.NestedType.Select(nestedMessageType => new MessageTypeSourceCode(nestedMessageType));

        /// <summary>
        /// Delphi source code representations of the protobuf fields of the message type
        /// </summary>
        private IEnumerable<FieldSourceCode> Fields => messageType.Field.Select(field => new FieldSourceCode(field));

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
            return baseDependencies().Concat(Fields.SelectMany(field => field.Dependencies(runtime))).Distinct();
        }

        /// <summary>
        /// Generated Delphi class (message class)
        /// </summary>
        public ClassDeclaration DelphiClass => new ClassDeclaration()
        {
            Name = DelphiClassName,
            Ancestor = messageRootClass,
            NestedDeclarations = { ClassNestedDeclarations },
            Comment = new AnnotationComment() { CommentLines = { ClassComment } }
            // TODO annotate
        };

        /// <summary>
        /// XML documentation comment for the generated Delphi class
        /// </summary>
        public IEnumerable<string> ClassComment => // TODO transfer protobuf comment
$@"<remarks>
This class corresponds to the protobuf message type <c>{messageType.Name}</c>.
</remarks>".Lines();

        /// <summary>
        /// Nested declarations in the message class
        /// </summary>
        private IEnumerable<ClassDeclarationNestedDeclaration> ClassNestedDeclarations
        {
            get
            {
                foreach (ClassDeclarationNestedDeclaration declaration in Fields.SelectMany(field => field.ClassNestedDeclarations)) yield return declaration;
                foreach (EnumSourceCode @enum in NestedEnums) yield return new ClassDeclarationNestedDeclaration()
                {
                    Visibility = Visibility.Public,
                    NestedTypeDeclaration = new NestedTypeDeclaration() { EnumDeclaration = @enum.DelphiEnum }
                };
                foreach (MessageTypeSourceCode nestedMessageType in NestedMessageTypes) yield return new ClassDeclarationNestedDeclaration()
                {
                    Visibility = Visibility.Public,
                    NestedTypeDeclaration = new NestedTypeDeclaration() { ClassDeclaration = nestedMessageType.DelphiClass }
                };
                yield return new ClassDeclarationNestedDeclaration()
                {
                    Visibility = Visibility.Public,
                    Member = new ClassMemberDeclaration() { MethodDeclaration = CreateInterface }
                };
                yield return new ClassDeclarationNestedDeclaration()
                {
                    Visibility = Visibility.Public,
                    Member = new ClassMemberDeclaration() { MethodDeclaration = DestroyInterface }
                };
                yield return new ClassDeclarationNestedDeclaration()
                {
                    Visibility = Visibility.Public,
                    Member = new ClassMemberDeclaration() { MethodDeclaration = ClearInterface }
                };
                yield return new ClassDeclarationNestedDeclaration()
                {
                    Visibility = Visibility.Public,
                    Member = new ClassMemberDeclaration() { MethodDeclaration = EncodeInterface }
                };
                yield return new ClassDeclarationNestedDeclaration()
                {
                    Visibility = Visibility.Public,
                    Member = new ClassMemberDeclaration() { MethodDeclaration = DecodeInterface }
                };
                yield return new ClassDeclarationNestedDeclaration()
                {
                    Visibility = Visibility.Public,
                    Member = new ClassMemberDeclaration() { MethodDeclaration = MergeFromInterface }
                };
                yield return new ClassDeclarationNestedDeclaration()
                {
                    Visibility = Visibility.Public,
                    Member = new ClassMemberDeclaration() { MethodDeclaration = AssignInterface }
                };
                yield return new ClassDeclarationNestedDeclaration()
                {
                    Visibility = Visibility.Private,
                    Member = new ClassMemberDeclaration() { MethodDeclaration = ClearOwnFieldsInterface }
                };
                yield return new ClassDeclarationNestedDeclaration()
                {
                    Visibility = Visibility.Private,
                    Member = new ClassMemberDeclaration() { MethodDeclaration = MergeFromOwnFieldsInterface }
                };
                yield return new ClassDeclarationNestedDeclaration()
                {
                    Visibility = Visibility.Private,
                    Member = new ClassMemberDeclaration() { MethodDeclaration = AssignOwnFieldsInterface }
                };
            }
        }

        /// <summary>
        /// Message declarations of the message class
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
                foreach (MethodDeclaration method in Fields.SelectMany(field => field.MethodDeclarations))
                {
                    method.Class = DelphiClassName;
                    yield return method;
                }
            }
        }

        /// <summary>
        /// Interface declaration of the generated <c>Create</c> method
        /// </summary>
        private MethodInterfaceDeclaration CreateInterface => new MethodInterfaceDeclaration()
        {
            Binding = Binding.Override,
            Prototype = CreatePrototype,
            Comment = new AnnotationComment { CommentLines = { CreateComment } }
        };

        /// <summary>
        /// Prototype of the generated <c>Create</c> method
        /// </summary>
        private Prototype CreatePrototype => new Prototype()
        {
            Name = "Create",
            Type = Prototype.Types.Type.Constructor
        };

        /// <summary>
        /// XML documentation comment for the generated <c>Create</c> method
        /// </summary>
        private IEnumerable<string> CreateComment =>
$@"<summary>
Creates an empty <see cref=""{DelphiClassName}""/> that can be used as a protobuf message.
Initially, all protobuf fields are absent, meaning that they are set to their default values.
</summary>
<remarks>
Protobuf's interpretation of the absence of a field may be counterintuitive for Delphi developers.
For a detailed explanation, see https://developers.google.com/protocol-buffers/docs/proto3#default.
</remarks>".Lines();

        /// <summary>
        /// Method declaration of the generated <c>Create</c> method
        /// </summary>
        private MethodDeclaration Create => new MethodDeclaration()
        {
            Class = DelphiClassName,
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
                foreach (string statement in Fields.SelectMany(field => field.CreateStatements)) yield return statement;
                yield return "ClearOwnFields;";
            }
        }

        /// <summary>
        /// Interface declaration of the generated <c>Destroy</c> method
        /// </summary>
        private MethodInterfaceDeclaration DestroyInterface => new MethodInterfaceDeclaration()
        {
            Binding = Binding.Override,
            Prototype = DestroyPrototype,
            Comment = new AnnotationComment { CommentLines = { DestroyComment } }
        };

        /// <summary>
        /// Prototype of the generated <c>Destroy</c> method
        /// </summary>
        private Prototype DestroyPrototype => new Prototype()
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
        private MethodDeclaration Destroy => new MethodDeclaration()
        {
            Class = DelphiClassName,
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
        private MethodInterfaceDeclaration ClearInterface => new MethodInterfaceDeclaration()
        {
            Binding = Binding.Override,
            Prototype = ClearPrototype,
            Comment = new AnnotationComment { CommentLines = { ClearComment } }
        };

        /// <summary>
        /// Prototype of the generated <c>Clear</c> method
        /// </summary>
        private Prototype ClearPrototype => new Prototype()
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
The resulting instance state is equivalent to a newly constructed <see cref=""{DelphiClassName}""/>.
For more details, see the documentation of <see cref=""Create""/>.
This procedure may cause the destruction of transitively owned objects.
Developers must ensure that no shared ownership of current field values or further nested embedded objects is held.
</remarks>".Lines();

        /// <summary>
        /// Method declaration of the generated <c>Clear</c> method
        /// </summary>
        private MethodDeclaration Clear => new MethodDeclaration()
        {
            Class = DelphiClassName,
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
        private MethodInterfaceDeclaration EncodeInterface => new MethodInterfaceDeclaration()
        {
            Binding = Binding.Override,
            Prototype = EncodePrototype,
            Comment = new AnnotationComment { CommentLines = { EncodeComment } }
        };

        /// <summary>
        /// Prototype of the generated <c>Encode</c> method
        /// </summary>
        private Prototype EncodePrototype => new Prototype()
        {
            Name = "Encode",
            Type = Prototype.Types.Type.Procedure,
            ParameterList = { EncodeDestinationParameter }
        };

        /// <summary>
        /// Destination stream parameter of the generated <c>Encode</c> method
        /// </summary>
        public static Parameter EncodeDestinationParameter => new Parameter()
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
        private MethodDeclaration Encode => new MethodDeclaration()
        {
            Class = DelphiClassName,
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
        private MethodInterfaceDeclaration DecodeInterface => new MethodInterfaceDeclaration()
        {
            Binding = Binding.Override,
            Prototype = DecodePrototype,
            Comment = new AnnotationComment { CommentLines = { DecodeComment } }
        };

        /// <summary>
        /// Prototype of the generated <c>Decode</c> method
        /// </summary>
        private Prototype DecodePrototype => new Prototype()
        {
            Name = "Decode",
            Type = Prototype.Types.Type.Procedure,
            ParameterList = { DecodeSourceParameter }
        };

        /// <summary>
        /// Source stream parameter of the generated <c>Decode</c> method
        /// </summary>
        private Parameter DecodeSourceParameter => new Parameter()
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
        private MethodDeclaration Decode => new MethodDeclaration()
        {
            Class = DelphiClassName,
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
        private MethodInterfaceDeclaration MergeFromInterface => new MethodInterfaceDeclaration()
        {
            Binding = Binding.Override,
            Prototype = MergeFromPrototype,
            Comment = new AnnotationComment { CommentLines = { MergeFromComment } }
        };

        /// <summary>
        /// Prototype of the generated <c>MergeFrom</c> method
        /// </summary>
        private Prototype MergeFromPrototype => new Prototype()
        {
            Name = "MergeFrom",
            Type = Prototype.Types.Type.Procedure,
            ParameterList = { MergeFromSourceParameter }
        };

        /// <summary>
        /// Source object parameter of the generated <c>MergeFrom</c> method
        /// </summary>
        private Parameter MergeFromSourceParameter => new Parameter()
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
        private MethodDeclaration MergeFrom => new MethodDeclaration()
        {
            Class = DelphiClassName,
            Prototype = MergeFromPrototype,
            LocalDeclarations = { $"{MergeFromScratchVariableName}: {DelphiClassName};" },
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
$@"{MergeFromScratchVariableName} := {MergeFromSourceParameter.Name} as {DelphiClassName};
inherited MergeFrom({MergeFromScratchVariableName});
if (Assigned({MergeFromScratchVariableName})) then MergeFromOwnFields({MergeFromScratchVariableName});".Lines();

        /// <summary>
        /// Interface declaration of the generated <c>Assign</c> method
        /// </summary>
        private MethodInterfaceDeclaration AssignInterface => new MethodInterfaceDeclaration()
        {
            Binding = Binding.Override,
            Prototype = AssignPrototype,
            Comment = new AnnotationComment { CommentLines = { AssignComment } }
        };

        /// <summary>
        /// Prototype of the generated <c>Assign</c> method
        /// </summary>
        private Prototype AssignPrototype => new Prototype()
        {
            Name = "Assign",
            Type = Prototype.Types.Type.Procedure,
            ParameterList = { AssignSourceParameter }
        };

        /// <summary>
        /// Source object parameter of the generated <c>Assign</c> method
        /// </summary>
        private Parameter AssignSourceParameter => new Parameter()
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
        private MethodDeclaration Assign => new MethodDeclaration()
        {
            Class = DelphiClassName,
            Prototype = AssignPrototype,
            LocalDeclarations = { $"{AssignScratchVariableName}: {DelphiClassName};" },
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
$@"{AssignScratchVariableName} := {AssignSourceParameter.Name} as {DelphiClassName};
inherited Assign({AssignScratchVariableName});
AssignOwnFields({AssignScratchVariableName});".Lines();

        /// <summary>
        /// Interface declaration of the generated <c>ClearOwnFields</c> method
        /// </summary>
        private MethodInterfaceDeclaration ClearOwnFieldsInterface => new MethodInterfaceDeclaration()
        {
            Binding = Binding.Static,
            Prototype = ClearOwnFieldsPrototype,
            Comment = new AnnotationComment { CommentLines = { ClearOwnFieldsComment } }
        };

        /// <summary>
        /// Prototype for the generated <c>ClearOwnFields</c> method
        /// </summary>
        private Prototype ClearOwnFieldsPrototype => new Prototype()
        {
            Name = "ClearOwnFields",
            Type = Prototype.Types.Type.Procedure
        };

        /// <summary>
        /// XML documentation comment for the generated <c>ClearOwnFields</c> method
        /// </summary>
        private IEnumerable<string> ClearOwnFieldsComment =>
$@"<summary>
Renders those protobuf fields absent that belong to <see cref=""{DelphiClassName}""/> (i.e., are not managed by an ancestor class), by setting them to their default values.
</summary>".Lines();

        /// <summary>
        /// Method declaration of the generated <c>ClearOwnFields</c> method
        /// </summary>
        private MethodDeclaration ClearOwnFields => new MethodDeclaration()
        {
            Class = DelphiClassName,
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
        private MethodInterfaceDeclaration MergeFromOwnFieldsInterface => new MethodInterfaceDeclaration()
        {
            Binding = Binding.Static,
            Prototype = MergeFromOwnFieldsPrototype,
            Comment = new AnnotationComment { CommentLines = { MergeFromOwnFieldsComment } }
        };

        /// <summary>
        /// Prototype of the generated <c>MergeFromOwnFields</c> method
        /// </summary>
        private Prototype MergeFromOwnFieldsPrototype => new Prototype()
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
        private Parameter MergeFromOwnFieldsSourceParameter => new Parameter()
        {
            Name = MergeFromOwnFieldsSourceParameterName,
            Type = DelphiClassName
        };

        /// <summary>
        /// XML documentation comment for the generated <c>MergeFromOwnFields</c> method
        /// </summary>
        private IEnumerable<string> MergeFromOwnFieldsComment =>
$@"<summary>
Merges those protobuf fields that belong to <see cref=""{DelphiClassName}""/> (i.e., are not managed by an ancestor class), during a call to <see cref=""MergeFrom""/>.
</summary>
<param name=""{MergeFromOwnFieldsSourceParameter.Name}"">Message to merge into this one</param>".Lines();

        /// <summary>
        /// Method declaration of the generated <c>MergeFromOwnFields</c> method
        /// </summary>
        private MethodDeclaration MergeFromOwnFields => new MethodDeclaration()
        {
            Class = DelphiClassName,
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
        private MethodInterfaceDeclaration AssignOwnFieldsInterface => new MethodInterfaceDeclaration()
        {
            Binding = Binding.Static,
            Prototype = AssignOwnFieldsPrototype,
            Comment = new AnnotationComment { CommentLines = { AssignOwnFieldsComment } }
        };

        /// <summary>
        /// Prototype of the generated <c>AssignOwnFields</c> method
        /// </summary>
        private Prototype AssignOwnFieldsPrototype => new Prototype()
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
        private Parameter AssignOwnFieldsSourceParameter => new Parameter()
        {
            Name = AssignOwnFieldsSourceParameterName,
            Type = DelphiClassName
        };

        /// <summary>
        /// XML documentation comment for the generated <c>AssignOwnFields</c> method
        /// </summary>
        private IEnumerable<string> AssignOwnFieldsComment =>
$@"<summary>
Copies those protobuf fields that belong to <see cref=""{DelphiClassName}""/> (i.e., are not managed by an ancestor class), during a call to <see cref=""TInterfacedPersistent.Assign""/>.
</summary>
<param name=""{AssignOwnFieldsSourceParameter.Name}"">Source message to copy from</param>".Lines();

        /// <summary>
        /// Method declaration of the generated <c>AssignOwnFields</c> method
        /// </summary>
        private MethodDeclaration AssignOwnFields => new MethodDeclaration()
        {
            Class = DelphiClassName,
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
    }
}
