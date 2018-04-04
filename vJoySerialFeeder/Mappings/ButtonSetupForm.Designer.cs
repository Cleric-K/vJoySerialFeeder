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
			this.label2 = new System.Windows.Forms.Label();
			this.comboFailsafe = new System.Windows.Forms.ComboBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.comboTrigerEdge = new System.Windows.Forms.ComboBox();
			this.numericTriggerDuration = new System.Windows.Forms.NumericUpDown();
			this.checkTriggerEnable = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericThresh1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericThresh2)).BeginInit();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericTriggerDuration)).BeginInit();
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
			this.checkInvert.TabIndex = 1;
			this.checkInvert.Text = "Invert";
			this.checkInvert.UseVisualStyleBackColor = true;
			this.checkInvert.CheckedChanged += new System.EventHandler(this.OnChange);
			// 
			// numericThresh1
			// 
			this.numericThresh1.Location = new System.Drawing.Point(117, 268);
			this.numericThresh1.Maximum = new decimal(new int[] {
									65535,
									0,
									0,
									0});
			this.numericThresh1.Name = "numericThresh1";
			this.numericThresh1.Size = new System.Drawing.Size(53, 20);
			this.numericThresh1.TabIndex = 3;
			this.numericThresh1.Value = new decimal(new int[] {
									1500,
									0,
									0,
									0});
			this.numericThresh1.ValueChanged += new System.EventHandler(this.OnChange);
			// 
			// numericThresh2
			// 
			this.numericThresh2.Location = new System.Drawing.Point(117, 288);
			this.numericThresh2.Maximum = new decimal(new int[] {
									65535,
									0,
									0,
									0});
			this.numericThresh2.Name = "numericThresh2";
			this.numericThresh2.Size = new System.Drawing.Size(53, 20);
			this.numericThresh2.TabIndex = 4;
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
			this.buttonCalibrate.TabIndex = 2;
			this.buttonCalibrate.Text = "Calibrate";
			this.buttonCalibrate.UseVisualStyleBackColor = true;
			this.buttonCalibrate.Click += new System.EventHandler(this.ButtonCalibrateClick);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(129, 445);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 5;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.ButtonCancelClick);
			// 
			// buttonOK
			// 
			this.buttonOK.Location = new System.Drawing.Point(210, 445);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(46, 23);
			this.buttonOK.TabIndex = 6;
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
			this.checkTwoThresholds.TabIndex = 0;
			this.checkTwoThresholds.Text = "Two thresholds";
			this.checkTwoThresholds.UseVisualStyleBackColor = true;
			this.checkTwoThresholds.CheckedChanged += new System.EventHandler(this.OnChange);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(12, 307);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(79, 20);
			this.label2.TabIndex = 35;
			this.label2.Text = "Failsafe Output";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboFailsafe
			// 
			this.comboFailsafe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboFailsafe.FormattingEnabled = true;
			this.comboFailsafe.Items.AddRange(new object[] {
									"Last",
									"Depressed",
									"Pressed"});
			this.comboFailsafe.Location = new System.Drawing.Point(97, 308);
			this.comboFailsafe.Name = "comboFailsafe";
			this.comboFailsafe.Size = new System.Drawing.Size(73, 21);
			this.comboFailsafe.TabIndex = 36;
			this.comboFailsafe.SelectedIndexChanged += new System.EventHandler(this.OnChange);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.comboTrigerEdge);
			this.groupBox1.Controls.Add(this.numericTriggerDuration);
			this.groupBox1.Controls.Add(this.checkTriggerEnable);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Location = new System.Drawing.Point(12, 335);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(168, 100);
			this.groupBox1.TabIndex = 37;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Trigger";
			// 
			// comboTrigerEdge
			// 
			this.comboTrigerEdge.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboTrigerEdge.FormattingEnabled = true;
			this.comboTrigerEdge.Items.AddRange(new object[] {
									"Rising",
									"Falling",
									"Both"});
			this.comboTrigerEdge.Location = new System.Drawing.Point(96, 42);
			this.comboTrigerEdge.Name = "comboTrigerEdge";
			this.comboTrigerEdge.Size = new System.Drawing.Size(62, 21);
			this.comboTrigerEdge.TabIndex = 8;
			this.comboTrigerEdge.SelectedIndexChanged += new System.EventHandler(this.OnChange);
			// 
			// numericTriggerDuration
			// 
			this.numericTriggerDuration.Location = new System.Drawing.Point(110, 69);
			this.numericTriggerDuration.Maximum = new decimal(new int[] {
									1000000,
									0,
									0,
									0});
			this.numericTriggerDuration.Minimum = new decimal(new int[] {
									1,
									0,
									0,
									0});
			this.numericTriggerDuration.Name = "numericTriggerDuration";
			this.numericTriggerDuration.Size = new System.Drawing.Size(48, 20);
			this.numericTriggerDuration.TabIndex = 11;
			this.numericTriggerDuration.Value = new decimal(new int[] {
									1,
									0,
									0,
									0});
			this.numericTriggerDuration.ValueChanged += new System.EventHandler(this.OnChange);
			// 
			// checkTriggerEnable
			// 
			this.checkTriggerEnable.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkTriggerEnable.Location = new System.Drawing.Point(7, 19);
			this.checkTriggerEnable.Name = "checkTriggerEnable";
			this.checkTriggerEnable.Size = new System.Drawing.Size(151, 22);
			this.checkTriggerEnable.TabIndex = 7;
			this.checkTriggerEnable.Text = "Enable";
			this.checkTriggerEnable.UseVisualStyleBackColor = true;
			this.checkTriggerEnable.CheckedChanged += new System.EventHandler(this.OnChange);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(7, 71);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(71, 20);
			this.label4.TabIndex = 10;
			this.label4.Text = "Duration [ms]";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(7, 45);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(50, 20);
			this.label3.TabIndex = 9;
			this.label3.Text = "Edge";
			// 
			// ButtonSetupForm
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(277, 479);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.comboFailsafe);
			this.Controls.Add(this.label2);
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
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Button";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericThresh1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericThresh2)).EndInit();
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numericTriggerDuration)).EndInit();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.CheckBox checkTriggerEnable;
		private System.Windows.Forms.NumericUpDown numericTriggerDuration;
		private System.Windows.Forms.ComboBox comboTrigerEdge;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox comboFailsafe;
		private System.Windows.Forms.Label label2;
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
