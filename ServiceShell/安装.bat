%SystemRoot%\Microsoft.NET\Framework\v2.0.50727\installutil.exe ServiceShell.exe
Net Start ServiceShell
sc config ServiceShell start= auto
pause