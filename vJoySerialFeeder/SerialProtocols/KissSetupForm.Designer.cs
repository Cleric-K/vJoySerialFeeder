/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 1.3.2018 г.
 * Time: 16:44 ч.
 */
namespace vJoySerialFeeder
{
	partial class KissSetupForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KissSetupForm));
			this.label1 = new System.Windows.Forms.Label();
			this.numericRate = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			((System.ComponentModel.ISupportInitialize)(this.numericRate)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(13, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(78, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Update Rate:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// numericRate
			// 
			this.numericRate.Location = new System.Drawing.Point(98, 15);
			this.numericRate.Maximum = new decimal(new int[] {
									1000,
									0,
									0,
									0});
			this.numericRate.Name = "numericRate";
			this.numericRate.Size = new System.Drawing.Size(49, 20);
			this.numericRate.TabIndex = 1;
			this.toolTip1.SetToolTip(this.numericRate, resources.GetString("numericRate.ToolTip"));
			this.numericRate.Value = new decimal(new int[] {
									1,
									0,
									0,
									0});
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(153, 15);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 23);
			this.label2.TabIndex = 2;
			this.label2.Text = "milliseconds";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// buttonOK
			// 
			this.buttonOK.Location = new System.Drawing.Point(150, 61);
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
			this.buttonCancel.Location = new System.Drawing.Point(69, 61);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 4;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.ButtonCancelClick);
			// 
			// toolTip1
			// 
			this.toolTip1.AutomaticDelay = 0;
			this.toolTip1.AutoPopDelay = 15000;
			this.toolTip1.InitialDelay = 100;
			this.toolTip1.IsBalloon = true;
			this.toolTip1.ReshowDelay = 100;
			// 
			// KissSetupForm
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(237, 96);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.numericRate);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "KissSetupForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "KISS Setup";
			((System.ComponentModel.ISupportInitialize)(this.numericRate)).EndInit();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown numericRate;
		private System.Windows.Forms.Label label1;
	}
}
