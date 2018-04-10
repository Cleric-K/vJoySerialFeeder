/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 10.4.2018 г.
 * Time: 17:01 ч.
 */
using System;
using System.Windows.Forms;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Description of ImportConfigurationForm.
	/// </summary>
	public partial class ImportConfigurationForm : Form
	{
		
		public bool ImportGlobalOptions { get; private set; }
		public bool ImportProfiles { get; private set; }
		
		public ImportConfigurationForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
		}
		
		void ButtonOKClick(object sender, EventArgs e)
		{
			ImportGlobalOptions = checkGlobal.Checked;
			ImportProfiles = checkProfiles.Checked;
			DialogResult = DialogResult.OK;
			Close();
		}
	}
}
