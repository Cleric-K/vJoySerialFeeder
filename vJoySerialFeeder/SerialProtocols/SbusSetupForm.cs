/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 6.3.2018 г.
 * Time: 10:42 ч.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Description of SbusSetupForm.
	/// </summary>
	public partial class SbusSetupForm : Form
	{
		public bool UseRawInput {get; private set; }
		public bool IgnoreSbusFailsafeFlag {get; private set; }
		
		public SbusSetupForm(bool useRawInput, bool useFailsafe)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			if(useRawInput)
				radioRaw.Checked = true;
			else
				radioPrescale.Checked = true;
			
			checkFailsafe.Checked = useFailsafe;
		}
		
		void ButtonCancelClick(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}
		
		void ButtonOKClick(object sender, EventArgs e)
		{
			UseRawInput = radioRaw.Checked;
			IgnoreSbusFailsafeFlag = checkFailsafe.Checked;
			DialogResult = DialogResult.OK;
			Close();
		}
	}
}
