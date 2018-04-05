/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 5.4.2018 г.
 * Time: 17:13 ч.
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Description of About.
	/// </summary>
	public partial class AboutForm : Form
	{
		public AboutForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			// remove zeros at the end of version
			labelVersion.Text = "v" + Regex.Replace(Assembly.GetEntryAssembly().GetName().Version.ToString(), @"[\.0]+$", "");
		}
		
		void LinkLabel1LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(linkLabel1.Text);
		}
	}
}
