/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 6.2.2018 г.
 * Time: 20:01 ч.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Dialog to setup ButtonMapping
	/// </summary>
	public partial class ButtonSetupForm : Form
	{
		static Pen linePen, inputPen, outputPen;
		
		public ButtonMapping.ButtonParameters Parameters {get ; private set; }
		
		private ButtonMapping buttonMapping;
		private bool initialized = false;
		private int calibrationStep;
		private int calibrationOff, calibrationOn;
		
		static ButtonSetupForm() {
			linePen = new Pen(Color.Blue, 2);
			inputPen = new Pen(Color.Green);
			inputPen.DashStyle = DashStyle.Dash;
			outputPen = new Pen(Color.Red);
			outputPen.DashStyle = DashStyle.Dash;
		}
		
		public ButtonSetupForm(ButtonMapping m)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			buttonMapping = m;
			this.Parameters = buttonMapping.Parameters;
			
			DialogResult = DialogResult.Cancel;
			
			MainForm.Instance.ChannelDataUpdate += onChannelDataUpdate;
		
			Disposed += (object sender, EventArgs e) => {
				MainForm.Instance.ChannelDataUpdate -= onChannelDataUpdate;
			};
			
			checkInvert.Checked = Parameters.invert;
			checkTwoThresholds.Checked = Parameters.notch;
			numericThresh1.Value = Parameters.thresh1;
			numericThresh2.Value = Parameters.thresh2;
			comboFailsafe.SelectedIndex = Parameters.Failsafe;
			
			initialized = true;
			
			OnChange(null, null);
			
		}
		
		
		void OnChange(object sender, EventArgs e) {
			if (!initialized)
				return;
			
			if(!checkTwoThresholds.Checked) {
				numericThresh2.Enabled = false;
				labelThresh2.Enabled = false;
				buttonCalibrate.Enabled = true;
			}
			else {
				numericThresh2.Enabled = true;
				labelThresh2.Enabled = true;
				buttonCalibrate.Enabled = false;
				
				if(numericThresh2.Value < numericThresh1.Value)
					numericThresh2.Value = numericThresh1.Value;
			}
			
			Parameters = new ButtonMapping.ButtonParameters() {
				notch = checkTwoThresholds.Checked,
				invert = checkInvert.Checked,
				thresh1 = (int)numericThresh1.Value,
				thresh2 = (int)numericThresh2.Value,
				Failsafe = comboFailsafe.SelectedIndex
			};
			
			pictureBox.Invalidate();
		}
		
		void ButtonCancelClick(object sender, EventArgs e)
		{
			Close();
		}
		
		void ButtonOKClick(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}
		
		private void onChannelDataUpdate(object sender, EventArgs e)
		{
			pictureBox.Invalidate();
		}
		
		/// <summary>
		/// Draw the button graph
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void PictureBoxPaint(object sender, PaintEventArgs e)
		{
			const int padding = 30;
			int p, x, y;
			bool val;
			var par = Parameters;
			var w = pictureBox.Width - 2*padding;
			var h = pictureBox.Height - 2*padding;
			int max;

			int y1 = !par.invert ? padding + h : padding;
			int y2 = par.invert ? padding + h : padding;

			GraphicsPath graphPath = new GraphicsPath();
			List<Point> points = new List<Point>();

			if(par.notch) {
				max = par.thresh2 + par.thresh1;
				int t1 = (int)(par.thresh1/(double)max*w);
				int t2 = (int)(par.thresh2/(double)max*w);

				points.Add(new Point(padding, y1));     // left start point
				points.Add(new Point(padding+t1, y1));  // left horizontal
				points.Add(new Point(padding+t1, y2));  // left vertical
				points.Add(new Point(padding+t2, y2));  // middle horizontal
				points.Add(new Point(padding+t2, y1));  // right vertical
				points.Add(new Point(padding+w, y1));   // right horizontal
			}
			else {
				max = 2*par.thresh1;

				points.Add(new Point(padding, y1));      // left start point
				points.Add(new Point(padding+w/2, y1));  // left horizontal
				points.Add(new Point(padding+w/2, y2));  // middle vertical
				points.Add(new Point(padding+w, y2));    // right horizontal
			}

			graphPath.AddLines(points.ToArray());
			e.Graphics.DrawPath(linePen, graphPath);

			p = Math.Min(max, Math.Max(0, buttonMapping.Input)); // clamp
			val = par.Transform(p);
			x = padding + (int)(p/(double)max*w);
			y = !par.Transform(p) ? padding + h: padding;

			e.Graphics.DrawLine(inputPen, x, h+padding, x, y);
			e.Graphics.DrawLine(outputPen, padding, y, x, y);
			e.Graphics.DrawString(buttonMapping.Input.ToString(), DefaultFont, Brushes.Green, x, h+padding);
			e.Graphics.DrawString(val ? "On" : "Off", DefaultFont, Brushes.Red, 0, y);
		}
		
		void ButtonCalibrateClick(object sender, EventArgs e)
		{
			
			if(calibrationStep == 0)
				// begin calibration
				checkTwoThresholds.Enabled = false;
			
			switch(calibrationStep++) {
				case 0:
					labelCalibrate.Text = "Set the input to the Off position and press Next";
					buttonCalibrate.Text = "Next";
					break;
				case 1:
					labelCalibrate.Text = "Set the input to the On position and press Done";
					calibrationOff = buttonMapping.Input;
					buttonCalibrate.Text = "Done";
					break;
				case 2:
					labelCalibrate.Text = "Calibration complete";
					buttonCalibrate.Text = "Calibrate";
					calibrationOn = buttonMapping.Input;
					calibrationStep = 0;
					checkTwoThresholds.Enabled = true;
					
					if(calibrationOff < calibrationOn) {
						checkInvert.Checked = false;
						numericThresh1.Value = calibrationOff + (calibrationOn-calibrationOff)/2;
					}
					else {
						checkInvert.Checked = true;
						numericThresh1.Value = calibrationOn + (calibrationOff-calibrationOn)/2;
					}
					
					OnChange(null, null);
					break;
			}
		}
	}
}
