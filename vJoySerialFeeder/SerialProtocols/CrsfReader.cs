using System;
using System.Collections;
using System.IO.Ports;
using System.Windows.Forms;

namespace vJoySerialFeeder
{
	public class Crc8
	{
		private byte[] _table = new byte[256];

		public Crc8(byte poly)
		{
			GenerateTable(poly);
			Out = 0;
		}

		public void Add(byte b)
		{
			_out = _table[_out ^ b];
		}

		private byte _out = 0;
		public byte Out
		{
			get { return _out;  }
			set { _out = 0;  }
		}

		private void GenerateTable(byte poly)
		{
			for (uint i = 0; i < 256; ++i)
			{
				uint curr = i;
				for (uint j = 0; j < 8; ++j)
				{
					if ((curr & 0x80) != 0)
					{
						curr = (curr << 1) ^ poly;
					}
					else
					{
						curr <<= 1;
					}
				}

				_table[i] = (byte)curr;
			}
		}
	}

	public class CrsfReader : SerialReader
	{
		const int CRSF_PAYLOAD_SIZE_MAX = 62;
		const int CRSF_SUBSET_RC_CHANNELS_PACKED_RESOLUTION = 11; // 11 bits per channel
		const uint CRSF_SUBSET_RC_CHANNELS_PACKED_MASK = 0x7ff; // 0b11111111111; // 11 bits, get it?!

		// AddressTypes
		const byte CRSF_ADDRESS_FLIGHT_CONTROLLER = 0xC8; // 200
		// FrameTypes
		const byte CRSF_FRAMETYPE_LINK_STATISTICS = 0x14;
		const byte CRSF_FRAMETYPE_RC_CHANNELS_PACKED = 0x16;

		private Crc8 crcCalc = new Crc8(0xD5);

		public override void Start()
		{
		}

		public override void Stop()
		{
		}

		public override int ReadChannels()
		{
			Buffer.FrameLength = 2;

			// First byte is dest addr, second is len of payload+crc
			byte len = Buffer[1];
			if (len < 2 || len > CRSF_PAYLOAD_SIZE_MAX)
			{
				Buffer.Slide(1);
				return 0;
			}
			Buffer.FrameLength = len + 2;

			// Read the whole frame up to the CRC byte
			crcCalc.Out = 0;
			for (int payloadIdx=0; payloadIdx < len - 1; ++payloadIdx)
				crcCalc.Add(Buffer[payloadIdx + 2]);

			int retVal = 0;
			byte inCrc = Buffer[2 + len - 1];
			if (crcCalc.Out != inCrc)
			{
				System.Diagnostics.Debug.WriteLine("Bad checksum {0:X} calced {1:X}", inCrc, crcCalc.Out);
			}
			else
			{
				//System.Diagnostics.Debug.WriteLine("CRSF packet type={0:X}", Buffer[2]);
				if ((Buffer[0] == CRSF_ADDRESS_FLIGHT_CONTROLLER)
					&& (Buffer[2] == CRSF_FRAMETYPE_RC_CHANNELS_PACKED))
				{
					const uint numOfChannels = 16;
					uint readByte = 0;
					int byteIndex = 3;
					int bitsMerged = 0;
					uint readValue = 0;
					for (uint n = 0; n < numOfChannels; n++)
					{
						while (bitsMerged < CRSF_SUBSET_RC_CHANNELS_PACKED_RESOLUTION)
						{
							readByte = Buffer[byteIndex++];
							readValue |= readByte << bitsMerged;
							bitsMerged += 8;
						}

						channelData[n] = (int)map((readValue & CRSF_SUBSET_RC_CHANNELS_PACKED_MASK), 191, 1792, 1000, 2000);
						readValue >>= CRSF_SUBSET_RC_CHANNELS_PACKED_RESOLUTION;
						bitsMerged -= CRSF_SUBSET_RC_CHANNELS_PACKED_RESOLUTION;
					}

					//System.Diagnostics.Debug.WriteLine("Roll={0}", channelData[0]);
					retVal = (int)numOfChannels;
				} /* if packed channels */
			} /* if valid checksum */

			Buffer.Slide(Buffer.FrameLength);
			return retVal;
		}

		public override Configuration.SerialParameters GetDefaultSerialParameters()
		{
			return new Configuration.SerialParameters()
			{
				BaudRate = 420000,
				DataBits = 8,
				Parity = Parity.None,
				StopBits = StopBits.One
			};
		}

		public override bool Configurable { get { return false; } }
		
		public override string ProtocolName { get { return "CRSF"; } }

		static private uint map(uint val, uint in_min, uint in_max, uint out_min, uint out_max)
		{
			// constrain(retVal, out_min, out_max)
			if (val < in_min)
				return out_min;
			if (val > in_max)
				return out_max;
			return (val - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;

		}
	}
}