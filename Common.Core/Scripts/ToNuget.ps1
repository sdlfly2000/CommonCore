##############################################################
# Push to Nuget.
##############################################################

# No Parameter
Param()

# Variables


$projectFileName = "Common.Core.csproj"
$projectFile = [xml](Get-Content -path $projectFileName)
$version = $projectFile.Project.PropertyGroup.Version

# CMD to Push Nuget
Write-Host $cmdline
#CMD.exe /c $cmdline

