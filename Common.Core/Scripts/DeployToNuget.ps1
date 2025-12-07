##############################################################
# Pack and Push to Nuget.
# Usage: Powershell.exe -ExecutionPolicy Bypass -Command .\Scripts\ToNuget.ps1
##############################################################

# No Parameter
Param()

# Upgrade Project Version
Powershell.exe .\Scripts\SetVersion.ps1

# Get NugetKey
Powershell.exe .\Scripts\FtpDownload.ps1
$nugetSettingFileName = "app.json"
$nugetSetting = (Get-Content -path $nugetSettingFileName) | ConvertFrom-Json
$nugetApiKey = [string]$nugetSetting.NugetApiKey

# Variables from appsetting.json
$appSettingFileName = "appsetting.json"
$appSetting = (Get-Content -path $appSettingFileName) | ConvertFrom-Json
$isUploadToNuget = [bool]$appSetting.UploadToNuget

# Variables from project file
$projectFileName = "Common.Core.csproj"
$projectFile = [xml](Get-Content -path $projectFileName)
$version = $projectFile.Project.PropertyGroup.Version

# Create Build Release
$cmdline = "dotnet build --configuration Release"
Write-Host $cmdline
CMD.exe /c $cmdline

# Create Package
$cmdline = "dotnet pack --no-build --verbosity n"
Write-Host $cmdline
CMD.exe /c $cmdline

Write-Host "-Upload Package to Nuget-"

# Push to Nuget if uploadToNuget is true
if($isUploadToNuget){	
	# CMD to Push Nuget
	$cmdline = "dotnet nuget push .\bin\Release\IdeaActivator.Common.Core.$version.nupkg --api-key $nugetApiKey --source https://api.nuget.org/v3/index.json"
	Write-Host $cmdline
	CMD.exe /c $cmdline
}else{
	Write-Host "Package is not uploaded to Nuget."
}

# Clean up
Remove-Item ".\$nugetSettingFileName"