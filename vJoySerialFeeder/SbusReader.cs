/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 24.2.2018 г.
 * Time: 12:57 ч.
 */
using System;
using System.IO.Ports;

namespace vJoySerialFeeder
{
	/// <summary>
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
	
		public override void Start()
		{
			Buffer.FrameLength = FRAME_LENGTH;
			serialPort.ReadTimeout = 500;
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
			
			// too lazy to compute the offsets myself ... took them from
			// https://github.com/zendes/SBUS/blob/master/SBUS.cpp
			channelData[0]  = ((Buffer[1]    |Buffer[2]<<8)                 & 0x07FF);
			channelData[1]  = ((Buffer[2]>>3 |Buffer[3]<<5)                 & 0x07FF);
			channelData[2]  = ((Buffer[3]>>6 |Buffer[4]<<2 |Buffer[5]<<10)  & 0x07FF);
			channelData[3]  = ((Buffer[5]>>1 |Buffer[6]<<7)                 & 0x07FF);
			channelData[4]  = ((Buffer[6]>>4 |Buffer[7]<<4)                 & 0x07FF);
			channelData[5]  = ((Buffer[7]>>7 |Buffer[8]<<1 |Buffer[9]<<9)   & 0x07FF);
			channelData[6]  = ((Buffer[9]>>2 |Buffer[10]<<6)                & 0x07FF);
			channelData[7]  = ((Buffer[10]>>5|Buffer[11]<<3)                & 0x07FF);
			channelData[8]  = ((Buffer[12]   |Buffer[13]<<8)                & 0x07FF);
			channelData[9]  = ((Buffer[13]>>3|Buffer[14]<<5)                & 0x07FF);
			channelData[10] = ((Buffer[14]>>6|Buffer[15]<<2|Buffer[16]<<10) & 0x07FF);
			channelData[11] = ((Buffer[16]>>1|Buffer[17]<<7)                & 0x07FF);
			channelData[12] = ((Buffer[17]>>4|Buffer[18]<<4)                & 0x07FF);
			channelData[13] = ((Buffer[18]>>7|Buffer[19]<<1|Buffer[20]<<9)  & 0x07FF);
			channelData[14] = ((Buffer[20]>>2|Buffer[21]<<6)                & 0x07FF);
			channelData[15] = ((Buffer[21]>>5|Buffer[22]<<3)                & 0x07FF);
			
			// do not check the flags byte, we don't really need anything from there
			
			Buffer.Slide(FRAME_LENGTH);
			return 16;
		}
		
		public override Configuration.SerialParameters GetDefaultSerialParameters()
		{
			Configuration.SerialParameters p;
			
			p.BaudRate = 100000;
			p.DataBits = 8;
			p.Parity = Parity.Even;
			p.StopBits = StopBits.Two;
			
			return p;
		}
	}
}
