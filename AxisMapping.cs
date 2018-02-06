/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 8.6.2017 г.
 * Time: 17:53 ч.
 */
using System;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Xml.Serialization;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Description of Axis.
	/// </summary>
	public class AxisMapping : Mapping
	{
		public struct AxisParameters
		{
			public int min, max, center, expo;
			public bool symmetric, invert;
			
			public double Transform(int val) {
				if(max <= min || symmetric && (center <= min || center >= max))
					return 0;
				
				bool neg = false;
				double v = Math.Max(min, val);
				v = Math.Min(max, v);
				
				if(symmetric) {
					neg = v < center;
					if(neg)
						v = (center - v)/(center - min);
					else
						v = (v - center)/(max - center);
				}
				else {
					v = (v - min)/(max - min);
				}
				
				if(expo != 0) {
					double e = Math.Abs(expo / 100.0);
					if(expo > 0)
						v = e*v*v*v + (1-e)*v;
					else
						v = e*Math.Pow(v, 0.333) + (1-e)*v;
				}
				
				if(symmetric) {
					if(neg)
						v = -v;
					v = v/2 + 0.5;
				}
					
				
				if(invert)
					v = 1-v;
				
				return v;
			}
		};
		
		public AxisParameters Parameters { get { return parameters; } }

		
		public override void ReadFromXmlElement(System.Xml.XmlElement e)
		{
			using(var reader = e.CreateNavigator().ReadSubtree()) {
				while(true) {
					
					
					switch(reader.Name) {
						case "channel":
							channel = reader.ReadElementContentAsInt();
							channelSpinner.Value = channel+1;
							break;
						case "axis":
							axis = reader.ReadElementContentAsInt();
							joystickAxisDropdown.SelectedIndex = axis;
							break;
						case "symmetric":
							parameters.symmetric = reader.ReadElementContentAsBoolean();
							break;
						case "invert":
							parameters.invert = reader.ReadElementContentAsBoolean();
							break;
						case "min":
							parameters.min = reader.ReadElementContentAsInt();
							break;
						case "max":
							parameters.max = reader.ReadElementContentAsInt();
							break;
						case "center":
							parameters.center = reader.ReadElementContentAsInt();
							break;
						case "expo":
							parameters.expo = reader.ReadElementContentAsInt();
							break;
						default:
							if(!reader.Read())
								return;
							break;
					}
				};
			}
		}
		public override void SaveToXmlElement(System.Xml.XmlElement e)
		{
			using(var writer = e.CreateNavigator().AppendChild()) {
				writer.WriteElementString("channel", channel.ToString());
				writer.WriteElementString("axis", axis.ToString());
				writer.WriteElementString("symmetric", parameters.symmetric.ToString().ToLower());
				writer.WriteElementString("invert", parameters.invert.ToString().ToLower());
				writer.WriteElementString("min", parameters.min.ToString());
				writer.WriteElementString("max", parameters.max.ToString());
				writer.WriteElementString("center", parameters.center.ToString());
				writer.WriteElementString("expo", parameters.expo.ToString());
			}
		}

		
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
		
		private int axis;
		private AxisParameters parameters = new AxisParameters {
			min = 1000,
			max = 2000,
			center = 1500,
			expo = 0,
			symmetric = true
		};
		private double lastTransformedValue;
		private FlowLayoutPanel panel;
		private NumericUpDown channelSpinner;
		private ComboBox joystickAxisDropdown;
		private Label inputLabel;
		private PictureBox progressBox;
		
		public AxisMapping(RemoveHandler rh) : base(rh)
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
			joystickAxisDropdown.SelectedIndex = 0;
			axis = 0;
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
		
		public override Control GetControl()
		{
			return panel;
		}
		
		public override void Paint()
		{
			inputLabel.Text = ChannelValue.ToString();
			progressBox.Invalidate();
		}
		
		public override void WriteChannel()
		{
			lastTransformedValue = parameters.Transform(ChannelValue);
			MainForm.instance.VJoy.SetAxis(lastTransformedValue, axisHidUsages[axis]);
		}
		
		
		
		private void onRemoveClick(object sender, EventArgs e)
		{
			Remove();
		}
		
		private void onSetupClick(object sender, EventArgs e)
		{
			using (var dialog = new AxisForm(this)) {
				if (dialog.ShowDialog() == DialogResult.OK)
					parameters = dialog.Parameters;
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
			channel = (int)channelSpinner.Value - 1;
		}
		
		private void onAxisChange(object sender, EventArgs e)
		{
			axis = joystickAxisDropdown.SelectedIndex;
			
		}
	}
}
