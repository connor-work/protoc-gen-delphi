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

# Performs the CI deployment step for GitHub Actions
# Requires Powershell 7+, the .NET Core CLI and Git

param (
    [Parameter(HelpMessage='Mark the package versions as stable')]
    [switch] $Stable,
    [Parameter(Mandatory=$true,
               HelpMessage='NuGet API key for NuGet.org')]
    [Security.SecureString] $NuGetOrgApiKey
)

$unstableCauses = @()
if (!$Stable) { $unstableCauses += "Package failed stability test" }
# Deploy
& $PSScriptRoot/integrate.ps1 -KnownUnstableCauses $unstableCauses -Production -ApiKey $NuGetOrgApiKey
