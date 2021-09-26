# CVE-2021-1675
C# PrintNightmare for .NET Framework 3.5  

This is a "bring your own dll" project. So once you have that, just build and specify the dll in the command-line arguments.

## Build  
You can build yourself with `C:\Windows\Microsoft.NET\Framework64\v3.5\csc.exe -out:SharpPN.exe C:\Path\to\Program.cs` or by opening the .sln file with Visual Studio.

## Usage  
`.\SharpPN.exe -DLL C:\path\to\your\dll`  

### References  
CalebStewart's PowerShell Implementation: https://github.com/calebstewart/CVE-2021-1675
