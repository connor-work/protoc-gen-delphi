/// <remarks>
/// This unit corresponds to the protobuf schema definition (.proto file) <c>nested_enums.proto</c>.
/// </remarks>
unit uNestedEnums;

{$IFDEF FPC}
  {$MODE DELPHI}
{$ENDIF}

interface

uses
  System.Classes,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uProtobufEnum,
  Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Runtime.uProtobufMessage;

type
  /// <remarks>
  /// This class corresponds to the protobuf message type <c>MessageX</c>.
  /// </remarks>
  TMessageX = class(TProtobufMessage)
    public type
      /// <remarks>
      /// This enumerated type corresponds to the protobuf enum <c>EnumX</c>.
      /// </remarks>
      TEnumX = (
        /// <remarks>
        /// This enumerated value corresponds to the protobuf enum constant <c>VALUE_X</c>.
        /// </remarks>
        EnumXValueX = 0
      );

    public type
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

    /// <summary>
    /// Creates an empty <see cref="TMessageX"/> that can be used as a protobuf message.
    /// Initially, all protobuf fields are absent, meaning that they are set to their default values.
    /// </summary>
    /// <remarks>
    /// Protobuf's interpretation of the absence of a field may be counterintuitive for Delphi developers.
    /// For a detailed explanation, see https://developers.google.com/protocol-buffers/docs/proto3#default.
    /// </remarks>
    public constructor Create; override;

    /// <summary>
    /// Destroys the instances and all objects and resources held by it, including the protobuf field values.
    /// </summary>
    /// <remarks>
    /// Developers must ensure that no shared ownership of current field values or further nested embedded objects is held.
    /// </remarks>
    public destructor Destroy; override;

    /// <summary>
    /// Renders all protobuf fields absent by setting them to their default values.
    /// </summary>
    /// <remarks>
    /// The resulting instance state is equivalent to a newly constructed <see cref="TMessageX"/>.
    /// For more details, see the documentation of <see cref="Create"/>.
    /// This procedure may cause the destruction of transitively owned objects.
    /// Developers must ensure that no shared ownership of current field values or further nested embedded objects is held.
    /// </remarks>
    public procedure Clear; override;

    /// <summary>
    /// Encodes the message using the protobuf binary wire format and writes it to a stream.
    /// </summary>
    /// <param name="aDest">The stream that the encoded message is written to</param>
    public procedure Encode(aDest: TStream); override;

    /// <summary>
    /// Fills the message's protobuf fields by decoding the message using the protobuf binary wire format from data that is read from a stream.
    /// </summary>
    /// <param name="aSource">The stream that the data is read from</param>
    /// <remarks>
    /// Protobuf fields that are not present in the read data are rendered absent by setting them to their default values.
    /// This may cause the destruction of transitively owned objects (this is also the case when a present fields overwrites a previous value)
    /// Developers must ensure that no shared ownership of current field values or further nested embedded objects is held.
    /// </remarks>
    public procedure Decode(aSource: TStream); override;

    /// <summary>
    /// Copies the protobuf data from another object to this one.
    /// </summary>
    /// <param name="aSource">Object to copy from</param>
    /// <remarks>
    /// The other object must be a protobuf message of the same type.
    /// This performs a deep copy; hence, no ownership is shared.
    /// This procedure may cause the destruction of transitively owned objects in this message instance.
    /// Developers must ensure that no shared ownership of current field values or further nested embedded objects is held.
    /// </remarks>
    public procedure Assign(aSource: TPersistent); override;

    /// <summary>
    /// Renders those protobuf fields absent that belong to <see cref="TMessageX"/> (i.e., are not managed by an ancestor class), by setting them to their default values.
    /// </summary>
    private procedure ClearOwnFields;

    /// <summary>
    /// Copies those protobuf fields that belong to <see cref="TMessageX"/> (i.e., are not managed by an ancestor class), during a call to <see cref="TInterfacedPersistent.Assign"/>.
    /// </summary>
    /// <param name="aSource">Source message to copy from</param>
    private procedure AssignOwnFields(aSource: TMessageX);
  end;

implementation

constructor TMessageX.Create;
begin
  inherited;
  ClearOwnFields;
end;

destructor TMessageX.Destroy;
begin
  inherited;
end;

procedure TMessageX.Clear;
begin
  inherited;
  ClearOwnFields;
end;

procedure TMessageX.Encode(aDest: TStream);
begin
  inherited;
end;

procedure TMessageX.Decode(aSource: TStream);
begin
  inherited;
end;

procedure TMessageX.Assign(aSource: TPersistent);
var
  lSource: TMessageX;
begin
  lSource := aSource as TMessageX;
  inherited Assign(lSource);
  AssignOwnFields(lSource);
end;

procedure TMessageX.ClearOwnFields;
begin
end;

procedure TMessageX.AssignOwnFields(aSource: TMessageX);
begin
end;

end.
