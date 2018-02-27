/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 24.2.2018 г.
 * Time: 12:57 ч.
 */
using System;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Description of SbusReader.
	/// </summary>
	public class SbusReader : SerialReader
	{		
		public override void Stop()
		{
		}
		
		public override void Start()
		{
			Buffer.FrameLength = 25; // sbus frame is always 25 bytes
			serialPort.ReadTimeout = 500;
		}
		
		public override int ReadChannels()
		{
			if(Buffer[0] != 0x0f || Buffer[24] != 0x00) {
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
			
			Buffer.Slide(25);
			return 16;
		}
	}
}
