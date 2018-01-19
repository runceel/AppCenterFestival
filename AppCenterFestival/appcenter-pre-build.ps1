$path = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $path

Get-Content "KeySample.cs" | 
    foreach { $_ -creplace "Keys_Sample", "Keys" } | 
    foreach { $_ -creplace "$APP_CENTER_KEY$", $env:APP_CENTER_KEY } |
    Out-File "Keys.cs" -Encoding "UTF-8" -Append
