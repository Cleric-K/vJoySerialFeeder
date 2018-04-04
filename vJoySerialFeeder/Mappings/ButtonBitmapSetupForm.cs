/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 19.2.2018 г.
 * Time: 21:48 ч.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Description of ButtonBitmapSetupForm.
	/// </summary>
	public partial class ButtonBitmapSetupForm : Form
	{
		public ButtonBitmapMapping.BitButtonParameters Parameters { get; private set; }

		public ButtonBitmapSetupForm(int bit, ButtonBitmapMapping.BitButtonParameters Params)
		{
			InitializeComponent();
			CenterToScreen();

			Text = "Setup Bit "+bit;

			checkEnable.Checked = Params.Enabled;
			numericButton.Value = Params.Button+1;
			checkInvert.Checked = Params.Invert;
			comboFailsafe.SelectedIndex = Params.Failsafe;
			checkTriggerEnable.Checked = Params.Trigger;
			comboTrigerEdge.SelectedIndex = (int)Params.TriggerEdge;
			numericTriggerDuration.Value = Params.TriggerDuration == 0 ? TriggerState.DEFAULT_DURATION : Params.TriggerDuration;

			onEnableCheck(null, null);
		}

		void ButtonCancelClick(object sender, EventArgs e)
		{
			Close();
		}

		void ButtonOKClick(object sender, EventArgs e)
		{
			Parameters = new ButtonBitmapMapping.BitButtonParameters() {
				Enabled = checkEnable.Checked,
				Button = (int)numericButton.Value-1,
				Invert = checkInvert.Checked,
				Failsafe = comboFailsafe.SelectedIndex,
				Trigger = checkTriggerEnable.Checked,
				TriggerEdge = (TriggerState.Edge)comboTrigerEdge.SelectedIndex,
				TriggerDuration = (int)numericTriggerDuration.Value
			};
			
			DialogResult = DialogResult.OK;
			Close();
		}

		private void onEnableCheck(object sender, EventArgs e)
		{
			var c = checkEnable.Checked;

			numericButton.Enabled = c;
			label1.Enabled = c;
			checkInvert.Enabled = c;
			comboFailsafe.Enabled = c;
			checkTriggerEnable.Enabled = c;
			
			var tc = checkTriggerEnable.Checked & c;
			comboTrigerEdge.Enabled = tc;
			numericTriggerDuration.Enabled = tc;
		}
	}
}
