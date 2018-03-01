/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 8.2.2018 г.
 * Time: 16:34 ч.
 */
using System;
using System.IO.Ports;
using System.Timers;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Description of MultiWiiReader.
	/// 
	/// From http://www.multiwii.com/wiki/index.php?title=Multiwii_Serial_Protocol
	/// 
	/// <preamble>,<direction>,<size>,<command>,,<crc>
	/// Where:
	/// preamble = the ASCII characters '$M'
	/// direction = the ASCII character '<' if to the MWC or '>' if from the MWC
	/// size = number of data bytes, binary. Can be zero as in the case of a data request to the MWC
	/// command = message_id as per the table below
	/// data = as per the table below. UINT16 values are LSB first.
	/// crc = XOR of <size>, <command> and each data byte into a zero'ed sum
	/// </summary>
	public class MultiWiiReader : SerialReader
	{
		const byte MSP_RC = 105; // get RC channels command code
		static readonly byte[] RC_COMMAND = new byte[] {(byte)'$', (byte)'M', (byte)'<', 0/*size*/, MSP_RC/*command*/, MSP_RC/*checksum*/};
		const int PROTOCOL_MAX_LENGTH = 0xff;
		const int COMMAND_INDEX = 4;
		
			
		private Timer timer;
	
		
		public override void Start()
		{
			serialPort.ReadTimeout = 500;
			Buffer.FrameLength = PROTOCOL_MAX_LENGTH;
			
			// request RC data from MultiWii every 25 milliseconds
			timer = new Timer(25);
			timer.Elapsed += delegate(object sender, ElapsedEventArgs e) {
					serialPort.Write(RC_COMMAND, 0, RC_COMMAND.Length); 
				};
			timer.Start();
		}
		
		public override void Stop()
		{
			timer.Stop();
		}
		
		public override int ReadChannels()
		{
			int idx;
			int ch = 0;
			byte checksum, len;

			if(!(Buffer[0] == (byte)'$' && Buffer[1] == (byte)'M' && Buffer[2] == (byte)'>')) {
				// incorrect magic signature, try parsing from next index
				System.Diagnostics.Debug.WriteLine("Resyncing");
				Buffer.Slide(1);
				return 0;
			}
			
			len = Buffer[3];
			checksum = len;
			idx = 4;
			
			while(idx < 5 + len) {
				checksum ^= Buffer[idx++];
			}
			
			if(Buffer[idx++] == checksum) {
				// correct checksum
				if(Buffer[4] == MSP_RC) {
					var data_start = 5; // first data byte
					while(data_start + 1 < 5 + len)
						channelData[ch++] = (Buffer[data_start++] | (Buffer[data_start++] << 8));

					Buffer.FrameLength = idx + 1; // use for next get
				}
			}
			// else incorrect checksum
			
			Buffer.Slide(idx);
			return ch;
			
		}
		
		
		public override Configuration.SerialParameters GetDefaultSerialParameters()
		{
			Configuration.SerialParameters p;
			p.BaudRate = 115200;
			p.DataBits = 8;
			p.Parity = Parity.None;
			p.StopBits = StopBits.One;
			
			return p;
		}
		
	}
}
