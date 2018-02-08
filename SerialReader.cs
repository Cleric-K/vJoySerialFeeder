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
	/// The initial structure of this reader is based on code at https://github.com/aanon4/FlySkyIBus
	/// The original code depends on the gap between the frames to determine where a frame starts.
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
	public class SerialReader
	{
		
		/// The reader is implemented as state machine, switching states
		/// depending on what part of the frame it is reading
		enum State
		{
			GET_LENGTH,
			GET_DATA,
			GET_CHKSUML,
			GET_CHKSUMH,
			DISCARD,
		};

		const byte PROTOCOL_OVERHEAD = 3; // Overhead is <len> + <chkl><chkh>
		const byte PROTOCOL_COMMAND40 = 0x40; // Command is always 0x40
		const int PROTOCOL_MAX_LENGTH = 0xff;
		const int MAX_DISCARD_RETRIES = 3;
  
		State state;
		readonly byte[] buf = new byte[1024]; // general storage for the frame bytes
		
		// the following vars are used as indexes in the buf array
		int buf_start = 0, // index of the current start of a (potential) frame
			buf_current = 0, // index of the current byte in the buffer
			buf_len = 0;  // used buffer length
		int data_start; // index of the start of command byte and payload in the buffer 
		int data_len; // length of the payload
		UInt16 chksum;
		byte lchksum;
		int frame_length = PROTOCOL_MAX_LENGTH;
		int numDiscards = 0;
        
		
		private SerialPort serialPort;
		
		
		public SerialReader(SerialPort serialPort)
		{
			this.serialPort = serialPort;
			serialPort.ReadTimeout = 100;
		}
		
		
		public int ReadChannels()
		{
			byte v;
			state = State.GET_LENGTH;
				
			while (true) {	
				if (buf_current == buf_len) {
					// reached the end of the buffer, get more data
					var bytes_to_get = frame_length - (buf_start - buf_current);
					
					if(buf_len + bytes_to_get > buf.Length) {
						// buffer overflow
						buf_current = buf_start = buf_len = 0;
						return 0;
					}
						
					// read data and put at the end of the buffer
					buf_len += serialPort.Read(buf, buf_len, bytes_to_get);
				}
				
				// get next byte
				v = buf[buf_current++];
				    
				switch (state) {
					case State.GET_LENGTH:
				    		
						if (v > PROTOCOL_OVERHEAD && v <= frame_length) {
							data_start = buf_current;
							data_len = (byte)(v - PROTOCOL_OVERHEAD);
							chksum = (UInt16)(0xFFFF - v);
							state = State.GET_DATA;
						} else {
							// not a valid frame length
							state = State.DISCARD;
						}
						break;
				
					case State.GET_DATA:
						// skip throught the data bytes and update the checksum
						chksum -= v;
						if (buf_current - data_start == data_len) {
							// done with the data, get the checksum LSB
							state = State.GET_CHKSUML;
						}
						break;
				        
					case State.GET_CHKSUML:
						lchksum = v;
						// next get the checksum MSB
						state = State.GET_CHKSUMH;
				        
						break;
				
					case State.GET_CHKSUMH:
				        // Validate checksum
						if (chksum == (v << 8) + lchksum) {
				        	// Valid packet
				        	
				        	frame_length = data_len + PROTOCOL_OVERHEAD; // used for next serial read
							numDiscards = 0; // reset discards
				        	
							// Execute command - we only know command 0x40
							switch (buf[data_start]) {
								case PROTOCOL_COMMAND40:
				              		// Valid - extract channel data
									var data_end = data_start + data_len;
									data_start++; // skip command byte
									int ch = 0;
				              		while(data_start < data_end) {
										MainForm.Instance.Channels[ch++] = (buf[data_start] | (buf[data_start + 1] << 8));
										data_start += 2;
									}
									
									state = State.GET_LENGTH;
									moveBuffer();
									
									return ch;
				
								default:
									// unknown command
									break;
							}
						} else {
							// wrong checksum
							state = State.DISCARD;
							if(numDiscards++ < MAX_DISCARD_RETRIES)
							{
								// Usually if it is a real bit error we should just try and read the next
								// packet.
								moveBuffer();
								state = State.GET_LENGTH;
							}
							// else if we are getting too many consecutive discards we should
							// try to re-sync							
						}
						break;
			
				}
				
				if (state == State.DISCARD) {
					// re-syncing. try to parse the packet from the next byte. 
					buf_current = ++buf_start;
					
					state = State.GET_LENGTH;
					frame_length = PROTOCOL_MAX_LENGTH; // reset frame length
				}
			}
				

		}
		
		private void moveBuffer()
		{
			// Move the remaining buffer to the beginning
			// This is not as efficient as using a ring buffer or something like that
			// but is much less code.
        	Array.Copy(buf, buf_current, buf, 0, buf_len - buf_current);
			buf_len -= buf_current;
			buf_start = buf_current = 0;
		}
		

	}
}
