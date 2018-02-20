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
		public ButtonBitmapMapping.BitButtonParameters Parameters { get {return parameters;} }
		private ButtonBitmapMapping.BitButtonParameters parameters;


		public ButtonBitmapSetupForm(int bit, ButtonBitmapMapping.BitButtonParameters Params)
		{
			InitializeComponent();
			CenterToScreen();

			Text = "Setup Bit "+bit;

			checkEnable.Checked = Params.Enabled;
			numericButton.Value = Params.Button+1;
			checkInvert.Checked = Params.Invert;

			onEnableCheck(null, null);
		}

		void ButtonCancelClick(object sender, EventArgs e)
		{
			Close();
		}

		void ButtonOKClick(object sender, EventArgs e)
		{
			parameters.Enabled = checkEnable.Checked;
			parameters.Button = (uint)numericButton.Value-1;
			parameters.Invert = checkInvert.Checked;

			DialogResult = DialogResult.OK;
			Close();
		}

		private void onEnableCheck(object sender, EventArgs e)
		{
			var c = checkEnable.Checked;

			numericButton.Enabled = c;
			label1.Enabled = c;
			checkInvert.Enabled = c;
		}
	}
}
