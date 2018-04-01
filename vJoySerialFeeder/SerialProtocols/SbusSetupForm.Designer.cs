/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 6.3.2018 г.
 * Time: 10:42 ч.
 */
namespace vJoySerialFeeder
{
	partial class SbusSetupForm
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
			this.radioPrescale = new System.Windows.Forms.RadioButton();
			this.radioRaw = new System.Windows.Forms.RadioButton();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// radioPrescale
			// 
			this.radioPrescale.Location = new System.Drawing.Point(9, 19);
			this.radioPrescale.Name = "radioPrescale";
			this.radioPrescale.Size = new System.Drawing.Size(67, 24);
			this.radioPrescale.TabIndex = 0;
			this.radioPrescale.TabStop = true;
			this.radioPrescale.Text = "OpenTX";
			this.toolTip1.SetToolTip(this.radioPrescale, "With default settings, OpenTX sends channel values\r\nin the range of 172 - 1811.\r\n" +
						"Select this option if you would like to pre-scale\r\nthese values to the standard " +
						"1000 - 2000 range.");
			this.radioPrescale.UseVisualStyleBackColor = true;
			// 
			// radioRaw
			// 
			this.radioRaw.Location = new System.Drawing.Point(9, 49);
			this.radioRaw.Name = "radioRaw";
			this.radioRaw.Size = new System.Drawing.Size(50, 24);
			this.radioRaw.TabIndex = 1;
			this.radioRaw.TabStop = true;
			this.radioRaw.Text = "Raw";
			this.toolTip1.SetToolTip(this.radioRaw, "Do not pre-scale the input values.");
			this.radioRaw.UseVisualStyleBackColor = true;
			// 
			// buttonOK
			// 
			this.buttonOK.Location = new System.Drawing.Point(94, 99);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(75, 23);
			this.buttonOK.TabIndex = 3;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.ButtonOKClick);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(13, 99);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.ButtonCancelClick);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radioPrescale);
			this.groupBox1.Controls.Add(this.radioRaw);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(157, 81);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Input Pre-scale";
			// 
			// toolTip1
			// 
			this.toolTip1.AutoPopDelay = 10000;
			this.toolTip1.InitialDelay = 100;
			this.toolTip1.IsBalloon = true;
			this.toolTip1.ReshowDelay = 100;
			// 
			// SbusSetupForm
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(182, 133);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SbusSetupForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "SBUS Setup";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.RadioButton radioRaw;
		private System.Windows.Forms.RadioButton radioPrescale;
	}
}
