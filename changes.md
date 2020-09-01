# About this file
This file tracks significant changes to the project setup that are not easily recognizable from file diffs (e.g., project creation wizard operations).

# Changes
1. Create a *[global.json file](https://docs.microsoft.com/en-us/dotnet/core/tools/global-json?tabs=netcore3x)* to fix .NET Core SDK version.

    ```powershell
    dotnet new globaljson --sdk-version $(dotnet --version)
    ```

2. Created a *[nuget.config file](https://docs.microsoft.com/en-us/nuget/reference/nuget-config-file)* to fix package sources.

    ```powershell
    dotnet new nugetconfig
    ```

3. Created new .NET Core project for a Console Application (protoc-gen-delphi).

    ```powershell
    dotnet new console --language C`# --name protoc-gen-delphi --framework netcoreapp3.1 --output protoc-gen-delphi
    ```

4. Created new .NET Core solution (protoc-gen-delphi).

    ```powershell
    dotnet new sln --name protoc-gen-delphi
    ```

5. Added `protoc-gen-delphi` project to `protoc-gen-delphi` solution.

    ```powershell
    dotnet sln add protoc-gen-delphi
    ```

6. Created new xUnit test project (tests for protoc-gen-delphi).

    ```powershell
    dotnet new xunit --name protoc-gen-delphi.tests --framework netcoreapp3.1 --output protoc-gen-delphi.tests
    ```

7. Added `protoc-gen-delphi.tests` project to `protoc-gen-delphi` solution.

    ```powershell
    dotnet sln add protoc-gen-delphi.tests
    ```

8. Added SonarAnalyzer for static code analysis to `protoc-gen-delphi` project. Further package additions do not need to be tracked here.

    ```powershell
    dotnet add protoc-gen-delphi package SonarAnalyzer.CSharp --version 8.12.0.21095
    ```

9. Created a *[manifest file](https://docs.microsoft.com/en-us/dotnet/core/tools/local-tools-how-to-use)* for .NET Core local tools.

    ```powershell
    dotnet new tool-manifest
    ```

10. Installed [`dotnet-grpc`](https://docs.microsoft.com/en-us/aspnet/core/grpc/dotnet-grpc?view=aspnetcore-3.1) tool to manage protobuf references.

    ```powershell
    dotnet tool install dotnet-grpc
    ```

11. Added `protoc-gen-delphi` project as a dependency of its test project `protoc-gen-delphi.tests`.

    ```powershell
    dotnet add protoc-gen-delphi.tests reference protoc-gen-delphi
    ```

12. Added first protobuf reference using `dotnet-grpc` to the `protoc-gen-delphi` project. Further reference additions do not need to be tracked here. Note that this tool silently fails when invoked from the top level with `--project` reference (we consider this a bug). Upgraded implicitly added package reference to current version.

    ```powershell
    cd protoc-gen-delphi
    dotnet grpc add-file --services None --access Public ../proto/google/protobuf/compiler/plugin.proto
    cd ..
    dotnet add protoc-gen-delphi package Google.Protobuf --version 3.13.0
    dotnet add protoc-gen-delphi package Grpc.Tools --version 2.23.0
    ```

13. Upgraded fixed .NET Core SDK version.

    ```powershell
    dotnet new globaljson --sdk-version $(($(dotnet --list-sdks | tail -1) -split ' ')[0]) --force
    ```

