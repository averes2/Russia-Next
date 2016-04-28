﻿using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ConsoleDA
{
    public class ProcessMemoryStream : Stream, IDisposable
    {
        private ProcessAccess access;
        private bool disposed;
        public IntPtr hProcess;

        public ProcessMemoryStream(int processId, ProcessAccess access)
        {
            this.access = access;
            this.ProcessId = processId;
            this.hProcess = Kernel32.OpenProcess(access, false, processId);
            if (this.hProcess == IntPtr.Zero)
            {
                throw new ArgumentException("Unable to open the process.");
            }
        }
        ~ProcessMemoryStream()
        {
            this.Dispose(false);
        }

        public override void Flush()
        {
            throw new NotSupportedException("Flush is not supported.");
        }
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("ProcessMemoryStream");
            }
            if (this.hProcess == IntPtr.Zero)
            {
                throw new InvalidOperationException("Process is not open.");
            }
            IntPtr ptr = Marshal.AllocHGlobal(count);
            if (ptr == IntPtr.Zero)
            {
                throw new InvalidOperationException("Unable to allocate memory.");
            }
            int bytesRead = 0;
            Kernel32.ReadProcessMemory(this.hProcess, (IntPtr)this.Position, ptr, count, out bytesRead);
            this.Position += bytesRead;
            Marshal.Copy(ptr, buffer, offset, count);
            Marshal.FreeHGlobal(ptr);
            return bytesRead;
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("ProcessMemoryStream");
            }
            switch (origin)
            {
                case SeekOrigin.Begin:
                    this.Position = offset;
                    break;

                case SeekOrigin.Current:
                    this.Position += offset;
                    break;

                case SeekOrigin.End:
                    throw new NotSupportedException("SeekOrigin.End not supported.");
            }
            return this.Position;
        }
        public override void SetLength(long value)
        {
            throw new NotSupportedException("Cannot set the length for this stream.");
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("ProcessMemoryStream");
            }
            if (this.hProcess == IntPtr.Zero)
            {
                throw new InvalidOperationException("Process is not open.");
            }
            IntPtr destination = Marshal.AllocHGlobal(count);
            if (destination == IntPtr.Zero)
            {
                throw new InvalidOperationException("Unable to allocate memory.");
            }
            Marshal.Copy(buffer, offset, destination, count);
            int bytesWritten = 0;
            Kernel32.WriteProcessMemory(this.hProcess, (IntPtr)this.Position, destination, count, out bytesWritten);
            this.Position += bytesWritten;
            Marshal.FreeHGlobal(destination);
        }
        public override void WriteByte(byte value)
        {
            this.Write(new byte[] { value }, 0, 1);
        }
        public override void Close()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("ProcessMemoryStream");
            }
            if (this.hProcess != IntPtr.Zero)
            {
                Kernel32.CloseHandle(this.hProcess);
                this.hProcess = IntPtr.Zero;
            }
            base.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (this.hProcess != IntPtr.Zero)
                {
                    Kernel32.CloseHandle(this.hProcess);
                    this.hProcess = IntPtr.Zero;
                }
                base.Dispose(disposing);
            }
            this.disposed = true;
        }

        public override bool CanRead
        {
            get
            {
                return ((this.access & ProcessAccess.VmRead) > ProcessAccess.None);
            }
        }
        public override bool CanSeek
        {
            get
            {
                return true;
            }
        }
        public override bool CanWrite
        {
            get
            {
                return ((this.access & (ProcessAccess.VmWrite | ProcessAccess.VmOperation)) > ProcessAccess.None);
            }
        }
        public override long Length
        {
            get
            {
                throw new NotSupportedException("Length is not supported.");
            }
        }
        public override long Position { get; set; }
        public int ProcessId { get; set; }
    }

    [Flags]
    public enum ProcessAccess
    {
        None = 0x00,
        Terminate = 0x01,
        CreateThread = 0x02,
        VmOperation = 0x08,
        VmRead = 0x10,
        VmWrite = 0x20,
        DuplicateHandle = 0x40,
        CreateProcess = 0x80,
        SetQuota = 0x100,
        SetInformation = 0x200,
        QueryInformation = 0x400,
        SuspendResume = 0x800,
        QueryLimitedInformation = 0x1000
    }

    [Flags]
    public enum ProcessCreationFlags
    {
        DebugProcess = 0x01,
        DebugOnlyThisProcess = 0x02,
        Suspended = 0x04,
        DetachedProcess = 0x08,
        NewConsole = 0x10,
        NewProcessGroup = 0x200,
        UnicodeEnvironment = 0x400,
        SeparateWowVdm = 0x800,
        SharedWowVdm = 0x1000,
        InheritParentAffinity = 0x1000,
        ProtectedProcess = 0x40000,
        ExtendedStartupInfoPresent = 0x80000,
        BreakawayFromJob = 0x1000000,
        PreserveCodeAuthZLevel = 0x2000000,
        DefaultErrorMode = 0x4000000,
        NoWindow = 0x8000000
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct StartupInfo
    {
        public int Size { get; set; }
        public string Reserved { get; set; }
        public string Desktop { get; set; }
        public string Title { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int XCountChars { get; set; }
        public int YCountChars { get; set; }
        public int FillAttribute { get; set; }
        public int Flags { get; set; }
        public short ShowWindow { get; set; }
        public short Reserved2 { get; set; }
        public IntPtr Reserved3 { get; set; }
        public IntPtr StdInputHandle { get; set; }
        public IntPtr StdOutputHandle { get; set; }
        public IntPtr StdErrorHandle { get; set; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ProcessInformation
    {
        public IntPtr ProcessHandle { get; set; }
        public IntPtr ThreadHandle { get; set; }
        public int ProcessId { get; set; }
        public int ThreadId { get; set; }
    }

    public enum WaitEventResult
    {
        Signaled = 0x00,
        Abandoned = 0x80,
        Timeout = 0x102
    }
}