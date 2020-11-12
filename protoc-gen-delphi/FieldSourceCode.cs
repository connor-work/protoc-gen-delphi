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

using System;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf.Reflection;
using Work.Connor.Delphi;
using Work.Connor.Delphi.CodeWriter;
using Label = Google.Protobuf.Reflection.FieldDescriptorProto.Types.Label;
using Type = Google.Protobuf.Reflection.FieldDescriptorProto.Types.Type;

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi
{
    /// <summary>
	/// Aggregation of Delphi source code elements that represent a protobuf field.
    /// </summary>
    /// <remarks>
    /// The protobuf field is mapped to a Delphi property with a backing field, getter and setter.
    /// The property is a member of the Delphi class representing the protobuf message type containing the field (<i>message class</i>).
    /// </remarks>
    internal class FieldSourceCode
    {
        /// <summary>
        /// Unit reference for the Delphi <c>SysUtils</c> unit
        /// </summary>
        private static UnitReference SysUtilsReference => new UnitReference() { Unit = new UnitIdentifier() { Unit = "SysUtils", Namespace = { "System" } } };

        /// <summary>
        /// Required unit reference for using runtime-independent support definitions for generated files (support code)
        /// </summary>
        private static readonly UnitReference SupportCodeReference = new UnitReference()
        {
            Unit = new UnitIdentifier()
            {
                Unit = "uProtobuf",
                Namespace = { "Work.Connor.Protobuf.Delphi.ProtocGenDelphi".Split(".") }
            }
        };

        /// <summary>
        /// Protobuf field to generate code for
        /// </summary>
        private readonly FieldDescriptorProto field;

        /// <summary>
        /// Constructs Delphi source code representing a protobuf field.
        /// </summary>
        /// <param name="field">Protobuf field to generate code for</param>
        public FieldSourceCode(FieldDescriptorProto field) => this.field = field;

        /// <summary>
        /// Name of the Delphi property
        /// </summary>
        private string DelphiPropertyName => field.Name.ToPascalCase();

        /// <summary>
        /// Delphi type identifier of the Delphi type that is used to represent the protobuf field value(s) when communicating with internal (runtime) code
        /// </summary>
        private string PrivateDelphiType => field.Label switch
        {
            Label.Optional => field.Type.GetPrivateDelphiSingleValueType(field.TypeName, name => ProtocGenDelphi.ConstructDelphiTypeName(name)),
            Label.Repeated => field.Type.GetDelphiRepeatedFieldSubclass(field.TypeName, name => ProtocGenDelphi.ConstructDelphiTypeName(name)),
            _ => throw new NotImplementedException()
        };

        /// <summary>
        /// Delphi type identifier of the Delphi type that is used to represent the protobuf field value(s) when communicating with client code
        /// </summary>
        private string PublicDelphiType => field.Label switch
        {
            Label.Optional => field.Type.GetPublicDelphiSingleValueType(field.TypeName, name => ProtocGenDelphi.ConstructDelphiTypeName(name)),
            Label.Repeated => $"IProtobufRepeatedFieldValues<{field.Type.GetPublicDelphiElementType(field.TypeName, name => ProtocGenDelphi.ConstructDelphiTypeName(name))}>",
            _ => throw new NotImplementedException()
        };

        /// <summary>
        /// <see langword="true"/> if the field is a protobuf repeated field
        /// </summary>
        private bool IsRepeated => field.Label == Label.Repeated;

        /// <summary>
        /// <see langword="true"/> if the field is a protobuf singular field
        /// </summary>
        private bool IsSingular => !IsRepeated;

        /// <summary>
        /// <see langword="true"/> if the field's type is a protobuf message type (<i>message field</i>)
        /// </summary>
        private bool IsMessage => field.Type == FieldDescriptorProto.Types.Type.Message;

        /// <summary>
        /// <see langword="true"/> if the field's type is a protobuf enum (<i>enum field</i>)
        /// </summary>
        private bool IsEnum => field.Type == FieldDescriptorProto.Types.Type.Enum;

        /// <summary>
        /// Determines the required unit references for handling this protobuf field.
        /// </summary>
        /// <param name="runtime">Support definition for the targetted protobuf runtime</param>
        /// <returns>Sequence of required unit references</returns>
        public IEnumerable<UnitReference> Dependencies(IRuntimeSupport runtime)
        {
            if (IsSingular) yield return SupportCodeReference; // For default value
            IEnumerable<UnitReference> runtimeDependencies = (IsRepeated) ? runtime.GetDependenciesForRepeatedFieldType(field.Type)
                                                                          : runtime.GetDependenciesForSingularFieldType(field.Type);
            foreach (UnitReference dependency in runtimeDependencies
                                         .Concat(PublicDelphiDependencies)
                                         .Concat(PrivateDelphiDependencies)
                                                 .Distinct()) yield return dependency;
        }

        /// <summary>
        /// Required Delphi-specific unit references for representing the field value(s) when communicating with client code
        /// </summary>
        public IEnumerable<UnitReference> PublicDelphiDependencies
        {
            get
            {
                if (field.Type == Type.Bytes) yield return SysUtilsReference; // For TBytes
            }
        }

        /// <summary>
        /// Required Delphi-specific unit references for representing the field value(s) when communicating with internal (runtime) code
        /// </summary>
        public IEnumerable<UnitReference> PrivateDelphiDependencies
        {
            get
            {
                if (field.Type == Type.Bytes) yield return SysUtilsReference; // For TBytes
            }
        }

        /// <summary>
        /// Nested declarations in the containing message class that are added to represent the field
        /// </summary>
        public IEnumerable<ClassDeclarationNestedDeclaration> ClassNestedDeclarations => new ClassDeclarationNestedDeclaration[]
        {
            new ClassDeclarationNestedDeclaration()
            {
                Visibility = Visibility.Public,
                NestedConstDeclaration = new ConstDeclaration() { TrueConstDeclaration = FieldNumberConstant }
            },
            new ClassDeclarationNestedDeclaration()
            {
                Visibility = Visibility.Public,
                NestedConstDeclaration = new ConstDeclaration() { TrueConstDeclaration = FieldNameConstant }
            },
            new ClassDeclarationNestedDeclaration()
            {
                Visibility = Visibility.Private,
                Member = new ClassMemberDeclaration() { FieldDeclaration = DelphiField }
            },
            new ClassDeclarationNestedDeclaration()
            {
                Visibility = Visibility.Protected,
                Member = new ClassMemberDeclaration() { MethodDeclaration = GetterInterface }
            },
            new ClassDeclarationNestedDeclaration()
            {
                Visibility = Visibility.Protected,
                Member = new ClassMemberDeclaration() { MethodDeclaration = SetterInterface }
            },
            new ClassDeclarationNestedDeclaration()
            {
                Visibility = Visibility.Public,
                Member = new ClassMemberDeclaration()
                {
                    PropertyDeclaration = DelphiProperty,
                    AttributeAnnotations = { DelphiPropertyAttribute }
                }
            }
        };

        /// <summary>
        /// Method declarations of the surrounding Delphi class that are added to represent the field
        /// </summary>
        public IEnumerable<MethodDeclaration> MethodDeclarations => new MethodDeclaration[]
        {
            Getter,
            Setter
        };

        /// <summary>
        /// Delphi true constant to hold the protobuf field number of the field
        /// </summary>
        private TrueConstDeclaration FieldNumberConstant => new TrueConstDeclaration()
        {
            Identifier = $"PROTOBUF_FIELD_NUMBER_{field.Name.ToScreamingSnakeCase()}",
            Value = field.Number.ToString(),
            Comment = new AnnotationComment() { CommentLines = { FieldNumberConstantComment } }
        };

        /// <summary>
        /// XML documentation comment for the generated Delphi true constant holding the protobuf field number
        /// </summary>
        private IEnumerable<string> FieldNumberConstantComment =>
$@"<summary>
Protobuf field number of the protobuf field <c>{field.Name}</c>.
</summary>".Lines();

        /// <summary>
        /// Delphi true constant to hold the protobuf field name of the field
        /// </summary>
        private TrueConstDeclaration FieldNameConstant => new TrueConstDeclaration()
        {
            Identifier = $"PROTOBUF_FIELD_NAME_{field.Name.ToScreamingSnakeCase()}",
            Value = $"'{field.Name}'",
            Comment = new AnnotationComment() { CommentLines = { FieldNameConstantComment } }
        };

        /// <summary>
        /// XML documentation comment for the generated Delphi true constant holding the protobuf field name
        /// </summary>
        private IEnumerable<string> FieldNameConstantComment =>
$@"<summary>
Protobuf field name of the protobuf field <c>{field.Name}</c>.
</summary>".Lines();

        /// <summary>
        /// Generated Delphi field backing the generated property
        /// </summary>
        private FieldDeclaration DelphiField => new FieldDeclaration()
        {
            Name = $"F{DelphiPropertyName}",
            Type = PrivateDelphiType,
            Comment = new AnnotationComment()
            {
                CommentLines = { DelphiFieldComment }
            }
        };

        /// <summary>
        /// XML documentation comment for the generated Delphi field
        /// </summary>
        private IEnumerable<string> DelphiFieldComment =>
$@"<summary>
Holds the decoded value of the protobuf field <c>{field.Name}</c>.
</summary>".Lines();

        /// <summary>
        /// Interface declaration of the generated Delphi getter method
        /// </summary>
        private MethodInterfaceDeclaration GetterInterface => new MethodInterfaceDeclaration()
        {
            Prototype = GetterPrototype,
            Binding = MethodInterfaceDeclaration.Types.Binding.Virtual,
            Comment = new AnnotationComment() { CommentLines = { GetterComment } }
        };

        /// <summary>
        /// Prototype of the generated Delphi getter method
        /// </summary>
        private Prototype GetterPrototype => new Prototype()
        {
            Name = $"Get{DelphiPropertyName}",
            Type = Prototype.Types.Type.Function,
            ReturnType = PublicDelphiType
        };

        /// <summary>
        /// XML documentation comment for the generated Delphi getter method
        /// </summary>
        private IEnumerable<string> GetterComment =>
$@"<summary>
Getter for <see cref=""{DelphiPropertyName}""/>.
</summary>
<returns>The value of the protobuf field <c>{field.Name}</c></returns>
<remarks>
May be overridden. Overriders shall only add side-effects and must call the ancestor implementation.
</remarks>".Lines();

        /// <summary>
        /// Method declaration of the Delphi getter method for the generated property
        /// </summary>
        private MethodDeclaration Getter => new MethodDeclaration()
        {
            // Class not assigned, caller shall assign
            Prototype = GetterPrototype,
            Statements = { GetterStatements }
        };

        /// <summary>
        /// Statement block of the generated Delphi getter method
        /// </summary>
        private IEnumerable<string> GetterStatements
        {
            get
            {
                string valueExpression = (IsSingular && IsEnum) ? $"{ProtocGenDelphi.ConstructDelphiTypeName(field.TypeName)}({DelphiField.Name})"
                                                                : DelphiField.Name;
                yield return $"result := {valueExpression};";
            }
        }

        /// <summary>
        /// Interface declaration of the generated Delphi setter method
        /// </summary>
        private MethodInterfaceDeclaration SetterInterface => new MethodInterfaceDeclaration()
        {
            Prototype = SetterPrototype,
            Binding = MethodInterfaceDeclaration.Types.Binding.Virtual,
            Comment = new AnnotationComment() { CommentLines = { SetterComment } }
        };

        /// <summary>
        /// Prototype of the generated Delphi setter method
        /// </summary>
        private Prototype SetterPrototype => new Prototype()
        {
            Name = $"Set{DelphiPropertyName}",
            Type = Prototype.Types.Type.Procedure,
            ParameterList = { SetterParameter }
        };

        /// <summary>
        /// Input parameter of the generated Delphi setter method
        /// </summary>
        private Parameter SetterParameter => new Parameter()
        {
            Name = IsRepeated ? "aValues" : "aValue",
            Type = PublicDelphiType
        };

        /// <summary>
        /// XML documentation comment for the generated Delphi setter method
        /// </summary>
        private IEnumerable<string> SetterComment
        {
            get
            {
                yield return $"<summary>";
                yield return $"Setter for <see cref=\"{DelphiPropertyName}\"/>.";
                yield return $"</summary>";
                string valueWord = IsRepeated ? "values" : "value";
                yield return $"<param name=\"{SetterParameter.Name}\">The new {valueWord} of the protobuf field <c>{field.Name}</c></param>";
                yield return $"<remarks>";
                if (IsRepeated) yield return $"Ownership of the inserted field value collection is transferred to the containing message.";
                if (IsSingular && IsMessage) yield return $"Ownership of the inserted message is transferred to the containing message.";
                yield return $"May be overridden. Overriders shall only add side-effects and must call the ancestor implementation.";
                yield return $"</remarks>";
            }
        }

        /// <summary>
        /// Method declaration of the Delphi setter method for the generated property
        /// </summary>
        private MethodDeclaration Setter => new MethodDeclaration()
        {
            // Class not assigned, caller shall assign
            Prototype = SetterInterface.Prototype,
            Statements = { SetterStatements }
        };

        /// <summary>
        /// Statement block of the generated Delphi setter method
        /// </summary>
        private IEnumerable<string> SetterStatements
        {
            get
            {
                string valueExpression;
                if (IsRepeated) valueExpression = $"{SetterParameter.Name} as {PrivateDelphiType}";
                else if (IsSingular && IsEnum) valueExpression = $"Ord({SetterParameter.Name})";
                else valueExpression = SetterParameter.Name;
                if (IsRepeated || IsMessage) yield return $"{DelphiField.Name}.Free;";
                yield return $"{DelphiField.Name} := {valueExpression};";
                if (IsRepeated || IsMessage) yield return $"{DelphiField.Name}.SetOwner(self);";
            }
        }

        /// <summary>
        /// Generated Delphi property
        /// </summary>
        private PropertyDeclaration DelphiProperty => new PropertyDeclaration()
        {
            Name = DelphiPropertyName,
            Type = PublicDelphiType,
            ReadSpecifier = GetterPrototype.Name,
            WriteSpecifier = SetterPrototype.Name,
            Comment = new AnnotationComment() { CommentLines = { DelphiPropertyComment } }
        };

        /// <summary>
        /// XML documentation comment for the generated Delphi property
        /// </summary>
        private IEnumerable<string> DelphiPropertyComment => // TODO transfer protobuf comment
$@"<remarks>
This property corresponds to the protobuf field <c>{field.Name}</c>.
</remarks>".Lines();

        /// <summary>
        /// RTTI attribute annotation for the generated Delphi property
        /// </summary>
        private AttributeAnnotation DelphiPropertyAttribute => new AttributeAnnotation() { Attribute = $"ProtobufField({FieldNameConstant.Identifier}, {FieldNumberConstant.Identifier})" };

        /// <summary>
        /// Source code lines to be added to the <c>Create</c> method's statement block in the containing message class
        /// </summary>
        public IEnumerable<string> CreateStatements
        {
            get
            {
                if (IsRepeated) yield return $"{DelphiField.Name} := {PrivateDelphiType}.Create;";
            }
        }

        /// <summary>
        /// Source code lines to be added to the <c>Destroy</c> method's statement block in the containing message class
        /// </summary>
        public IEnumerable<string> DestroyStatements
        {
            get
            {
                if (IsRepeated) yield return $"{DelphiField.Name}.Destroy;";
                else if (IsMessage) yield return $"{DelphiField.Name}.Free;";
            }
        }

        /// <summary>
        /// Source code lines to be added to the <c>Encode</c> method's statement block in the containing message class
        /// </summary>
        public IEnumerable<string> EncodeStatements
        {
            get
            {
                if (IsRepeated) yield return $"{DelphiField.Name}.EncodeAsRepeatedField(self, {FieldNumberConstant.Identifier}, {MessageTypeSourceCode.EncodeDestinationParameter.Name});";
                else if (IsMessage) yield return $"{DelphiField.Name}.EncodeAsSingularField(self, {FieldNumberConstant.Identifier}, {MessageTypeSourceCode.EncodeDestinationParameter.Name});";
                else yield return $"{field.Type.GetDelphiWireCodec()}.EncodeSingularField({DelphiField.Name}, self, {FieldNumberConstant.Identifier}, {MessageTypeSourceCode.EncodeDestinationParameter.Name});";
            }
        }

        /// <summary>
        /// Source code lines to be added to the <c>Decode</c> method's statement block in the containing message class
        /// </summary>
        public IEnumerable<string> DecodeStatements
        {
            get
            {
                if (IsRepeated) yield return $"{DelphiField.Name}.DecodeAsUnknownRepeatedField(self, {FieldNumberConstant.Identifier});";
                else if (IsMessage)
                {
                    IEnumerable<string> lines =
$@"{DelphiField.Name}.Free;
if HasUnknownField({FieldNumberConstant.Identifier}) then
begin
  {DelphiField.Name} := {PrivateDelphiType}.Create;
  {DelphiField.Name}.DecodeAsUnknownSingularField(self, {FieldNumberConstant.Identifier});
end;".Lines();
                    foreach (string line in lines) yield return line;
                }
                else yield return $"{DelphiField.Name} := {field.Type.GetDelphiWireCodec()}.DecodeUnknownField(self, {FieldNumberConstant.Identifier});";
            }
        }

        /// <summary>
        /// Source code lines to be added to the <c>ClearOwnFields</c> method's statement block in the containing message class
        /// </summary>
        public IEnumerable<string> ClearOwnFieldsStatements
        {
            get
            {
                if (IsRepeated) yield return $"{DelphiField.Name}.Clear;";
                else if (IsMessage) yield return $"{DelphiField.Name}.Free;";
                if (IsSingular) yield return $"{DelphiField.Name} := {field.Type.GetDelphiDefaultValueExpression()};";
            }
        }

        /// <summary>
        /// Source code lines to be added to the <c>AssignOwnFields</c> method's local declaration section in the containing message class
        /// </summary>
        public IEnumerable<string> AssignOwnFieldsLocalDeclarations
        {
            get
            {
                if (IsSingular && IsMessage) yield return $"{AssignOwnFieldsScratchVariableName}: {PrivateDelphiType};";
            }
        }

        /// <summary>
        /// Name of the local variable of the <c>AssignOwnFields</c> method in the containing class, that is used for the field
        /// </summary>
        private string AssignOwnFieldsScratchVariableName => $"l{field.Name.ToPascalCase()}";

        /// <summary>
        /// Source code lines to be added to the <c>AssignOwnFields</c> method's statement block in the containing message class
        /// </summary>
        public IEnumerable<string> AssignOwnFieldsStatements
        {
            get
            {
                if (IsRepeated) yield return $"({DelphiPropertyName} as TInterfacedPersistent).Assign({MessageTypeSourceCode.AssignOwnFieldsSourceParameterName}.{DelphiPropertyName} as TInterfacedPersistent);";
                else if (IsMessage)
                {
                    IEnumerable<string> lines =
$@"{AssignOwnFieldsScratchVariableName} := {PrivateDelphiType}.Create;
{AssignOwnFieldsScratchVariableName}.Assign({MessageTypeSourceCode.AssignOwnFieldsSourceParameterName}.{DelphiPropertyName});
{DelphiPropertyName} := {AssignOwnFieldsScratchVariableName};".Lines();
                    foreach (string line in lines) yield return line;
                }
                else yield return $"{DelphiPropertyName} := {MessageTypeSourceCode.AssignOwnFieldsSourceParameterName}.{DelphiPropertyName};";
            }
        }
    }
}
