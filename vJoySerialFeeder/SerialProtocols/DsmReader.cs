/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 16.10.2018 г.
 * Time: 22:19 ч.
 */
using System;
using System.IO.Ports;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Description of DsmReader.
	/// </summary>
	public class DsmReader : SerialReader
	{
		const int PROTOCOL_LENGTH = 16;
		
		private enum DsmModes {
			DSM2_22MS_1024 = 0x01,
			DSM2_11MS_2048 = 0x12,
			DSM2_22MS_2048 = 0xa2,
			DSMX_11MS_2048 = 0xb2
		}
		
		private DsmModes mode;
		
		public DsmReader()
		{
		}
		
		public override void Stop()
		{

		}
		
		public override void Start()
		{
			serialPort.ReadTimeout = 500;
			Buffer.FrameLength = 16;
		}
		
		public override int ReadChannels()
		{
			Buffer.FindInterFrame();
			
			mode = (DsmModes)Buffer[1];
			
			for(var i = 0; i < 7; i++) {
				var offset = 2 + 2*i;
				int chVal;
				int chId;
				
				if(mode == DsmModes.DSM2_22MS_1024) {
					// 1024 mode
					chVal = ((Buffer[offset]&3)<<8) + Buffer[offset+1];
					chVal <<= 1; // multiply by 2 to make 0-2048
					chId = (Buffer[offset]>>2)&0xf;
				}
				else if(mode == DsmModes.DSM2_11MS_2048
				        || mode == DsmModes.DSM2_22MS_2048
				        || mode == DsmModes.DSMX_11MS_2048) {
					// 2048 mode
					chVal = ((Buffer[offset]&7)<<8) + Buffer[offset+1];
					chId = (Buffer[offset]>>3)&0xf;
				}
				else
					// unknown protocol
					return 0;
				
				//System.Diagnostics.Debug.WriteLine(Buffer[1] + " "+chId+" "+chVal);
				// map 341/1024/1707 to 1000/1500/2000
				chVal = (int)Math.Round(0.732*chVal + 750.366);
				
				channelData[chId] = chVal;
			}
			
			return 16;
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
	}
}
