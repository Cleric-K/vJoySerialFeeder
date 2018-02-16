/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 8.6.2017 г.
 * Time: 22:44 ч.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Dialog to setup AxisMapping
	/// </summary>
	public partial class AxisSetupForm : Form
	{
		static Pen linePen, inputPen, outputPen;
		
		public AxisMapping.AxisParameters Parameters { get { return parameters; } }
		private AxisMapping axisMapping;
		private AxisMapping.AxisParameters parameters;
		
		private bool badValues;
		private bool initialized = false;
		private int calibrationStep = 0;
		
		static AxisSetupForm() {
			linePen = new Pen(Color.Blue, 2);
			inputPen = new Pen(Color.Green);
			inputPen.DashStyle = DashStyle.Dash;
			outputPen = new Pen(Color.Red);
			outputPen.DashStyle = DashStyle.Dash;
		}
		
		public AxisSetupForm(AxisMapping axisMapping)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			this.axisMapping = axisMapping;
			this.parameters = axisMapping.Parameters;
			
			DialogResult = DialogResult.Cancel;
			
			MainForm.Instance.ChannelDataUpdate += onChannelDataUpdate;
		
			Disposed += delegate(object sender, EventArgs e) {
				MainForm.Instance.ChannelDataUpdate -= onChannelDataUpdate;
			};
			
			numericMin.Value = parameters.Min;
			numericMax.Value = parameters.Max;
			numericCenter.Value = parameters.Center;
			numericExpo.Value = parameters.Expo;
			numericDeadband.Value = parameters.Deadband;
			checkInvert.Checked = parameters.Invert;
			checkSymmetric.Checked = parameters.Symmetric;
			
			initialized = true;
			OnChange(null, null);
			
		}
		
		private void onChannelDataUpdate(object sender, EventArgs e)
		{
			pictureBox.Invalidate();
			
			if(calibrationStep == 1) {
				numericCenter.Value = axisMapping.ChannelValue;
			}
			else if(calibrationStep == 2) {
				numericMin.Value = Math.Min(numericMin.Value, axisMapping.ChannelValue);
				numericMax.Value = Math.Max(numericMax.Value, axisMapping.ChannelValue);
			}
		}
		
		
		void OnChange(object sender, EventArgs e)
		{
			if (!initialized)
				return;
			
			badValues = true;
			if (calibrationStep ==0 && numericMin.Value >= numericMax.Value) {
				MessageBox.Show("Min cannot be bigger than Max");
				return;
			}
			if (checkSymmetric.Checked) {
				numericCenter.Enabled = true;
				numericDeadband.Enabled = true;
				if (calibrationStep ==0 && (numericCenter.Value <= numericMin.Value
				          || numericCenter.Value >= numericMax.Value)) {
					MessageBox.Show("Center must be between Min and Max");
					return;
				}
			} else {
				numericCenter.Enabled = false;
				numericDeadband.Enabled = false;
			}
			
			parameters.Min = (int)numericMin.Value;
			parameters.Max = (int)numericMax.Value;
			parameters.Center = (int)numericCenter.Value;
			parameters.Expo = (int)numericExpo.Value;
			parameters.Invert = checkInvert.Checked;
			parameters.Symmetric = checkSymmetric.Checked;
			parameters.Deadband = (int)numericDeadband.Value;
			pictureBox.Invalidate();
			badValues = false;
		}
			
		
		void PictureBox1Paint(object sender, PaintEventArgs e)
		{
			//e.Graphics.DrawLine(Pens.Blue, axisMapping.ChannelValue, 0, 100, 50);
			const int padding = 30;
			int p, x, y;
			double val;
			var w = pictureBox.Width - 2*padding;
			var h = pictureBox.Height - 2*padding;

			GraphicsPath graphPath = new GraphicsPath();
			List<Point> points = new List<Point>();

			for(var i=0; i<=w; i++) {
				p = (int)Math.Round(parameters.Min + (parameters.Max-parameters.Min)*(double)i/w);
				val = parameters.Transform(p);
				y = (int)Math.Round(padding+h-val*h);
				
				points.Add(new Point(padding+i, y));
			}

			graphPath.AddLines(points.ToArray());
			e.Graphics.DrawPath(linePen, graphPath);

			p = Math.Min(parameters.Max, Math.Max(parameters.Min, axisMapping.ChannelValue)); // clamp
			val = parameters.Transform(p);
			if(parameters.Max == parameters.Min)
				x = padding + w/2;
			else
				x = padding + (int)((double)(p-parameters.Min)/(parameters.Max-parameters.Min)*w);
			y = padding + h - (int)(val*h);
			e.Graphics.DrawLine(inputPen, x, h+padding, x, y);
			e.Graphics.DrawLine(outputPen, padding, y, x, y);
			e.Graphics.DrawString(axisMapping.ChannelValue.ToString(), DefaultFont, Brushes.Green, x, h+padding);
			e.Graphics.DrawString(((int)(val*100)).ToString()+"%", DefaultFont, Brushes.Red, 0, y);
		}
		void ButtonCancelClick(object sender, EventArgs e)
		{
			Close();
		}
		void ButtonOKClick(object sender, EventArgs e)
		{
			if (badValues) {
				MessageBox.Show("Incorrect values");
				return;
			}
			DialogResult = DialogResult.OK;
			Close();
		}
		void ButtonCalibrateClick(object sender, EventArgs e)
		{
			if(calibrationStep == 0)
					// begin calibration
				calibrationStep = checkSymmetric.Checked ? 1 : 2;
			else
				calibrationStep++;
			
			
			switch(calibrationStep) {
				case 1:
					labelCalibrate.Text = "Center the input and press Next";
					buttonCalibrate.Text = "Next";
					break;
				case 2:
					labelCalibrate.Text = "Move the input to its extents several times and press Done";
					numericMin.Value = numericMax.Value = axisMapping.ChannelValue;
					buttonCalibrate.Text = "Done";
					break;
				case 3:
					labelCalibrate.Text = "Calibration complete";
					buttonCalibrate.Text = "Calibrate";
					calibrationStep = 0;
					break;
			}
		}
	}
}
