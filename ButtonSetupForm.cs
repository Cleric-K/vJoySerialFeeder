/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 6.2.2018 г.
 * Time: 20:01 ч.
 */
using System;
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
		
		public ButtonMapping.ButtonParameters Parameters {get {return parameters; } }
		 
		private ButtonMapping.ButtonParameters parameters;
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
			this.parameters = buttonMapping.Parameters;
			
			DialogResult = DialogResult.Cancel;
			
			MainForm.Instance.ChannelDataUpdate += onChannelDataUpdate;
		
			Disposed += delegate(object sender, EventArgs e) {
				MainForm.Instance.ChannelDataUpdate -= onChannelDataUpdate;
			};
			
			checkInvert.Checked = parameters.invert;
			checkTwoThresholds.Checked = parameters.notch;
			numericThresh1.Value = parameters.thresh1;
			numericThresh2.Value = parameters.thresh2;
			
			initialized = true;
			
			OnChange(null, null);
			
		}
		
		
		void OnChange(object sender, EventArgs e) {
			if (!initialized)
				return;
			
			if(!checkTwoThresholds.Checked) {
				numericThresh2.Visible = false;
				labelThresh2.Visible = false;
				buttonCalibrate.Visible = true;
			}
			else {
				numericThresh2.Visible = true;
				labelThresh2.Visible = true;
				buttonCalibrate.Visible = false;
				
				if(numericThresh2.Value < numericThresh1.Value)
					numericThresh2.Value = numericThresh1.Value;
			}
			
			parameters.notch = checkTwoThresholds.Checked;
			parameters.invert = checkInvert.Checked;
			parameters.thresh1 = (int)numericThresh1.Value;
			parameters.thresh2 = (int)numericThresh2.Value;
			
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
			var w = pictureBox.Width - 2*padding;
			var h = pictureBox.Height - 2*padding;
			int max;
			
			if(parameters.notch) {
				max = parameters.thresh2 + parameters.thresh1;
				int t1 = (int)(parameters.thresh1/(double)max*w);
				int t2 = (int)(parameters.thresh2/(double)max*w);
				
				y = !parameters.invert ? padding + h : padding;
				// left horizontal segment
				e.Graphics.DrawLine(linePen, padding, y,
				                    	padding+t1, y);
				// left vertical segment
				e.Graphics.DrawLine(linePen, padding+t1, padding,
				                    	padding+t1, padding + h);
				y = parameters.invert ? padding + h : padding;
				// middle horizontal segment
				e.Graphics.DrawLine(linePen, padding+t1, y,
				                    	padding+t2, y);
				// right vertical segment
				e.Graphics.DrawLine(linePen, padding+t2, padding,
				                    	padding+t2, padding + h);
				y = !parameters.invert ? padding + h : padding;
				// right horizontal segment
				e.Graphics.DrawLine(linePen, padding+t2, y,
				                    	padding+w, y);
			}
			else {
				max = 2*parameters.thresh1;
				
				y = !parameters.invert ? padding + h : padding;
				// left horizontal segment
				e.Graphics.DrawLine(linePen, padding, y,
				                    	padding+w/2, y);
				// vertical segment
				e.Graphics.DrawLine(linePen, padding+w/2, padding,
				                    	padding+w/2, padding + h);
				y = parameters.invert ? padding + h : padding;
				// right horizontal segment
				e.Graphics.DrawLine(linePen, padding+w/2, y,
				                    	padding+w, y);
			}
			
			p = Math.Min(max, Math.Max(0, buttonMapping.ChannelValue)); // clamp
			val = parameters.Transform(p);
			x = padding + (int)(p/(double)max*w);
			y = !parameters.Transform(p) ? padding + h: padding;
			
			e.Graphics.DrawLine(inputPen, x, h+padding, x, y);
			e.Graphics.DrawLine(outputPen, padding, y, x, y);
			e.Graphics.DrawString(buttonMapping.ChannelValue.ToString(), DefaultFont, Brushes.Green, x, h+padding);
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
					calibrationOff = buttonMapping.ChannelValue;
					buttonCalibrate.Text = "Done";
					break;
				case 2:
					labelCalibrate.Text = "Calibration complete";
					buttonCalibrate.Text = "Calibrate";
					calibrationOn = buttonMapping.ChannelValue;
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
