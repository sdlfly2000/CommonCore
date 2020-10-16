##############################################################
# To Upgrade Version by Patch before build and push to Nuget.
##############################################################

# No Parameter
Param()

# Variables from project file
$projectFileName = "Common.Core.csproj"
$projectFile = [xml](Get-Content -path $projectFileName)
$propertyGroupNodes = $projectFile.Project.PropertyGroup
$versionParts = $propertyGroupNodes.Version.ToString().Split(".")

# Update Version in Project File, version format is like "Major.Minor.Patch". This script only upgrades Patch.
$major = $versionParts[0]
$minor = $versionParts[1]
$patch = $versionParts[2]
$upgradedPatch = [string]([int]$patch + 1)

$propertyGroupNodes.Version = $major + "." + $minor + "." + $upgradedPatch

# Variables from appsetting.json
$appSettingFileName = "appsetting.json"
$appSetting = (Get-Content -path $appSettingFileName) | ConvertFrom-Json
$isUploadToNuget = [bool]$appSetting.UploadToNuget

# Summary
Write-Host "-Upgrade Version-" -foregroundcolor "magenta"
Write-Host "Script:			.\Scripts\SetVersion.ps1"   
Write-Host "Current Version:	"$major"."$minor"."$patch
Write-Host "Upgraded Version:	"$major"."$minor"."$upgradedPatch
Write-Host "UploadToNuget:	"$isUploadToNuget

# Save the chagne if isUploadToNuget is true.
if($isUploadToNuget){
	$projectFile.save($projectFileName)
}else{
	Write-Host "Version is not upgraded."
}