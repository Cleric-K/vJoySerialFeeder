/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 8.2.2018 г.
 * Time: 16:34 ч.
 */
using System;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;

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
		const int DEFAULT_UPDATE_RATE = 10;
		const byte MSP_RC = 105; // get RC channels command code
		static readonly byte[] RC_COMMAND = new byte[] {(byte)'$', (byte)'M', (byte)'<', 0/*size*/, MSP_RC/*command*/, MSP_RC/*checksum*/};
		const int PROTOCOL_MAX_LENGTH = 0xff;
		const int COMMAND_INDEX = 4;

		private double lastSuccessfulRead;
		private int updateRate;
	
		
		public override void Start()
		{
			parseConfig(config);
			
			serialPort.ReadTimeout = 500;
			serialPort.WriteTimeout = 500;
			Buffer.FrameLength = PROTOCOL_MAX_LENGTH;
			lastSuccessfulRead = 0;
		}
		
		public override void Stop()
		{
		}
		
		public override int ReadChannels()
		{
			try {
				int idx;
				int ch = 0;
				byte checksum, len;
				double now, timeDiff;
				
				now = MainForm.Now;
				timeDiff = now - lastSuccessfulRead; // time elapsed since last read
				
				if(timeDiff < updateRate) {
					// not yet time for update 
					Thread.Sleep((int)(updateRate - timeDiff));
					now = MainForm.Now;
				}
				
				if(Buffer.Empty)
					// send request to the FC
					serialPort.Write(RC_COMMAND, 0, RC_COMMAND.Length);
				
				lastSuccessfulRead = 0;
	
				if(!(Buffer[0] == (byte)'$' && Buffer[1] == (byte)'M' && Buffer[2] == (byte)'>')) {
					// incorrect magic signature, try parsing from next index
					System.Diagnostics.Debug.WriteLine("Resyncing");
					Buffer.Slide(1);
					lastSuccessfulRead = 0;
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
	
						Buffer.FrameLength = idx; // use for next get
						
						lastSuccessfulRead = now;
					}
					else {
						System.Diagnostics.Debug.WriteLine("Unexpected MSP resopnse command");
					}
				}
				else {
					System.Diagnostics.Debug.WriteLine("Bad checksum");
				}
				
				Buffer.Slide(idx);
				return ch;
				
			}
			catch(TimeoutException ex) {
				// if timeout occurs we better send another request asap
				Buffer.Clear();
				lastSuccessfulRead = 0;
				throw ex;
			}
		}
		
		
		public override Configuration.SerialParameters GetDefaultSerialParameters()
		{
			return new Configuration.SerialParameters() {
				BaudRate = 115200,
				DataBits = 8,
				Parity = Parity.None,
				StopBits = StopBits.One
			};
		}
		
		public override bool Configurable { get { return true; } }
		
		/// <summary>
		/// Show the MultiWii configuration
		/// </summary>
		/// <param name="config"></param>
		/// <returns></returns>
		public override string Configure(string config)
		{
			parseConfig(config);
			using(var d = new MultiWiiSetupForm(updateRate)) {
				d.ShowDialog();
				if(d.DialogResult == DialogResult.OK) {
					updateRate = d.UpdateRate;
					return buildConfig();
				}
				return null;
			}
		}
		
		public override string ProtocolName { get { return "MultiWii"; } }
		
		/// <summary>
		/// MultiWii configuration - just a simple number for the update rate
		/// </summary>
		/// <param name="config"></param>
		/// <returns></returns>
		private void parseConfig(string config) {
			try {
				updateRate = int.Parse(config);
			}
			catch(Exception) {
				updateRate = DEFAULT_UPDATE_RATE;
			}
		}
		
		private string buildConfig() {
			return updateRate.ToString();
		}
	}
}
