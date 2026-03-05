
using Google.Protobuf.Reflection;
using Google.Protobuf.WellKnownTypes;
using System;

/// Copyright 2025 Connor Erdmann (connor.work)
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
namespace Work.Connor.Protobuf.Delphi.ProtocGenDelphi;

/// <summary>
/// Extensions to <see cref="FieldDescriptorProto"/>.
/// </summary>
internal static partial class ExtFieldDescriptorProto
{
    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public static string DelphiFieldConcreteType(this FieldDescriptorProto field)
    {
        if (field.Label is FieldDescriptorProto.Types.Label.Repeated)
        {
            if (field.Type is FieldDescriptorProto.Types.Type.Message) return $"TProtobufRepeatedMessageFieldValues<{ProtocGenDelphi.ConstructDelphiTypeName(field.TypeName)}>";
            if (field.Type is FieldDescriptorProto.Types.Type.Enum) return $"TProtobufRepeatedEnumFieldValues<{ProtocGenDelphi.ConstructDelphiTypeName(field.TypeName)}>";
            return field.Type.DelphiRepeatedFieldConcreteType();
        }
        if (field.Type is FieldDescriptorProto.Types.Type.Message or FieldDescriptorProto.Types.Type.Enum) return ProtocGenDelphi.ConstructDelphiTypeName(field.TypeName);
        return field.Type.DelphiSingularFieldType();
    }

	/// <summary>
	/// TODO
	/// </summary>
	/// <param name="field"></param>
	/// <returns></returns>
	public static string DelphiFieldDeclaredType(this FieldDescriptorProto field)
	{
		if (field.Label is FieldDescriptorProto.Types.Label.Repeated)
		{
			// TODO
			if (field.Type is FieldDescriptorProto.Types.Type.Message) return $"TProtobufRepeatedMessageFieldValues<{ProtocGenDelphi.ConstructDelphiTypeName(field.TypeName)}>";
			if (field.Type is FieldDescriptorProto.Types.Type.Enum) return $"TProtobufRepeatedEnumFieldValues<{ProtocGenDelphi.ConstructDelphiTypeName(field.TypeName)}>";
			return field.Type.DelphiRepeatedFieldConcreteType();
		}
		if (field.Type is FieldDescriptorProto.Types.Type.Message or FieldDescriptorProto.Types.Type.Enum) return ProtocGenDelphi.ConstructDelphiTypeName(field.TypeName);
		return field.Type.DelphiSingularFieldType();
	}
}
