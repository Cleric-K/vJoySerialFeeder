/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 2.4.2018 г.
 * Time: 22:21 ч.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace vJoySerialFeeder
{
	partial class ProfileOptions
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
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.numericFailsafeUpdateRate = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.numericFailsafeTime = new System.Windows.Forms.NumericUpDown();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericFailsafeUpdateRate)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericFailsafeTime)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.numericFailsafeUpdateRate);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.numericFailsafeTime);
			this.groupBox1.Location = new System.Drawing.Point(13, 13);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(212, 84);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Failsafe";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(170, 45);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(36, 20);
			this.label3.TabIndex = 5;
			this.label3.Text = "ms";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(7, 45);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(78, 20);
			this.label4.TabIndex = 4;
			this.label4.Text = "Update Rate";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// numericFailsafeUpdateRate
			// 
			this.numericFailsafeUpdateRate.Location = new System.Drawing.Point(91, 45);
			this.numericFailsafeUpdateRate.Maximum = new decimal(new int[] {
									10000,
									0,
									0,
									0});
			this.numericFailsafeUpdateRate.Minimum = new decimal(new int[] {
									1,
									0,
									0,
									0});
			this.numericFailsafeUpdateRate.Name = "numericFailsafeUpdateRate";
			this.numericFailsafeUpdateRate.Size = new System.Drawing.Size(73, 20);
			this.numericFailsafeUpdateRate.TabIndex = 3;
			this.toolTip1.SetToolTip(this.numericFailsafeUpdateRate, "When Failsafe is active Scripts and Iteractions\r\nwill still be executed at this r" +
						"ate.");
			this.numericFailsafeUpdateRate.Value = new decimal(new int[] {
									1,
									0,
									0,
									0});
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(170, 19);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(36, 20);
			this.label2.TabIndex = 2;
			this.label2.Text = "ms";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(7, 19);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(78, 20);
			this.label1.TabIndex = 1;
			this.label1.Text = "Activate After";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// numericFailsafeTime
			// 
			this.numericFailsafeTime.Location = new System.Drawing.Point(91, 19);
			this.numericFailsafeTime.Maximum = new decimal(new int[] {
									100000,
									0,
									0,
									0});
			this.numericFailsafeTime.Minimum = new decimal(new int[] {
									1,
									0,
									0,
									0});
			this.numericFailsafeTime.Name = "numericFailsafeTime";
			this.numericFailsafeTime.Size = new System.Drawing.Size(73, 20);
			this.numericFailsafeTime.TabIndex = 0;
			this.toolTip1.SetToolTip(this.numericFailsafeTime, "When Serial Communication is unsuccessful for\r\nthis amount of milliseconds, Fails" +
						"afe will be activated.");
			this.numericFailsafeTime.Value = new decimal(new int[] {
									1,
									0,
									0,
									0});
			// 
			// buttonOK
			// 
			this.buttonOK.Location = new System.Drawing.Point(143, 104);
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
			this.buttonCancel.Location = new System.Drawing.Point(62, 104);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.ButtonCancelClick);
			// 
			// ProfileOptions
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(240, 137);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ProfileOptions";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Profile Options";
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numericFailsafeUpdateRate)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericFailsafeTime)).EndInit();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.NumericUpDown numericFailsafeTime;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.NumericUpDown numericFailsafeUpdateRate;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox groupBox1;
	}
}
