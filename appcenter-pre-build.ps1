$path = Split-Path -Parent $MyInvocation.MyCommand.Path
Join-Path -Path $path -ChildPath "AppCenterFestival" | Set-Location

if (Test-Path "Keys.cs") {
    Remove-Item "Keys.cs"
}

Get-Content "KeysSample.cs" | 
    foreach { $_ -creplace "Keys_Sample", "Keys" } | 
    foreach { $_ -creplace "#APP_CENTER_KEY#", $env:APP_CENTER_KEY } |
    Out-File "Keys.cs" -Encoding "utf8" -Append

Set-Location $path
