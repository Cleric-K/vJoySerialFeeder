using System;
using System.IO.Ports;
using System.Threading;

namespace vJoySerialFeeder
{
	public class NasaReader : SerialReader
	{
		public static int FRAME_LENGTH = 9;
		public override void Start()
		{
			Buffer.FrameLength = FRAME_LENGTH;
		}

		public override void Stop()
		{
		}
		
		public override int ReadChannels()
		{
			Buffer.Clear();
			//Thread.Sleep(5);
			serialPort.DiscardInBuffer();
			serialPort.Write("r"); // request data
			
			for(int i = 0; i < FRAME_LENGTH; i++) {
				channelData[i] = Buffer[i];
			}
			
			return FRAME_LENGTH;
		}
		
		public override Configuration.SerialParameters GetDefaultSerialParameters()
		{
			return new Configuration.SerialParameters()
			{
				BaudRate = 9600,
				DataBits = 8,
				Parity = Parity.None,
				StopBits = StopBits.One
			};
		}

		public override bool Configurable { get { return false; } }
		
		public override string ProtocolName { get { return "NASA"; } }
	}
}
