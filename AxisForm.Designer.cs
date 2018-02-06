/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 8.6.2017 г.
 * Time: 22:44 ч.
 */
namespace vJoySerialFeeder
{
	partial class AxisForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.PictureBox pictureBox;
		private System.Windows.Forms.CheckBox checkSymmetric;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown numericExpo;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonCalibrate;
		private System.Windows.Forms.NumericUpDown numericMax;
		private System.Windows.Forms.NumericUpDown numericCenter;
		private System.Windows.Forms.NumericUpDown numericMin;
		private System.Windows.Forms.CheckBox checkInvert;
		private System.Windows.Forms.Label labelCalibrate;
		
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
			this.checkSymmetric = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.numericExpo = new System.Windows.Forms.NumericUpDown();
			this.label5 = new System.Windows.Forms.Label();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonCalibrate = new System.Windows.Forms.Button();
			this.numericMax = new System.Windows.Forms.NumericUpDown();
			this.numericCenter = new System.Windows.Forms.NumericUpDown();
			this.numericMin = new System.Windows.Forms.NumericUpDown();
			this.checkInvert = new System.Windows.Forms.CheckBox();
			this.labelCalibrate = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericExpo)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericMax)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericCenter)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericMin)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox
			// 
			this.pictureBox.Location = new System.Drawing.Point(48, 12);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(180, 180);
			this.pictureBox.TabIndex = 0;
			this.pictureBox.TabStop = false;
			this.pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureBox1Paint);
			// 
			// checkSymmetric
			// 
			this.checkSymmetric.Location = new System.Drawing.Point(11, 211);
			this.checkSymmetric.Name = "checkSymmetric";
			this.checkSymmetric.Size = new System.Drawing.Size(104, 24);
			this.checkSymmetric.TabIndex = 1;
			this.checkSymmetric.Text = "Symmetric";
			this.checkSymmetric.UseVisualStyleBackColor = true;
			this.checkSymmetric.CheckStateChanged += new System.EventHandler(this.OnChange);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(11, 267);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(67, 20);
			this.label1.TabIndex = 2;
			this.label1.Text = "Minimum";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(11, 287);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(67, 20);
			this.label3.TabIndex = 4;
			this.label3.Text = "Center";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(10, 307);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(67, 20);
			this.label2.TabIndex = 6;
			this.label2.Text = "Maximum";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(11, 327);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(67, 20);
			this.label4.TabIndex = 8;
			this.label4.Text = "Expo";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// numericExpo
			// 
			this.numericExpo.Location = new System.Drawing.Point(83, 329);
			this.numericExpo.Minimum = new decimal(new int[] {
			100,
			0,
			0,
			-2147483648});
			this.numericExpo.Name = "numericExpo";
			this.numericExpo.Size = new System.Drawing.Size(53, 20);
			this.numericExpo.TabIndex = 9;
			this.numericExpo.ValueChanged += new System.EventHandler(this.OnChange);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(143, 330);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(67, 20);
			this.label5.TabIndex = 10;
			this.label5.Text = "%";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// buttonOK
			// 
			this.buttonOK.Location = new System.Drawing.Point(209, 373);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(46, 23);
			this.buttonOK.TabIndex = 11;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.ButtonOKClick);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Location = new System.Drawing.Point(128, 373);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 12;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.ButtonCancelClick);
			// 
			// buttonCalibrate
			// 
			this.buttonCalibrate.Location = new System.Drawing.Point(180, 267);
			this.buttonCalibrate.Name = "buttonCalibrate";
			this.buttonCalibrate.Size = new System.Drawing.Size(75, 23);
			this.buttonCalibrate.TabIndex = 13;
			this.buttonCalibrate.Text = "Calibrate";
			this.buttonCalibrate.UseVisualStyleBackColor = true;
			this.buttonCalibrate.Click += new System.EventHandler(this.ButtonCalibrateClick);
			// 
			// numericMax
			// 
			this.numericMax.Location = new System.Drawing.Point(83, 309);
			this.numericMax.Maximum = new decimal(new int[] {
			16777216,
			0,
			0,
			0});
			this.numericMax.Name = "numericMax";
			this.numericMax.Size = new System.Drawing.Size(53, 20);
			this.numericMax.TabIndex = 14;
			this.numericMax.Value = new decimal(new int[] {
			1000,
			0,
			0,
			0});
			this.numericMax.ValueChanged += new System.EventHandler(this.OnChange);
			// 
			// numericCenter
			// 
			this.numericCenter.Location = new System.Drawing.Point(83, 289);
			this.numericCenter.Maximum = new decimal(new int[] {
			16777216,
			0,
			0,
			0});
			this.numericCenter.Name = "numericCenter";
			this.numericCenter.Size = new System.Drawing.Size(53, 20);
			this.numericCenter.TabIndex = 15;
			this.numericCenter.Value = new decimal(new int[] {
			500,
			0,
			0,
			0});
			this.numericCenter.ValueChanged += new System.EventHandler(this.OnChange);
			// 
			// numericMin
			// 
			this.numericMin.Location = new System.Drawing.Point(83, 269);
			this.numericMin.Maximum = new decimal(new int[] {
			16777216,
			0,
			0,
			0});
			this.numericMin.Name = "numericMin";
			this.numericMin.Size = new System.Drawing.Size(53, 20);
			this.numericMin.TabIndex = 16;
			this.numericMin.ValueChanged += new System.EventHandler(this.OnChange);
			// 
			// checkInvert
			// 
			this.checkInvert.Location = new System.Drawing.Point(11, 240);
			this.checkInvert.Name = "checkInvert";
			this.checkInvert.Size = new System.Drawing.Size(104, 24);
			this.checkInvert.TabIndex = 17;
			this.checkInvert.Text = "Invert";
			this.checkInvert.UseVisualStyleBackColor = true;
			this.checkInvert.CheckStateChanged += new System.EventHandler(this.OnChange);
			// 
			// labelCalibrate
			// 
			this.labelCalibrate.Location = new System.Drawing.Point(108, 211);
			this.labelCalibrate.Name = "labelCalibrate";
			this.labelCalibrate.Size = new System.Drawing.Size(147, 53);
			this.labelCalibrate.TabIndex = 18;
			// 
			// AxisForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(271, 408);
			this.Controls.Add(this.labelCalibrate);
			this.Controls.Add(this.checkInvert);
			this.Controls.Add(this.numericMin);
			this.Controls.Add(this.numericCenter);
			this.Controls.Add(this.numericMax);
			this.Controls.Add(this.buttonCalibrate);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.numericExpo);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.checkSymmetric);
			this.Controls.Add(this.pictureBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AxisForm";
			this.ShowIcon = false;
			this.Text = "AxisForm";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericExpo)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericMax)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericCenter)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericMin)).EndInit();
			this.ResumeLayout(false);

			this.MinimizeBox = false;
			this.Name = "AxisForm";
			this.ShowIcon = false;
			this.Text = "AxisForm";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericExpo)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericMax)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericCenter)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericMin)).EndInit();
			this.ResumeLayout(false);

		}
	}
}
