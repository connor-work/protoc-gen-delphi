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
using Work.Connor.Delphi;

namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi
{
    /// <summary>
    /// Internal base structure of a Delphi class for a protobuf message type.
    /// Can be used to inject code into so-called "skeleton methods" that are present in each such class.
    /// </summary>
    internal class MessageClassSkeleton
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
            PropertyAccessors = new List<MethodDeclaration>();
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

        /// <summary>
        /// Mutable list of method declarations that serve as getter or setters of generated properties.
        /// </summary>
        public List<MethodDeclaration> PropertyAccessors { get; }
    }
}
