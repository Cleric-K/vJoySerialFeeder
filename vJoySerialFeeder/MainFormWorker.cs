/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 1.4.2018 г.
 * Time: 13:02
 */
using System;
using MoonSharp.Interpreter;

namespace vJoySerialFeeder
{
	/// <summary>
	/// The background worker in MainForm is the actual main loop of the program
	/// </summary>
	partial class MainForm
	{		
		void BackgroundWorkerDoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
		{
			try {
				serialReader.Init(Channels, protocolConfig);
				serialReader.Start();
				
				double nextUIUpdateTime = 0, nextRateUpdateTime = 0, prevTime = 0;
				double updateSum = 0;
				int updateCount = 0;
				
				while(true) {
					if(backgroundWorker.CancellationPending) {
						e.Cancel = true;
						serialReader.Stop();
						return;
					}
					
					try {
						ActiveChannels = serialReader.ReadChannels();
					}
					catch(InvalidOperationException ex) {
						System.Diagnostics.Debug.WriteLine(ex.Message);
						this.Invoke((Action)( () => ErrorMessageBox("The Serial Port was Disconnected!",
						                                            "Disconnect")));
						backgroundWorker.CancelAsync();
						continue;
					}
					catch(Exception ex) {
						ActiveChannels = 0;
						System.Diagnostics.Debug.WriteLine(ex.Message);
					}
					if(ActiveChannels > 0) {
						foreach(Mapping m in mappings) {
							if(m.Channel >= 0 && m.Channel < ActiveChannels)
								m.Input = Channels[m.Channel];
						}
						
						try {
							lua.Update(VJoy, Channels);
						}
						catch(NullReferenceException) {}
						catch(InterpreterException ex) {
							this.Invoke((Action)( () =>
							            ErrorMessageBox("Lua script execution failed. Scripting disabled:\n\n" + ex.DecoratedMessage,
							                  "Lua Error")));
						}
						
						foreach(Mapping m in mappings) {
							m.UpdateJoystick(VJoy);
						}
						
						VJoy.SetState();
						
						if(comAutomation != null)
							comAutomation.Dispatch();
						
						if(webSocket != null)
							webSocket.Dispatch();
					}
					
					
					double now = (double)DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
					
					// since the time between frames may vary we sum the times here
					// and later publish the average
					if(now > prevTime && ActiveChannels > 0) {
						updateSum += 1000.0/(now - prevTime);
						updateCount++;
					}
					
					// update UI on every 100ms
					if(now >= nextUIUpdateTime) {
						nextUIUpdateTime = now + 100;
						
						// update the Rate on evert 500ms
						if(now >= nextRateUpdateTime) {
							nextRateUpdateTime = now + 500;
							
							if(ActiveChannels == 0)
								updateRate = 0;
							else if(updateCount > 0) {
								updateRate = updateSum/updateCount;
								updateSum = updateCount = 0;
							}
						}
						
						// will emit the ChannelDataUpdate event on the UI thread
						backgroundWorker.ReportProgress(0);
					}
					
					if(ActiveChannels > 0)
						prevTime = now;
				}
			}
			catch(Exception ex) {
				this.Invoke((Action)( () =>
				                     ErrorMessageBox(ex.ToString(), "Main Worker")));
			}
		}
		void BackgroundWorkerRunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
		{
			disconnect2();
		}
		
		void BackgroundWorkerProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
		{
			ChannelDataUpdate(this, e);
		}
	}
}
