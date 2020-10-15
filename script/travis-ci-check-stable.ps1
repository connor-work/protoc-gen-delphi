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

# Performs checking of version stability for TravisCI
# This command must be run after testing and fails if package versions are considered unstable.
# Version stability information is fetched from thisDB
# Requires Powershell 7+

param (
    [Parameter(Mandatory=$true,
               HelpMessage='Key indicating version stability at thisDB')]
    [string] $StableKey,
    [Parameter(Mandatory=$true,
               HelpMessage='Bucket for version stability information at thisDB')]
    [Security.SecureString] $Bucket,
    [Parameter(Mandatory=$true,
               HelpMessage='thisDB API key for reading results')]
    [Security.SecureString] $ApiKey
)

# Fetch version stability information
$headers = @{ 'X-Api-Key' = ConvertFrom-SecureString -SecureString $ApiKey -AsPlainText }
# Throws if key is missing
$result = Invoke-RestMethod -Uri "https://api.thisdb.com/v1/$(ConvertFrom-SecureString -SecureString $Bucket -AsPlainText)/$StableKey" -Method Get -Headers $headers
$stable = [System.Convert]::ToBoolean($result)
if (!$stable) { throw "Considered unstable for $StableKey (check log of corresponding test job)" }
