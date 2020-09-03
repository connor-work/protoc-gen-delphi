# Delphi Generated Code

This page describes exactly what Delphi code the protocol buffer compiler, combined with the `protoc-gen-delphi` plug-in, generates for protocol definitions using `proto3` syntax. You should read the [proto3 language guide](https://developers.google.com/protocol-buffers/docs/proto3) before reading this document.

Note: `protoc-gen-delphi` currently does not support `proto2` syntax, but future support is under consideration [GitHub issue](https://github.com/connor-work/protoc-gen-delphi/issues/2).

## Compiler Invocation

The protocol buffer compiler produces Delphi output when invoked with both the `--plugin=<path>` and the `--delphi_out` command line flag. `<path>` must be replaced by the path to the `protoc-gen-delphi` plug-in executable (called `protoc-gen-delphi.exe` on Windows). The parameter to the `--delphi_out` option is the directory where you want the compiler to write your Delphi output. The compiler creates a single source file for each `.proto` file input, with the `.pas` extension. Only `proto3` messages are supported by the Delphi code generator. Ensure that each `.proto` file begins with a declaration of:

```protobuf
syntax = "proto3";
```

## File Structure
The name of the output file is derived from the `.proto` filename by converting it to Pascal-case, treating underscores (`_`) and dashes (`-`) as word separators. The name is then prefixed with a lowercase `u`, to indicate that the file defines a Delphi unit. So, for example, a file called `player_record.proto` will result in an output file called `uPlayerRecord.pas` that contains a Delphi unit with the unqualified name `uPlayerRecord`.

Each generated file takes the following form, in terms of public declarations. (The implementation is not shown here.)

```delphi
unit [...];

interface

uses
  [... Unit references for protobuf dependencies ...];

  [... Enums ...]
  [... Message classes ...]
[...]
end.
```

Each top-level enum and message results in an enum or class being declared within the unit's interface section.

## Messages

Given a simple message declaration:

```protobuf
message Foo {}
```

The protocol buffer compiler generates class called `TFoo`, which is an immediate descendant of `TProtobufMessage`. See the inline comments for more information.

```delphi
type
  TFoo = class(TProtobufMessage)
    // Parameterless constructor that creates an empty message with all protobuf fields absent (i.e., set to their default values)
    constructor Create; override;

    // Parameterless destructor that destroys the message and all objects and resources held by it (e.g., protobuf field values)
    destructor Destroy; override;

    // Clears all protobuf fields by setting their value to their default value (rendering them absent from the message).
    procedure Clear();

    // Encodes the message using the protobuf binary wire format and writes it to the stream aDest
    procedure Encode(aDest: TStream);

    // Fills the message's protobuf fields by decoding the message using the protobuf binary wire format from data that is read from the stream aSource
    procedure Decode(aSource: TStream);
  end;
```

## Nested types

TODO

## Fields

The protocol buffer compiler generates a Delphi property for each field defined within a message. The exact nature of the property depends on the nature of the field: its type, and whether it is singular, repeated, or a map field.

### Singular Fields

Any singular field generates a read/write property. A `bytes` field shall never be set to `nil`; fetching a value from a field which hasn't been explicitly set will return an empty  `TBytes` TODO correct?. Message fields can be set to `nil` values, which is effectively clearing the field. This is not equivalent to setting the value to an "empty" instance of the message type.

### Repeated Fields

TODO

### Map Fields

TODO

### Oneof Fields

TODO

## Enumerations

TODO

## Services

The Delphi code generator ignores services entirely.
