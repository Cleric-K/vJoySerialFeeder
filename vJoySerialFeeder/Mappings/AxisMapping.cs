/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 8.6.2017 г.
 * Time: 17:53 ч.
 */
using System;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.Serialization;
using System.Windows.Forms;

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
			public const int DEFAULT_FAILSAFE = -1;
			
			[DataMember]
			public int Min, Max, Center, Expo, Deadband, Failsafe;
			[DataMember]
			public bool Symmetric, Invert;
			
			/// <summary>
			/// Transforms the raw data to axis value
			/// </summary>
			/// <param name="val">raw channel integer</param>
			/// <returns>double in the range [0; 1]</returns>
			public float Transform(int val) {
				if(Max <= Min || Symmetric && (Center <= Min || Center >= Max))
					// invalid parameters
					return 0;
				
				bool neg = false;
				float v = Math.Max(Min, val);
				v = Math.Min(Max, v);
				
				// map v to [0; 1]
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
	
				if(Symmetric && Deadband > 0) {
					float d = Deadband/100.01f; // do not divide at exactly 100 to avoid deadband becoming 1
					if(v < d)
						// inside deadband
						return 0.5f;

					// map [d; 1] -> [0; 1]
					var b = d/(d-1);
					v = (1-b)*v + b;
				}
				
				if(Expo != 0) {
					/// expo is based on Super Rates found in Clean/Beta Flight
					/// super = 1 / (1 - factor*x)
					/// axis = x * super
					/// we also apply normalization to keep max value at 1
					/// super = (1-factor) / (1 - factor*x)
					float factor = Math.Abs(Expo)/ 100.01f; // do not divide at exactly 100 to avoid factor becoming 1
					if(Expo > 0)
						v = v*(1-factor)/(1-v*factor);
					else
						v = v/(factor*v-factor+1); // the inverse function of the above


				}
				
				if(Symmetric) {
					// map [0; 1] to [0; 0.5] if neg
					// map [0; 1] to [0.5; 1] if !neg
					if(neg)
						v = -v;
					v = v/2 + 0.5f;
				}
				
				if(Invert)
					v = 1-v;
				
				return v;
			}
		};
		
		[DataMember]
		public int Axis;
		
		[DataMember]
		public AxisParameters Parameters = new AxisParameters {
			Min = 1000,
			Max = 2000,
			Center = 1500,
			Expo = 0,
			Symmetric = true,
			Failsafe = AxisParameters.DEFAULT_FAILSAFE
		};
		
		private FlowLayoutPanel panel;
		private NumericUpDown channelSpinner;
		private ComboBox joystickAxisDropdown;
		private Label inputLabel;
		private PictureBox progressBox;
		
		protected override float Transform(int val)
		{
			return Parameters.Transform(val);
		}
		
		protected override float Clamp(float val)
		{
			return val > 1f ? 1.0f : val < 0f ? 0f : val;
		}
		
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
			inputLabel.Text = Input.ToString();
			progressBox.Invalidate();
		}
		
		public override void UpdateJoystick(VJoyBase vjoy)
		{
			vjoy.SetAxis(Axis, Output);
		}
		
		public override void Failsafe()
		{
			if(Parameters.Failsafe >= 0)
				Output = Parameters.Failsafe/100.0f;
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
				Brushes.LightGreen, 0, 0, (progressBox.ClientSize.Width * Output),
				progressBox.ClientSize.Height);
			// Draw the text.
			using (StringFormat sf = new StringFormat()) {
				sf.Alignment = StringAlignment.Center;
				sf.LineAlignment = StringAlignment.Center;
				int percent = (int)Math.Round(Output * 100f);
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
			channelSpinner.Minimum = 0;
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
			joystickAxisDropdown.Items.AddRange(Enum.GetNames(typeof(VJoyBase.Axes)));
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
