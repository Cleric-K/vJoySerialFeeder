/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 19.2.2018 г.
 * Time: 21:48 ч.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace vJoySerialFeeder
{
	partial class ButtonBitmapSetupForm
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
			this.label1 = new System.Windows.Forms.Label();
			this.numericButton = new System.Windows.Forms.NumericUpDown();
			this.checkInvert = new System.Windows.Forms.CheckBox();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.checkEnable = new System.Windows.Forms.CheckBox();
			this.comboFailsafe = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.checkTriggerEnable = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.comboTrigerEdge = new System.Windows.Forms.ComboBox();
			this.numericTriggerDuration = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			((System.ComponentModel.ISupportInitialize)(this.numericButton)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericTriggerDuration)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(31, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(50, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "Button";
			// 
			// numericButton
			// 
			this.numericButton.Location = new System.Drawing.Point(109, 38);
			this.numericButton.Maximum = new decimal(new int[] {
									128,
									0,
									0,
									0});
			this.numericButton.Minimum = new decimal(new int[] {
									1,
									0,
									0,
									0});
			this.numericButton.Name = "numericButton";
			this.numericButton.Size = new System.Drawing.Size(48, 20);
			this.numericButton.TabIndex = 1;
			this.numericButton.Value = new decimal(new int[] {
									1,
									0,
									0,
									0});
			// 
			// checkInvert
			// 
			this.checkInvert.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkInvert.Location = new System.Drawing.Point(31, 64);
			this.checkInvert.Name = "checkInvert";
			this.checkInvert.Size = new System.Drawing.Size(126, 22);
			this.checkInvert.TabIndex = 2;
			this.checkInvert.Text = "Invert";
			this.checkInvert.UseVisualStyleBackColor = true;
			// 
			// buttonOK
			// 
			this.buttonOK.Location = new System.Drawing.Point(99, 244);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(75, 23);
			this.buttonOK.TabIndex = 5;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.ButtonOKClick);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(18, 244);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 4;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.ButtonCancelClick);
			// 
			// checkEnable
			// 
			this.checkEnable.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkEnable.Location = new System.Drawing.Point(31, 12);
			this.checkEnable.Name = "checkEnable";
			this.checkEnable.Size = new System.Drawing.Size(126, 24);
			this.checkEnable.TabIndex = 0;
			this.checkEnable.Text = "Enabled";
			this.checkEnable.UseVisualStyleBackColor = true;
			this.checkEnable.CheckedChanged += new System.EventHandler(this.onEnableCheck);
			// 
			// comboFailsafe
			// 
			this.comboFailsafe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboFailsafe.FormattingEnabled = true;
			this.comboFailsafe.Items.AddRange(new object[] {
									"Last",
									"Depressed",
									"Pressed"});
			this.comboFailsafe.Location = new System.Drawing.Point(94, 93);
			this.comboFailsafe.Name = "comboFailsafe";
			this.comboFailsafe.Size = new System.Drawing.Size(62, 21);
			this.comboFailsafe.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(31, 96);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(50, 20);
			this.label2.TabIndex = 6;
			this.label2.Text = "Failsafe";
			// 
			// checkTriggerEnable
			// 
			this.checkTriggerEnable.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkTriggerEnable.Location = new System.Drawing.Point(6, 19);
			this.checkTriggerEnable.Name = "checkTriggerEnable";
			this.checkTriggerEnable.Size = new System.Drawing.Size(126, 22);
			this.checkTriggerEnable.TabIndex = 0;
			this.checkTriggerEnable.Text = "Enable";
			this.checkTriggerEnable.UseVisualStyleBackColor = true;
			this.checkTriggerEnable.CheckedChanged += new System.EventHandler(this.onEnableCheck);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(7, 50);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(50, 20);
			this.label3.TabIndex = 9;
			this.label3.Text = "Edge";
			// 
			// comboTrigerEdge
			// 
			this.comboTrigerEdge.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboTrigerEdge.FormattingEnabled = true;
			this.comboTrigerEdge.Items.AddRange(new object[] {
									"Rising",
									"Falling",
									"Both"});
			this.comboTrigerEdge.Location = new System.Drawing.Point(70, 47);
			this.comboTrigerEdge.Name = "comboTrigerEdge";
			this.comboTrigerEdge.Size = new System.Drawing.Size(62, 21);
			this.comboTrigerEdge.TabIndex = 1;
			// 
			// numericTriggerDuration
			// 
			this.numericTriggerDuration.Location = new System.Drawing.Point(84, 74);
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
			this.numericTriggerDuration.TabIndex = 2;
			this.numericTriggerDuration.Value = new decimal(new int[] {
									1,
									0,
									0,
									0});
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(6, 76);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(72, 20);
			this.label4.TabIndex = 10;
			this.label4.Text = "Duration [ms]";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.comboTrigerEdge);
			this.groupBox1.Controls.Add(this.numericTriggerDuration);
			this.groupBox1.Controls.Add(this.checkTriggerEnable);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Location = new System.Drawing.Point(22, 124);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(150, 109);
			this.groupBox1.TabIndex = 12;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Trigger";
			// 
			// ButtonBitmapSetupForm
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(189, 278);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.comboFailsafe);
			this.Controls.Add(this.checkEnable);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.checkInvert);
			this.Controls.Add(this.numericButton);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ButtonBitmapSetupForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "ButtonBitmapSetupForm";
			((System.ComponentModel.ISupportInitialize)(this.numericButton)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericTriggerDuration)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown numericTriggerDuration;
		private System.Windows.Forms.ComboBox comboTrigerEdge;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.CheckBox checkTriggerEnable;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox comboFailsafe;
		private System.Windows.Forms.CheckBox checkEnable;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.CheckBox checkInvert;
		private System.Windows.Forms.NumericUpDown numericButton;
		private System.Windows.Forms.Label label1;
	}
}
