/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 19.2.2018 г.
 * Time: 15:47 ч.
 */
using System;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Maps channel bits to buttons.
	///
	/// Channels are normally 16bit integers and thus we can use them
	/// to map 16 buttons.
	/// Every bit can command one of the vJoy's 128 buttons, with optionally
	/// inverted logic.
	/// </summary>
	[DataContract]
	public class ButtonBitmapMapping : Mapping
	{
		/// <summary>
		/// Carries the config for a single bit/button
		/// </summary>
		[DataContract]
		public struct BitButtonParameters {
			[DataMember]
			public uint Button; // vJoy button id
			[DataMember]
			public bool Invert, Enabled;
		}

		[DataMember]
		public BitButtonParameters[] Parameters = new BitButtonParameters[16];

		static private Brush BRUSH_PUSHED = Brushes.LightGreen;
		static private Brush BRUSH_NOT_PUSHED = new SolidBrush(Color.FromArgb(0, 64, 0));

		static private Font bitFont;
		static private StringFormat bitStyle;

		private Panel panel;
		private NumericUpDown channelSpinner;
		private PictureBox bitsBox;

		private const int BIT_RECT_SIZE = 16;

		static ButtonBitmapMapping() {
			bitFont = new Font("sans serif", 6.5f);

			bitStyle = new StringFormat();
			bitStyle.LineAlignment = StringAlignment.Center;
			bitStyle.Alignment = StringAlignment.Center;
			bitStyle.FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.NoClip;
		}
		
		protected override float Transform(int val)
		{
			for(var i=0; i<16; i++) {
				var p = Parameters[i];
				if(p.Enabled && p.Invert)
					val ^= 1<<i;
			}
			return val;
		}

		public override void UpdateJoystick(VJoy vjoy)
		{
			int v = (int)Output;
			
			for(var i=0; i<16; i++) {
				var p = Parameters[i];
				if(p.Enabled)
					vjoy.SetButton(((v&(1<<i))!=0), p.Button);
			}
		}

		public override void Paint()
		{
			bitsBox.Invalidate();
		}

		public override Control GetControl()
		{
			if(panel == null)
				initializePanel();
			return panel;
		}

		public override Mapping Copy()
		{
			var m = new ButtonBitmapMapping();
			m.Channel = Channel;

			if(Parameters == null)
				// this could happen only if deserializing corrupt profile
				Parameters = new ButtonBitmapMapping.BitButtonParameters[16];

			Array.Copy(Parameters, m.Parameters, Parameters.Length);

			return m;
		}

		private void onChannelChange(object sender, EventArgs e)
		{
			Channel = (int)channelSpinner.Value - 1;
		}

		private void onInputBitsPaint(object sender, PaintEventArgs e)
		{
			var v = Input;

			for(var i=0; i<16; i++) {
				var p = Parameters[i];
				var state = ((v&(1<<i))!=0)^(p.Enabled&p.Invert);
				var bitBrush = state ? BRUSH_PUSHED:BRUSH_NOT_PUSHED;
				var textBrush = state ? Brushes.Black : Brushes.White;
				var pen = state ? Pens.Black : Pens.White;

				var org = i*BIT_RECT_SIZE;

				// draw bit box
				e.Graphics.FillRectangle(bitBrush, org, 0, BIT_RECT_SIZE, BIT_RECT_SIZE);
				e.Graphics.DrawRectangle(Pens.Green, org, 0, BIT_RECT_SIZE, BIT_RECT_SIZE);


				if(p.Enabled) {
					var s = (p.Button+1).ToString();

					e.Graphics.DrawString(s, bitFont, textBrush,
					      new Rectangle(org-1, 0, BIT_RECT_SIZE+2, BIT_RECT_SIZE), bitStyle);

					if(p.Invert)
						// draw the horizontal overbar if inverted
						e.Graphics.DrawLine(pen, org+2, 3, org+BIT_RECT_SIZE-2, 3);
				}
			}
		}

		private void onInputBitsClick(object sender, MouseEventArgs e)
		{
			if(e.X > 16*BIT_RECT_SIZE || e.Y > BIT_RECT_SIZE)
				return;

			int bit = e.X/BIT_RECT_SIZE;

			var dialog = new ButtonBitmapSetupForm(bit, Parameters[bit]);

			if(dialog.ShowDialog() == DialogResult.OK) {
				Parameters[bit] = dialog.Parameters;
				bitsBox.Invalidate();
			}

		}

		private void onRemoveClick(object sender, EventArgs e)
		{
			Remove();
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
			label.Text = "Bits (click to setup):";
			label.Size = new Size(100, 20);
			label.TextAlign = ContentAlignment.MiddleLeft;
			panel.Controls.Add(label);

			bitsBox = new PictureBox();
			bitsBox.Size = new Size(270, 20);
			bitsBox.Paint += onInputBitsPaint;
			bitsBox.MouseClick += onInputBitsClick;
			panel.Controls.Add(bitsBox);

			var button = new Button();
			button.Text = "Remove";
			button.Click += onRemoveClick;
			button.Size = new Size(55, 20);
			panel.Controls.Add(button);

			panel.ResumeLayout();
		}
	}
}
