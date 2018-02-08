/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 6.2.2018 г.
 * Time: 20:01 ч.
 */
using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Description of ButtonMapping.
	/// </summary>
	
	[DataContract]
	public class ButtonMapping : Mapping
	{
		[DataContract]
		public struct ButtonParameters {
			[DataMember]
			public int thresh1, thresh2;
			[DataMember]
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
		
		[DataMember]
		public uint Button = 1;
		
		[DataMember]
		public ButtonParameters Parameters = new ButtonParameters {
			thresh1 = 1500
		};
		
		private bool pushed;
		

		private FlowLayoutPanel panel;
		private NumericUpDown channelSpinner;
		private NumericUpDown buttonSpinner;
		private Label inputLabel;
		private PictureBox buttonStateBox;
		
		static private Brush BRUSH_PUSHED = Brushes.LightGreen;
		static private Brush BRUSH_NOT_PUSHED = new SolidBrush(Color.FromArgb(0, 64, 0));
		

		public override Mapping Copy()
		{
			var bm = new ButtonMapping();
			bm.Parameters = Parameters;
			bm.Button = Button;
			bm.Channel = Channel;
			
			return bm;
		}
		
		
		public override void WriteJoystick()
		{
			pushed = Parameters.Transform(ChannelValue);
			MainForm.Instance.VJoy.SetButton(pushed, Button);
		}
		
		public override void Paint()
		{
			inputLabel.Text = ChannelValue.ToString();
			buttonStateBox.Invalidate();
		}
		
		public override System.Windows.Forms.Control GetControl()
		{
			if(panel == null)
				initializePanel();
			return panel;
		}
		
		private void onChannelChange(object sender, EventArgs e)
		{
			Channel = (int)channelSpinner.Value - 1;
		}
		
		private void onButtonChange(object sender, EventArgs e)
		{
			Button = (uint)buttonSpinner.Value;
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
					Parameters = dialog.Parameters;
			}
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
			channelSpinner.Value = Channel + 1;
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
			buttonSpinner.Value = Button;
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
	}
}
