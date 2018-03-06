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
		
		public SbusSetupForm(bool useRawInput)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			if(useRawInput)
				radioRaw.Checked = true;
			else
				radioPrescale.Checked = true;
		}
		
		void ButtonCancelClick(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}
		
		void ButtonOKClick(object sender, EventArgs e)
		{
			UseRawInput = radioRaw.Checked;
			DialogResult = DialogResult.OK;
			Close();
		}
	}
}
