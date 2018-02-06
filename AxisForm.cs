/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 8.6.2017 г.
 * Time: 22:44 ч.
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net;
using System.Windows.Forms;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Description of AxisForm.
	/// </summary>

	public partial class AxisForm : Form
	{
		static Pen linePen, inputPen, outputPen;
		
		public AxisMapping.AxisParameters Parameters { get { return parameters; } }
		private AxisMapping axisMapping;
		private AxisMapping.AxisParameters parameters;
		
		private bool badValues;
		private bool initialized = false;
		private int calibrationStep = 0;
		
		static AxisForm() {
			linePen = new Pen(Color.Blue, 2);
			inputPen = new Pen(Color.Green);
			inputPen.DashStyle = DashStyle.Dash;
			outputPen = new Pen(Color.Red);
			outputPen.DashStyle = DashStyle.Dash;
		}
		
		public AxisForm(AxisMapping axisMapping)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			this.axisMapping = axisMapping;
			parameters = axisMapping.Parameters;
			
			DialogResult = DialogResult.Cancel;
			
			MainForm.instance.OnChannelData += onChannelData;
		
			Disposed += delegate(object sender, EventArgs e) {
				MainForm.instance.OnChannelData -= onChannelData;
			};
			
			numericMin.Value = parameters.min;
			numericMax.Value = parameters.max;
			numericCenter.Value = parameters.center;
			numericExpo.Value = parameters.expo;
			checkInvert.Checked = parameters.invert;
			checkSymmetric.Checked = parameters.symmetric;
			
			initialized = true;
			OnChange(null, null);
			
		}
		
		private void onChannelData(object sender, EventArgs e)
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
				if (calibrationStep ==0 && (numericCenter.Value <= numericMin.Value
				          || numericCenter.Value >= numericMax.Value)) {
					MessageBox.Show("Center must be between Min and Max");
					return;
				}
			} else {
				numericCenter.Enabled = false;
			}
			
			parameters.min = (int)numericMin.Value;
			parameters.max = (int)numericMax.Value;
			parameters.center = (int)numericCenter.Value;
			parameters.expo = (int)numericExpo.Value;
			parameters.invert = checkInvert.Checked;
			parameters.symmetric = checkSymmetric.Checked;
			pictureBox.Invalidate();
			badValues = false;
		}
			
		
		void PictureBox1Paint(object sender, PaintEventArgs e)
		{
			//e.Graphics.DrawLine(Pens.Blue, axisMapping.ChannelValue, 0, 100, 50);
			const int padding = 30;
			int p, x, y, lastY = 0;
			double val;
			var w = pictureBox.Width - 2*padding;
			var h = pictureBox.Height - 2*padding;
			
			for(var i=0; i<w; i++) {
				
				p = (int)(parameters.min + (parameters.max-parameters.min)*(double)i/w);
				val = parameters.Transform(p);
				y = (int)(padding+h-val*h);
				
				//System.Diagnostics.Debug.Print((float)(h-lastVal*h)+","+(float)(h-val*h));
				if(i > 0)
					e.Graphics.DrawLine(linePen, padding+i-1, (float)lastY,
					                    padding+i, (float)y);
				
				lastY = y;
			}
			
			p = Math.Min(parameters.max, Math.Max(parameters.min, axisMapping.ChannelValue)); // clamp
			val = parameters.Transform(p);
			if(parameters.max == parameters.min)
				x = padding + w/2;
			else
				x = padding + (int)((double)(p-parameters.min)/(parameters.max-parameters.min)*w);
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
