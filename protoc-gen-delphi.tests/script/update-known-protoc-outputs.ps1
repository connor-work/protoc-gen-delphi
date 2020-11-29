# Copyright 2020 Connor Roehricht (connor.work)
# 
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
# 
#     http://www.apache.org/licenses/LICENSE-2.0
# 
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.

# Updates the folders "*.protoc-output" folders of test vectors to match the current implementation
# Changes should be reviewed by a developer, to not defeat the purpose of the tests
# Requires Powershell 7+ and the .NET Core CLI

[CmdletBinding(DefaultParameterSetName='SingleVector')]
param (
    [Parameter(ParameterSetName='SingleVector',
               Mandatory=$true,
               HelpMessage='Name of the test vector to update')]
    [string] $Vector,

    [Parameter(ParameterSetName="AllVectors",
               Mandatory=$true,
               HelpMessage='Update all test vectors with existing known protoc outputs')]
    [switch] $All
)

# Helper function to create a temporary directory as described at https://stackoverflow.com/a/34559554
function New-TemporaryDirectory {
    $parent = [System.IO.Path]::GetTempPath()
    [string] $name = [System.Guid]::NewGuid()
    New-Item -ItemType Directory -Path (Join-Path $parent $name)
}

# Folder containing the test project
$projectFolder = (Get-Item $PSScriptRoot).parent
# Folder containing the known protoc outputs for all test vectors
$vectorsFolder = Join-Path $projectFolder 'test-vectors' 'known-protoc-outputs'

# Build test project
$buildFolder = (New-TemporaryDirectory).FullName
dotnet build $projectFolder --output $buildFolder

# Names of test vectors to update outputs for
$vectorNames = If ($All) { Get-ChildItem $vectorsFolder -Name -Filter *.protoc-output | % { $_ -replace '.protoc-output' } } Else { @($Vector) }

foreach ($vectorName in $vectorNames)
{
    # Folder containing the known protoc outputs of the test vector
    $outputFolder = Join-Path $vectorsFolder "$vectorName.protoc-output"

    # Clear old outputs
    if (Test-Path $outputFolder) { Get-ChildItem -Path $outputFolder -Include * | Remove-Item -Recurse -Force }
    else { New-Item -ItemType directory -Path $outputFolder }

    # Create new outputs
    # Based on https://stackoverflow.com/a/3374673
    $updateJob = Start-Job -ScriptBlock {
        [Environment]::CurrentDirectory = $args[0]
        Add-Type -Path (Join-Path $args[0] 'protoc-gen-delphi.tests.dll')
        [Work.Connor.Protobuf.Delphi.ProtocGenDelphi.Tests.KnownProtocOutputTestUpdater]::UpdateKnownProtocOutputs($args[1], $args[2])
    } -ArgumentList $buildFolder, $vectorName, $outputFolder
    Wait-Job $updateJob
    Receive-Job $updateJob
}
