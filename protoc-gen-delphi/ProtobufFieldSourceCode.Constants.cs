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

internal sealed partial class ProtobufFieldSourceCode
{
    /// <summary>
    /// TODO
    /// </summary>
    public static string RepeatedFieldDelphiInterfaceName => "IProtobufRepeatedFieldValues";

	/// <summary>
	/// TODO
	/// </summary>
	public static string RepeatedMessageFieldDelphiClassName => "TProtobufRepeatedMessageFieldValues";

	/// <summary>
	/// TODO
	/// </summary>
	public static string RepeatedEnumFieldDelphiClassName => "TProtobufRepeatedEnumFieldValues";

	/// <summary>
	/// Name of the Delphi method of an object that represents a repeated field, that encodes the field using the Protobuf binary wire format and writes it to a stream.
	/// </summary>
	public static string RepeatedFieldEncodeDelphiMethodName => "EncodeField";

    /// <summary>
    /// Name of the Delphi method TODO.
    /// </summary>
    public static string MergeFromMessageFieldDelphiMethodName => "MergeFromProtobufMessageField";

    /// <summary>
    /// Name of the Delphi method of an object that represents a repeated field, that TODO.
    /// </summary>
    public static string RepeatedFieldMergeFromDelphiMethodName => "MergeFromField";
}
