/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 1.4.2018 г.
 * Time: 13:02
 */
using System;
using System.Threading.Tasks;
using MoonSharp.Interpreter;

namespace vJoySerialFeeder
{
	/// <summary>
	/// The background worker in MainForm is the actual main loop of the program
	/// </summary>
	partial class MainForm
	{		
		public static double Now { get { return (double)DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond; } }
		public bool Failsafe { get; private set; }
		
		void BackgroundWorkerDoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
		{
			try {
				serialReader.Init(Channels, protocolConfig);
				serialReader.Start();
				
				double nextUIUpdateTime = 0, nextRateUpdateTime = 0, prevTime = 0;
				double updateSum = 0;
				double now;
				int updateCount = 0;
				int timeToWait;
				double failsafeAt = Now + failsafeTime;
				double nextFailsafeUpdate = 0;
				
				Failsafe = false;
				Task<int> readChannelsTask = null;
				
				/**
				 * The main loop is a little complicated because it supports two modes of action:
				 * 1. Normal mode. In this case the update rate is dictated by the the serial
				 *    reader
				 * 
				 * 2. If we do not receive serial data for certain amount of time (failsafeTime)
				 *    we enter failsafe mode. In this mode we generate our own update rate,
				 *    while continuing to try to read the serial port. 
				 */
				while(true) {
					if(backgroundWorker.CancellationPending) {
						e.Cancel = true;
						return;
					}
					
					// need a serial reading task
					if(readChannelsTask == null) {
						readChannelsTask = Task<int>.Factory.StartNew(serialReader.ReadChannels);
					}
					
					// If not in failsafe mode, we can afford to wait at most up to the time
					// when failsafe shall be activated.
					// If already in failsafe - wait until it is time for a failsafe update
					timeToWait = Failsafe ? 
						(int)(nextFailsafeUpdate - Now)
						:
						(int)(failsafeAt - Now);
					
					if(timeToWait < 0)
						timeToWait = 0;
					
					bool readDone = false;
					try {
						if(readChannelsTask.Wait(timeToWait)) {
							// serial read completed
							ActiveChannels = readChannelsTask.Result;
							readDone = true;
						}
						// else Wait timedout
					}
					catch(AggregateException aex) {
						// the SerialReader threw exception
						var ex = aex.InnerException;
						readDone = true;
						ActiveChannels = 0;
						
						if(ex is InvalidOperationException) {
							System.Diagnostics.Debug.WriteLine(ex.Message);
							this.Invoke((Action)( () => ErrorMessageBox("The Serial Port was Disconnected!",
							                                            "Disconnect")));
							backgroundWorker.CancelAsync();
							continue;
						}
						else if(ex is TimeoutException) {
							failsafeReason = "Serial Port Read Timeout";
						}
						else {
							failsafeReason = ex.Message;
						}
					}
					
					if(readDone)
						readChannelsTask = null;
					
					now = Now;
					
					if(readDone && ActiveChannels > 0) {
						// normal mode, we have serial data
						Failsafe = false;
						failsafeReason = null;
					}
					else if(!Failsafe && now >= failsafeAt) {
						// failsafeTime elapsed, time to get in failsafe
						ActiveChannels = 0;
						Failsafe = true;
						if(failsafeReason == null)
							failsafeReason = "Waiting for Serial Data";
						
					}
					else if(Failsafe && now >= nextFailsafeUpdate) {
						// time for failsafe update
					}
					else
						continue;
					
					
					
					// Update

					foreach(Mapping m in mappings) {
						if(m.Channel >= 0 && m.Channel < Channels.Length) {
							if(Failsafe)
								m.Failsafe();
							else
								m.Input = Channels[m.Channel];
						}
					}
					
					try {
						lua.Update(VJoy, Channels, Failsafe);
					}
					catch(NullReferenceException) {
						 // could happen lua==null if we loadProfile while connected
					}
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
					

					
					// since the time between frames may vary we sum the times here
					// and later publish the average
					if(now > prevTime) {
						updateSum += 1000.0/(now - prevTime);
						updateCount++;
					}
					
					// update UI on every 100ms
					if(now >= nextUIUpdateTime) {
						nextUIUpdateTime = now + 100;
						
						// update the Rate on evert 500ms
						if(now >= nextRateUpdateTime) {
							nextRateUpdateTime = now + 500;
							
							//if(ActiveChannels == 0)
							//	updateRate = 0;
							if(updateCount > 0) {
								updateRate = updateSum/updateCount;
								updateSum = updateCount = 0;
							}
						}
						
						// will emit the ChannelDataUpdate event on the UI thread
						backgroundWorker.ReportProgress(0);
					}
					
					prevTime = now;
					
					if(!Failsafe)
						failsafeAt = now + failsafeTime;
					else
						nextFailsafeUpdate = now + failsafeUpdateRate;
				}
			}
			catch(Exception ex) {
				this.Invoke((Action)( () =>
				                     ErrorMessageBox(ex.ToString(), "Main Worker")));
			}
			finally {
				serialReader.Stop();
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
