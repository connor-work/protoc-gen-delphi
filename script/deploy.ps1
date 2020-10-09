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

# Packs packages and pushes them to a NuGet source.
# The package contents must already have been built and locatable by running "dotnet pack" in the working directory.

param (
    [Parameter(HelpMessage="NuGet source to push to, defaults to nuget.org")]
    [string] $Source = "https://api.nuget.org/v3/index.json",
    [Parameter(HelpMessage="Intermediate folder for storing packages (defaults to a new temporary folder")]
    [string] $PackageFolder,
    [Parameter(HelpMessage="Mark the package versions as built by the official maintainer/CI system")]
    [switch] $Official,
    [Parameter(HelpMessage="Mark the package versions as built by this inofficial maintainer/CI system (name must not contain dots)")]
    [string] $PrivateVersionOwner,
    [Parameter(HelpMessage="Mark the package versions as stable")]
    [switch] $Stable,
    [Parameter(Mandatory=$true, HelpMessage="NuGet API key for the push source")]
    [string] $ApiKey = $(Read-Host "Enter NuGet API key for pushing to source:")
)

# Helper function to create a temporary directory as described at https://stackoverflow.com/a/34559554
function New-TemporaryDirectory {
    $parent = [System.IO.Path]::GetTempPath()
    [string] $name = [System.Guid]::NewGuid()
    New-Item -ItemType Directory -Path (Join-Path $parent $name)
}

$packOptions = @('pack', '--no-build', '--no-restore')
# Determine additional pack options to tag versions
if ($Official) { $packOptions += '/p:LocalVersion=false' }
if ($PrivateVersionOwner -ne '')
{
    $packOptions += '/p:LocalVersion=false'
    $packOptions += "/p:PrivateVersionOwner=$PrivateVersionOwner"
}
if ($Stable) { $packOptions += '/p:StableVersion=true' }

# Use a temporary directory as package folder if not specified
if ($PackageFolder -eq '') { $PackageFolder = (New-TemporaryDirectory).FullName }
# Pack everything specified by a project or solution in the current folder
$packOptions += '--output'
$packOptions += $PackageFolder
& dotnet $packOptions
# Push all packages
$packages = Get-ChildItem $PackageFolder -Filter *.nupkg
foreach ($package in $packages)
{
    dotnet nuget push --force-english-output --source $Source --api-key $ApiKey "$PackageFolder/*.nupkg"
}
