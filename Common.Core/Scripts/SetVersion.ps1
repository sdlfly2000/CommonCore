##############################################################
# To Upgrade Version by Patch before build and push to Nuget.
##############################################################

# No Parameter
Param()

# Variables
$projectFileName = "Common.Core.csproj"
$projectFile = [xml](Get-Content -path $projectFileName)

# Update Version in Project File, version format is like "Major.Minor.Patch". This script only upgrades Patch.
$propertyGroupNodes = $projectFile.Project.PropertyGroup
$versionParts = $propertyGroupNodes.Version.ToString().Split(".")
$major = $versionParts[0]
$minor = $versionParts[1]
$patch = $versionParts[2]
$upgradedPatch = [string]([int]$patch + 1)

$propertyGroupNodes.Version = $major + "." + $minor + "." + $upgradedPatch

# Summary
Write-Host "-Upgrade Version-" -foregroundcolor "magenta"
Write-Host "Script:			.\Scripts\SetVersion.ps1"   
Write-Host "Current Version:	"$major"."$minor"."$patch
Write-Host "Upgraded Version:	"$major"."$minor"."$upgradedPatch

# Save the chagne
$projectFile.save($projectFileName)