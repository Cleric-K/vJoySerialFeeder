/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 6.2.2018 г.
 * Time: 20:01 ч.
 */
using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Description of ButtonMapping.
	/// </summary>
	public class ButtonMapping : Mapping
	{
		
		public struct ButtonParameters {
			public int thresh1, thresh2;
			public bool notch, invert;
			
			public bool Transform(int val) {
				bool state;
				if(!notch) {
					state = val >= thresh1;
				}
				else {
					state = val >= thresh1 && val <= thresh2;
				}
				
				return invert ^ state;
			}
		}
		
		private uint button = 1;
		private bool pushed;
		private ButtonParameters parameters = new ButtonParameters {
			thresh1 = 1500
		};
		
		public ButtonParameters Parameters { get { return parameters; } }
		
		private FlowLayoutPanel panel;
		private NumericUpDown channelSpinner;
		private NumericUpDown buttonSpinner;
		private Label inputLabel;
		private PictureBox buttonStateBox;
		
		static private Brush BRUSH_PUSHED = Brushes.LightGreen;
		static private Brush BRUSH_NOT_PUSHED = new SolidBrush(Color.FromArgb(0, 64, 0));
		

		
		public ButtonMapping()
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
			label.Text = "Button:";
			label.Size = new Size(42, 20);
			label.TextAlign = ContentAlignment.MiddleLeft;
			panel.Controls.Add(label);
			
			buttonSpinner = new NumericUpDown();
			buttonSpinner.Minimum = 1;
			buttonSpinner.Maximum = 128;
			buttonSpinner.Size = new Size(42, 20);
			buttonSpinner.ValueChanged += onButtonChange;
			panel.Controls.Add(buttonSpinner);
			
			
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
			
			buttonStateBox = new PictureBox();
			buttonStateBox.Size = new Size(80, 20);
			//progressBox.BorderStyle = BorderStyle.FixedSingle;
			buttonStateBox.Paint += onButtonStatePaint;
			panel.Controls.Add(buttonStateBox);
			
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
		
		public override void WriteChannel()
		{
			pushed = parameters.Transform(ChannelValue);
			MainForm.instance.VJoy.SetButton(pushed, button);
		}
		
		public override void SaveToXmlElement(System.Xml.XmlElement e)
		{
			using(var writer = e.CreateNavigator().AppendChild()) {
				writer.WriteElementString("channel", channel.ToString());
				writer.WriteElementString("button", button.ToString());
				writer.WriteElementString("notch", parameters.notch.ToString().ToLower());
				writer.WriteElementString("invert", parameters.invert.ToString().ToLower());
				writer.WriteElementString("thresh1", parameters.thresh1.ToString());
				if(parameters.notch)
					writer.WriteElementString("thresh2", parameters.thresh2.ToString());
			}
		}
		
		public override void ReadFromXmlElement(System.Xml.XmlElement e)
		{
			using(var reader = e.CreateNavigator().ReadSubtree()) {
				//return;
				while(true) {
					switch(reader.Name) {
						case "channel":
							channel = reader.ReadElementContentAsInt();
							channelSpinner.Value = channel+1;
							break;
						case "button":
							button = (uint)reader.ReadElementContentAsInt();
							buttonSpinner.Value = button;
							break;
						case "notch":
							parameters.notch = reader.ReadElementContentAsBoolean();
							//reader.Read();
							break;
						case "invert":
							parameters.invert = reader.ReadElementContentAsBoolean();
							break;
						case "thresh1":
							parameters.thresh1 = reader.ReadElementContentAsInt();
							break;
						case "thresh2":
							parameters.thresh2 = reader.ReadElementContentAsInt();
							break;
						default:
							if(!reader.Read())
								return;
							break;
					}
				};
			}
		}
		
		public override void Paint()
		{
			inputLabel.Text = ChannelValue.ToString();
			buttonStateBox.Invalidate();
		}
		
		public override System.Windows.Forms.Control GetControl()
		{
			return panel;
		}
		
		
		
		
		
		
		private void onChannelChange(object sender, EventArgs e)
		{
			channel = (int)channelSpinner.Value - 1;
		}
		
		private void onButtonChange(object sender, EventArgs e)
		{
			button = (uint)buttonSpinner.Value;
		}
		
		private void onButtonStatePaint(object sender, PaintEventArgs e)
		{	
			e.Graphics.FillEllipse(pushed ? BRUSH_PUSHED : BRUSH_NOT_PUSHED,
			                       30, 0, 18, 18);
			e.Graphics.DrawEllipse(Pens.Black, 
			                       30, 0, 18, 18);
		}
		
		private void onRemoveClick(object sender, EventArgs e)
		{
			Remove();
		}
		
		private void onSetupClick(object sender, EventArgs e)
		{
			using (var dialog = new ButtonSetupForm(this)) {
				if (dialog.ShowDialog() == DialogResult.OK)
					parameters = dialog.Parameters;
			}
		}
	}
}
