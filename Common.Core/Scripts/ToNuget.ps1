##############################################################
# Pack and Push to Nuget.
# Usage: Powershell.exe -ExecutionPolicy Bypass -Command .\Scripts\ToNuget.ps1
##############################################################

# No Parameter
Param()

# Variables from appsetting.json
$appSettingFileName = "appsetting.json"
$appSetting = (Get-Content -path $appSettingFileName) | ConvertFrom-Json
$isUploadToNuget = [bool]$appSetting.UploadToNuget
$nugetApiKey = [string]$appSetting.NugetApiKey

# Variables from project file
$projectFileName = "Common.Core.csproj"
$projectFile = [xml](Get-Content -path $projectFileName)
$version = $projectFile.Project.PropertyGroup.Version

# Create Package
$cmdline = "dotnet pack --no-build --verbosity n"
Write-Host $cmdline
CMD.exe /c $cmdline

Write-Host "-Upload Package to Nuget-"

# Push to Nuget if uploadToNuget is true
if($isUploadToNuget){	
	# CMD to Push Nuget
	$cmdline = "dotnet nuget push .\bin\Debug\IdeaActivator.Common.Core.$version.nupkg --api-key $nugetApiKey --source https://api.nuget.org/v3/index.json"
	Write-Host $cmdline
	CMD.exe /c $cmdline
}else{
	Write-Host "Package is not uploaded to Nuget."
}