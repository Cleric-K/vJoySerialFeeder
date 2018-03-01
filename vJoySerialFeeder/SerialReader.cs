/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 8.2.2018 г.
 * Time: 14:15 ч.
 */
using System;
using System.IO.Ports;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Description of SerialReader.
	/// </summary>
	public abstract class SerialReader
	{
		/// <summary>
		/// Helper class which allows access to the serial port as indexed object (Buffer[index])
		/// Data is read from the serial port automatically when needed.
		/// </summary>
		public class SerialBuffer {
			int length;
			byte[] buf = new byte[1024];
			SerialPort sp;
			
			public SerialBuffer(SerialPort sp) {
				this.sp = sp;
			}
			
			public byte this[int index] {
				get {
					if(index >= length) {
						// need more data
						int bytesToGet;
						
						if(index < FrameLength)
							// get the remaining of the frame
							bytesToGet = FrameLength - length;
						else {
							// reading beyond the FrameLength?
							// we don't know what is going on, so try get some more bytes;
							bytesToGet = index - length + 1;
							System.Diagnostics.Debug.WriteLine("Read beyond frame");
						}
							
						length += sp.Read(buf, length, bytesToGet);
						
						if(index >= length)
							// still not enought bytes, even after reading some more
							throw new IndexOutOfRangeException();
					}
					return buf[index];
				}
			}
			
			/// <summary>
			/// This property has a role of a hint when reading serial data.
			/// When the FrameLength is set correctly, the correct number of bytes will
			/// be requested when reading from the serial port, thus avoiding too many
			/// small reads or waiting too long for more data than it is actually needed
			/// </summary>
			public int FrameLength { get; set; }
									
			/// <summary>
			/// Moves the Buffer data so that 'newStart' will be at index 0.
			/// Bytes before newStart are lost.
			/// </summary>
			/// <param name="newStart"></param>
			public void Slide(int newStart) {
				if(newStart < length) {
					Array.Copy(buf, newStart, buf, 0, length - newStart);
					length -= newStart;
				}
				else
					length = 0;
			}
		}
		
		
		
		protected SerialPort serialPort;
		protected SerialBuffer Buffer;
		protected string config;
		
		/// <summary>
		/// the ReadChannels method should put its data in this array.
		/// </summary>
		protected int[] channelData;
		
		public void Init(SerialPort sp, int[] channelData, string config)
		{
			serialPort = sp;
			Buffer = new SerialBuffer(sp);
			this.channelData = channelData;
			this.config = config;
		}
		
		/// <summary>
		/// Derived class should perform any specific startup actions here
		/// </summary>
		public abstract void Start();
		
		/// <summary>
		/// Derived class should perform any specific "on disconnect" actions here
		/// </summary>
		public abstract void Stop();
		
		/// <summary>
		/// Derived class must implement this method dat it populates the channelData 
		/// array with data.
		/// </summary>
		/// <returns>The number of channels written to channelData</returns>
		public abstract int ReadChannels();
		
		/// <summary>
		/// Get's the default SerialParameters for this protocol
		/// </summary>
		/// <returns></returns>
		public abstract Configuration.SerialParameters GetDefaultSerialParameters();
		
		/// <summary>
		/// Override this to return yes if protocl should display protocol 'Setup' butto
		/// </summary>
		/// <returns></returns>
		public virtual bool IsConfigurable() {
			return false;
		}
		
		/// <summary>
		/// Override this to display a dialog or whatever.
		/// 
		/// Procotol configuration is a simple string (could be empty or null). The deriving class should
		/// decide for itself how it's going to serialize its configuration.
		/// </summary>
		/// <param name="config">This is the current configuration</param>
		/// <returns>The new configuration. Return null if the configuration should not be changed.</returns>
		public virtual string Configure(string config) {
			throw new NotImplementedException("You must implement this yourself");
		}
	}
}
