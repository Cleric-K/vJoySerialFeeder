/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 1.3.2018 г.
 * Time: 11:49 ч.
 */
using System;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Description of SerialFramingForm.
	/// </summary>
	public partial class PortSetupForm : Form
	{
		public Configuration.SerialParameters SerialParameters { get; private set; }
		public bool UseCustomSerialParameters { get; private set; }
		
		public PortSetupForm(bool useCustom, Configuration.SerialParameters serParams)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			SerialParameters = serParams;
			
			if(useCustom)
				radioCustom.Checked = true;
			else
				radioDefault.Checked = true;
			
			textBaudrate.Text = serParams.BaudRate.ToString();
			
			comboDataBits.SelectedIndex = serParams.DataBits - 1;
			
			comboParity.Items.AddRange(Enum.GetNames(typeof(Parity)));
			comboStopBits.Items.AddRange(Enum.GetNames(typeof(StopBits)));
			
			comboParity.SelectedIndex = (int)serParams.Parity;
			comboStopBits.SelectedIndex = (int)serParams.StopBits;
		}
		
		
		
		
		void ButtonOKClick(object sender, EventArgs e)
		{
			Configuration.SerialParameters p;
			UseCustomSerialParameters = radioCustom.Checked;
			try {
				p.BaudRate = (int)uint.Parse(textBaudrate.Text);
			}
			catch(Exception) {
				MessageBox.Show("Invalid Baud Rate", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			p.DataBits = comboDataBits.SelectedIndex + 1;
			p.Parity = (Parity)comboParity.SelectedIndex;
			p.StopBits = (StopBits)comboStopBits.SelectedIndex;
			
			SerialParameters = p;
			DialogResult = DialogResult.OK;
			Dispose();
		}
		
		void ButtonCancelClick(object sender, EventArgs e)
		{
			Dispose();
		}
		
		void useCustomChanged(object sender, EventArgs e)
		{
			comboParity.Enabled = comboDataBits.Enabled = comboStopBits.Enabled = textBaudrate.Enabled = !radioDefault.Checked;
		}
	}
}
