/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 16.6.2018 г.
 * Time: 20:42 ч.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Description of IbusSetupForm.
	/// </summary>
	public partial class IbusSetupForm : Form
	{
		public bool Ia6Ibus { get; private set; }
		public bool Use16bitChannels { get; private set; }
		
		public IbusSetupForm(bool ia6Ibus, bool use16bitChannels)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			checkIa6Ibus.Checked = ia6Ibus;
			checkUse16bitChannels.Checked = use16bitChannels;
		}
		
		void ButtonOKClick(object sender, EventArgs e)
		{
			Ia6Ibus = checkIa6Ibus.Checked;
			Use16bitChannels = checkUse16bitChannels.Checked;
			DialogResult = DialogResult.OK;
			Close();
		}
		
		void ButtonCancelClick(object sender, EventArgs e)
		{
			Close();
		}
	}
}
