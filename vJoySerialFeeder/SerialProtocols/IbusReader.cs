/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 9.6.2017 г.
 * Time: 17:47 ч.
 */
using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace vJoySerialFeeder
{
	/// <summary>
	/// For initial reference I looked at the code at https://github.com/aanon4/FlySkyIBus
	/// That code depends on the gap between the frames to determine where a frame starts.
	/// I wanted to make a reader that is independent of inter-frame gap. This is achieved by keeping
	/// a buffer and trying to parse a valid frame starting at different offsets. Once a vlid frame
	/// is found, from then on, frames can be read one after another without checking for a
	/// gap. If for some reason the checksum is wrong for more than three frames we assume that we have
	/// lost sync and we start looking for the beginning of a valid frame anew.
	/// 
	/// IBUS protocol is very simple, but yet it is enough for most uses. It consists of frame with
	/// the following structure:
	/// * 1 byte            -> length of the frame including the length byte itself and checksum.
	/// * (length-3) bytes  -> data
	/// * 2 bytes           -> checksum. It is calculated by taking 0xffff and subtracting each frame byte
	/// 	                       from that. The checksum is stored with LSB first.
	/// 
	/// The 'data' part of the frame has the following format:
	/// * 1 byte           -> command. This determines the payload type - currently only command 0x40 is known
	/// 	                   for 'channel data'
	/// * remaining bytes  -> payload
	/// 	for the 0x40 command, the payload has the form of an array of 16bit integers - one integer
	/// 	for each channel. The integers are stored with LSB first
	/// 
	/// So the general form is:
	/// | frame length |               data                           |   checksum     |
	/// |    <len>     |   <cmd(0x40)> <ch1l><ch1h> <ch2l><ch2h> ...  |  <chkl><chkh>  |
	/// 
	/// </summary>
	public class IbusReader : SerialReader
	{
		const byte PROTOCOL_OVERHEAD = 3; // Overhead is <len> + <chkl><chkh>
		const byte PROTOCOL_COMMAND40 = 0x40; // Command is always 0x40
		const int PROTOCOL_MAX_LENGTH = 0xff;
		const int MAX_CHECKSUM_RETRIES = 3;
		const byte PROTOCOL_IA6_MAGIC = 0x55;
		const int PROTOCOL_IA6_LENGTH = 31;
		const int PROTOCOL_IA6_NUM_CHANNELS = 14;
  
		
		int numDiscards = 0;
		bool ia6Ibus;
		UInt16[] tempIa6Channels;
		
		public override void Start()
		{
			parseConfig(config);
			
			serialPort.ReadTimeout = 500;
			if(ia6Ibus) {
				Buffer.FrameLength = PROTOCOL_IA6_LENGTH;
				tempIa6Channels = new UInt16[PROTOCOL_IA6_NUM_CHANNELS];
			}
			else {
				Buffer.FrameLength = PROTOCOL_MAX_LENGTH;
			}
		}
		
		public override void Stop()
		{
			
		}
		
		public override int ReadChannels() {
			if(ia6Ibus)
				return ReadChannelsIa6();
			else
				return ReadChannelsStandard();
		}
		
		public int ReadChannelsStandard()
		{
			int idx; // index in the buffer
			int data_start; // index of the start of command byte and payload in the buffer 
			int data_len; // length of the payload
			int data_end; // end index of the payload
			UInt16 chksum = 0;
			
			// check length byte
			var len = Buffer[0];
			if(len > PROTOCOL_OVERHEAD) {
				idx = 1;
				data_start = idx;
				data_len = (byte)(len - PROTOCOL_OVERHEAD);
				data_end = data_start + data_len;
				chksum = (UInt16)(0xFFFF - len);
			}
			else {
				// not a valid frame length, try parsing from the next index
				Buffer.Slide(1);
				Buffer.FrameLength = PROTOCOL_MAX_LENGTH;
				System.Diagnostics.Debug.WriteLine("bad start");
				return 0;
			}
			
			// consume all the data
			while(idx < data_start + data_len) {
				chksum -= Buffer[idx++];
			}
			
			// check checksum
			if(chksum == Buffer[idx++] + (Buffer[idx++] << 8)) {
				// Valid packet
				        	
	        	Buffer.FrameLength = data_len + PROTOCOL_OVERHEAD; // used for next serial read
				numDiscards = 0; // reset discards
	        	
				if(Buffer[data_start] == PROTOCOL_COMMAND40) {
					// Execute command - we only know command 0x40
					data_end = data_start + data_len;
					data_start++; // skip command byte
					int ch = 0;
					int index = data_start;
					while (index + 1 < data_end)
						channelData[ch++] = (Buffer[index++] | ((Buffer[index++] & 0x0F) << 8));
					//see https://github.com/betaflight/betaflight/pull/8749
					index = data_start + 1;
					while (index + 1 < data_end) {
						channelData[ch++] = ((Buffer[index] & 0xF0) >> 4) | (Buffer[index + 2] & 0xF0) | ((Buffer[index + 4] & 0xF0) << 4);
						index += 6;
					}
					Buffer.Slide(idx);
					
					return ch;
				}
			}
			else {
				// incorrect checksum
				// wrong checksum
				if(numDiscards++ < MAX_CHECKSUM_RETRIES)
				{
					// Usually if it is a real bit error we should just try and read the next
					// packet.
					Buffer.Slide(idx);
					System.Diagnostics.Debug.WriteLine("bad checksum");
				}
				else {
					// we are getting too many consecutive discards we should
					// try to re-sync							
					Buffer.Slide(1);
					Buffer.FrameLength = PROTOCOL_MAX_LENGTH; // reset frame length
					System.Diagnostics.Debug.WriteLine("bad checksum - resyncing");
				}
			}
			
			return 0;
		}
		
		
		/// <summary>
		/// The IA6 receiver has undocumented IBUS-like output.
		/// See http://endoflifecycle.blogspot.com/2016/10/flysky-ia6-ibus-setup.html
		/// for details.
		/// 
		/// The protocol is also different. See:
		/// https://www.rcgroups.com/forums/showthread.php?2711184-Serial-output-from-FS-IA6-%28Semi-I-BUS%29
		/// 
		/// The frame size is fixed to 31 bytes. There are 14 channels.
		/// 
		/// | magic signature  |              data                |   checksum     |
		/// |    <0x55>        |   <ch1l><ch1h> <ch2l><ch2h> ...  |  <chkl><chkh>  |
		/// The checksum is simply the sum of the 16bit int values of the channels.
		/// 
		/// </summary>
		/// <returns></returns>
		public int ReadChannelsIa6()
		{
			int idx = 0; // index in the buffer
			UInt16 chksum = 0;
			int tempChIdx = 0;
			UInt16 val;
			
			// check magic first byte
			if(Buffer[idx++] != PROTOCOL_IA6_MAGIC) {
				Buffer.Slide(1);
				System.Diagnostics.Debug.WriteLine("bad start - resyncing");
				return 0;
			}

			// consume all the data
			while(true) {
				val = (UInt16)(Buffer[idx++] | (Buffer[idx++] << 8));
				
				if(idx >= PROTOCOL_IA6_LENGTH)
					break; // val will hold the checksum
				
				tempIa6Channels[tempChIdx++] = val;
				chksum += val;
			}
			
			// check checksum
			if(chksum == val) {
				// Valid packet
				tempIa6Channels.CopyTo(channelData, 0);
				Buffer.Slide(idx);
					
				return tempIa6Channels.Length;
			}
			else {
				// incorrect checksum
				Buffer.Slide(idx);
				System.Diagnostics.Debug.WriteLine("bad checksum");
			}
			
			return 0;
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
		/// Show Ibus configuration
		/// </summary>
		/// <param name="config"></param>
		/// <returns></returns>
		public override string Configure(string config)
		{
			parseConfig(config);
			using(var d = new IbusSetupForm(ia6Ibus)) {
				d.ShowDialog();
				if(d.DialogResult == DialogResult.OK) {
					ia6Ibus = d.Ia6Ibus;
					return buildConfig();
				}
				return null;
			}
		}
		
		/// <summary>
		/// Ibus configuration - "ia6" string if IA6 ibus should be used
		/// </summary>
		/// <param name="config"></param>
		/// <returns></returns>
		private void parseConfig(string config) {
			ia6Ibus = config != null && config.Contains("ia6");
		}
		
		private string buildConfig() {
			return ia6Ibus ? "ia6b" : "";
		}
	}
}
