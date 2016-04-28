using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace ConsoleDA
{
    public partial class ConsoleView : Form
    {
        public Client clientless;
        public ConsoleView(Client clientless)
        {
            this.clientless = clientless;
            InitializeComponent();
            this.DAConsoleTextConsole.Width = this.Width;
            this.DAConsoleTextConsole.Height = (this.Height / 3) * 2;
            this.DAConsoleTextConsole.Location = new System.Drawing.Point(0, 0);
            this.DAConsoleRichTextInput.Width = this.Width;
            this.DAConsoleRichTextInput.Height = (this.Height / 11);
            this.DAConsoleRichTextInput.Location = new System.Drawing.Point(0, (this.Height / 3 * 2));
            if (InvokeRequired)
                this.Invoke(new MethodInvoker(() => this.Show()));
            else
                this.Show();
        }
        public ConsoleView(string username, string password)
        {
            InitializeComponent();
            this.DAConsoleTextConsole.Width = this.Width;
            this.DAConsoleTextConsole.Height = (this.Height / 3) * 2;
            this.DAConsoleTextConsole.Location = new System.Drawing.Point(0, 0);
            this.DAConsoleRichTextInput.Width = this.Width;
            this.DAConsoleRichTextInput.Height = (this.Height / 11);
            this.DAConsoleRichTextInput.Location = new System.Drawing.Point(0,(this.Height/3*2));
            if (InvokeRequired)
                this.Invoke(new MethodInvoker(() => this.Show()));
            else
                this.Show();
        }

        public void WriteLine(string text) {
            if(InvokeRequired)
                Invoke(new MethodInvoker(() => { this.DAConsoleTextConsole.Items.Add(text); }));
            else
                this.DAConsoleTextConsole.Items.Add(text);
        }

        private void DAConsoleRichTextInput_TextChanged(Object sender, EventArgs e) 
        {
            
        }

        private void DAConsoleRichTextConsole_KeyDown(Object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                        this.DAConsoleTextConsole.Items.Add(this.DAConsoleRichTextInput.Text) ;
                        this.DAConsoleRichTextInput.Clear();
                      break;
                default:
                    break;
            }
        }
    }
}