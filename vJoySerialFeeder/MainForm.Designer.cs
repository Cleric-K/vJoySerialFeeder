﻿/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 8.6.2017 г.
 * Time: 16:58 ч.
 */
namespace vJoySerialFeeder
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.FlowLayoutPanel panelMappings;
		private System.Windows.Forms.Button buttonAddAxis;
		private System.Windows.Forms.Button buttonAddButton;
		private System.Windows.Forms.ComboBox comboPorts;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button buttonConnect;
		private System.Windows.Forms.Button buttonPortsRefresh;
		private System.ComponentModel.BackgroundWorker backgroundWorker;
		private System.Windows.Forms.ComboBox comboJoysticks;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox comboProfiles;
		private System.Windows.Forms.Button buttonLoadProfile;
		private System.Windows.Forms.Button buttonSaveProfile;
		private System.Windows.Forms.Button buttonDeleteProfile;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent() {
        	this.panelMappings = new System.Windows.Forms.FlowLayoutPanel();
        	this.buttonAddAxis = new System.Windows.Forms.Button();
        	this.buttonAddButton = new System.Windows.Forms.Button();
        	this.comboPorts = new System.Windows.Forms.ComboBox();
        	this.label1 = new System.Windows.Forms.Label();
        	this.buttonConnect = new System.Windows.Forms.Button();
        	this.buttonPortsRefresh = new System.Windows.Forms.Button();
        	this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
        	this.comboJoysticks = new System.Windows.Forms.ComboBox();
        	this.label3 = new System.Windows.Forms.Label();
        	this.comboProfiles = new System.Windows.Forms.ComboBox();
        	this.buttonLoadProfile = new System.Windows.Forms.Button();
        	this.buttonSaveProfile = new System.Windows.Forms.Button();
        	this.buttonDeleteProfile = new System.Windows.Forms.Button();
        	this.label4 = new System.Windows.Forms.Label();
        	this.label5 = new System.Windows.Forms.Label();
        	this.statusStrip1 = new System.Windows.Forms.StatusStrip();
        	this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
        	this.label6 = new System.Windows.Forms.Label();
        	this.comboProtocol = new System.Windows.Forms.ComboBox();
        	this.buttonBitmappedButton = new System.Windows.Forms.Button();
        	this.buttonPortSetup = new System.Windows.Forms.Button();
        	this.buttonProtocolSetup = new System.Windows.Forms.Button();
        	this.buttonNewProfile = new System.Windows.Forms.Button();
        	this.menuStrip1 = new System.Windows.Forms.MenuStrip();
        	this.programToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.channelMonitorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.menuProfileOptions = new System.Windows.Forms.ToolStripMenuItem();
        	this.menuGlobalOptions = new System.Windows.Forms.ToolStripMenuItem();
        	this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.scriptingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.outputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.manualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.menuAbout = new System.Windows.Forms.ToolStripMenuItem();
        	this.label2 = new System.Windows.Forms.Label();
        	this.buttonScript = new System.Windows.Forms.Button();
        	this.statusStrip1.SuspendLayout();
        	this.menuStrip1.SuspendLayout();
        	this.SuspendLayout();
        	// 
        	// panelMappings
        	// 
        	this.panelMappings.AutoScroll = true;
        	this.panelMappings.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
        	this.panelMappings.Location = new System.Drawing.Point(12, 135);
        	this.panelMappings.Name = "panelMappings";
        	this.panelMappings.Size = new System.Drawing.Size(703, 378);
        	this.panelMappings.TabIndex = 100;
        	this.panelMappings.WrapContents = false;
        	this.panelMappings.MouseEnter += new System.EventHandler(this.FlowLayoutPanel1MouseEnter);
        	// 
        	// buttonAddAxis
        	// 
        	this.buttonAddAxis.Location = new System.Drawing.Point(12, 106);
        	this.buttonAddAxis.Name = "buttonAddAxis";
        	this.buttonAddAxis.Size = new System.Drawing.Size(66, 23);
        	this.buttonAddAxis.TabIndex = 12;
        	this.buttonAddAxis.Text = "Add Axis";
        	this.buttonAddAxis.UseVisualStyleBackColor = true;
        	this.buttonAddAxis.Click += new System.EventHandler(this.ButtonAddAxisClick);
        	// 
        	// buttonAddButton
        	// 
        	this.buttonAddButton.Location = new System.Drawing.Point(84, 106);
        	this.buttonAddButton.Name = "buttonAddButton";
        	this.buttonAddButton.Size = new System.Drawing.Size(75, 23);
        	this.buttonAddButton.TabIndex = 13;
        	this.buttonAddButton.Text = "Add Button";
        	this.buttonAddButton.UseVisualStyleBackColor = true;
        	this.buttonAddButton.Click += new System.EventHandler(this.ButtonAddButtonClick);
        	// 
        	// comboPorts
        	// 
        	this.comboPorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        	this.comboPorts.FormattingEnabled = true;
        	this.comboPorts.Location = new System.Drawing.Point(566, 36);
        	this.comboPorts.Name = "comboPorts";
        	this.comboPorts.Size = new System.Drawing.Size(67, 21);
        	this.comboPorts.TabIndex = 8;
        	// 
        	// label1
        	// 
        	this.label1.Location = new System.Drawing.Point(525, 36);
        	this.label1.Name = "label1";
        	this.label1.Size = new System.Drawing.Size(35, 21);
        	this.label1.TabIndex = 8;
        	this.label1.Text = "Port:";
        	this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        	// 
        	// buttonConnect
        	// 
        	this.buttonConnect.Location = new System.Drawing.Point(640, 35);
        	this.buttonConnect.Name = "buttonConnect";
        	this.buttonConnect.Size = new System.Drawing.Size(75, 23);
        	this.buttonConnect.TabIndex = 10;
        	this.buttonConnect.Text = "Connect";
        	this.buttonConnect.UseVisualStyleBackColor = true;
        	this.buttonConnect.Click += new System.EventHandler(this.ButtonConnectClick);
        	// 
        	// buttonPortsRefresh
        	// 
        	this.buttonPortsRefresh.Location = new System.Drawing.Point(640, 64);
        	this.buttonPortsRefresh.Name = "buttonPortsRefresh";
        	this.buttonPortsRefresh.Size = new System.Drawing.Size(75, 23);
        	this.buttonPortsRefresh.TabIndex = 11;
        	this.buttonPortsRefresh.Text = "Refresh";
        	this.buttonPortsRefresh.UseVisualStyleBackColor = true;
        	this.buttonPortsRefresh.Click += new System.EventHandler(this.ButtonPortsRefreshClick);
        	// 
        	// backgroundWorker
        	// 
        	this.backgroundWorker.WorkerReportsProgress = true;
        	this.backgroundWorker.WorkerSupportsCancellation = true;
        	this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorkerDoWork);
        	this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BackgroundWorkerProgressChanged);
        	this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorkerRunWorkerCompleted);
        	// 
        	// comboJoysticks
        	// 
        	this.comboJoysticks.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        	this.comboJoysticks.FormattingEnabled = true;
        	this.comboJoysticks.Location = new System.Drawing.Point(360, 37);
        	this.comboJoysticks.Name = "comboJoysticks";
        	this.comboJoysticks.Size = new System.Drawing.Size(72, 21);
        	this.comboJoysticks.TabIndex = 5;
        	// 
        	// label3
        	// 
        	this.label3.Location = new System.Drawing.Point(266, 37);
        	this.label3.Name = "label3";
        	this.label3.Size = new System.Drawing.Size(88, 21);
        	this.label3.TabIndex = 13;
        	this.label3.Text = "Virtual Joystick:";
        	this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        	// 
        	// comboProfiles
        	// 
        	this.comboProfiles.FormattingEnabled = true;
        	this.comboProfiles.Location = new System.Drawing.Point(60, 34);
        	this.comboProfiles.Name = "comboProfiles";
        	this.comboProfiles.Size = new System.Drawing.Size(144, 21);
        	this.comboProfiles.TabIndex = 0;
        	// 
        	// buttonLoadProfile
        	// 
        	this.buttonLoadProfile.Location = new System.Drawing.Point(60, 64);
        	this.buttonLoadProfile.Name = "buttonLoadProfile";
        	this.buttonLoadProfile.Size = new System.Drawing.Size(40, 20);
        	this.buttonLoadProfile.TabIndex = 2;
        	this.buttonLoadProfile.Text = "Load";
        	this.buttonLoadProfile.UseVisualStyleBackColor = true;
        	this.buttonLoadProfile.Click += new System.EventHandler(this.ButtonLoadProfileClick);
        	// 
        	// buttonSaveProfile
        	// 
        	this.buttonSaveProfile.Location = new System.Drawing.Point(106, 64);
        	this.buttonSaveProfile.Name = "buttonSaveProfile";
        	this.buttonSaveProfile.Size = new System.Drawing.Size(40, 20);
        	this.buttonSaveProfile.TabIndex = 3;
        	this.buttonSaveProfile.Text = "Save";
        	this.buttonSaveProfile.UseVisualStyleBackColor = true;
        	this.buttonSaveProfile.Click += new System.EventHandler(this.ButtonSaveProfileClick);
        	// 
        	// buttonDeleteProfile
        	// 
        	this.buttonDeleteProfile.Location = new System.Drawing.Point(152, 64);
        	this.buttonDeleteProfile.Name = "buttonDeleteProfile";
        	this.buttonDeleteProfile.Size = new System.Drawing.Size(49, 20);
        	this.buttonDeleteProfile.TabIndex = 4;
        	this.buttonDeleteProfile.Text = "Delete";
        	this.buttonDeleteProfile.UseVisualStyleBackColor = true;
        	this.buttonDeleteProfile.Click += new System.EventHandler(this.ButtonDeleteProfileClick);
        	// 
        	// label4
        	// 
        	this.label4.Location = new System.Drawing.Point(13, 34);
        	this.label4.Name = "label4";
        	this.label4.Size = new System.Drawing.Size(41, 21);
        	this.label4.TabIndex = 18;
        	this.label4.Text = "Profile:";
        	this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        	// 
        	// label5
        	// 
        	this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
        	this.label5.Location = new System.Drawing.Point(0, 93);
        	this.label5.Name = "label5";
        	this.label5.Size = new System.Drawing.Size(730, 2);
        	this.label5.TabIndex = 19;
        	this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        	// 
        	// statusStrip1
        	// 
        	this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        	        	        	this.toolStripStatusLabel});
        	this.statusStrip1.Location = new System.Drawing.Point(0, 527);
        	this.statusStrip1.Name = "statusStrip1";
        	this.statusStrip1.Size = new System.Drawing.Size(730, 22);
        	this.statusStrip1.SizingGrip = false;
        	this.statusStrip1.TabIndex = 20;
        	this.statusStrip1.Text = "statusStrip1";
        	// 
        	// toolStripStatusLabel
        	// 
        	this.toolStripStatusLabel.Name = "toolStripStatusLabel";
        	this.toolStripStatusLabel.Size = new System.Drawing.Size(112, 17);
        	this.toolStripStatusLabel.Text = "toolStripStatusLabel";
        	// 
        	// label6
        	// 
        	this.label6.Location = new System.Drawing.Point(266, 63);
        	this.label6.Name = "label6";
        	this.label6.Size = new System.Drawing.Size(78, 21);
        	this.label6.TabIndex = 21;
        	this.label6.Text = "Protocol:";
        	this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        	// 
        	// comboProtocol
        	// 
        	this.comboProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        	this.comboProtocol.FormattingEnabled = true;
        	this.comboProtocol.Items.AddRange(new object[] {
        	        	        	"IBUS",
        	        	        	"MultiWii",
        	        	        	"SBUS",
        	        	        	"Dummy"});
        	this.comboProtocol.Location = new System.Drawing.Point(360, 64);
        	this.comboProtocol.Name = "comboProtocol";
        	this.comboProtocol.Size = new System.Drawing.Size(72, 21);
        	this.comboProtocol.TabIndex = 6;
        	this.comboProtocol.SelectedIndexChanged += new System.EventHandler(this.ComboProtocolSelectedIndexChanged);
        	// 
        	// buttonBitmappedButton
        	// 
        	this.buttonBitmappedButton.Location = new System.Drawing.Point(166, 106);
        	this.buttonBitmappedButton.Name = "buttonBitmappedButton";
        	this.buttonBitmappedButton.Size = new System.Drawing.Size(133, 23);
        	this.buttonBitmappedButton.TabIndex = 14;
        	this.buttonBitmappedButton.Text = "Add Bit-mapped Button";
        	this.buttonBitmappedButton.UseVisualStyleBackColor = true;
        	this.buttonBitmappedButton.Click += new System.EventHandler(this.ButtonBitmappedButtonClick);
        	// 
        	// buttonPortSetup
        	// 
        	this.buttonPortSetup.Location = new System.Drawing.Point(566, 64);
        	this.buttonPortSetup.Name = "buttonPortSetup";
        	this.buttonPortSetup.Size = new System.Drawing.Size(68, 23);
        	this.buttonPortSetup.TabIndex = 9;
        	this.buttonPortSetup.Text = "Port Setup";
        	this.buttonPortSetup.UseVisualStyleBackColor = true;
        	this.buttonPortSetup.Click += new System.EventHandler(this.ButtonPortSetupClick);
        	// 
        	// buttonProtocolSetup
        	// 
        	this.buttonProtocolSetup.Location = new System.Drawing.Point(439, 63);
        	this.buttonProtocolSetup.Name = "buttonProtocolSetup";
        	this.buttonProtocolSetup.Size = new System.Drawing.Size(46, 23);
        	this.buttonProtocolSetup.TabIndex = 7;
        	this.buttonProtocolSetup.Text = "Setup";
        	this.buttonProtocolSetup.UseVisualStyleBackColor = true;
        	this.buttonProtocolSetup.Click += new System.EventHandler(this.ButtonProtocolSetupClick);
        	// 
        	// buttonNewProfile
        	// 
        	this.buttonNewProfile.Location = new System.Drawing.Point(13, 64);
        	this.buttonNewProfile.Name = "buttonNewProfile";
        	this.buttonNewProfile.Size = new System.Drawing.Size(41, 20);
        	this.buttonNewProfile.TabIndex = 1;
        	this.buttonNewProfile.Text = "New";
        	this.buttonNewProfile.UseVisualStyleBackColor = true;
        	this.buttonNewProfile.Click += new System.EventHandler(this.ButtonNewProfileClick);
        	// 
        	// menuStrip1
        	// 
        	this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        	        	        	this.programToolStripMenuItem,
        	        	        	this.scriptingToolStripMenuItem,
        	        	        	this.helpToolStripMenuItem});
        	this.menuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
        	this.menuStrip1.Location = new System.Drawing.Point(0, 0);
        	this.menuStrip1.Name = "menuStrip1";
        	this.menuStrip1.Size = new System.Drawing.Size(730, 24);
        	this.menuStrip1.TabIndex = 22;
        	this.menuStrip1.Text = "menuMain";
        	this.menuStrip1.MenuActivate += new System.EventHandler(this.MenuStrip1MenuActivate);
        	// 
        	// programToolStripMenuItem
        	// 
        	this.programToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
        	        	        	this.channelMonitorToolStripMenuItem,
        	        	        	this.menuProfileOptions,
        	        	        	this.menuGlobalOptions,
        	        	        	this.exitToolStripMenuItem});
        	this.programToolStripMenuItem.Name = "programToolStripMenuItem";
        	this.programToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
        	this.programToolStripMenuItem.Text = "&Program";
        	// 
        	// channelMonitorToolStripMenuItem
        	// 
        	this.channelMonitorToolStripMenuItem.Name = "channelMonitorToolStripMenuItem";
        	this.channelMonitorToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
        	this.channelMonitorToolStripMenuItem.Text = "Channel &Monitor";
        	this.channelMonitorToolStripMenuItem.Click += new System.EventHandler(this.ChannelMonitorMenuClick);
        	// 
        	// menuProfileOptions
        	// 
        	this.menuProfileOptions.Name = "menuProfileOptions";
        	this.menuProfileOptions.Size = new System.Drawing.Size(164, 22);
        	this.menuProfileOptions.Text = "&Profile Options";
        	this.menuProfileOptions.Click += new System.EventHandler(this.MenuProfileOptionsClick);
        	// 
        	// menuGlobalOptions
        	// 
        	this.menuGlobalOptions.Name = "menuGlobalOptions";
        	this.menuGlobalOptions.Size = new System.Drawing.Size(164, 22);
        	this.menuGlobalOptions.Text = "&Global Options";
        	this.menuGlobalOptions.Click += new System.EventHandler(this.OptionsMenuClick);
        	// 
        	// exitToolStripMenuItem
        	// 
        	this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
        	this.exitToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
        	this.exitToolStripMenuItem.Text = "E&xit";
        	this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitMenuClick);
        	// 
        	// scriptingToolStripMenuItem
        	// 
        	this.scriptingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
        	        	        	this.editToolStripMenuItem,
        	        	        	this.outputToolStripMenuItem});
        	this.scriptingToolStripMenuItem.Name = "scriptingToolStripMenuItem";
        	this.scriptingToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
        	this.scriptingToolStripMenuItem.Text = "&Script";
        	// 
        	// editToolStripMenuItem
        	// 
        	this.editToolStripMenuItem.Name = "editToolStripMenuItem";
        	this.editToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
        	this.editToolStripMenuItem.Text = "&Edit";
        	this.editToolStripMenuItem.Click += new System.EventHandler(this.ScriptEditMenuClick);
        	// 
        	// outputToolStripMenuItem
        	// 
        	this.outputToolStripMenuItem.Name = "outputToolStripMenuItem";
        	this.outputToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
        	this.outputToolStripMenuItem.Text = "&Output";
        	this.outputToolStripMenuItem.Click += new System.EventHandler(this.ScriptOutputMenuClick);
        	// 
        	// helpToolStripMenuItem
        	// 
        	this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
        	        	        	this.manualToolStripMenuItem,
        	        	        	this.menuAbout});
        	this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
        	this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
        	this.helpToolStripMenuItem.Text = "&Help";
        	// 
        	// manualToolStripMenuItem
        	// 
        	this.manualToolStripMenuItem.Name = "manualToolStripMenuItem";
        	this.manualToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
        	this.manualToolStripMenuItem.Text = "&Manual";
        	this.manualToolStripMenuItem.Click += new System.EventHandler(this.ManualMenuClick);
        	// 
        	// menuAbout
        	// 
        	this.menuAbout.Name = "menuAbout";
        	this.menuAbout.Size = new System.Drawing.Size(114, 22);
        	this.menuAbout.Text = "&About";
        	this.menuAbout.Click += new System.EventHandler(this.MenuAboutClick);
        	// 
        	// label2
        	// 
        	this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
        	this.label2.Location = new System.Drawing.Point(0, 22);
        	this.label2.Name = "label2";
        	this.label2.Size = new System.Drawing.Size(730, 2);
        	this.label2.TabIndex = 23;
        	this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        	// 
        	// buttonScript
        	// 
        	this.buttonScript.Location = new System.Drawing.Point(306, 106);
        	this.buttonScript.Name = "buttonScript";
        	this.buttonScript.Size = new System.Drawing.Size(75, 23);
        	this.buttonScript.TabIndex = 15;
        	this.buttonScript.Text = "Edit Script";
        	this.buttonScript.UseVisualStyleBackColor = true;
        	this.buttonScript.Click += new System.EventHandler(this.ScriptEditMenuClick);
        	// 
        	// MainForm
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(730, 549);
        	this.Controls.Add(this.buttonScript);
        	this.Controls.Add(this.label2);
        	this.Controls.Add(this.buttonNewProfile);
        	this.Controls.Add(this.buttonProtocolSetup);
        	this.Controls.Add(this.buttonPortSetup);
        	this.Controls.Add(this.buttonBitmappedButton);
        	this.Controls.Add(this.comboProtocol);
        	this.Controls.Add(this.label6);
        	this.Controls.Add(this.statusStrip1);
        	this.Controls.Add(this.menuStrip1);
        	this.Controls.Add(this.label5);
        	this.Controls.Add(this.label4);
        	this.Controls.Add(this.buttonDeleteProfile);
        	this.Controls.Add(this.buttonSaveProfile);
        	this.Controls.Add(this.buttonLoadProfile);
        	this.Controls.Add(this.comboProfiles);
        	this.Controls.Add(this.label3);
        	this.Controls.Add(this.comboJoysticks);
        	this.Controls.Add(this.buttonPortsRefresh);
        	this.Controls.Add(this.buttonConnect);
        	this.Controls.Add(this.label1);
        	this.Controls.Add(this.comboPorts);
        	this.Controls.Add(this.buttonAddButton);
        	this.Controls.Add(this.buttonAddAxis);
        	this.Controls.Add(this.panelMappings);
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
        	this.Icon = global::vJoySerialFeeder.Resources.Joystick_icon;
        	this.MainMenuStrip = this.menuStrip1;
        	this.MaximizeBox = false;
        	this.Name = "MainForm";
        	this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        	this.Text = "vJoySerialFeeder";
        	this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
        	this.statusStrip1.ResumeLayout(false);
        	this.statusStrip1.PerformLayout();
        	this.menuStrip1.ResumeLayout(false);
        	this.menuStrip1.PerformLayout();
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }
        private System.Windows.Forms.ToolStripMenuItem menuAbout;
        private System.Windows.Forms.ToolStripMenuItem menuProfileOptions;
        private System.Windows.Forms.Button buttonScript;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripMenuItem manualToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem outputToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scriptingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuGlobalOptions;
        private System.Windows.Forms.ToolStripMenuItem channelMonitorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem programToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Button buttonNewProfile;
        private System.Windows.Forms.Button buttonProtocolSetup;
        private System.Windows.Forms.Button buttonPortSetup;
        private System.Windows.Forms.Button buttonBitmappedButton;
        private System.Windows.Forms.ComboBox comboProtocol;
        private System.Windows.Forms.Label label6;
	}
}
