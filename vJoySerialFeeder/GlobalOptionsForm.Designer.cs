/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 28.3.2018 г.
 * Time: 00:02 ч.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace vJoySerialFeeder
{
	partial class GlobalOptionsForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
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
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.numericWSPort = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.checkWSEnable = new System.Windows.Forms.CheckBox();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.checkAutoconnect = new System.Windows.Forms.CheckBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.checkMinimizeToTray = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericWSPort)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.numericWSPort);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.checkWSEnable);
			this.groupBox1.Location = new System.Drawing.Point(13, 70);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(175, 88);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "WebSocket Interaction";
			// 
			// numericWSPort
			// 
			this.numericWSPort.Enabled = false;
			this.numericWSPort.Location = new System.Drawing.Point(64, 54);
			this.numericWSPort.Maximum = new decimal(new int[] {
									65535,
									0,
									0,
									0});
			this.numericWSPort.Minimum = new decimal(new int[] {
									1,
									0,
									0,
									0});
			this.numericWSPort.Name = "numericWSPort";
			this.numericWSPort.Size = new System.Drawing.Size(69, 20);
			this.numericWSPort.TabIndex = 1;
			this.numericWSPort.Value = new decimal(new int[] {
									40000,
									0,
									0,
									0});
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(7, 19);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(51, 24);
			this.label2.TabIndex = 1;
			this.label2.Text = "Enable";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(7, 51);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(51, 23);
			this.label1.TabIndex = 1;
			this.label1.Text = "TCP Port";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// checkWSEnable
			// 
			this.checkWSEnable.Location = new System.Drawing.Point(66, 20);
			this.checkWSEnable.Name = "checkWSEnable";
			this.checkWSEnable.Size = new System.Drawing.Size(27, 24);
			this.checkWSEnable.TabIndex = 0;
			this.checkWSEnable.UseVisualStyleBackColor = true;
			this.checkWSEnable.CheckedChanged += new System.EventHandler(this.CheckWSEnableCheckedChanged);
			// 
			// buttonOK
			// 
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new System.Drawing.Point(113, 164);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(75, 23);
			this.buttonOK.TabIndex = 1;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.ButtonOKClick);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(32, 164);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 0;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// checkAutoconnect
			// 
			this.checkAutoconnect.Location = new System.Drawing.Point(20, 13);
			this.checkAutoconnect.Name = "checkAutoconnect";
			this.checkAutoconnect.Size = new System.Drawing.Size(168, 24);
			this.checkAutoconnect.TabIndex = 2;
			this.checkAutoconnect.Text = "Autoconnect";
			this.toolTip1.SetToolTip(this.checkAutoconnect, "Automatically Connect the last used\r\nprofile on program startup");
			this.checkAutoconnect.UseVisualStyleBackColor = true;
			// 
			// checkMinimizeToTray
			// 
			this.checkMinimizeToTray.Location = new System.Drawing.Point(20, 40);
			this.checkMinimizeToTray.Name = "checkMinimizeToTray";
			this.checkMinimizeToTray.Size = new System.Drawing.Size(168, 24);
			this.checkMinimizeToTray.TabIndex = 3;
			this.checkMinimizeToTray.Text = "Minimize to Tray";
			this.checkMinimizeToTray.UseVisualStyleBackColor = true;
			// 
			// GlobalOptionsForm
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(202, 199);
			this.Controls.Add(this.checkMinimizeToTray);
			this.Controls.Add(this.checkAutoconnect);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "GlobalOptionsForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Global Options";
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numericWSPort)).EndInit();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.CheckBox checkMinimizeToTray;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.CheckBox checkAutoconnect;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.CheckBox checkWSEnable;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown numericWSPort;
		private System.Windows.Forms.GroupBox groupBox1;
	}
}
