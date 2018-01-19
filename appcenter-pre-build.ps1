$path = Join-Path -Path $env:APPCENTER_SOURCE_DIRECTORY -ChildPath "AppCenterFestival"
$inputFile = Join-Path -Path $path -ChildPath "KeysSample.cs"
$outputFile = Join-Path -Path $path -ChildPath "Keys.cs"


if (Test-Path $outputFile) {
    Remove-Item $outputFile
}

Get-Content $inputFile | 
    foreach { $_ -creplace "Keys_Sample", "Keys" } | 
    foreach { $_ -creplace "#APP_CENTER_KEY#", $env:APP_CENTER_KEY } |
    Out-File $outputFile -Encoding "utf8" -Append
