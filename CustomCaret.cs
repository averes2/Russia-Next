using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
namespace ConsoleDA
{
    class CustomCaret : NativeWindow
    {
        const int WM_USER = 0x0400;
        const int WM_NOTIFY = 0x004E;
        const int WM_REFLECT = WM_USER + 0x1C00;
        const int WM_PAINT = 0xF;
        Bitmap myCaret;
        Assembly _assembly;
        Stream _imageStream;

        [DllImport("user32.dll")]
        static extern bool CreateCaret(IntPtr hWnd, IntPtr hBitmap, int nWidth, int nHeight);
        [DllImport("user32.dll")]
        static extern bool ShowCaret(IntPtr hWnd);

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if ((m.Msg == (WM_REFLECT + WM_NOTIFY)) || (m.Msg == WM_PAINT))
            {
                CreateCaret(this.Handle, myCaret.GetHbitmap(), 0, 0);
                ShowCaret(this.Handle);
            }
        }

        public CustomCaret(RichTextBox CallingTextBox)
        {
            this.AssignHandle(CallingTextBox.Handle);
            _assembly = Assembly.GetExecutingAssembly();
            this._imageStream = _assembly.GetManifestResourceStream("DAConsole.Properties.Resources.customCARROT.bmp");
            
            using (Bitmap graphics = Resources.customCARROT)
            {
                    myCaret = new Bitmap(graphics);
            }
        }
    }
}