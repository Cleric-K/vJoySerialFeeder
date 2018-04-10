/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 10.4.2018 г.
 * Time: 17:01 ч.
 */
namespace vJoySerialFeeder
{
	partial class ImportConfigurationForm
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
			this.checkGlobal = new System.Windows.Forms.CheckBox();
			this.checkProfiles = new System.Windows.Forms.CheckBox();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// checkGlobal
			// 
			this.checkGlobal.Location = new System.Drawing.Point(13, 13);
			this.checkGlobal.Name = "checkGlobal";
			this.checkGlobal.Size = new System.Drawing.Size(165, 24);
			this.checkGlobal.TabIndex = 0;
			this.checkGlobal.Text = "Import Global Options";
			this.checkGlobal.UseVisualStyleBackColor = true;
			// 
			// checkProfiles
			// 
			this.checkProfiles.Checked = true;
			this.checkProfiles.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkProfiles.Location = new System.Drawing.Point(13, 44);
			this.checkProfiles.Name = "checkProfiles";
			this.checkProfiles.Size = new System.Drawing.Size(165, 24);
			this.checkProfiles.TabIndex = 1;
			this.checkProfiles.Text = "Import Profiles";
			this.checkProfiles.UseVisualStyleBackColor = true;
			// 
			// buttonOK
			// 
			this.buttonOK.Location = new System.Drawing.Point(103, 81);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(75, 23);
			this.buttonOK.TabIndex = 2;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.ButtonOKClick);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(22, 81);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 3;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// ImportConfigurationForm
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(190, 116);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.checkProfiles);
			this.Controls.Add(this.checkGlobal);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ImportConfigurationForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Import";
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.CheckBox checkProfiles;
		private System.Windows.Forms.CheckBox checkGlobal;
	}
}
