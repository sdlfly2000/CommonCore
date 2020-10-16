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
$cmdline = "dotnet nuget push .\Common.Core\bin\Debug\IdeaActivator.Common.Core.$version.nupkg -k oy2niutbai55vj2eic7q3jem3xlmvdnenlm7iz2eg3evli --source https://api.nuget.org/v3/index.json"
Write-Host $cmdline
#CMD.exe /c $cmdline

