/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 8.6.2017 г.
 * Time: 17:53 ч.
 */
using System;
using System.Drawing.Text;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Drawing;
using System.Xml.Serialization;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Maps a channel to a joystick axis
	/// 
	/// The two main parameters are Channel and Axis
	/// The remaining parameters are stored in a struct AxisParameters
	/// </summary>
	[DataContract]
	public class AxisMapping : Mapping
	{
		/// <summary>
		/// Stores the mapping parameters
		/// </summary>
		[DataContract]
		public struct AxisParameters
		{
			[DataMember]
			public int Min, Max, Center, Expo;
			[DataMember]
			public bool Symmetric, Invert;
			
			/// <summary>
			/// Transforms the raw data to axis value
			/// </summary>
			/// <param name="val">raw channel integer</param>
			/// <returns>double in the range [0; 1]</returns>
			public double Transform(int val) {
				if(Max <= Min || Symmetric && (Center <= Min || Center >= Max))
					// invalid parameters
					return 0;
				
				bool neg = false;
				double v = Math.Max(Min, val);
				v = Math.Min(Max, v);
				
				if(Symmetric) {
					neg = v < Center;
					if(neg)
						v = (Center - v)/(Center - Min);
					else
						v = (v - Center)/(Max - Center);
				}
				else {
					v = (v - Min)/(Max - Min);
				}
				
				if(Expo != 0) {
					double e = Math.Abs(Expo / 100.0);
					if(Expo > 0)
						v = e*v*v*v + (1-e)*v;
					else
						v = e*Math.Pow(v, 0.333) + (1-e)*v;
				}
				
				if(Symmetric) {
					if(neg)
						v = -v;
					v = v/2 + 0.5;
				}
					
				
				if(Invert)
					v = 1-v;
				
				return v;
			}
		};

		
		private static string[] axisNames = new string[] {
			"X",
			"Y",
			"Z",
			"Rx",
			"Ry",
			"Rz",
			"Sl0",
			"Sl1"
		};
		
		private static HID_USAGES[] axisHidUsages = new HID_USAGES[] {
			HID_USAGES.HID_USAGE_X,
			HID_USAGES.HID_USAGE_Y,
			HID_USAGES.HID_USAGE_Z,
			HID_USAGES.HID_USAGE_RX,
			HID_USAGES.HID_USAGE_RY,
			HID_USAGES.HID_USAGE_RZ,
			HID_USAGES.HID_USAGE_SL0,
			HID_USAGES.HID_USAGE_SL1
		};
		
		[DataMember]
		public int Axis;
		
		[DataMember]
		public AxisParameters Parameters = new AxisParameters {
			Min = 1000,
			Max = 2000,
			Center = 1500,
			Expo = 0,
			Symmetric = true
		};
		
		private double lastTransformedValue;
		private FlowLayoutPanel panel;
		private NumericUpDown channelSpinner;
		private ComboBox joystickAxisDropdown;
		private Label inputLabel;
		private PictureBox progressBox;
		
		public override Mapping Copy()
		{
			var am = new AxisMapping();
			am.Parameters = Parameters;
			am.Channel = Channel;
			am.Axis = Axis;
			
			return am;
		}
		
		public override Control GetControl()
		{
			if(panel == null)
				initializePanel();
			return panel;
		}

		
		public override void Paint()
		{
			inputLabel.Text = ChannelValue.ToString();
			progressBox.Invalidate();
		}
		
		public override void WriteJoystick()
		{
			lastTransformedValue = Parameters.Transform(ChannelValue);
			MainForm.Instance.VJoy.SetAxis(lastTransformedValue, axisHidUsages[Axis]);
		}
		
		
		
		private void onRemoveClick(object sender, EventArgs e)
		{
			Remove();
		}
		
		private void onSetupClick(object sender, EventArgs e)
		{
			using (var dialog = new AxisSetupForm(this)) {
				if (dialog.ShowDialog() == DialogResult.OK)
					Parameters = dialog.Parameters;
			}
			
		}
		
		private void onProgressPaint(object sender, PaintEventArgs e)
		{
			// this custom progress bar is based on code from
			// http://csharphelper.com/blog/2016/07/display-text-progressbar-c/
			
			e.Graphics.FillRectangle(
				Brushes.LightGreen, 0, 0, (float)(progressBox.ClientSize.Width * lastTransformedValue),
				progressBox.ClientSize.Height);
			// Draw the text.
			e.Graphics.TextRenderingHint =
		        TextRenderingHint.AntiAliasGridFit;
			using (StringFormat sf = new StringFormat()) {
				sf.Alignment = StringAlignment.Center;
				sf.LineAlignment = StringAlignment.Center;
				int percent = (int)(lastTransformedValue * 100);
				e.Graphics.DrawString(
					percent.ToString() + "%",
					progressBox.Font, Brushes.Black,
					progressBox.ClientRectangle, sf);
			}
		}
		
		private void onChannelChange(object sender, EventArgs e)
		{
			Channel = (int)channelSpinner.Value - 1;
		}
		
		private void onAxisChange(object sender, EventArgs e)
		{
			Axis = joystickAxisDropdown.SelectedIndex;
			
		}
		
		private void initializePanel()
		{
			panel = new FlowLayoutPanel();
			panel.SuspendLayout();
			panel.Size = new Size(600, 30);
			
			var label = new Label();
			label.Text = "Channel:";
			label.Size = new Size(52, 20);
			label.TextAlign = ContentAlignment.MiddleLeft;
			panel.Controls.Add(label);
			
			channelSpinner = new NumericUpDown();
			channelSpinner.Minimum = 1;
			channelSpinner.Maximum = 255;
			channelSpinner.Size = new Size(42, 20);
			channelSpinner.Value = Channel+1;
			channelSpinner.ValueChanged += onChannelChange;
			panel.Controls.Add(channelSpinner);
			
			label = new Label();
			label.Text = "Axis:";
			label.Size = new Size(42, 20);
			label.TextAlign = ContentAlignment.MiddleLeft;
			panel.Controls.Add(label);
			
			joystickAxisDropdown = new ComboBox();
			joystickAxisDropdown.DropDownStyle = ComboBoxStyle.DropDownList;
			joystickAxisDropdown.Size = new Size(42, 20);
			joystickAxisDropdown.Items.AddRange(axisNames);
			joystickAxisDropdown.SelectedIndex = Axis;
			joystickAxisDropdown.SelectedIndexChanged += onAxisChange;
			panel.Controls.Add(joystickAxisDropdown);
			
			label = new Label();
			label.Text = "Input:";
			label.Size = new Size(42, 20);
			label.TextAlign = ContentAlignment.MiddleLeft;
			panel.Controls.Add(label);
				
			inputLabel = new Label();
			inputLabel.Text = "-";
			inputLabel.Size = new Size(42, 20);
			inputLabel.TextAlign = ContentAlignment.MiddleLeft;
			panel.Controls.Add(inputLabel);
			
			label = new Label();
			label.Text = "Output:";
			label.Size = new Size(42, 20);
			label.TextAlign = ContentAlignment.MiddleLeft;
			panel.Controls.Add(label);
			
			progressBox = new PictureBox();
			progressBox.Size = new Size(80, 20);
			progressBox.BorderStyle = BorderStyle.FixedSingle;
			progressBox.Paint += onProgressPaint;
			panel.Controls.Add(progressBox);
			
			var button = new Button();
			button.Text = "Setup";
			button.Click += onSetupClick;
			button.Size = new Size(50, 20);
			panel.Controls.Add(button);
			
			button = new Button();
			button.Text = "Remove";
			button.Click += onRemoveClick;
			button.Size = new Size(55, 20);
			panel.Controls.Add(button);
			
			panel.ResumeLayout();
		}
	}
}
