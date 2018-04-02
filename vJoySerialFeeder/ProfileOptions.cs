/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 2.4.2018 г.
 * Time: 22:21 ч.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Description of ProfileOptions.
	/// </summary>
	public partial class ProfileOptions : Form
	{
		public int FailsafeTime { get; private set; }
		public int FailsafeUpdateRate { get; private set; }
		
		public ProfileOptions(int failsafeTime, int failsafeUpdateRate)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			numericFailsafeTime.Value = failsafeTime;
			numericFailsafeUpdateRate.Value = failsafeUpdateRate;
		}
		
		void ButtonOKClick(object sender, EventArgs e)
		{
			FailsafeTime = (int)numericFailsafeTime.Value;
			FailsafeUpdateRate = (int)numericFailsafeUpdateRate.Value;
			
			DialogResult = DialogResult.OK;
			Close();
		}
		
		void ButtonCancelClick(object sender, EventArgs e)
		{
			Close();
		}
	}
}
