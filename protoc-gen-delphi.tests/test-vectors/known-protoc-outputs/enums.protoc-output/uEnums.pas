/// <remarks>
/// This unit corresponds to the protobuf schema definition (.proto file) <c>enums.proto</c>.
/// </remarks>
unit uEnums;

{$INCLUDE Work.Connor.Delphi.CompilerFeatures.inc}

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

type
  /// <remarks>
  /// This enumerated type corresponds to the protobuf enum <c>EnumX</c>.
  /// </remarks>
  TEnumX = (
    /// <remarks>
    /// This enumerated value corresponds to the protobuf enum constant <c>VALUE_X</c>.
    /// </remarks>
    EnumXValueX = 0
  );

type
  /// <remarks>
  /// This enumerated type corresponds to the protobuf enum <c>EnumY</c>.
  /// </remarks>
  TEnumY = (
    /// <remarks>
    /// This enumerated value corresponds to the protobuf enum constant <c>NONE</c>.
    /// </remarks>
    EnumYNone = 0,

    /// <remarks>
    /// This enumerated value corresponds to the protobuf enum constant <c>VALUE_Y</c>.
    /// </remarks>
    EnumYValueY = 3
  );

implementation

end.
