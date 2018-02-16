/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 6.2.2018 г.
 * Time: 20:01 ч.
 */
namespace vJoySerialFeeder
{
	partial class ButtonSetupForm
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
			this.pictureBox = new System.Windows.Forms.PictureBox();
			this.labelCalibrate = new System.Windows.Forms.Label();
			this.checkInvert = new System.Windows.Forms.CheckBox();
			this.numericThresh1 = new System.Windows.Forms.NumericUpDown();
			this.numericThresh2 = new System.Windows.Forms.NumericUpDown();
			this.buttonCalibrate = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonOK = new System.Windows.Forms.Button();
			this.labelThresh2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.checkTwoThresholds = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericThresh1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericThresh2)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox
			// 
			this.pictureBox.Location = new System.Drawing.Point(48, 12);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(180, 180);
			this.pictureBox.TabIndex = 0;
			this.pictureBox.TabStop = false;
			this.pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureBoxPaint);
			// 
			// labelCalibrate
			// 
			this.labelCalibrate.Location = new System.Drawing.Point(122, 210);
			this.labelCalibrate.Name = "labelCalibrate";
			this.labelCalibrate.Size = new System.Drawing.Size(134, 53);
			this.labelCalibrate.TabIndex = 33;
			// 
			// checkInvert
			// 
			this.checkInvert.Location = new System.Drawing.Point(12, 239);
			this.checkInvert.Name = "checkInvert";
			this.checkInvert.Size = new System.Drawing.Size(104, 24);
			this.checkInvert.TabIndex = 32;
			this.checkInvert.Text = "Invert";
			this.checkInvert.UseVisualStyleBackColor = true;
			this.checkInvert.CheckedChanged += new System.EventHandler(this.OnChange);
			// 
			// numericThresh1
			// 
			this.numericThresh1.Location = new System.Drawing.Point(84, 268);
			this.numericThresh1.Maximum = new decimal(new int[] {
									16777216,
									0,
									0,
									0});
			this.numericThresh1.Name = "numericThresh1";
			this.numericThresh1.Size = new System.Drawing.Size(53, 20);
			this.numericThresh1.TabIndex = 31;
			this.numericThresh1.Value = new decimal(new int[] {
									1500,
									0,
									0,
									0});
			this.numericThresh1.ValueChanged += new System.EventHandler(this.OnChange);
			// 
			// numericThresh2
			// 
			this.numericThresh2.Location = new System.Drawing.Point(84, 288);
			this.numericThresh2.Maximum = new decimal(new int[] {
									16777216,
									0,
									0,
									0});
			this.numericThresh2.Name = "numericThresh2";
			this.numericThresh2.Size = new System.Drawing.Size(53, 20);
			this.numericThresh2.TabIndex = 30;
			this.numericThresh2.Value = new decimal(new int[] {
									1750,
									0,
									0,
									0});
			this.numericThresh2.ValueChanged += new System.EventHandler(this.OnChange);
			// 
			// buttonCalibrate
			// 
			this.buttonCalibrate.Location = new System.Drawing.Point(181, 266);
			this.buttonCalibrate.Name = "buttonCalibrate";
			this.buttonCalibrate.Size = new System.Drawing.Size(75, 23);
			this.buttonCalibrate.TabIndex = 28;
			this.buttonCalibrate.Text = "Calibrate";
			this.buttonCalibrate.UseVisualStyleBackColor = true;
			this.buttonCalibrate.Click += new System.EventHandler(this.ButtonCalibrateClick);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Location = new System.Drawing.Point(129, 372);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 27;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.ButtonCancelClick);
			// 
			// buttonOK
			// 
			this.buttonOK.Location = new System.Drawing.Point(210, 372);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(46, 23);
			this.buttonOK.TabIndex = 26;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.ButtonOKClick);
			// 
			// labelThresh2
			// 
			this.labelThresh2.Location = new System.Drawing.Point(12, 286);
			this.labelThresh2.Name = "labelThresh2";
			this.labelThresh2.Size = new System.Drawing.Size(67, 20);
			this.labelThresh2.TabIndex = 21;
			this.labelThresh2.Text = "Threshold 2";
			this.labelThresh2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 266);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(67, 20);
			this.label1.TabIndex = 20;
			this.label1.Text = "Threshold 1";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// checkTwoThresholds
			// 
			this.checkTwoThresholds.Location = new System.Drawing.Point(12, 210);
			this.checkTwoThresholds.Name = "checkTwoThresholds";
			this.checkTwoThresholds.Size = new System.Drawing.Size(104, 24);
			this.checkTwoThresholds.TabIndex = 19;
			this.checkTwoThresholds.Text = "Two thresholds";
			this.checkTwoThresholds.UseVisualStyleBackColor = true;
			this.checkTwoThresholds.CheckedChanged += new System.EventHandler(this.OnChange);
			// 
			// ButtonSetupForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(271, 408);
			this.Controls.Add(this.labelCalibrate);
			this.Controls.Add(this.checkInvert);
			this.Controls.Add(this.numericThresh1);
			this.Controls.Add(this.numericThresh2);
			this.Controls.Add(this.buttonCalibrate);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.labelThresh2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.checkTwoThresholds);
			this.Controls.Add(this.pictureBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ButtonSetupForm";
			this.Text = "Button";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericThresh1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericThresh2)).EndInit();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.CheckBox checkTwoThresholds;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label labelThresh2;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonCalibrate;
		private System.Windows.Forms.NumericUpDown numericThresh2;
		private System.Windows.Forms.NumericUpDown numericThresh1;
		private System.Windows.Forms.CheckBox checkInvert;
		private System.Windows.Forms.Label labelCalibrate;
		private System.Windows.Forms.PictureBox pictureBox;
	}
}
