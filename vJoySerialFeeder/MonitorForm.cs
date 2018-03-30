/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 12.2.2018 г.
 * Time: 12:14 ч.
 */
using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Draw dialog to show raw channel data
	/// </summary>
	public partial class MonitorForm : Form
	{
		private int numChannels = 0;
		private const int CHAN_WIDTH = 40,
					SLIDER_TOP = 30,
					SLIDER_HEIGHT = 70;

		private int min = -1, max = -1;
		private static Pen linePen = new Pen(Color.Blue, 2);

		public MonitorForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();

			panel.MouseEnter += (s, e) => panel.Focus(); // to allow wheel scroll
			
		}



		private void onChannelDataUpdate(object sender, EventArgs e) {
			pictureBox.Invalidate();
		}

		void PictureBoxPaint(object sender, PaintEventArgs e)
		{
			if(MainForm.Instance.ActiveChannels == 0 && min == -1) {
				e.Graphics.DrawString("No channel data", DefaultFont, Brushes.Blue, 0, 0);
				return;
			}

			if(MainForm.Instance.ActiveChannels != numChannels) {
				numChannels = MainForm.Instance.ActiveChannels;
				pictureBox.Width = 30 + CHAN_WIDTH*numChannels;
			}

			for(int i=0; i < numChannels; i++) {
				int v = MainForm.Instance.Channels[i];
				if(max == -1 || v > max)
					max = v;
				if(min == -1 || v < min)
					min = v;
			}

			e.Graphics.DrawString(max.ToString(), DefaultFont, Brushes.Blue, 0, SLIDER_TOP);
			e.Graphics.DrawString(min.ToString(), DefaultFont, Brushes.Blue, 0, SLIDER_TOP + SLIDER_HEIGHT - 15);

			for(int i=0; i < numChannels; i++) {
				int org = 30 + i*CHAN_WIDTH;
				int v = MainForm.Instance.Channels[i];

				e.Graphics.DrawString("ch"+(i+1)+"\n"+v, DefaultFont, Brushes.Blue, org, 0);

				e.Graphics.DrawLine(linePen, org+CHAN_WIDTH/2, SLIDER_TOP, org+CHAN_WIDTH/2, SLIDER_TOP + SLIDER_HEIGHT);

				float r;
				if(min == max)
					r = .5f;
				else
					r = ((float)max-v)/(max-min);
				r = 70*r;
				e.Graphics.FillEllipse(Brushes.Blue, org+CHAN_WIDTH/2 - 5, SLIDER_TOP+r-5, 9, 9);
			}
		}
		
		void MonitorFormVisibleChanged(object sender, EventArgs e)
		{
			if(Visible)
				MainForm.Instance.ChannelDataUpdate += onChannelDataUpdate;
			else
				MainForm.Instance.ChannelDataUpdate -= onChannelDataUpdate;
		}
		
		void MonitorFormFormClosing(object sender, FormClosingEventArgs e)
		{
			Hide();
			e.Cancel = true;
		}
	}
}
