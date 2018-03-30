/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 30.3.2018 г.
 * Time: 14:30 ч.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Description of LuaOutput.
	/// </summary>
	public partial class LuaOutputForm : Form
	{
		static LuaOutputForm instance;
		
		public LuaOutputForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			instance = this;
		}
		
		public static void Write(string s) {
			if(instance.Visible && !instance.checkStopOutput.Checked)
				instance.Invoke((Action)(()=>instance._write(s)));
		}
		
		private void _write(string s) {
			textBox.AppendText(s);
			textBox.SelectionStart = textBox.Text.Length;
			textBox.SelectionLength = 0;
			textBox.ScrollToCaret();
		}
		
		void ButtonClearClick(object sender, EventArgs e)
		{
			textBox.Clear();
		}
		
		void LuaOutputFormFormClosing(object sender, FormClosingEventArgs e)
		{
			Hide();
			e.Cancel = true;
		}
		
		void ButtonCloseClick(object sender, EventArgs e)
		{
			Hide();
		}
		
		void CopyToolStripMenuItemClick(object sender, EventArgs e)
		{
			textBox.Copy();
		}
		
		void ContextMenuStrip1Opening(object sender, System.ComponentModel.CancelEventArgs e)
		{
			copyToolStripMenuItem.Enabled = textBox.SelectionLength > 0;
		}
	}
}
