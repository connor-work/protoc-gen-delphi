# Delphi-Specific Changes to the Language Guide (proto3)

## Note
This document describes Delphi-specific extensions to the [Language Guide (proto3)](https://developers.google.com/protocol-buffers/docs/proto3).

## Defining a Message Type

### Specifying Field Types
No changes.

### Assigning Field Numbers
No changes.

### Specifying Field Rules
No changes.

### Adding More Message Types
No changes.

### Adding Comments
No changes.

### Reserved Fields
No changes.

### What's Generated From Your **.proto**?
Addition to list:
- For **Delphi**, the compiler generates a `.pas` file from each `.proto`, defining a unit that contains a class for each message type described in your file.

## Scalar Value Types
Column addition to table:

| .proto Type | Delphi Type          |
|-------------|----------------------|
| double      | TODO                 |
| float       | TODO                 |
| int32       | TODO                 |
| int64       | TODO                 |
| uint32      | System.UInt32        |
| uint64      | TODO                 |
| sint32      | TODO                 |
| sint64      | TODO                 |
| fixed32     | TODO                 |
| fixed64     | TODO                 |
| sfixed32    | TODO                 |
| sfixed64    | TODO                 |
| bool        | TODO                 |
| string      | System.UnicodeString |
| bytes       | TODO                 |

## Default Values
No changes.

## Enumerations
No changes.

## Using Other Message Types
No changes.

## Nested Types
No changes.

## Updating a Message Type
No changes.

## Unknown Fields
No changes.

## Any
No changes.

## Oneof
No changes.

## Maps
The generated API is not available for Delphi (yet).

### Backwards compatibility
No changes.

## Packages
Addition to list:
- In **Delphi** TODO

### Packages and Name Resolution
No changes.

## Defining Services
No changes.

## JSON Mapping
No changes.

## Options
No changes.

## Generating Your Classes
Mention Delphi in languages list.
Mention required code generator plugin.
Addition to sub-list for *output directives*:
- `--delphi_out` generates Delphi code in `DST_DIR`. See the [Delphi generated code reference]() (TODO link to delphi-generated.md) for more.
