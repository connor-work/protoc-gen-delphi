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

# Builds and tests all packages in this repository, and optionally deploys generated artifacts
# Requires Powershell 7+, the .NET Core CLI and Git
# Requires the NuGet CLI if deploying locally

[CmdletBinding(DefaultParameterSetName='Developer')]
param (
    [Parameter(HelpMessage='Prevents deployment of unstable package versions')]
    [switch] $OnlyDeployStable,
    [Parameter(HelpMessage='Instead of throwing an error on unstable package versions, output a boolean indicating stability')]
    [switch] $CheckStable,
    [Parameter(HelpMessage='Instead of testing, consider the package versions as stable if this array is empty, or unstable with the specified causes otherwise')]
    [string[]] $KnownUnstableCauses,

    [Parameter(ParameterSetName="NoDeploy",
               HelpMessage='Prevents any deployment (e.g., for a pull request)')]
    [switch] $NoDeploy,

    [Parameter(ParameterSetName="Developer",
               HelpMessage='Local NuGet package feed (folder) to push to (defaults to $home/.local-nuget-packages)')]
    [string] $LocalFeed="$home/.local-nuget-packages",

    [Parameter(ParameterSetName="Production",
               Mandatory=$true,
               HelpMessage='Mark the package versions as built by a maintainer/CI system (the official one if -PrivateVersionOwner is not set)')]
    [switch] $Production,
    [Parameter(ParameterSetName="Production",
               HelpMessage='Mark the package versions as built by this inofficial maintainer/CI system (name must not contain dots)')]
    [string] $PrivateVersionOwner,
    [Parameter(HelpMessage='NuGet source to push to, defaults to nuget.org')]
    [string] $Source = "https://api.nuget.org/v3/index.json",
    [Parameter(ParameterSetName="Production",
               Mandatory=$true,
               HelpMessage='NuGet API key for the push source')]
    [Security.SecureString] $ApiKey
)

$unstableCauses = @()

# Helper function to signal an issue that renders the current version unstable
function New-UnstableCause([string] $cause) {
    $script:unstableCauses += $cause
    Write-Error $cause
}

# Helper function to write a new log section heading to the host
function Write-HostSection([string] $heading) {
    Write-Host ""
    Write-Host -BackgroundColor DarkBlue -ForegroundColor White -NoNewline (" " * ($heading.length + 2))
    Write-Host ""
    Write-Host -BackgroundColor DarkBlue -ForegroundColor White -NoNewline " $heading "
    Write-Host ""
    Write-Host -BackgroundColor DarkBlue -ForegroundColor White -NoNewline (" " * ($heading.length + 2))
    Write-Host ""
    Write-Host ""
}

# Helper function to write a success event message to the host
function Write-SuccessEvent([string] $message) {
    Write-Host ""
    Write-Host -BackgroundColor DarkGreen -ForegroundColor White -NoNewline " $message "
    Write-Host ""
    Write-Host ""
}

Write-HostSection "Restoring .NET dependencies and tools:"
dotnet restore | Out-Host
if ($LastExitCode -ne 0) { throw "dotnet restore failed" }
else { Write-SuccessEvent "Restore successful" }

Write-HostSection "Building .NET projects:"
dotnet build --no-restore | Out-Host
if ($LastExitCode -ne 0) { throw "dotnet build failed" }
else { Write-SuccessEvent "Build successful" }

if ($null -eq $KnownUnstableCauses)
{
    Write-HostSection "Testing .NET projects:"
    dotnet test --no-build --no-restore | Out-Host
    if ($LastExitCode -ne 0) { New-UnstableCause "dotnet test failed" }
    else { Write-SuccessEvent "Tests successful" }

    Write-HostSection "Checking line endings:"
    try
    {
        & $PSScriptRoot/check-eol.ps1
        Write-SuccessEvent "Line endings OK"
    }
    catch { New-UnstableCause "Unexpected line endings" }
}
else { $unstableCauses += $KnownUnstableCauses}

$stable = $unstableCauses.count -eq 0

if (!$NoDeploy)
{
    Write-HostSection "Deploying artifacts:"
    if ($OnlyDeployStable -And !$stable)
    {
        Write-Host "No artifacts to deploy (unstable versions)"
    }
    else
    {
        $deployOptions = @{ Stable = $stable }
        if ($Production)
        {
            $deployOptions.Production = $true
            if ($PrivateVersionOwner -ne '') { $deployOptions.PrivateVersionOwner = $PrivateVersionOwner }
            $deployOptions.Source = $Source
            $deployOptions.ApiKey = $ApiKey
        }
        else { $deployOptions.LocalFeed = $LocalFeed }
        & $PSScriptRoot/deploy.ps1 @deployOptions
        Write-SuccessEvent "Deployment successful"
    }
}

Write-HostSection "Summarizing operation:"
if ($CheckStable)
{
    if (!$stable) { Write-Host ("Package versions considered unstable, causes: " + ($unstableCauses -join ', ')) }
    Write-Host "Writing stability to output:"
    Write-Output $stable
}
elseif (!$stable) { throw "Package versions considered unstable, causes: " + ($unstableCauses -join ', ') }
Write-SuccessEvent "Operation OK"
