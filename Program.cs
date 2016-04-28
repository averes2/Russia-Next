using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace ConsoleDA
{
    public static class Program
    {
        public static object SyncObj = new object();
        public static Dictionary<string, string> Vault = new Dictionary<string, string>();

        public static string StartupPath { get; set; }

        [STAThread]
        static void Main()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            StartupPath = Application.StartupPath;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }


        public static string GetHashString(string value)
        {
            return BitConverter.ToString(MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(value))).Replace("-", string.Empty).ToLower();
        }
       /* public static string GetHashString(string value)
        {
            var md5 = MD5.Create();
            var buffer = Encoding.ASCII.GetBytes(value);
            var hash = md5.ComputeHash(buffer);
            return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
        }*/
    }
}