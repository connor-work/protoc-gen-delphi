/// Copyright 2020 Connor Roehricht (connor.work)
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
using static Work.Connor.Delphi.CodeWriter.StringExtensions;

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi
{
    /// <summary>
    /// Aggregation of Delphi source code elements that represent a protobuf oneof.
    /// </summary>
    /// <remarks>
    /// The protobuf oneof is mapped to a Delphi property with a backing field and getter.
    /// The property is a member of the Delphi class representing the protobuf message type containing the oneof (<i>message class</i>).
    /// </remarks>
    internal class OneofSourceCode
    {
        /// <summary>
        /// Mapping of protobuf oneofs to identifiers for Delphi enumerated types representing the cases of the oneof
        /// </summary>
        private static IdentifierGenerator<OneofDescriptorProto> EnumIdentifier => new IdentifierTemplate<OneofDescriptorProto>("case type", x => x.Name, "_ProtobufOneof", IdentifierCase.Pascal, "T", "Case", caseSensitive: false);

        /// <summary>
        /// Mapping of protobuf oneofs to identifiers for Delphi enumerated values representing the absence case of the oneof
        /// </summary>
        private static IdentifierGenerator<OneofDescriptorProto> AbsenceValueIdentifier => new IdentifierTemplate<OneofDescriptorProto>("absence case value", x => x.Name, "_ProtobufOneof", IdentifierCase.Pascal, suffix: "CaseNone", caseSensitive: false);

        /// <summary>
        /// Mapping of protobuf oneofs to identifiers for Delphi fields marking the oneof case
        /// </summary>
        private static IdentifierGenerator<OneofDescriptorProto> FieldIdentifier => new IdentifierTemplate<OneofDescriptorProto>("case field", x => x.Name, "_ProtobufOneof", IdentifierCase.Pascal, "F", "Case", caseSensitive: false);

        /// <summary>
        /// Mapping of protobuf oneofs to identifiers for Delphi getters for the oneof case
        /// </summary>
        private static IdentifierGenerator<OneofDescriptorProto> GetterIdentifier => new IdentifierTemplate<OneofDescriptorProto>("case field getter", x => x.Name, "_ProtobufOneof", IdentifierCase.Pascal, "Get", "Case", caseSensitive: false);

        /// <summary>
        /// Mapping of protobuf oneofs to identifiers for Delphi setters for the oneof case
        /// </summary>
        private static IdentifierGenerator<OneofDescriptorProto> SetterIdentifier => new IdentifierTemplate<OneofDescriptorProto>("case field getter", x => x.Name, "_ProtobufOneof", IdentifierCase.Pascal, "Set", "Case", caseSensitive: false);

        /// <summary>
        /// Mapping of protobuf oneofs to identifiers for Delphi properties representing the oneof case
        /// </summary>
        private static IdentifierGenerator<OneofDescriptorProto> PropertyIdentifier => new IdentifierTemplate<OneofDescriptorProto>("case property", x => x.Name, "_ProtobufOneof", IdentifierCase.Pascal, suffix: "Case", caseSensitive: false);

        /// <summary>
        /// Protobuf oneof to generate code for
        /// </summary>
        private readonly OneofDescriptorProto oneof;

        /// <summary>
        /// Protobuf fields that are part of the oneof
        /// </summary>
        private IEnumerable<FieldSourceCode> Fields { get; }

        /// <summary>
        /// Constructs Delphi source code representing a protobuf oneof.
        /// </summary>
        /// <param name="oneof">Protobuf oneof to generate code for</param>
        /// <param name="fields">Protobuf fields that are part of the oneof</param>
        public OneofSourceCode(OneofDescriptorProto oneof, IEnumerable<FieldSourceCode> fields)
        {
            this.oneof = oneof;
            Fields = fields;
        }

        /// <summary>
        /// Prefix to generated identifiers for presence cases of the oneof
        /// </summary>
        public string PresenceCasePrefix => $"{oneof.Name.ToCase(IdentifierCase.Pascal)}Case";

        /// <summary>
        /// String to be used in generated Delphi comments to refer to this oneof
        /// </summary>
        public string DelphiCommentReference => $"<c>{oneof.Name}</c>";

        /// <summary>
        /// Name of the Delphi property representing the oneof case
        /// </summary>
        public string DelphiPropertyName => PropertyIdentifier.Generate(oneof, reservedIdentifiers: ProtocGenDelphi.ReservedIdentifiers);

        /// <summary>
        /// Delphi type identifier of the Delphi enumerated type that is used to represent the cases of the oneof
        /// </summary>
        private string DelphiEnumName => EnumIdentifier.Generate(oneof, reservedIdentifiers: ProtocGenDelphi.ReservedIdentifiers);

        /// <summary>
        /// Delphi identifier of the Delphi enumerated value that is used to represent the absence case of the oneof
        /// </summary>
        public string AbsenceEnumValueName => AbsenceValueIdentifier.Generate(oneof, reservedIdentifiers: ProtocGenDelphi.ReservedIdentifiers);

        /// <summary>
        /// Nested declarations in the containing message class that are added to represent the oneof
        /// </summary>
        public IEnumerable<ClassDeclarationNestedDeclaration> ClassNestedDeclarations
        {
            get
            {
                yield return new()
                {
                    Visibility = Visibility.Public,
                    NestedTypeDeclaration = new() { EnumDeclaration = DelphiEnum }
                };
                yield return new()
                {
                    Visibility = Visibility.Private,
                    Member = new() { FieldDeclaration = DelphiField }
                };
                yield return new()
                {
                    Visibility = Visibility.Protected,
                    Member = new() { MethodDeclaration = GetterInterface }
                };
                yield return new()
                {
                    Visibility = Visibility.Protected,
                    Member = new() { MethodDeclaration = SetterInterface }
                };
                yield return new()
                {
                    Visibility = Visibility.Public,
                    Member = new() { PropertyDeclaration = DelphiProperty }
                };
            }
        }

        /// <summary>
        /// Method declarations of the surrounding Delphi class that are added to represent the oneof
        /// </summary>
        public IEnumerable<MethodDeclaration> MethodDeclarations
        {
            get
            {
                yield return Getter;
                yield return Setter;
            }
        }

        /// <summary>
        /// Generated Delphi enumerated value representing the absence case of the oneof
        /// </summary>
        private EnumValueDeclaration AbsenceEnumValue => new()
        {
            Name = AbsenceEnumValueName,
            Ordinality = 0,
            Comment = new() { CommentLines = { AbsenceEnumValueComment } }
        };

        /// <summary>
        /// XML documentation comment for the generated Delphi enumerated value representing the absence case of the oneof
        /// </summary>
        private IEnumerable<string> AbsenceEnumValueComment =>
$@"<summary>
Indicates absence of the protobuf oneof {DelphiCommentReference}.
</summary>".Lines();

        /// <summary>
        /// Generated Delphi enumerated values representing the presence cases of the oneof
        /// </summary>
        private IEnumerable<EnumValueDeclaration> PresenceEnumValues
        {
            get
            {
                foreach ((EnumValueDeclaration value, int index) in Fields.Select((field, index) => (field.PresenceEnumValue, index)))
                {
                    value.Ordinality = index + 1;
                    yield return value;
                }
            }
        }

        /// <summary>
        /// Generated Delphi enumerated type representing the cases of the oneof
        /// </summary>
        private EnumDeclaration DelphiEnum => new()
        {
            Name = DelphiEnumName,
            Values = { PresenceEnumValues.Prepend(AbsenceEnumValue) },
            Comment = new() { CommentLines = { DelphiEnumComment } }
            // TODO annotate
        };

        /// <summary>
        /// XML documentation comment for the generated Delphi enumerated type representing the cases of the oneof
        /// </summary>
        private IEnumerable<string> DelphiEnumComment => // TODO transfer protobuf comment
$@"<remarks>
This enumerated type represents the cases of the protobuf oneof {DelphiCommentReference}.
</remarks>".Lines();

        /// <summary>
        /// Generated Delphi field backing the generated property for the oneof case
        /// </summary>
        private FieldDeclaration DelphiField => new()
        {
            Name = FieldIdentifier.Generate(oneof, reservedIdentifiers: ProtocGenDelphi.ReservedIdentifiers),
            Type = DelphiEnumName,
            Comment = new() { CommentLines = { DelphiFieldComment } }
        };

        /// <summary>
        /// XML documentation comment for the generated Delphi field for the oneof case
        /// </summary>
        private IEnumerable<string> DelphiFieldComment =>
$@"<summary>
Holds the case of the protobuf oneof {DelphiCommentReference}.
</summary>".Lines();

        /// <summary>
        /// Interface declaration of the generated Delphi getter method for the oneof case
        /// </summary>
        private MethodInterfaceDeclaration GetterInterface => new()
        {
            Prototype = GetterPrototype,
            Binding = MethodInterfaceDeclaration.Types.Binding.Virtual,
            Comment = new() { CommentLines = { GetterComment } }
        };

        /// <summary>
        /// Prototype of the generated Delphi getter method for the oneof case
        /// </summary>
        private Prototype GetterPrototype => new()
        {
            Name = GetterIdentifier.Generate(oneof, reservedIdentifiers: ProtocGenDelphi.ReservedIdentifiers),
            Type = Prototype.Types.Type.Function,
            ReturnType = DelphiEnumName
        };

        /// <summary>
        /// XML documentation comment for the generated Delphi getter method for the oneof case
        /// </summary>
        private IEnumerable<string> GetterComment =>
$@"<summary>
Getter for <see cref=""{DelphiPropertyName}""/>.
</summary>
<returns>The case of the protobuf oneof {DelphiCommentReference}</returns>
<remarks>
May be overridden. Overriders shall only add side-effects and must call the ancestor implementation.
</remarks>".Lines();

        /// <summary>
        /// Method declaration of the Delphi getter method for the generated property for the oneof case
        /// </summary>
        private MethodDeclaration Getter => new()
        {
            // Class not assigned, caller shall assign
            Prototype = GetterPrototype,
            Statements = { GetterStatements }
        };

        /// <summary>
        /// Statement block of the generated Delphi getter method for the oneof case
        /// </summary>
        private IEnumerable<string> GetterStatements
        {
            get
            {
                yield return $"result := {DelphiField.Name};";
            }
        }

        /// <summary>
        /// Interface declaration of the generated Delphi setter method for the oneof case
        /// </summary>
        private MethodInterfaceDeclaration SetterInterface => new()
        {
            Prototype = SetterPrototype,
            Binding = MethodInterfaceDeclaration.Types.Binding.Virtual,
            Comment = new() { CommentLines = { SetterComment } }
        };

        /// <summary>
        /// Prototype of the generated Delphi setter method for the oneof case
        /// </summary>
        private Prototype SetterPrototype => new()
        {
            Name = SetterIdentifier.Generate(oneof, reservedIdentifiers: ProtocGenDelphi.ReservedIdentifiers),
            Type = Prototype.Types.Type.Procedure,
            ParameterList = { SetterParameter }
        };

        /// <summary>
        /// Input parameter of the generated Delphi setter method for the oneof case
        /// </summary>
        private Parameter SetterParameter => new()
        {
            Name = "aCase",
            Type = DelphiEnumName
        };

        /// <summary>
        /// XML documentation comment for the generated Delphi setter method for the oneof case
        /// </summary>
        private IEnumerable<string> SetterComment =>
$@"<summary>
Setter for <see cref=""{DelphiPropertyName}""/>.
</summary>
<param name=""{SetterParameter.Name}"">The new case of the protobuf oneof {DelphiCommentReference}</param>
<remarks>
May be overridden. Overriders shall only add side-effects and must call the ancestor implementation.
</remarks>".Lines();

        /// <summary>
        /// Method declaration of the Delphi setter method for the generated property for the oneof case
        /// </summary>
        private MethodDeclaration Setter => new()
        {
            // Class not assigned, caller shall assign
            Prototype = SetterPrototype,
            LocalDeclarations = { $"{SetterVariableName}: {DelphiEnumName};" },
            Statements = { SetterStatements }
        };

        /// <summary>
        /// Name of the local variable of the setter method for the oneof case
        /// </summary>
        private string SetterVariableName => "lCase";

        /// <summary>
        /// Statement block of the generated Delphi setter method for the oneof case
        /// </summary>
        private IEnumerable<string> SetterStatements
        {
            get
            {
                yield return $"if ({SetterParameter.Name} <> {DelphiField.Name}) then";
                yield return $"begin";
                yield return $"  {SetterVariableName} := {DelphiField.Name};";
                yield return $"  {DelphiField.Name} := {SetterParameter.Name};";
                yield return $"  case {SetterVariableName} of";
                foreach (FieldSourceCode field in Fields) yield return $"    {field.PresenceEnumValue.Name}: {field.DelphiPresencePropertyName} := False;";
                yield return $"  end;";
                yield return $"  case {DelphiField.Name} of";
                foreach (FieldSourceCode field in Fields) yield return $"    {field.PresenceEnumValue.Name}: {field.DelphiPresencePropertyName} := True;";
                yield return $"  end;";
                yield return $"end;";
            }
        }

        /// <summary>
        /// Generated Delphi property for the oneof case
        /// </summary>
        private PropertyDeclaration DelphiProperty => new()
        {
            Name = DelphiPropertyName,
            Type = DelphiEnumName,
            ReadSpecifier = GetterPrototype.Name,
            WriteSpecifier = SetterPrototype.Name,
            Comment = new() { CommentLines = { DelphiPropertyComment } }
        };

        /// <summary>
        /// XML documentation comment for the generated Delphi property for the oneof case
        /// </summary>
        private IEnumerable<string> DelphiPropertyComment => // TODO transfer protobuf comment
$@"<remarks>
This property corresponds to the case of the protobuf oneof {DelphiCommentReference}.
</remarks>".Lines();

        /// <summary>
        /// Source code lines to be added to the <c>Create</c> method's statement block in the containing message class
        /// </summary>
        public IEnumerable<string> CreateStatements
        {
            get
            {
                yield return $"{DelphiField.Name} := {AbsenceEnumValueName};";
            }
        }
    }
}
