using BuildingXYAdj_Concept;
using devDept.Eyeshot;
using devDept.Eyeshot.Labels;
using devDept.Geometry;
using DevExpress.ExpressApp.Win.Templates.Controls;
using DevExpress.Utils.Menu;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraEditors;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Configuration;

namespace ConsoleDA
{
    partial class ClientTab
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
            this.textConsoleOutput = new System.Windows.Forms.RichTextBox();
            this.packetTab = new System.Windows.Forms.Panel();
            this.textConsoleInput = new System.Windows.Forms.RichTextBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.buttonRecv = new System.Windows.Forms.Button();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.checkRecv = new System.Windows.Forms.ToolStripButton();
            this.checkSend = new System.Windows.Forms.ToolStripButton();
            this.controlTabs = new System.Windows.Forms.TabControl();
            this.mainControl = new System.Windows.Forms.TabPage();
            this.controlLaunchWatcher = new System.Windows.Forms.Button();
            this.controlRunTasks = new System.Windows.Forms.ListBox();
            this.unusedcontrol = new System.Windows.Forms.Button();
            this.xtraUserControl1 = new DevExpress.XtraEditors.XtraUserControl();
            this.controlEditWaypoint = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.controlCurrentMapLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.controlMapWaypoints = new System.Windows.Forms.ListBox();
            this.controlConsolePassword = new System.Windows.Forms.TextBox();
            this.controlConsoleUsername = new System.Windows.Forms.TextBox();
            this.controlConsoleDisconnect = new System.Windows.Forms.Button();
            this.controlConsoleConnect = new System.Windows.Forms.Button();
            this.controlManaText = new System.Windows.Forms.Label();
            this.controlHealthText = new System.Windows.Forms.Label();
            this.controlManaLabel = new System.Windows.Forms.Label();
            this.controlHealthLabel = new System.Windows.Forms.Label();
            this.controlManaBar = new System.Windows.Forms.ProgressBar();
            this.controlHealthBar = new System.Windows.Forms.ProgressBar();
            this.packetControl = new System.Windows.Forms.TabPage();
            this.pathControl = new System.Windows.Forms.TabPage();
            this.pathingSpeedControlBox = new System.Windows.Forms.GroupBox();
            this.pathWalkingDefaults = new System.Windows.Forms.Label();
            this.pathMsDelay = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pathingSpeedControl = new System.Windows.Forms.TrackBar();
            this.pathingSpeedType = new System.Windows.Forms.ComboBox();
            this.pathingSpeedControlLabel = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pathingWaypointsList = new System.Windows.Forms.ComboBox();
            this.pathingWaypoints = new System.Windows.Forms.CheckBox();
            this.pathingWaypointUse = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pathingMapList = new System.Windows.Forms.ComboBox();
            this.pathingMap = new System.Windows.Forms.CheckBox();
            this.pathingMapGoto = new System.Windows.Forms.Label();
            this.groupBoxPathFollow = new System.Windows.Forms.GroupBox();
            this.pathFollowDistance = new System.Windows.Forms.NumericUpDown();
            this.pathFollow = new System.Windows.Forms.CheckBox();
            this.pathFollowTarget = new System.Windows.Forms.TextBox();
            this.pathFollowTargetLabel = new System.Windows.Forms.Label();
            this.pathDynamic = new System.Windows.Forms.RadioButton();
            this.pathSingle = new System.Windows.Forms.RadioButton();
            this.pathNone = new System.Windows.Forms.RadioButton();
            this.mapControl = new System.Windows.Forms.TabPage();
            this.mapControlBack = new System.Windows.Forms.Panel();
            this.mapResumeDynamicMap = new System.Windows.Forms.Button();
            this.mapPauseDynamicMap = new System.Windows.Forms.Button();
            this.mapDynamicScrollBarControl = new DevExpress.XtraEditors.XtraScrollableControl();
            this.mapDynamicView = new DevExpress.XtraEditors.PictureEdit();
            this.mapResumeWaypoints = new System.Windows.Forms.Button();
            this.mapPauseWaypoints = new System.Windows.Forms.Button();
            this.mapWaypointsPath = new System.Windows.Forms.Button();
            this.pathAddNone = new System.Windows.Forms.RadioButton();
            this.mapAddDoors = new System.Windows.Forms.RadioButton();
            this.mapAddBlocks = new System.Windows.Forms.RadioButton();
            this.mapAddWaypoints = new System.Windows.Forms.RadioButton();
            this.mapEditOptionsGroup = new DevExpress.XtraEditors.RadioGroup();
            this.mapListGotoCurrent = new System.Windows.Forms.Button();
            this.mapDimList = new System.Windows.Forms.ListBox();
            this.listBoxWorldMaps = new System.Windows.Forms.ListBox();
            this.textBoxMapAxis = new System.Windows.Forms.TextBox();
            this.textBoxWarpPortalLocs = new System.Windows.Forms.TextBox();
            this.textBoxMapNum = new System.Windows.Forms.TextBox();
            this.textBoxMapFolder = new System.Windows.Forms.TextBox();
            this.controlSpells = new System.Windows.Forms.TabPage();
            this.spellsKnownSpellsList = new System.Windows.Forms.ListBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.clientStart = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.clientStop = new System.Windows.Forms.Button();
            this.view3DTab = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.controlChatText = new System.Windows.Forms.RichTextBox();
            this.controlChatTextBox = new System.Windows.Forms.RichTextBox();
            this.controlChatSend = new System.Windows.Forms.Button();
            this.packetTab.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.controlTabs.SuspendLayout();
            this.mainControl.SuspendLayout();
            this.packetControl.SuspendLayout();
            this.pathControl.SuspendLayout();
            this.pathingSpeedControlBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pathingSpeedControl)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBoxPathFollow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pathFollowDistance)).BeginInit();
            this.mapControl.SuspendLayout();
            this.mapControlBack.SuspendLayout();
            this.mapDynamicScrollBarControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mapDynamicView.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapEditOptionsGroup.Properties)).BeginInit();
            this.controlSpells.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textConsoleOutput
            // 
            this.textConsoleOutput.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textConsoleOutput.Location = new System.Drawing.Point(3, 3);
            this.textConsoleOutput.Name = "textConsoleOutput";
            this.textConsoleOutput.Size = new System.Drawing.Size(735, 285);
            this.textConsoleOutput.TabIndex = 3;
            this.textConsoleOutput.Text = "";
            // 
            // packetTab
            // 
            this.packetTab.Controls.Add(this.textConsoleInput);
            this.packetTab.Controls.Add(this.buttonSend);
            this.packetTab.Controls.Add(this.buttonRecv);
            this.packetTab.Controls.Add(this.toolStrip2);
            this.packetTab.Location = new System.Drawing.Point(3, 290);
            this.packetTab.Name = "packetTab";
            this.packetTab.Size = new System.Drawing.Size(735, 75);
            this.packetTab.TabIndex = 3;
            this.packetTab.Tag = "packets";
            // 
            // textConsoleInput
            // 
            this.textConsoleInput.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textConsoleInput.Location = new System.Drawing.Point(0, 25);
            this.textConsoleInput.Name = "textConsoleInput";
            this.textConsoleInput.Size = new System.Drawing.Size(591, 50);
            this.textConsoleInput.TabIndex = 1;
            this.textConsoleInput.Text = "";
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(591, 25);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(69, 50);
            this.buttonSend.TabIndex = 2;
            this.buttonSend.Text = "Send";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // buttonRecv
            // 
            this.buttonRecv.Location = new System.Drawing.Point(660, 25);
            this.buttonRecv.Name = "buttonRecv";
            this.buttonRecv.Size = new System.Drawing.Size(75, 50);
            this.buttonRecv.TabIndex = 3;
            this.buttonRecv.Text = "Recv";
            this.buttonRecv.UseVisualStyleBackColor = true;
            this.buttonRecv.Click += new System.EventHandler(this.buttonRecv_Click);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkRecv,
            this.checkSend});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(735, 25);
            this.toolStrip2.TabIndex = 2;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // checkRecv
            // 
            this.checkRecv.CheckOnClick = true;
            this.checkRecv.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.checkRecv.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.checkRecv.Name = "checkRecv";
            this.checkRecv.Size = new System.Drawing.Size(105, 22);
            this.checkRecv.Text = "Incoming Packets";
            // 
            // checkSend
            // 
            this.checkSend.CheckOnClick = true;
            this.checkSend.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.checkSend.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.checkSend.Name = "checkSend";
            this.checkSend.Size = new System.Drawing.Size(105, 22);
            this.checkSend.Text = "Outgoing Packets";
            // 
            // controlTabs
            // 
            this.controlTabs.Controls.Add(this.mainControl);
            this.controlTabs.Controls.Add(this.packetControl);
            this.controlTabs.Controls.Add(this.pathControl);
            this.controlTabs.Controls.Add(this.mapControl);
            this.controlTabs.Controls.Add(this.controlSpells);
            this.controlTabs.Controls.Add(this.tabPage1);
            this.controlTabs.Location = new System.Drawing.Point(0, 40);
            this.controlTabs.Name = "controlTabs";
            this.controlTabs.SelectedIndex = 0;
            this.controlTabs.Size = new System.Drawing.Size(749, 394);
            this.controlTabs.TabIndex = 0;
            // 
            // mainControl
            // 
            this.mainControl.Controls.Add(this.controlLaunchWatcher);
            this.mainControl.Controls.Add(this.controlRunTasks);
            this.mainControl.Controls.Add(this.unusedcontrol);
            this.mainControl.Controls.Add(this.xtraUserControl1);
            this.mainControl.Controls.Add(this.controlEditWaypoint);
            this.mainControl.Controls.Add(this.label4);
            this.mainControl.Controls.Add(this.controlCurrentMapLabel);
            this.mainControl.Controls.Add(this.label2);
            this.mainControl.Controls.Add(this.controlMapWaypoints);
            this.mainControl.Controls.Add(this.controlConsolePassword);
            this.mainControl.Controls.Add(this.controlConsoleUsername);
            this.mainControl.Controls.Add(this.controlConsoleDisconnect);
            this.mainControl.Controls.Add(this.controlConsoleConnect);
            this.mainControl.Controls.Add(this.controlManaText);
            this.mainControl.Controls.Add(this.controlHealthText);
            this.mainControl.Controls.Add(this.controlManaLabel);
            this.mainControl.Controls.Add(this.controlHealthLabel);
            this.mainControl.Controls.Add(this.controlManaBar);
            this.mainControl.Controls.Add(this.controlHealthBar);
            this.mainControl.Location = new System.Drawing.Point(4, 24);
            this.mainControl.Name = "mainControl";
            this.mainControl.Padding = new System.Windows.Forms.Padding(3);
            this.mainControl.Size = new System.Drawing.Size(741, 366);
            this.mainControl.TabIndex = 1;
            this.mainControl.Text = "Control";
            this.mainControl.UseVisualStyleBackColor = true;
            // 
            // controlLaunchWatcher
            // 
            this.controlLaunchWatcher.Location = new System.Drawing.Point(413, 136);
            this.controlLaunchWatcher.Name = "controlLaunchWatcher";
            this.controlLaunchWatcher.Size = new System.Drawing.Size(115, 23);
            this.controlLaunchWatcher.TabIndex = 18;
            this.controlLaunchWatcher.Text = "Launch Watcher";
            this.controlLaunchWatcher.UseVisualStyleBackColor = true;
            this.controlLaunchWatcher.Click += new System.EventHandler(this.controlLaunchWatcher_Click);
            // 
            // controlRunTasks
            // 
            this.controlRunTasks.FormattingEnabled = true;
            this.controlRunTasks.ItemHeight = 15;
            this.controlRunTasks.Items.AddRange(new object[] {
            "No Thread",
            "Primary Hider Thread",
            "Primary Banker Thread"});
            this.controlRunTasks.Location = new System.Drawing.Point(165, 110);
            this.controlRunTasks.Name = "controlRunTasks";
            this.controlRunTasks.Size = new System.Drawing.Size(100, 49);
            this.controlRunTasks.TabIndex = 17;
            this.controlRunTasks.SelectedIndexChanged += new System.EventHandler(this.controlRunTasks_SelectedIndexChanged);
            // 
            // unusedcontrol
            // 
            this.unusedcontrol.Location = new System.Drawing.Point(18, 184);
            this.unusedcontrol.Name = "unusedcontrol";
            this.unusedcontrol.Size = new System.Drawing.Size(115, 23);
            this.unusedcontrol.TabIndex = 16;
            this.unusedcontrol.Text = "Unused";
            this.unusedcontrol.UseVisualStyleBackColor = true;
            this.unusedcontrol.Click += new System.EventHandler(this.unusedfunction);
            // 
            // xtraUserControl1
            // 
            this.xtraUserControl1.Location = new System.Drawing.Point(704, 6);
            this.xtraUserControl1.Name = "xtraUserControl1";
            this.xtraUserControl1.Size = new System.Drawing.Size(23, 19);
            this.xtraUserControl1.TabIndex = 15;
            this.xtraUserControl1.Load += new System.EventHandler(this.clientTab_Load);
            // 
            // controlEditWaypoint
            // 
            this.controlEditWaypoint.Location = new System.Drawing.Point(80, 67);
            this.controlEditWaypoint.Name = "controlEditWaypoint";
            this.controlEditWaypoint.Size = new System.Drawing.Size(100, 23);
            this.controlEditWaypoint.TabIndex = 14;
            this.controlEditWaypoint.Text = "Change Waypoint";
            this.controlEditWaypoint.TextChanged += new System.EventHandler(this.controlEditWaypoint_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 15);
            this.label4.TabIndex = 13;
            this.label4.Text = "Waypoints";
            // 
            // controlCurrentMapLabel
            // 
            this.controlCurrentMapLabel.AutoSize = true;
            this.controlCurrentMapLabel.Location = new System.Drawing.Point(11, 92);
            this.controlCurrentMapLabel.Name = "controlCurrentMapLabel";
            this.controlCurrentMapLabel.Size = new System.Drawing.Size(77, 15);
            this.controlCurrentMapLabel.TabIndex = 12;
            this.controlCurrentMapLabel.Text = "Current Map:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 15);
            this.label2.TabIndex = 11;
            this.label2.Text = "label2";
            // 
            // controlMapWaypoints
            // 
            this.controlMapWaypoints.FormattingEnabled = true;
            this.controlMapWaypoints.ItemHeight = 15;
            this.controlMapWaypoints.Location = new System.Drawing.Point(14, 110);
            this.controlMapWaypoints.Name = "controlMapWaypoints";
            this.controlMapWaypoints.Size = new System.Drawing.Size(45, 49);
            this.controlMapWaypoints.TabIndex = 10;
            this.controlMapWaypoints.SelectedIndexChanged += new System.EventHandler(this.controlMapWaypoints_SelectedIndexChanged);
            // 
            // controlConsolePassword
            // 
            this.controlConsolePassword.Location = new System.Drawing.Point(489, 48);
            this.controlConsolePassword.Name = "controlConsolePassword";
            this.controlConsolePassword.Size = new System.Drawing.Size(100, 23);
            this.controlConsolePassword.TabIndex = 9;
            this.controlConsolePassword.Text = "Password";
            // 
            // controlConsoleUsername
            // 
            this.controlConsoleUsername.Location = new System.Drawing.Point(348, 48);
            this.controlConsoleUsername.Name = "controlConsoleUsername";
            this.controlConsoleUsername.Size = new System.Drawing.Size(100, 23);
            this.controlConsoleUsername.TabIndex = 8;
            this.controlConsoleUsername.Text = "Username";
            // 
            // controlConsoleDisconnect
            // 
            this.controlConsoleDisconnect.Location = new System.Drawing.Point(489, 89);
            this.controlConsoleDisconnect.Name = "controlConsoleDisconnect";
            this.controlConsoleDisconnect.Size = new System.Drawing.Size(75, 23);
            this.controlConsoleDisconnect.TabIndex = 7;
            this.controlConsoleDisconnect.Text = "Disconnect";
            this.controlConsoleDisconnect.UseVisualStyleBackColor = true;
            this.controlConsoleDisconnect.Click += new System.EventHandler(this.controlDisconnect_Click);
            // 
            // controlConsoleConnect
            // 
            this.controlConsoleConnect.Location = new System.Drawing.Point(373, 89);
            this.controlConsoleConnect.Name = "controlConsoleConnect";
            this.controlConsoleConnect.Size = new System.Drawing.Size(75, 23);
            this.controlConsoleConnect.TabIndex = 6;
            this.controlConsoleConnect.Text = "Connect";
            this.controlConsoleConnect.UseVisualStyleBackColor = true;
            this.controlConsoleConnect.Click += new System.EventHandler(this.controlConnect_Click);
            // 
            // controlManaText
            // 
            this.controlManaText.AutoSize = true;
            this.controlManaText.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlManaText.Location = new System.Drawing.Point(220, 19);
            this.controlManaText.Name = "controlManaText";
            this.controlManaText.Size = new System.Drawing.Size(0, 15);
            this.controlManaText.TabIndex = 5;
            // 
            // controlHealthText
            // 
            this.controlHealthText.AutoSize = true;
            this.controlHealthText.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlHealthText.Location = new System.Drawing.Point(77, 19);
            this.controlHealthText.Name = "controlHealthText";
            this.controlHealthText.Size = new System.Drawing.Size(0, 15);
            this.controlHealthText.TabIndex = 4;
            // 
            // controlManaLabel
            // 
            this.controlManaLabel.AutoSize = true;
            this.controlManaLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlManaLabel.Location = new System.Drawing.Point(162, 19);
            this.controlManaLabel.Name = "controlManaLabel";
            this.controlManaLabel.Size = new System.Drawing.Size(37, 15);
            this.controlManaLabel.TabIndex = 3;
            this.controlManaLabel.Text = "Mana";
            // 
            // controlHealthLabel
            // 
            this.controlHealthLabel.AutoSize = true;
            this.controlHealthLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlHealthLabel.Location = new System.Drawing.Point(15, 19);
            this.controlHealthLabel.Name = "controlHealthLabel";
            this.controlHealthLabel.Size = new System.Drawing.Size(44, 15);
            this.controlHealthLabel.TabIndex = 2;
            this.controlHealthLabel.Text = "Health";
            // 
            // controlManaBar
            // 
            this.controlManaBar.Location = new System.Drawing.Point(165, 37);
            this.controlManaBar.Maximum = 1000;
            this.controlManaBar.Name = "controlManaBar";
            this.controlManaBar.Size = new System.Drawing.Size(100, 23);
            this.controlManaBar.Step = 1;
            this.controlManaBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.controlManaBar.TabIndex = 1;
            // 
            // controlHealthBar
            // 
            this.controlHealthBar.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.controlHealthBar.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.controlHealthBar.Location = new System.Drawing.Point(16, 37);
            this.controlHealthBar.Maximum = 1000;
            this.controlHealthBar.Name = "controlHealthBar";
            this.controlHealthBar.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.controlHealthBar.RightToLeftLayout = true;
            this.controlHealthBar.Size = new System.Drawing.Size(100, 23);
            this.controlHealthBar.Step = 1;
            this.controlHealthBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.controlHealthBar.TabIndex = 0;
            // 
            // packetControl
            // 
            this.packetControl.Controls.Add(this.textConsoleOutput);
            this.packetControl.Controls.Add(this.packetTab);
            this.packetControl.Location = new System.Drawing.Point(4, 24);
            this.packetControl.Name = "packetControl";
            this.packetControl.Padding = new System.Windows.Forms.Padding(3);
            this.packetControl.Size = new System.Drawing.Size(741, 366);
            this.packetControl.TabIndex = 2;
            this.packetControl.Text = "Packets";
            this.packetControl.UseVisualStyleBackColor = true;
            // 
            // pathControl
            // 
            this.pathControl.Controls.Add(this.pathingSpeedControlBox);
            this.pathControl.Controls.Add(this.groupBox2);
            this.pathControl.Controls.Add(this.groupBox1);
            this.pathControl.Controls.Add(this.groupBoxPathFollow);
            this.pathControl.Controls.Add(this.pathDynamic);
            this.pathControl.Controls.Add(this.pathSingle);
            this.pathControl.Controls.Add(this.pathNone);
            this.pathControl.Location = new System.Drawing.Point(4, 24);
            this.pathControl.Name = "pathControl";
            this.pathControl.Size = new System.Drawing.Size(741, 366);
            this.pathControl.TabIndex = 3;
            this.pathControl.Text = "Pathing";
            this.pathControl.UseVisualStyleBackColor = true;
            // 
            // pathingSpeedControlBox
            // 
            this.pathingSpeedControlBox.Controls.Add(this.pathWalkingDefaults);
            this.pathingSpeedControlBox.Controls.Add(this.pathMsDelay);
            this.pathingSpeedControlBox.Controls.Add(this.label1);
            this.pathingSpeedControlBox.Controls.Add(this.pathingSpeedControl);
            this.pathingSpeedControlBox.Controls.Add(this.pathingSpeedType);
            this.pathingSpeedControlBox.Controls.Add(this.pathingSpeedControlLabel);
            this.pathingSpeedControlBox.Location = new System.Drawing.Point(305, 197);
            this.pathingSpeedControlBox.Name = "pathingSpeedControlBox";
            this.pathingSpeedControlBox.Size = new System.Drawing.Size(257, 152);
            this.pathingSpeedControlBox.TabIndex = 7;
            this.pathingSpeedControlBox.TabStop = false;
            this.pathingSpeedControlBox.Text = "Walking and Walkspeed";
            // 
            // pathWalkingDefaults
            // 
            this.pathWalkingDefaults.AutoSize = true;
            this.pathWalkingDefaults.Font = new System.Drawing.Font("GungsuhChe", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pathWalkingDefaults.Location = new System.Drawing.Point(6, 137);
            this.pathWalkingDefaults.Name = "pathWalkingDefaults";
            this.pathWalkingDefaults.Size = new System.Drawing.Size(53, 9);
            this.pathWalkingDefaults.TabIndex = 7;
            this.pathWalkingDefaults.Text = "defaults";
            this.pathWalkingDefaults.Click += new System.EventHandler(this.pathWalkingDefaults_Click);
            // 
            // pathMsDelay
            // 
            this.pathMsDelay.AutoSize = true;
            this.pathMsDelay.Font = new System.Drawing.Font("Segoe UI", 7F);
            this.pathMsDelay.Location = new System.Drawing.Point(183, 117);
            this.pathMsDelay.Name = "pathMsDelay";
            this.pathMsDelay.Size = new System.Drawing.Size(20, 12);
            this.pathMsDelay.TabIndex = 6;
            this.pathMsDelay.Text = "385";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 7F);
            this.label1.Location = new System.Drawing.Point(149, 134);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "Delay (ms)";
            // 
            // pathingSpeedControl
            // 
            this.pathingSpeedControl.Location = new System.Drawing.Point(206, 16);
            this.pathingSpeedControl.Maximum = 480;
            this.pathingSpeedControl.Minimum = 25;
            this.pathingSpeedControl.Name = "pathingSpeedControl";
            this.pathingSpeedControl.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.pathingSpeedControl.Size = new System.Drawing.Size(45, 130);
            this.pathingSpeedControl.TabIndex = 4;
            this.pathingSpeedControl.Value = 385;
            this.pathingSpeedControl.ValueChanged += new System.EventHandler(this.pathingSpeedControl_change);
            // 
            // pathingSpeedType
            // 
            this.pathingSpeedType.Font = new System.Drawing.Font("Segoe UI", 7F);
            this.pathingSpeedType.FormattingEnabled = true;
            this.pathingSpeedType.Items.AddRange(new object[] {
            "Manual",
            "Automatic"});
            this.pathingSpeedType.Location = new System.Drawing.Point(44, 16);
            this.pathingSpeedType.Name = "pathingSpeedType";
            this.pathingSpeedType.Size = new System.Drawing.Size(106, 20);
            this.pathingSpeedType.TabIndex = 3;
            this.pathingSpeedType.Text = "No Change";
            // 
            // pathingSpeedControlLabel
            // 
            this.pathingSpeedControlLabel.AutoSize = true;
            this.pathingSpeedControlLabel.Font = new System.Drawing.Font("Segoe UI", 7F);
            this.pathingSpeedControlLabel.Location = new System.Drawing.Point(6, 19);
            this.pathingSpeedControlLabel.Name = "pathingSpeedControlLabel";
            this.pathingSpeedControlLabel.Size = new System.Drawing.Size(32, 12);
            this.pathingSpeedControlLabel.TabIndex = 0;
            this.pathingSpeedControlLabel.Text = "Speed";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pathingWaypointsList);
            this.groupBox2.Controls.Add(this.pathingWaypoints);
            this.groupBox2.Controls.Add(this.pathingWaypointUse);
            this.groupBox2.Location = new System.Drawing.Point(42, 197);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(257, 152);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Waypoint Options";
            // 
            // pathingWaypointsList
            // 
            this.pathingWaypointsList.FormattingEnabled = true;
            this.pathingWaypointsList.Location = new System.Drawing.Point(51, 32);
            this.pathingWaypointsList.Name = "pathingWaypointsList";
            this.pathingWaypointsList.Size = new System.Drawing.Size(121, 23);
            this.pathingWaypointsList.TabIndex = 3;
            // 
            // pathingWaypoints
            // 
            this.pathingWaypoints.AutoSize = true;
            this.pathingWaypoints.Location = new System.Drawing.Point(107, 2);
            this.pathingWaypoints.Name = "pathingWaypoints";
            this.pathingWaypoints.Size = new System.Drawing.Size(15, 14);
            this.pathingWaypoints.TabIndex = 1;
            this.pathingWaypoints.UseVisualStyleBackColor = true;
            // 
            // pathingWaypointUse
            // 
            this.pathingWaypointUse.AutoSize = true;
            this.pathingWaypointUse.Location = new System.Drawing.Point(6, 36);
            this.pathingWaypointUse.Name = "pathingWaypointUse";
            this.pathingWaypointUse.Size = new System.Drawing.Size(37, 15);
            this.pathingWaypointUse.TabIndex = 0;
            this.pathingWaypointUse.Text = "Using";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pathingMapList);
            this.groupBox1.Controls.Add(this.pathingMap);
            this.groupBox1.Controls.Add(this.pathingMapGoto);
            this.groupBox1.Location = new System.Drawing.Point(305, 38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(257, 152);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Map Options";
            // 
            // pathingMapList
            // 
            this.pathingMapList.FormattingEnabled = true;
            this.pathingMapList.Location = new System.Drawing.Point(51, 32);
            this.pathingMapList.Name = "pathingMapList";
            this.pathingMapList.Size = new System.Drawing.Size(121, 23);
            this.pathingMapList.TabIndex = 3;
            // 
            // pathingMap
            // 
            this.pathingMap.AutoSize = true;
            this.pathingMap.Location = new System.Drawing.Point(80, 2);
            this.pathingMap.Name = "pathingMap";
            this.pathingMap.Size = new System.Drawing.Size(15, 14);
            this.pathingMap.TabIndex = 1;
            this.pathingMap.UseVisualStyleBackColor = true;
            // 
            // pathingMapGoto
            // 
            this.pathingMapGoto.AutoSize = true;
            this.pathingMapGoto.Location = new System.Drawing.Point(6, 36);
            this.pathingMapGoto.Name = "pathingMapGoto";
            this.pathingMapGoto.Size = new System.Drawing.Size(38, 15);
            this.pathingMapGoto.TabIndex = 0;
            this.pathingMapGoto.Text = "Go To";
            // 
            // groupBoxPathFollow
            // 
            this.groupBoxPathFollow.Controls.Add(this.pathFollowDistance);
            this.groupBoxPathFollow.Controls.Add(this.pathFollow);
            this.groupBoxPathFollow.Controls.Add(this.pathFollowTarget);
            this.groupBoxPathFollow.Controls.Add(this.pathFollowTargetLabel);
            this.groupBoxPathFollow.Location = new System.Drawing.Point(42, 39);
            this.groupBoxPathFollow.Name = "groupBoxPathFollow";
            this.groupBoxPathFollow.Size = new System.Drawing.Size(257, 152);
            this.groupBoxPathFollow.TabIndex = 4;
            this.groupBoxPathFollow.TabStop = false;
            this.groupBoxPathFollow.Text = "Follow Options";
            // 
            // pathFollowDistance
            // 
            this.pathFollowDistance.Location = new System.Drawing.Point(168, 33);
            this.pathFollowDistance.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.pathFollowDistance.Name = "pathFollowDistance";
            this.pathFollowDistance.Size = new System.Drawing.Size(38, 23);
            this.pathFollowDistance.TabIndex = 2;
            this.pathFollowDistance.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // pathFollow
            // 
            this.pathFollow.AutoSize = true;
            this.pathFollow.Location = new System.Drawing.Point(91, 2);
            this.pathFollow.Name = "pathFollow";
            this.pathFollow.Size = new System.Drawing.Size(15, 14);
            this.pathFollow.TabIndex = 1;
            this.pathFollow.UseVisualStyleBackColor = true;
            this.pathFollow.CheckedChanged += new System.EventHandler(this.pathFollowPlayer_CheckedChanged);
            // 
            // pathFollowTarget
            // 
            this.pathFollowTarget.Location = new System.Drawing.Point(53, 33);
            this.pathFollowTarget.MaxLength = 12;
            this.pathFollowTarget.Name = "pathFollowTarget";
            this.pathFollowTarget.Size = new System.Drawing.Size(109, 23);
            this.pathFollowTarget.TabIndex = 0;
            // 
            // pathFollowTargetLabel
            // 
            this.pathFollowTargetLabel.AutoSize = true;
            this.pathFollowTargetLabel.Location = new System.Drawing.Point(6, 36);
            this.pathFollowTargetLabel.Name = "pathFollowTargetLabel";
            this.pathFollowTargetLabel.Size = new System.Drawing.Size(40, 15);
            this.pathFollowTargetLabel.TabIndex = 0;
            this.pathFollowTargetLabel.Text = "Target";
            // 
            // pathDynamic
            // 
            this.pathDynamic.AutoSize = true;
            this.pathDynamic.Location = new System.Drawing.Point(268, 13);
            this.pathDynamic.Name = "pathDynamic";
            this.pathDynamic.Size = new System.Drawing.Size(100, 19);
            this.pathDynamic.TabIndex = 3;
            this.pathDynamic.Text = "Dynamic Type";
            this.pathDynamic.UseVisualStyleBackColor = true;
            this.pathDynamic.Click += new System.EventHandler(this.pathDyanmic_Click);
            // 
            // pathSingle
            // 
            this.pathSingle.AutoSize = true;
            this.pathSingle.Location = new System.Drawing.Point(156, 14);
            this.pathSingle.Name = "pathSingle";
            this.pathSingle.Size = new System.Drawing.Size(85, 19);
            this.pathSingle.TabIndex = 2;
            this.pathSingle.Text = "Single Type";
            this.pathSingle.UseVisualStyleBackColor = true;
            this.pathSingle.Click += new System.EventHandler(this.pathSingle_Click);
            // 
            // pathNone
            // 
            this.pathNone.AutoSize = true;
            this.pathNone.Checked = true;
            this.pathNone.Location = new System.Drawing.Point(42, 13);
            this.pathNone.Name = "pathNone";
            this.pathNone.Size = new System.Drawing.Size(85, 19);
            this.pathNone.TabIndex = 1;
            this.pathNone.TabStop = true;
            this.pathNone.Text = "No Pathing";
            this.pathNone.UseVisualStyleBackColor = true;
            this.pathNone.Click += new System.EventHandler(this.pathNone_Click);
            // 
            // mapControl
            // 
            this.mapControl.Controls.Add(this.mapControlBack);
            this.mapControl.Location = new System.Drawing.Point(4, 24);
            this.mapControl.Name = "mapControl";
            this.mapControl.Padding = new System.Windows.Forms.Padding(3);
            this.mapControl.Size = new System.Drawing.Size(741, 366);
            this.mapControl.TabIndex = 4;
            this.mapControl.Text = "Map Control";
            this.mapControl.UseVisualStyleBackColor = true;
            // 
            // mapControlBack
            // 
            this.mapControlBack.Controls.Add(this.mapResumeDynamicMap);
            this.mapControlBack.Controls.Add(this.mapPauseDynamicMap);
            this.mapControlBack.Controls.Add(this.mapDynamicScrollBarControl);
            this.mapControlBack.Controls.Add(this.mapResumeWaypoints);
            this.mapControlBack.Controls.Add(this.mapPauseWaypoints);
            this.mapControlBack.Controls.Add(this.mapWaypointsPath);
            this.mapControlBack.Controls.Add(this.pathAddNone);
            this.mapControlBack.Controls.Add(this.mapAddDoors);
            this.mapControlBack.Controls.Add(this.mapAddBlocks);
            this.mapControlBack.Controls.Add(this.mapAddWaypoints);
            this.mapControlBack.Controls.Add(this.mapEditOptionsGroup);
            this.mapControlBack.Controls.Add(this.mapListGotoCurrent);
            this.mapControlBack.Controls.Add(this.mapDimList);
            this.mapControlBack.Controls.Add(this.listBoxWorldMaps);
            this.mapControlBack.Controls.Add(this.textBoxMapAxis);
            this.mapControlBack.Controls.Add(this.textBoxWarpPortalLocs);
            this.mapControlBack.Controls.Add(this.textBoxMapNum);
            this.mapControlBack.Controls.Add(this.textBoxMapFolder);
            this.mapControlBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapControlBack.Location = new System.Drawing.Point(3, 3);
            this.mapControlBack.Name = "mapControlBack";
            this.mapControlBack.Size = new System.Drawing.Size(735, 360);
            this.mapControlBack.TabIndex = 0;
            // 
            // mapResumeDynamicMap
            // 
            this.mapResumeDynamicMap.Location = new System.Drawing.Point(369, 176);
            this.mapResumeDynamicMap.Name = "mapResumeDynamicMap";
            this.mapResumeDynamicMap.Size = new System.Drawing.Size(91, 23);
            this.mapResumeDynamicMap.TabIndex = 22;
            this.mapResumeDynamicMap.Text = "Map Resume";
            this.mapResumeDynamicMap.UseVisualStyleBackColor = true;
            this.mapResumeDynamicMap.Click += new System.EventHandler(this.mapResumeDynamicMap_Click);
            // 
            // mapPauseDynamicMap
            // 
            this.mapPauseDynamicMap.Location = new System.Drawing.Point(369, 147);
            this.mapPauseDynamicMap.Name = "mapPauseDynamicMap";
            this.mapPauseDynamicMap.Size = new System.Drawing.Size(91, 23);
            this.mapPauseDynamicMap.TabIndex = 21;
            this.mapPauseDynamicMap.Text = "Map Pause";
            this.mapPauseDynamicMap.UseVisualStyleBackColor = true;
            this.mapPauseDynamicMap.Click += new System.EventHandler(this.mapPauseDynamicMap_Click);
            // 
            // mapDynamicScrollBarControl
            // 
            this.mapDynamicScrollBarControl.Controls.Add(this.mapDynamicView);
            this.mapDynamicScrollBarControl.Location = new System.Drawing.Point(13, 66);
            this.mapDynamicScrollBarControl.Name = "mapDynamicScrollBarControl";
            this.mapDynamicScrollBarControl.Padding = new System.Windows.Forms.Padding(1);
            this.mapDynamicScrollBarControl.Size = new System.Drawing.Size(350, 272);
            this.mapDynamicScrollBarControl.TabIndex = 20;
            // 
            // mapDynamicView
            // 
            this.mapDynamicView.Location = new System.Drawing.Point(0, 0);
            this.mapDynamicView.Name = "mapDynamicView";
            this.mapDynamicView.Size = new System.Drawing.Size(172, 171);
            this.mapDynamicView.TabIndex = 5;
            this.mapDynamicView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PictureBoxMapPreview_MouseClick);
            // 
            // mapResumeWaypoints
            // 
            this.mapResumeWaypoints.Location = new System.Drawing.Point(369, 76);
            this.mapResumeWaypoints.Name = "mapResumeWaypoints";
            this.mapResumeWaypoints.Size = new System.Drawing.Size(60, 23);
            this.mapResumeWaypoints.TabIndex = 19;
            this.mapResumeWaypoints.Text = "Resume";
            this.mapResumeWaypoints.UseVisualStyleBackColor = true;
            this.mapResumeWaypoints.Click += new System.EventHandler(this.controlResumeWaypointsThread_Click);
            // 
            // mapPauseWaypoints
            // 
            this.mapPauseWaypoints.Location = new System.Drawing.Point(369, 47);
            this.mapPauseWaypoints.Name = "mapPauseWaypoints";
            this.mapPauseWaypoints.Size = new System.Drawing.Size(50, 23);
            this.mapPauseWaypoints.TabIndex = 18;
            this.mapPauseWaypoints.Text = "Pause";
            this.mapPauseWaypoints.UseVisualStyleBackColor = true;
            this.mapPauseWaypoints.Click += new System.EventHandler(this.controlPauseWaypointsThread_Click);
            // 
            // mapWaypointsPath
            // 
            this.mapWaypointsPath.Location = new System.Drawing.Point(369, 18);
            this.mapWaypointsPath.Name = "mapWaypointsPath";
            this.mapWaypointsPath.Size = new System.Drawing.Size(72, 23);
            this.mapWaypointsPath.TabIndex = 13;
            this.mapWaypointsPath.Text = "Start Path";
            this.mapWaypointsPath.UseVisualStyleBackColor = true;
            this.mapWaypointsPath.Click += new System.EventHandler(this.mapWaypointsPath_Click);
            // 
            // pathAddNone
            // 
            this.pathAddNone.AutoSize = true;
            this.pathAddNone.Checked = true;
            this.pathAddNone.Location = new System.Drawing.Point(288, 31);
            this.pathAddNone.Name = "pathAddNone";
            this.pathAddNone.Size = new System.Drawing.Size(54, 19);
            this.pathAddNone.TabIndex = 12;
            this.pathAddNone.TabStop = true;
            this.pathAddNone.Text = "None";
            this.pathAddNone.UseVisualStyleBackColor = true;
            // 
            // mapAddDoors
            // 
            this.mapAddDoors.AutoSize = true;
            this.mapAddDoors.Location = new System.Drawing.Point(210, 31);
            this.mapAddDoors.Name = "mapAddDoors";
            this.mapAddDoors.Size = new System.Drawing.Size(56, 19);
            this.mapAddDoors.TabIndex = 11;
            this.mapAddDoors.Text = "Doors";
            this.mapAddDoors.UseVisualStyleBackColor = true;
            // 
            // mapAddBlocks
            // 
            this.mapAddBlocks.AutoSize = true;
            this.mapAddBlocks.Location = new System.Drawing.Point(127, 31);
            this.mapAddBlocks.Name = "mapAddBlocks";
            this.mapAddBlocks.Size = new System.Drawing.Size(59, 19);
            this.mapAddBlocks.TabIndex = 10;
            this.mapAddBlocks.Text = "Blocks";
            this.mapAddBlocks.UseVisualStyleBackColor = true;
            // 
            // mapAddWaypoints
            // 
            this.mapAddWaypoints.AutoSize = true;
            this.mapAddWaypoints.Location = new System.Drawing.Point(23, 31);
            this.mapAddWaypoints.Name = "mapAddWaypoints";
            this.mapAddWaypoints.Size = new System.Drawing.Size(81, 19);
            this.mapAddWaypoints.TabIndex = 9;
            this.mapAddWaypoints.Text = "Waypoints";
            this.mapAddWaypoints.UseVisualStyleBackColor = true;
            // 
            // mapEditOptionsGroup
            // 
            this.mapEditOptionsGroup.Location = new System.Drawing.Point(13, 18);
            this.mapEditOptionsGroup.Name = "mapEditOptionsGroup";
            this.mapEditOptionsGroup.Size = new System.Drawing.Size(350, 42);
            this.mapEditOptionsGroup.TabIndex = 8;
            // 
            // mapListGotoCurrent
            // 
            this.mapListGotoCurrent.Location = new System.Drawing.Point(604, 193);
            this.mapListGotoCurrent.Name = "mapListGotoCurrent";
            this.mapListGotoCurrent.Size = new System.Drawing.Size(120, 23);
            this.mapListGotoCurrent.TabIndex = 7;
            this.mapListGotoCurrent.Text = "Goto My Map";
            this.mapListGotoCurrent.UseVisualStyleBackColor = true;
            this.mapListGotoCurrent.Click += new System.EventHandler(this.mapListGotoCurrent_Click);
            // 
            // mapDimList
            // 
            this.mapDimList.FormattingEnabled = true;
            this.mapDimList.ItemHeight = 15;
            this.mapDimList.Location = new System.Drawing.Point(528, 76);
            this.mapDimList.Name = "mapDimList";
            this.mapDimList.Size = new System.Drawing.Size(70, 94);
            this.mapDimList.TabIndex = 6;
            this.mapDimList.SelectedIndexChanged += new System.EventHandler(this.MapDimList_SelectedIndexChanged);
            // 
            // listBoxWorldMaps
            // 
            this.listBoxWorldMaps.FormattingEnabled = true;
            this.listBoxWorldMaps.ItemHeight = 15;
            this.listBoxWorldMaps.Location = new System.Drawing.Point(604, 18);
            this.listBoxWorldMaps.Name = "listBoxWorldMaps";
            this.listBoxWorldMaps.Size = new System.Drawing.Size(120, 169);
            this.listBoxWorldMaps.TabIndex = 4;
            this.listBoxWorldMaps.SelectedIndexChanged += new System.EventHandler(this.ListBoxWorldMaps_SelectedIndexChanged);
            // 
            // textBoxMapAxis
            // 
            this.textBoxMapAxis.Location = new System.Drawing.Point(509, 47);
            this.textBoxMapAxis.Name = "textBoxMapAxis";
            this.textBoxMapAxis.Size = new System.Drawing.Size(89, 23);
            this.textBoxMapAxis.TabIndex = 3;
            this.textBoxMapAxis.Text = "Axis";
            // 
            // textBoxWarpPortalLocs
            // 
            this.textBoxWarpPortalLocs.Location = new System.Drawing.Point(509, 18);
            this.textBoxWarpPortalLocs.Multiline = true;
            this.textBoxWarpPortalLocs.Name = "textBoxWarpPortalLocs";
            this.textBoxWarpPortalLocs.Size = new System.Drawing.Size(89, 23);
            this.textBoxWarpPortalLocs.TabIndex = 2;
            this.textBoxWarpPortalLocs.Text = "Warps";
            // 
            // textBoxMapNum
            // 
            this.textBoxMapNum.Location = new System.Drawing.Point(509, 193);
            this.textBoxMapNum.Name = "textBoxMapNum";
            this.textBoxMapNum.Size = new System.Drawing.Size(89, 23);
            this.textBoxMapNum.TabIndex = 1;
            this.textBoxMapNum.Text = "Number";
            // 
            // textBoxMapFolder
            // 
            this.textBoxMapFolder.Location = new System.Drawing.Point(526, 222);
            this.textBoxMapFolder.Name = "textBoxMapFolder";
            this.textBoxMapFolder.Size = new System.Drawing.Size(198, 23);
            this.textBoxMapFolder.TabIndex = 0;
            this.textBoxMapFolder.Text = "Map Folder Path";
            // 
            // controlSpells
            // 
            this.controlSpells.Controls.Add(this.spellsKnownSpellsList);
            this.controlSpells.Location = new System.Drawing.Point(4, 24);
            this.controlSpells.Name = "controlSpells";
            this.controlSpells.Padding = new System.Windows.Forms.Padding(3);
            this.controlSpells.Size = new System.Drawing.Size(741, 366);
            this.controlSpells.TabIndex = 5;
            this.controlSpells.Text = "Spells";
            this.controlSpells.UseVisualStyleBackColor = true;
            // 
            // spellsKnownSpellsList
            // 
            this.spellsKnownSpellsList.FormattingEnabled = true;
            this.spellsKnownSpellsList.ItemHeight = 15;
            this.spellsKnownSpellsList.Location = new System.Drawing.Point(19, 18);
            this.spellsKnownSpellsList.Name = "spellsKnownSpellsList";
            this.spellsKnownSpellsList.Size = new System.Drawing.Size(184, 184);
            this.spellsKnownSpellsList.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.controlChatSend);
            this.tabPage1.Controls.Add(this.controlChatTextBox);
            this.tabPage1.Controls.Add(this.controlChatText);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(741, 366);
            this.tabPage1.TabIndex = 6;
            this.tabPage1.Text = "Chat";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // clientStart
            // 
            this.clientStart.Font = new System.Drawing.Font("Segoe UI", 7F);
            this.clientStart.Location = new System.Drawing.Point(3, 10);
            this.clientStart.Name = "clientStart";
            this.clientStart.Size = new System.Drawing.Size(58, 24);
            this.clientStart.TabIndex = 1;
            this.clientStart.Text = "Start";
            this.clientStart.UseVisualStyleBackColor = true;
            this.clientStart.Click += new System.EventHandler(this.clientStart_Click);
            // 
            // clientStop
            // 
            this.clientStop.Font = new System.Drawing.Font("Segoe UI", 7F);
            this.clientStop.Location = new System.Drawing.Point(67, 10);
            this.clientStop.Name = "clientStop";
            this.clientStop.Size = new System.Drawing.Size(58, 24);
            this.clientStop.TabIndex = 2;
            this.clientStop.Text = "Stop";
            this.clientStop.UseVisualStyleBackColor = true;
            this.clientStop.Click += new System.EventHandler(this.clientStop_Click);
            // 
            // view3DTab
            // 
            this.view3DTab.Location = new System.Drawing.Point(0, 24);
            this.view3DTab.Name = "view3DTab";
            this.view3DTab.Size = new System.Drawing.Size(468, 276);
            this.view3DTab.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Segoe UI", 7F);
            this.button1.Location = new System.Drawing.Point(145, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(102, 24);
            this.button1.TabIndex = 3;
            this.button1.Text = "clientBasherStart";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.clientBasherStart_Click);
            // 
            // controlChatText
            // 
            this.controlChatText.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlChatText.Location = new System.Drawing.Point(-1, 0);
            this.controlChatText.Name = "controlChatText";
            this.controlChatText.Size = new System.Drawing.Size(735, 285);
            this.controlChatText.TabIndex = 4;
            this.controlChatText.Text = "";
            // 
            // controlChatTextBox
            // 
            this.controlChatTextBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlChatTextBox.Location = new System.Drawing.Point(0, 291);
            this.controlChatTextBox.Name = "controlChatTextBox";
            this.controlChatTextBox.Size = new System.Drawing.Size(591, 50);
            this.controlChatTextBox.TabIndex = 5;
            this.controlChatTextBox.Text = "";
            // 
            // controlChatSend
            // 
            this.controlChatSend.Location = new System.Drawing.Point(597, 291);
            this.controlChatSend.Name = "controlChatSend";
            this.controlChatSend.Size = new System.Drawing.Size(69, 50);
            this.controlChatSend.TabIndex = 6;
            this.controlChatSend.Text = "Send";
            this.controlChatSend.UseVisualStyleBackColor = true;
            this.controlChatSend.Click += new System.EventHandler(this.button2_Click);
            // 
            // ClientTab
            // 
            this.Controls.Add(this.button1);
            this.Controls.Add(this.clientStop);
            this.Controls.Add(this.clientStart);
            this.Controls.Add(this.controlTabs);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ClientTab";
            this.Size = new System.Drawing.Size(734, 429);
            this.packetTab.ResumeLayout(false);
            this.packetTab.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.controlTabs.ResumeLayout(false);
            this.mainControl.ResumeLayout(false);
            this.mainControl.PerformLayout();
            this.packetControl.ResumeLayout(false);
            this.pathControl.ResumeLayout(false);
            this.pathControl.PerformLayout();
            this.pathingSpeedControlBox.ResumeLayout(false);
            this.pathingSpeedControlBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pathingSpeedControl)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBoxPathFollow.ResumeLayout(false);
            this.groupBoxPathFollow.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pathFollowDistance)).EndInit();
            this.mapControl.ResumeLayout(false);
            this.mapControlBack.ResumeLayout(false);
            this.mapControlBack.PerformLayout();
            this.mapDynamicScrollBarControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mapDynamicView.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapEditOptionsGroup.Properties)).EndInit();
            this.controlSpells.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox textConsoleOutput;
        private System.Windows.Forms.Panel packetTab;
        private System.Windows.Forms.RichTextBox textConsoleInput;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.Button buttonRecv;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton checkRecv;
        private System.Windows.Forms.ToolStripButton checkSend;
        public System.Windows.Forms.TabControl controlTabs;
        private System.Windows.Forms.TabPage mainControl;
        private System.Windows.Forms.TabPage packetControl;
        private System.Windows.Forms.TabPage pathControl;
        public System.Windows.Forms.RadioButton pathDynamic;
        public System.Windows.Forms.RadioButton pathSingle;
        public System.Windows.Forms.RadioButton pathNone;
        private System.Windows.Forms.Label pathFollowTargetLabel;
        private System.Windows.Forms.GroupBox groupBoxPathFollow;
        public System.Windows.Forms.TextBox pathFollowTarget;
        public System.Windows.Forms.NumericUpDown pathFollowDistance;
        public System.Windows.Forms.CheckBox pathFollow;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.ComboBox pathingWaypointsList;
        public System.Windows.Forms.CheckBox pathingWaypoints;
        private System.Windows.Forms.Label pathingWaypointUse;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.ComboBox pathingMapList;
        public System.Windows.Forms.CheckBox pathingMap;
        private System.Windows.Forms.Label pathingMapGoto;
        private System.Windows.Forms.GroupBox pathingSpeedControlBox;
        private System.Windows.Forms.Label pathMsDelay;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TrackBar pathingSpeedControl;
        public System.Windows.Forms.ComboBox pathingSpeedType;
        private System.Windows.Forms.Label pathingSpeedControlLabel;
        public System.Windows.Forms.Button clientStart;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        public System.Windows.Forms.Button clientStop;
        private System.Windows.Forms.Label pathWalkingDefaults;
        private System.Windows.Forms.Panel view3DTab;
        private System.Windows.Forms.Label controlManaLabel;
        private System.Windows.Forms.Label controlHealthLabel;
        private System.Windows.Forms.ProgressBar controlManaBar;
        private System.Windows.Forms.ProgressBar controlHealthBar;
        private System.Windows.Forms.Label controlManaText;
        private System.Windows.Forms.Label controlHealthText;
        private TabPage mapControl;
        private Panel mapControlBack;
        public TextBox textBoxWarpPortalLocs;
        public TextBox textBoxMapAxis;
        public TextBox textBoxMapNum;
        public TextBox textBoxMapFolder;
        private ListBox listBoxWorldMaps;
        public PictureEdit mapDynamicView;
        public ListBox mapDimList;
        public Button mapListGotoCurrent;
        private RadioButton mapAddDoors;
        private RadioButton mapAddBlocks;
        private RadioButton mapAddWaypoints;
        private RadioGroup mapEditOptionsGroup;
        private Button controlConsoleConnect;
        private Button controlConsoleDisconnect;
        private TextBox controlConsolePassword;
        private TextBox controlConsoleUsername;
        private RadioButton pathAddNone;
        private TextBox controlEditWaypoint;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label controlCurrentMapLabel;
        private System.Windows.Forms.Label label2;
        public ListBox controlMapWaypoints;
        private TabPage controlSpells;
        private ListBox spellsKnownSpellsList;
        public Button mapWaypointsPath;
        private XtraUserControl xtraUserControl1;
        private Button mapResumeWaypoints;
        private Button mapPauseWaypoints;
        private XtraScrollableControl mapDynamicScrollBarControl;
        private Button mapResumeDynamicMap;
        private Button mapPauseDynamicMap;
        public Button button1;
        private TabPage tabPage1;
        private Button unusedcontrol;
        public ListBox controlRunTasks;
        private Button controlLaunchWatcher;
        private Button controlChatSend;
        private RichTextBox controlChatTextBox;
        public RichTextBox controlChatText;
        /*
        public Client client;
        public CompilerResults results;
        private OpenFileDialog openFileDialog1;
        private MenuStrip menuStrip1;
        private DockManager dockManager1;
        private ControlContainer controlContainer1;
        private ControlContainer dockPanel2_Container;
        private Panel panel1;
        private BarDockControl barDockControlLeft;
        //private XafBarManager xafBarManager1;
        private XafBar xafBar1;
        private BarButtonItem barButtonItem2;
        private XafBar xafBar2;
        private BarButtonItem barButtonItem1;
        private BarButtonItem barButtonItem3;
        private BarDockControl barDockControl1;
        private BarDockControl barDockControlRight;
        private BarDockControl barDockControlBottom;
        public DockPanel dockPanel2;
        private ToolStripMenuItem waypointsToolStripMenuItem;
        private ToolStripMenuItem clearToolStripMenuItem;
        private ToolStripMenuItem invertToolStripMenuItem;
        private ToolStripMenuItem resetPositionToolStripMenuItem;
        private GroupControl groupControl1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.ProgressBar progressBar1;
        private DockPanel dockPanel1;
        private ControlContainer dockPanel1_Container;
        private ToolStripMenuItem actionsToolStripMenuItem;
        private ToolStripMenuItem a_adding_wps;
        private ToolStripMenuItem a_adding_blocks;
        private ToolStripMenuItem mapToolStripMenuItem;
        private ToolStripMenuItem a_editmode;
        private DockPanel dockPanel3;
        private ControlContainer dockPanel3_Container;
        private TileControl tileControl1;
        private TileGroup tileGroup1;
        private TileItem tileItem1;
        private AutoHideContainer hideContainerLeft;
        private AutoHideContainer hideContainerBottom;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private RichTextBox richTextBox1;
        //ugh end*/

    }
}