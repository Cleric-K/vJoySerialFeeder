/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 26.3.2018 г.
 * Time: 17:19 ч.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Description of DummySetupForm.
	/// </summary>
	public partial class DummySetupForm : Form
	{
		public int UpdateRate { get; private set; }
		
		public DummySetupForm(int updateRate)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			numericRate.Value = updateRate;
		}
		
		void ButtonOKClick(object sender, EventArgs e)
		{
			UpdateRate = (int)numericRate.Value;
			DialogResult = DialogResult.OK;
			Close();
		}
		
		void ButtonCancelClick(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}
	}
}
