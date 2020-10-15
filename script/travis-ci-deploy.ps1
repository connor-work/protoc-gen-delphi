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

# Performs the CI deployment stage for Travis CI
# Version stability information is fetched from thisDB.
# Requires Powershell 7+, the .NET Core CLI and Git

param (
    [Parameter(Mandatory=$true,
               HelpMessage='Keys indicating version stability at thisDB')]
    [string[]] $StableKeys,
    [Parameter(Mandatory=$true,
               HelpMessage='Bucket for version stability information at thisDB')]
    [Security.SecureString] $Bucket,
    [Parameter(Mandatory=$true,
               HelpMessage='thisDB API key for reading results')]
    [Security.SecureString] $ThisDbApiKey,
    [Parameter(Mandatory=$true,
               HelpMessage='NuGet API key for NuGet.org')]
    [Security.SecureString] $NuGetOrgApiKey
)
# TODO general:
# Travis for production:
# test: Spawn one job running -test per platform
# publish: Spawn one job running -check-stable per platform, spawn -deploy on windows
# Travis for PR:
# Spawn one job running test-pull-request per platform

# Fetch version stability information
$unstableCauses = @()
$headers = @{ 'X-Api-Key' = ConvertFrom-SecureString -SecureString $ThisDbApiKey -AsPlainText }
foreach ($stableKey in $StableKeys)
{
    # Throws if key is missing
    $result = Invoke-RestMethod -Uri "https://api.thisdb.com/v1/$(ConvertFrom-SecureString -SecureString $Bucket -AsPlainText)/$stableKey" -Method Get -Headers $headers
    $stable = [System.Convert]::ToBoolean($result)
    if (!$stable) { $unstableCauses += "Considered unstable for $stableKey" }
}

# Deploy
& $PSScriptRoot/integrate.ps1 -KnownUnstableCauses $unstableCauses -Production -ApiKey $NuGetOrgApiKey
