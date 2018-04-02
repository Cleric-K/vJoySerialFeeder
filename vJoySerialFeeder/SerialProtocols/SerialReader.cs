/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 8.2.2018 г.
 * Time: 14:15 ч.
 */
using System;
using System.IO.Ports;
using System.Runtime.InteropServices;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Description of SerialReader.
	/// </summary>
	public abstract class SerialReader
	{
		/// <summary>
		/// Is the serial protocol supports its own Failsafe signalling it may
		/// throw this exception to let the caller know that it is not some
		/// other serial read problem.
		/// </summary>
		public class FailsafeException : Exception {
			FailsafeException(string msg) : base(msg) {}
		}
		
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
					if(index >= buf.Length) {
						// this shouldn't really ever happen, but just to be sure
						index = length = 0;
						throw new IndexOutOfRangeException("Buffer overrun");
					}
					
					while(index >= length) {
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
							
                        var read = sp.Read(buf, length, bytesToGet);
                        if (read == 0)
                            throw new InvalidOperationException("Serial read error");
                        length += read;
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
			
			public bool Empty { get { return length == 0 && sp.BytesToRead == 0; } }
									
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
		
			/// <summary>
		/// Override this to return true if protocol should display protocol 'Setup' butto
		/// </summary>
		/// <returns></returns>
		public virtual bool Configurable { get { return false; } }
		
		public void Init(int[] channelData, string config)
		{
			Buffer = new SerialBuffer(serialPort);
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



        public virtual bool OpenPort(string port, Configuration.SerialParameters sp) {
            try
            {
                serialPort = new SerialPort(port, sp.BaudRate, sp.Parity, sp.DataBits, sp.StopBits);
                serialPort.Open();

                return true;
            }
            catch(Exception) {
                if(System.Environment.OSVersion.Platform == PlatformID.Unix) {
                    // mono on linux has trouble opening serial ports with non standard baud rates
                    return OpenLinuxPortCustomBaudRate(port, sp);
                }
                return false;
            }
        }

        public virtual void ClosePort() {
            serialPort.Close();
            serialPort = null;
        }

        bool OpenLinuxPortCustomBaudRate(string port, Configuration.SerialParameters sp) {
            try {
                // first try to open with safe baudrate
                serialPort = new SerialPort(port, 9600, sp.Parity, sp.DataBits, sp.StopBits);
                serialPort.Open();
                // it worked, no try to set the custom baud rate
                if (!SetLinuxCustomBaudRate(port, sp.BaudRate))
                {
                    serialPort.Close();
                    return false;
                }
                return true;
            }
            catch(Exception) {
                return false;
            }
        }


        internal class Linux
        {
            [StructLayout(LayoutKind.Sequential)]
            internal struct Termios2 {
                internal uint c_iflag;       /* input mode flags */
                internal uint c_oflag;       /* output mode flags */
                internal uint c_cflag;       /* control mode flags */
                internal uint c_lflag;       /* local mode flags */
                internal byte c_line;            /* line discipline */
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
                internal byte[] c_cc;        /* control characters */
                internal int c_ispeed;       /* input speed */
                internal int c_ospeed;       /* output speed */
            }

            [DllImport("libc")]
            internal static extern int open([MarshalAs(UnmanagedType.LPStr)]string path, uint flag);

            [DllImport("libc")]
            internal static extern int close(int handle);

            [DllImport("libc")]
            internal static extern int ioctl(int handle, uint request, ref Termios2 termios2);


        }

        bool SetLinuxCustomBaudRate(string port, int baud) {
            // based on code from https://gist.github.com/lategoodbye/f2d76134aa6c404cd92c

            int fd = Linux.open(port, 0 /*O_RDONLY*/);
            if (fd < 0) return false;

            try
            {
                Linux.Termios2 t2 = new Linux.Termios2();

                int e = Linux.ioctl(fd, 2150388778 /*TCGETS2*/, ref t2);
                if (e < 0) return false;

                t2.c_cflag &= ~(uint)4111 /*CBAUD*/;
                t2.c_cflag |= 4096 /*BOTHER*/;
                t2.c_ispeed = baud;
                t2.c_ospeed = baud;

                e = Linux.ioctl(fd, 1076646955 /*TCSETS2*/, ref t2);
                if (e < 0) return false;

                e = Linux.ioctl(fd, 2150388778 /*TCGETS2*/, ref t2);
                if (e < 0) return false;

                return true;
            }
            finally
            {
                Linux.close(fd);
            }
        }
	}
}
