/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 28.3.2018 г.
 * Time: 00:02 ч.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Description of OptionsForm.
	/// </summary>
	public partial class GlobalOptionsForm : Form
	{
		public bool WebSocketEnabled { get; private set; }
		public int WebSocketPort { get; private set; }
		public GlobalOptionsForm(bool wsEn, int wsPort)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			checkWSEnable.Checked = wsEn;
			numericWSPort.Value = wsPort;
		}
		
		void ButtonOKClick(object sender, EventArgs e)
		{
			WebSocketEnabled = checkWSEnable.Checked;
			WebSocketPort = (int)numericWSPort.Value;
			
			//DialogResult = DialogResult.OK;
		}
		
		void CheckWSEnableCheckedChanged(object sender, EventArgs e)
		{
			numericWSPort.Enabled = checkWSEnable.Checked;
		}
	}
}
