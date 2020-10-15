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

# Performs testing for Travis CI
# If the build succeeds, but the package versions are considered unstable, this command still succeeds.
# If this command succeeds, version stability is written to thisDB.
# Requires Powershell 7+, the .NET Core CLI and Git

param (
    [Parameter(Mandatory=$true,
               HelpMessage='Key for the version stability at thisDB')]
    [string] $StableKey,
    [Parameter(Mandatory=$true,
               HelpMessage='Bucket for results at thisDB')]
    [Security.SecureString] $Bucket,
    [Parameter(Mandatory=$true,
               HelpMessage='thisDB API key for storing results')]
    [Security.SecureString] $ApiKey
)

# Test and store version stability information
$stable = & $PSScriptRoot/integrate.ps1 -NoDeploy -CheckStable
Write-Host "Writing to thisDB: $StableKey=$stable"
$headers = @{ 'X-Api-Key' = ConvertFrom-SecureString -SecureString $ApiKey -AsPlainText }
Invoke-RestMethod -Uri "https://api.thisdb.com/v1/$(ConvertFrom-SecureString -SecureString $Bucket -AsPlainText)/$StableKey" -Method Post -Headers $headers -Body $stable
