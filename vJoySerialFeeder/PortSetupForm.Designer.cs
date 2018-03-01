/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 1.3.2018 г.
 * Time: 11:49 ч.
 */
namespace vJoySerialFeeder
{
	partial class PortSetupForm
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
			this.radioDefault = new System.Windows.Forms.RadioButton();
			this.radioCustom = new System.Windows.Forms.RadioButton();
			this.comboDataBits = new System.Windows.Forms.ComboBox();
			this.comboParity = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.comboStopBits = new System.Windows.Forms.ComboBox();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.textBaudrate = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// radioDefault
			// 
			this.radioDefault.Location = new System.Drawing.Point(13, 13);
			this.radioDefault.Name = "radioDefault";
			this.radioDefault.Size = new System.Drawing.Size(139, 24);
			this.radioDefault.TabIndex = 0;
			this.radioDefault.TabStop = true;
			this.radioDefault.Text = "Use Protocol Defaults";
			this.radioDefault.UseVisualStyleBackColor = true;
			this.radioDefault.CheckedChanged += new System.EventHandler(this.useCustomChanged);
			// 
			// radioCustom
			// 
			this.radioCustom.Location = new System.Drawing.Point(13, 44);
			this.radioCustom.Name = "radioCustom";
			this.radioCustom.Size = new System.Drawing.Size(104, 24);
			this.radioCustom.TabIndex = 1;
			this.radioCustom.TabStop = true;
			this.radioCustom.Text = "Custom";
			this.radioCustom.UseVisualStyleBackColor = true;
			this.radioCustom.CheckedChanged += new System.EventHandler(this.useCustomChanged);
			// 
			// comboDataBits
			// 
			this.comboDataBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboDataBits.FormattingEnabled = true;
			this.comboDataBits.Items.AddRange(new object[] {
									"1",
									"2",
									"3",
									"4",
									"5",
									"6",
									"7",
									"8"});
			this.comboDataBits.Location = new System.Drawing.Point(74, 99);
			this.comboDataBits.Name = "comboDataBits";
			this.comboDataBits.Size = new System.Drawing.Size(78, 21);
			this.comboDataBits.TabIndex = 3;
			// 
			// comboParity
			// 
			this.comboParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboParity.FormattingEnabled = true;
			this.comboParity.Location = new System.Drawing.Point(74, 126);
			this.comboParity.Name = "comboParity";
			this.comboParity.Size = new System.Drawing.Size(78, 21);
			this.comboParity.TabIndex = 4;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 97);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 23);
			this.label1.TabIndex = 4;
			this.label1.Text = "Data bits:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(12, 124);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(39, 23);
			this.label2.TabIndex = 5;
			this.label2.Text = "Parity:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(12, 151);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(56, 23);
			this.label3.TabIndex = 6;
			this.label3.Text = "Stop bits:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboStopBits
			// 
			this.comboStopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboStopBits.FormattingEnabled = true;
			this.comboStopBits.Location = new System.Drawing.Point(74, 153);
			this.comboStopBits.Name = "comboStopBits";
			this.comboStopBits.Size = new System.Drawing.Size(78, 21);
			this.comboStopBits.TabIndex = 5;
			// 
			// buttonOK
			// 
			this.buttonOK.Location = new System.Drawing.Point(98, 199);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(54, 23);
			this.buttonOK.TabIndex = 7;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.ButtonOKClick);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(17, 199);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 6;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.ButtonCancelClick);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(12, 71);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(56, 23);
			this.label4.TabIndex = 10;
			this.label4.Text = "Baud rate:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textBaudrate
			// 
			this.textBaudrate.Location = new System.Drawing.Point(74, 73);
			this.textBaudrate.Name = "textBaudrate";
			this.textBaudrate.Size = new System.Drawing.Size(78, 20);
			this.textBaudrate.TabIndex = 2;
			// 
			// PortSetupForm
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(164, 234);
			this.Controls.Add(this.textBaudrate);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.comboStopBits);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.comboParity);
			this.Controls.Add(this.comboDataBits);
			this.Controls.Add(this.radioCustom);
			this.Controls.Add(this.radioDefault);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PortSetupForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Framing";
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.TextBox textBaudrate;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.ComboBox comboStopBits;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboParity;
		private System.Windows.Forms.ComboBox comboDataBits;
		private System.Windows.Forms.RadioButton radioCustom;
		private System.Windows.Forms.RadioButton radioDefault;
	}
}
