##############################################################
# Push to Nuget.
##############################################################

# No Parameter
Param()

# Variables
$appSettingFileName = "appsetting.json"
$appSetting = (Get-Content -path $appSettingFileName) | ConvertFrom-Json
$uploadToNuget = [bool]$appSetting.UploadToNuget

if($uploadToNuget){
	$projectFileName = "Common.Core.csproj"
	$projectFile = [xml](Get-Content -path $projectFileName)
	$version = $projectFile.Project.PropertyGroup.Version

	# CMD to Push Nuget
	Write-Host $cmdline
	#CMD.exe /c $cmdline
}



