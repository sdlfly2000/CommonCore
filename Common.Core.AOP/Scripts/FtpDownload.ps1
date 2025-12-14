# Parameters
param(
	[string]$tempDirectory = ".",
	[string]$username = "sdlfly2000",
	[string]$password = "sdl@1215",
	[string]$url = "ftp://homeserver2/Projects/Configurations",
	[string]$configFileName = "app.json"
)

# Download file
$webclient = New-Object -TypeName System.Net.WebClient
$webclient.Credentials = New-Object System.Net.NetworkCredential($username,$password)

$uri = New-Object System.Uri("$url/$configFileName")

Write-Host "****************************************"
Write-Host "url : $url"
Write-Host "configFileName : $configFileName"
Write-Host "****************************************"

Write-Host "Download File $url/$configFileName to $tempDirectory"
$webclient.DownloadFile($uri, "$tempDirectory/$configFileName")

$webclient.Dispose()
