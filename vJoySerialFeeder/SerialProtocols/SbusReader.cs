/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 24.2.2018 г.
 * Time: 12:57 ч.
 */
using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace vJoySerialFeeder
{
	/// <summary>
	/// !!! NOTE !!! The description below says that the highest bit is sent first but this
	/// does not seem to be the case - at least on FrSky. That's why the bit order if inverted
	/// in the implementation in comparison to the description below
	/// Taken from: https://os.mbed.com/users/Digixx/notebook/futaba-s-bus-controlled-by-mbed/
	/// 
	/// The protocol is 25 Byte long and is send every 14ms (analog mode) or 7ms (highspeed mode).
	///	One Byte = 1 startbit + 8 databit + 1 paritybit + 2 stopbit (8E2), baudrate = 100'000 bit/s
	///	The highest bit is send first. The logic is inverted (Level High = 1)
	///	
	///	[startbyte] [data1] [data2] .... [data22] [flags][endbyte]
	///	
	///	startbyte = 11110000b (0xF0)
	///	
	///	data 1-22 = [ch1, 11bit][ch2, 11bit] .... [ch16, 11bit] (ch# = 0 bis 2047)
	///	channel 1 uses 8 bits from data1 and 3 bits from data2
	///	channel 2 uses last 5 bits from data2 and 6 bits from data3
	///	etc.
	///	
	///	flags = bit7 = ch17 = digital channel (0x80)
	///	bit6 = ch18 = digital channel (0x40)
	///	bit5 = Frame lost, equivalent red LED on receiver (0x20)
	///	bit4 = failsafe activated (0x10)
	///	bit3 = n/a
	///	bit2 = n/a
	///	bit1 = n/a
	///	bit0 = n/a
	///	
	///	endbyte = 00000000b
	/// </summary>
	/// 
	public class SbusReader : SerialReader
	{
		private const byte FIRST_BYTE = 0x0f;
		private const byte LAST_BYTE = 0x00;
		private const int FRAME_LENGTH = 25; // sbus frame is always 25 bytes
		private const int NUM_CHANNELS = 16;
		private const int SBUS_CH_BITS = 11;
		private const int SBUS_CH_MASK = (1<<SBUS_CH_BITS)-1;
		
		private bool useRawInput;
	
		public override void Start()
		{
			Buffer.FrameLength = FRAME_LENGTH;
			serialPort.ReadTimeout = 500;
			
			parseConfig(config);
		}
		
		public override void Stop()
		{
		}
		
		
		public override int ReadChannels()
		{
			if(Buffer[0] != FIRST_BYTE || Buffer[24] != LAST_BYTE) {
				// first and last bytes incorrect, try resyncing
				System.Diagnostics.Debug.WriteLine("resyncing");
				Buffer.Slide(1);
				return 0;
			}
			
			int inputbits = 0;
			int inputbitsavailable = 0;
			int bufIdx = 1;
			
			// channel parser based on
			// https://github.com/opentx/opentx/blob/6bd38ce13a89ade70aa8e83914063464a8d9750a/radio/src/sbus.cpp#L50
			
			for (var i=0; i<NUM_CHANNELS; i++) {
			    while (inputbitsavailable < SBUS_CH_BITS) {
		      		inputbits |= Buffer[bufIdx++] << inputbitsavailable;
		      		inputbitsavailable += 8;
			    }
			
				var v = inputbits & SBUS_CH_MASK;
				
				if(useRawInput) {
					channelData[i] = v;
				}
				else {
					// OpenTX sends channel data with in its own values. We can prescale them to the standard 1000 - 2000 range.
					// Thanks to @fape for providing the raw data:
					// min   mid   max
					// 172   992   1811
	
					// http://www.wolframalpha.com/input/?i=linear+fit+%7B172,+1000%7D,+%7B1811,+2000%7D,+%7B992,+1500%7D
					// slightly adjusted to give better results in integer math
					channelData[i] = (610127*v + 895364000)/1000000;
				}

			    inputbitsavailable -= SBUS_CH_BITS;
			    inputbits >>= SBUS_CH_BITS;
			}	
			
			// do not check the flags byte, we don't really need anything from there	
			
			Buffer.Slide(FRAME_LENGTH);
			return NUM_CHANNELS;
		}
		
		public override bool Configurable {
			get { return true; }
		}
		
		public override string Configure(string config)
		{
			parseConfig(config);
			
			using(var d = new SbusSetupForm(useRawInput)) {
				d.ShowDialog();
				if(d.DialogResult == DialogResult.OK) {
					useRawInput = d.UseRawInput;
					return buildConfig();
				}
					
				return null;
			}
		}
		
		public override Configuration.SerialParameters GetDefaultSerialParameters()
		{
			return new Configuration.SerialParameters() {
				BaudRate = 100000,
				DataBits = 8,
				Parity = Parity.Even,
				StopBits = StopBits.Two
			};
		}
		
		private void parseConfig(string config) {
			useRawInput = "raw".Equals(config);
		}
		
		private string buildConfig() {
			return useRawInput ? "raw" : "";
		}
	}
}
