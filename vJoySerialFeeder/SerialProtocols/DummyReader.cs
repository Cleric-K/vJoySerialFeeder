/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 26.3.2018 г.
 * Time: 17:10 ч.
 */
using System;
using System.IO.Ports;
using System.Windows.Forms;
using System.Threading;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Description of DummyReader.
	/// </summary>
	public class DummyReader : SerialReader
	{
		const int DEFAULT_UPDATE_RATE = 10;
		
		int updateRate;
		double time;

		
		public override bool OpenPort(string port, Configuration.SerialParameters sp)
		{
			// do not do anything
			return true;
		}
		
		public override void ClosePort()
		{
			// do not do anything
		}
		
		public override void Stop()
		{
			
		}
		
		public override void Start()
		{
			parseConfig(config);
			time = 0;
		}
		
		public override int ReadChannels()
		{
			var now = MainForm.Now;
			var timeDiff = now - time;
			
			if(timeDiff < updateRate) {
				Thread.Sleep((int)(updateRate - timeDiff));
				now = MainForm.Now;
			}
			
			time = now;
			
			channelData[0] = (int)Math.Round(1500 + 500*Math.Sin(2*Math.PI*0.00005*now));
			return 1;
		}
		
		public override Configuration.SerialParameters GetDefaultSerialParameters()
		{
			// not really necessary
			
			return new Configuration.SerialParameters {
				BaudRate = 115200,
				DataBits = 8,
				Parity = Parity.None,
				StopBits = StopBits.One
			};
		}
		
		public override bool Configurable { get { return true; } }
		
		public override string Configure(string config)
		{
			parseConfig(config);
			using(var d = new DummySetupForm(updateRate)) {
				d.ShowDialog();
				
				if(d.DialogResult == DialogResult.OK) {
					return d.UpdateRate.ToString();
				}
			}
			return null;
		}
		
		public override string ProtocolName { get { return "Dummy"; } }
		
		/// <summary>
		/// Dummy configuration - just a simple number for the update rate
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
	}
}
