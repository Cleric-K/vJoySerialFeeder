/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 9.6.2017 г.
 * Time: 17:47 ч.
 */
using System;
using System.IO.Ports;

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
  
		
		int numDiscards = 0;
		
		public override void Start()
		{
			serialPort.ReadTimeout = 500;
			Buffer.FrameLength = PROTOCOL_MAX_LENGTH;
		}
		
		public override void Stop()
		{
			
		}
		
		public override int ReadChannels()
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
			if(chksum == Buffer[idx] + (Buffer[++idx] << 8)) {
				// Valid packet
				        	
	        	Buffer.FrameLength = data_len + PROTOCOL_OVERHEAD; // used for next serial read
				numDiscards = 0; // reset discards
	        	
				if(Buffer[data_start] == PROTOCOL_COMMAND40) {
					// Execute command - we only know command 0x40
					data_end = data_start + data_len;
					data_start++; // skip command byte
					int ch = 0;
              		while(data_start+1 < data_end) {
						channelData[ch++] = (Buffer[data_start] | (Buffer[data_start + 1] << 8));
						data_start += 2;
					}
					
					Buffer.Slide(idx + 1);
					
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
					Buffer.Slide(idx + 1);
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
		
		
		
		
	}
}
