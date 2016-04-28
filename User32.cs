using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ConsoleDA
{
    internal static class User32
    {
        private const int SW_SHOWNORMAL = 1;

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);


        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);


        [System.Runtime.InteropServices.DllImport("user32.dll")]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        static extern bool ShowWindow(IntPtr hWnd, ShowWindowEnum flags);
        private enum ShowWindowEnum
        {
            Hide = 0,
            ShowNormal = 1, ShowMinimized = 2, ShowMaximized = 3,
            Maximize = 3, ShowNormalNoActivate = 4, Show = 5,
            Minimize = 6, ShowMinNoActivate = 7, ShowNoActivate = 8,
            Restore = 9, ShowDefault = 10, ForceMinimized = 11
        };

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SetActiveWindow(int hwnd);
        public static void BringWindowToFront(int processid)
        {
            //get the process
            System.Diagnostics.Process bProcess = System.Diagnostics.Process.GetProcessById(processid);// FirstOrDefault<System.Diagnostics.Process>;
            //check if the process is nothing or not.
            if (bProcess != null)
            {
                //get the (int) hWnd of the process
                IntPtr hwnd = bProcess.MainWindowHandle;
                //check if its nothing
                if (hwnd != IntPtr.Zero)
                {
                    //if the handle is other than 0, then set the active window
                    bProcess.WaitForInputIdle();
                    SetForegroundWindow(hwnd);
                }
                else
                {
                    //we can assume that it is fully hidden or minimized, so lets show it!
                    bProcess.WaitForInputIdle();
                    ShowWindow(hwnd, ShowWindowEnum.Restore);
                    SetForegroundWindow(hwnd);
                }
            }
            else
            {
                //tthe process is nothing, so start it
                //System.Diagnostics.Process.Start(@"C:\Program Files\B\B.exe");
            }
        }



        public static bool SetWindow(IntPtr hwnd)
        {
            return User32.SetForegroundWindow(hwnd);
        }

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        public static void sendKeystroke(ushort k, int processID)
        {
            const uint WM_KEYDOWN = 0x100;
            const uint WM_SYSCOMMAND = 0x018;
            const uint SC_CLOSE = 0x053;

            System.Diagnostics.Process bProcess = System.Diagnostics.Process.GetProcessById(processID);
             IntPtr hwnd = bProcess.MainWindowHandle;
            //IntPtr WindowToFind = FindWindow(null, "Untitled1 - Notepad++");

            IntPtr result3 = SendMessage(hwnd, WM_KEYDOWN, ((IntPtr)k), (IntPtr)0);
            //IntPtr result3 = SendMessage(WindowToFind, WM_KEYUP, ((IntPtr)c), (IntPtr)0);
        }

    }
}
