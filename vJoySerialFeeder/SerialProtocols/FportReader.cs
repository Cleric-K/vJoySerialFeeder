/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 28.11.2019 г.
 * Time: 21:14 ч.
 * 
 */

using System;
using System.Collections;
using System.IO.Ports;
using System.Windows.Forms;

namespace vJoySerialFeeder
{
	/// <summary>
	/// 
	/// Info at:
	/// https://github.com/betaflight/betaflight/files/1491056/F.Port.protocol.betaFlight.V2.1.2017.11.21.pdf
	/// </summary>
	/// 
	public class FportReader : SbusReader
	{
		private const byte BORDER_BYTE = 0x7e;
		private const byte ESCAPE_BYTE = 0x7d;
		private const byte ESCAPE_MASK = 0x20;
		private const byte CONTROL_FRAME = 0x00;
		private const byte CRC_CORRECT = 0xff;
		private const int FRAME_LENGTH = 25; // sbus frame is always 25 bytes
		private const int NUM_CHANNELS = 16;
		private const int SBUS_CH_BITS = 11;
		private const int SBUS_CH_MASK = (1<<SBUS_CH_BITS)-1;
		private const int SBUS_FAILSAFE_MASK = 1<<3;
		
		private bool useRawInput;
		private bool ignoreSbusFailsafeFlag;
	
		public override void Start()
		{
			//Buffer.FrameLength = FRAME_LENGTH;
			serialPort.ReadTimeout = 500;
			
			parseConfig(config);
		}
		
		public override void Stop()
		{
		}
		
		
		public override int ReadChannels()
		{
			var retries = 0;
			while(retries++ < 3) {
				Buffer.FrameLength = 2;
				
				if(Buffer[0] != BORDER_BYTE) {
					// first byte incorrect
					Buffer.Slide(1);
					System.Diagnostics.Debug.WriteLine("bad start - resyncing");
					return 0;
				}
				
				var data_len = Buffer[1];
				var frame_len = Buffer.FrameLength = 2 + data_len + 2; // start + len + [dataLen] + crc + end byte
				var full_frame_len = frame_len;
				
				// fix escapes
				var tmp = new byte[frame_len*2];
				tmp[0] = Buffer[0];
				tmp[1] = Buffer[1];
				for(int src = 2, dst = 2; dst < full_frame_len; src++, dst++) {
					if(Buffer[dst] == ESCAPE_BYTE) {
						Buffer.FrameLength++;
						full_frame_len++;
						tmp[dst] = Buffer[dst];
						tmp[dst+1] = Buffer[dst+1];
						Buffer[src] = (byte)(Buffer[++dst] ^ ESCAPE_MASK);
						
					}
					else {
						tmp[dst] = Buffer[dst];
						Buffer[src] = Buffer[dst];
					}
				}
				
				if(Buffer[frame_len - 1] != BORDER_BYTE) {
					// end byte incorrect
					Buffer.Slide(1);
					System.Diagnostics.Debug.WriteLine("bad end - resyncing");
					return 0;
				}
				
				if(Buffer[2] != CONTROL_FRAME) {
					// probably downlink frame
					Buffer.Slide(full_frame_len);
					continue;
				}
				
				short crc = 0;
				for(var i=1; i < frame_len - 1; i++) {
					crc += Buffer[i];
				}
				crc = (byte)((crc & 0xff) + (crc >> 8));
				if(crc != CRC_CORRECT) {
					System.Diagnostics.Debug.WriteLine("bad crc");
					Buffer.Slide(full_frame_len);
					return 0;
				}

				
				if(!ignoreSbusFailsafeFlag && (Buffer[25]&SBUS_FAILSAFE_MASK) != 0) {
					Buffer.Slide(full_frame_len);
					throw new FailsafeException("SBUS Failsafe active");
				}

				// Fport uses the same channel format as Sbus				
				DecodeSbusChannels(3);
				
				// do not check the flags byte, we don't really need anything from there	
				Buffer.Slide(full_frame_len);
				return NUM_CHANNELS;
				
			}
			return 0;
		}
		
		public override bool Configurable {
			get { return true; }
		}
		
		public override string Configure(string config)
		{
			parseConfig(config);
			
			using(var d = new SbusSetupForm(useRawInput, ignoreSbusFailsafeFlag)) {
				d.ShowDialog();
				if(d.DialogResult == DialogResult.OK) {
					useRawInput = d.UseRawInput;
					ignoreSbusFailsafeFlag = d.IgnoreSbusFailsafeFlag;
					return buildConfig();
				}
					
				return null;
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
		
		public override string ProtocolName { get { return "FPort"; } }
		
		private void parseConfig(string config) {
			var tokens = config == null ?
					new string[0]
					:
					config.Split(',');
			
			foreach(var s in tokens) {
				if(s == "raw")
					useRawInput = true;
				else if(s == "failsafe")
					ignoreSbusFailsafeFlag = true;
			}
		}
		
		private string buildConfig() {
			var cfg = new ArrayList();
			if(useRawInput)
				cfg.Add("raw");
			if(ignoreSbusFailsafeFlag)
				cfg.Add("failsafe");
			
			return string.Join(",", cfg.ToArray());
		}
	}
}
