using System;
using System.Collections;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;

namespace vJoySerialFeeder
{
	public class DjiControllerReader : SerialReader
	{
		private byte[] PING_REQUEST = new byte[] { 0x55, 0x0d, 0x04, 0x33, 0x0a, 0x0e, 0x03, 0x00, 0x40, 0x06, 0x01, 0xf4, 0x4a };
		private int[] lastChannelData = new int[5];
		private int RANGE_MAX = 1684;
		private int RANGE_MIN = 364;

		public override void Start()
		{
			Buffer.FrameLength = 40;
			serialPort.ReadTimeout = 20;
		}

		public override void Stop()
		{
		}

		public override int ReadChannels()
		{
			byte[] readBuffer = new byte[40];
			int byteCount = 0;
			bool readSuccess = false;
            try
            {
				serialPort.Write(PING_REQUEST, 0, PING_REQUEST.Length);
				byteCount = serialPort.Read(readBuffer, 0, 38);
				readSuccess = true;
			} catch (TimeoutException e)
            {
				// reading too fast, sleep
				Thread.Sleep(1);
            }
            
            if (byteCount != 38 || !readSuccess)
            {
				return returnPreviousData();
			}

			var returnArray = new Int16[]
			{
				BitConverter.ToInt16(readBuffer, 21),
				BitConverter.ToInt16(readBuffer, 18),
				BitConverter.ToInt16(readBuffer, 12),
				BitConverter.ToInt16(readBuffer, 15),
				BitConverter.ToInt16(readBuffer, 24)
			};

			// filter for bad readings
			for (int iterator = 0; iterator < 5; iterator++)
            {
				if (returnArray[iterator] < RANGE_MIN || returnArray[iterator] > RANGE_MAX) {
					return returnPreviousData();
				}
            }

			// commit to output
			for (int channelIterator = 0; channelIterator < 5; channelIterator++)
            {
				channelData[channelIterator] = remap(returnArray[channelIterator]);
				lastChannelData[channelIterator]  = channelData[channelIterator];
			}
			return 5;
        }

		public override Configuration.SerialParameters GetDefaultSerialParameters()
		{
			return new Configuration.SerialParameters()
			{
				BaudRate = 115200,
				DataBits = 8,
				Parity = Parity.None,
				StopBits = StopBits.One
			};
		}

		public override bool Configurable { get { return false; } }


		// because default is the best
		// from min: 364, center 1024, max 1684 -> min 1000 center 1500, max 2000
		private Int32 remap(Int32 rawByteValue)
		{
			// return (rawByteValue - 364) * 1000 / 1000 + 1000;
			return Convert.ToInt32( (rawByteValue - 364) / (RANGE_MAX - RANGE_MIN * 1.0) * (1000.0) + 1000);
		}

		private int returnPreviousData()
        {
			for (int channelIterator = 0; channelIterator < 5; channelIterator++)
			{
				channelData[channelIterator] = lastChannelData[channelIterator];
			}
			return 5;
        }
	}
}