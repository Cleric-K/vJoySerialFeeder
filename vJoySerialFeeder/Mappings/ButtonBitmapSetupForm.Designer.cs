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
			((System.ComponentModel.ISupportInitialize)(this.numericButton)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(33, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(50, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "Button";
			// 
			// numericButton
			// 
			this.numericButton.Location = new System.Drawing.Point(111, 38);
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
			this.checkInvert.Location = new System.Drawing.Point(33, 64);
			this.checkInvert.Name = "checkInvert";
			this.checkInvert.Size = new System.Drawing.Size(126, 22);
			this.checkInvert.TabIndex = 2;
			this.checkInvert.Text = "Invert";
			this.checkInvert.UseVisualStyleBackColor = true;
			// 
			// buttonOK
			// 
			this.buttonOK.Location = new System.Drawing.Point(99, 138);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(75, 23);
			this.buttonOK.TabIndex = 4;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.ButtonOKClick);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(18, 138);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 3;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.ButtonCancelClick);
			// 
			// checkEnable
			// 
			this.checkEnable.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkEnable.Location = new System.Drawing.Point(33, 12);
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
			this.comboFailsafe.Location = new System.Drawing.Point(96, 93);
			this.comboFailsafe.Name = "comboFailsafe";
			this.comboFailsafe.Size = new System.Drawing.Size(62, 21);
			this.comboFailsafe.TabIndex = 5;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(33, 96);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(50, 20);
			this.label2.TabIndex = 6;
			this.label2.Text = "Failsafe";
			// 
			// ButtonBitmapSetupForm
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(186, 173);
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
			this.ResumeLayout(false);
		}
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
