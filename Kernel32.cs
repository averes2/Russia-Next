// Decompiled with JetBrains decompiler
// Type: s2.Kernel32
// Assembly: s2, Version=1.0.1.3, Culture=neutral, PublicKeyToken=null
// MVID: 43510899-C91A-4623-9F79-2D24FD18A057
// Assembly location: C:\Users\GOON\Downloads\s2(1)\s2.exe

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ConsoleDA
{
    internal static class Kernel32
    {
        internal const int WM_HOTKEY = 786;

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll")]
        public static extern bool CreateProcess(string applicationName, string commandLine, IntPtr processAttributes, IntPtr threadAttributes, bool inheritHandles, ProcessCreationFlags creationFlags, IntPtr environment, string currentDirectory, ref StartupInfo startupInfo, out ProcessInformation processInfo);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(ProcessAccess access, bool inheritHandle, int processId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr baseAddress, IntPtr buffer, int count, out int bytesRead);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, int lpBaseAddress, [Out] byte[] lpBuffer, int nSize, byte lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern int ResumeThread(IntPtr hThread);

        [DllImport("kernel32.dll")]
        public static extern WaitEventResult WaitForSingleObject(IntPtr hObject, int timeout);

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr baseAddress, IntPtr buffer, int count, out int bytesWritten);

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(IntPtr hProcess, int lpBaseAddress, byte[] lpBuffer, int nSize, byte lpNumberOfBytesWritten);

        public static bool Peek(Process proc, int target, byte[] data)
        {
            return Kernel32.ReadProcessMemory(proc.Handle, target, data, data.Length, (byte)0);
        }

        public static bool Poke(Process proc, int target, byte[] data)
        {
            return Kernel32.WriteProcessMemory(proc.Handle, target, data, data.Length, (byte)0);
        }
    }
}
