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
	/// https://github.com/flyduino/kissfc-chrome-gui/blob/master/js/protocol.js
	/// was used for protocol reference
	///
	/// In short, there are single byte requests to the FC and responses in the followring format
	/// <START_BYTE>,<PAYLOAD_LENGTH>,<PAYLOAD ...>,<CRC>
	/// Where:
	/// START_BYTE = 0x05
	/// PAYLOAD_LENGTH = number of PAYLOAD bytes
	/// CRC = single byte checksum (see code for algorithm)
	/// </summary>
	public class KissReader : SerialReader
	{
		const int DEFAULT_UPDATE_RATE = 10;
		
		const byte START_BYTE = 0x05;
		const int OFFSET_SETTINGS_VERSION = 94;
		const int OFFSET_TELEMETRY_CH1 = 2;
		const int OFFSET_TELEMETRY_CH9 = 156;
		
		// KISS commands
		static readonly byte[] GET_TELEMETRY = { 0x20 };
		static readonly byte[] GET_SETTINGS = { 0x30 };
		
		const int PROTOCOL_MAX_LENGTH = 0xff + 3;

		private double lastSuccessfulRead;
		private int updateRate;
		int? protocolVersion;
	
		
		public override void Start()
		{
			parseConfig(config);
			
			serialPort.ReadTimeout = 500;
			serialPort.WriteTimeout = 500;
			Buffer.FrameLength = PROTOCOL_MAX_LENGTH;
			lastSuccessfulRead = 0;
			protocolVersion = null;
		}
		
		public override void Stop()
		{
		}
		
		public override int ReadChannels()
		{
			try {
				int idx;
				int ch = 0;
				int checksum;
				byte checksum2, len;
				double now, timeDiff;
				
				now = MainForm.Now;
				timeDiff = now - lastSuccessfulRead; // time elapsed since last read
				
				if(timeDiff < updateRate) {
					// not yet time for update
					Thread.Sleep((int)(updateRate - timeDiff));
					now = MainForm.Now;
				}
				
				if(Buffer.Empty) {
					// send request to the FC
					if(protocolVersion == null)
						serialPort.Write(GET_SETTINGS, 0, GET_SETTINGS.Length);
					else
						serialPort.Write(GET_TELEMETRY, 0, GET_TELEMETRY.Length);
				}
				
				lastSuccessfulRead = 0;
	
				if(Buffer[0] != START_BYTE) {
					// incorrect start byte, try parsing from next index
					System.Diagnostics.Debug.WriteLine("Resyncing");
					Buffer.Slide(1);
					lastSuccessfulRead = 0;
					return 0;
				}
				
				len = Buffer[1];
				checksum = checksum2 = 0;
				idx = 2;
				
				while(idx < 2 + len) {
					var b = Buffer[idx++];
					
					checksum += b; // old style checksum (average sum)
					
					checksum2 ^= b; // new style checksum (CRC-8/DVB-S2)
                    for (var j = 0; j < 8; j++) {
                        if (( checksum2 & 0x80) != 0) {
							checksum2 = (byte)((checksum2 << 1) ^ 0xD5);
                        } else {
                            checksum2 <<= 1;
                        }
                    }
				}
				checksum = checksum/len;
				
				var chsm = Buffer[idx++];
				if(chsm == checksum || chsm == checksum2) {
					// correct checksum
					
					
					if(protocolVersion == null) {
						// process the GET_SETTINGS request
						protocolVersion = Buffer[OFFSET_SETTINGS_VERSION];
					}
					else {
						// process the GET_TELEMETRY request
						
						// there are 8 channels - signed 16bit integers, starting from index 2 in the Buffer
						
						// first channel is throttle in range [0; 1000]
						// https://github.com/flyduino/kissfc-chrome-gui/blob/83eb07baecfb906da1ac7fe83420eae4b9356293/js/protocol.js#L225
						var bi = OFFSET_TELEMETRY_CH1;
						channelData[ch++] = 1000 + getInt16(ref bi);
						
						// the remaining 7 channels are in range [-1000; 1000]
						for(var i = 0; i < 7; i++) {
							channelData[ch++] = 1500 + getInt16(ref bi)/2;
						}
						
						// if version is > 110, there are three additional channels
						// https://github.com/flyduino/kissfc-chrome-gui/blob/83eb07baecfb906da1ac7fe83420eae4b9356293/js/protocol.js#L314
						if(protocolVersion > 110) {
							bi = OFFSET_TELEMETRY_CH9;
							for(var i = 0; i < 3; i++) {
								channelData[ch++] = 1500 + getInt16(ref bi)/2;
							}
						}
	
						Buffer.FrameLength = idx; // use for next get
						
						lastSuccessfulRead = now;
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
		/// Show the Kiss configuration
		/// </summary>
		/// <param name="config"></param>
		/// <returns></returns>
		public override string Configure(string config)
		{
			parseConfig(config);
			using(var d = new KissSetupForm(updateRate)) {
				d.ShowDialog();
				if(d.DialogResult == DialogResult.OK) {
					updateRate = d.UpdateRate;
					return buildConfig();
				}
				return null;
			}
		}
		
		public override string ProtocolName { get { return "KISS"; } }
		
		/// <summary>
		/// Kiss configuration - just a simple number for the update rate
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
		
		private short getInt16(ref int index) {
			return (short)((Buffer[index++] << 8) | Buffer[index++]);
		}
	}
}
