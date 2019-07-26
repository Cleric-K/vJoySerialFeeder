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
		public bool Autoconnect { get; private set; }
		public bool MinimizeToTray { get; private set; }
		
		public GlobalOptionsForm(bool wsEn, int wsPort, bool autoconnect, bool minimizeToTray)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			checkWSEnable.Checked = wsEn;
			numericWSPort.Value = wsPort;
			checkAutoconnect.Checked = autoconnect;
			checkMinimizeToTray.Checked = minimizeToTray;
		}
		
		void ButtonOKClick(object sender, EventArgs e)
		{
			WebSocketEnabled = checkWSEnable.Checked;
			WebSocketPort = (int)numericWSPort.Value;
			Autoconnect = checkAutoconnect.Checked;
			MinimizeToTray = checkMinimizeToTray.Checked;
		}
		
		void CheckWSEnableCheckedChanged(object sender, EventArgs e)
		{
			numericWSPort.Enabled = checkWSEnable.Checked;
		}
	}
}
