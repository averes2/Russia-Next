namespace ConsoleDA
{
    partial class ConsoleView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.DAConsoleTextConsole = new System.Windows.Forms.ListBox();
            this.DAConsoleRichTextInput = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // DAConsoleTextConsole
            // 
            this.DAConsoleTextConsole.BackColor = System.Drawing.SystemColors.MenuText;
            this.DAConsoleTextConsole.ForeColor = System.Drawing.Color.Lime;
            this.DAConsoleTextConsole.FormattingEnabled = true;
            this.DAConsoleTextConsole.Location = new System.Drawing.Point(12, 12);
            this.DAConsoleTextConsole.Name = "DAConsoleTextConsole";
            this.DAConsoleTextConsole.Size = new System.Drawing.Size(142, 95);
            this.DAConsoleTextConsole.TabIndex = 0;
            // 
            // DAConsoleRichTextInput
            // 
            this.DAConsoleRichTextInput.BackColor = System.Drawing.SystemColors.InfoText;
            this.DAConsoleRichTextInput.ForeColor = System.Drawing.Color.Lime;
            this.DAConsoleRichTextInput.Location = new System.Drawing.Point(12, 113);
            this.DAConsoleRichTextInput.Multiline = false;
            this.DAConsoleRichTextInput.Name = "DAConsoleRichTextInput";
            this.DAConsoleRichTextInput.Size = new System.Drawing.Size(142, 31);
            this.DAConsoleRichTextInput.TabIndex = 1;
            this.DAConsoleRichTextInput.Text = "";
            this.DAConsoleRichTextInput.TextChanged += new System.EventHandler(this.DAConsoleRichTextInput_TextChanged);
            this.DAConsoleRichTextInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DAConsoleRichTextConsole_KeyDown);
            // 
            // ConsoleView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(613, 277);
            this.Controls.Add(this.DAConsoleRichTextInput);
            this.Controls.Add(this.DAConsoleTextConsole);
            this.Name = "ConsoleView";
            this.Text = "ConsoleView";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox DAConsoleRichTextInput;
        public System.Windows.Forms.ListBox DAConsoleTextConsole;



    }
}