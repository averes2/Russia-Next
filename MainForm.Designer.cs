namespace ConsoleDA
{
    partial class MainForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.launchDarkAgesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.launchClientlessDAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.toolStripTextBoxClientlessUsername = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripTextBoxClientlessPassword = new System.Windows.Forms.ToolStripTextBox();
            this.launchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(809, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.launchDarkAgesToolStripMenuItem,
            this.launchClientlessDAToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // launchDarkAgesToolStripMenuItem
            // 
            this.launchDarkAgesToolStripMenuItem.Name = "launchDarkAgesToolStripMenuItem";
            this.launchDarkAgesToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.launchDarkAgesToolStripMenuItem.Text = "Launch Dark Ages";
            this.launchDarkAgesToolStripMenuItem.Click += new System.EventHandler(this.launchDarkAgesToolStripMenuItem_Click);
            // 
            // launchClientlessDAToolStripMenuItem
            // 
            this.launchClientlessDAToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBoxClientlessUsername,
            this.toolStripTextBoxClientlessPassword,
            this.launchToolStripMenuItem});
            this.launchClientlessDAToolStripMenuItem.Name = "launchClientlessDAToolStripMenuItem";
            this.launchClientlessDAToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.launchClientlessDAToolStripMenuItem.Text = "Launch Clientless DA";
            this.launchClientlessDAToolStripMenuItem.Click += new System.EventHandler(this.launchClientlessDAToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(182, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // mainTabControl
            // 
            this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabControl.Location = new System.Drawing.Point(0, 24);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(809, 485);
            this.mainTabControl.TabIndex = 2;
            // 
            // toolStripTextBoxClientlessUsername
            // 
            this.toolStripTextBoxClientlessUsername.Name = "toolStripTextBoxClientlessUsername";
            this.toolStripTextBoxClientlessUsername.Size = new System.Drawing.Size(100, 23);
            // 
            // toolStripTextBoxClientlessPassword
            // 
            this.toolStripTextBoxClientlessPassword.Name = "toolStripTextBoxClientlessPassword";
            this.toolStripTextBoxClientlessPassword.Size = new System.Drawing.Size(100, 23);
            // 
            // launchToolStripMenuItem
            // 
            this.launchToolStripMenuItem.Name = "launchToolStripMenuItem";
            this.launchToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.launchToolStripMenuItem.Text = "Launch";
            this.launchToolStripMenuItem.Click += new System.EventHandler(this.launchToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(809, 509);
            this.Controls.Add(this.mainTabControl);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Russia (Credits to Dean, Acht, and biomagus) and some other people!";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem launchDarkAgesToolStripMenuItem;
        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem launchClientlessDAToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxClientlessUsername;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxClientlessPassword;
        private System.Windows.Forms.ToolStripMenuItem launchToolStripMenuItem;

    }
}

