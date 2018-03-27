/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 19.3.2018 г.
 * Time: 17:42 ч.
 */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Implementing interaction with vJoySerialFeeder via WebSocket
	/// 
	/// The implementation of WebSocket is minimal, but compliant with RFC6455
	/// (https://tools.ietf.org/html/rfc6455#section-7.1.1),
	/// with the following exceptions:
	///  * no fragmented messages
	///  * no packets longer than 125 bytes
	///  * no subprotocols, no extensions
	/// 
	/// vJoySerialFeeder acts as server. It accepts simple commands - see onMessage() for details
	/// Response is in JSON format: {"mapping": .., "input": .., "output": ..}
	/// </summary>
	class WebSocket {
		
		/// <summary>
		/// Encapsulates a subscription for Mapping updates
		/// </summary>
		class Subscription {
			internal readonly int mappingIndex;
			internal bool IgnoreInputChanges;
			
			int? Input;
			float? Output;
			
			internal Subscription(int i, bool ignInp) {
				mappingIndex = i;
				IgnoreInputChanges = ignInp;
			}
			
			/// <summary>
			/// Updates the input/output cache and returns true if there
			/// were changes
			/// </summary>
			/// <param name="Input"></param>
			/// <param name="Output"></param>
			/// <returns></returns>
			internal bool Update(int Input, float Output) {
				bool upd = this.Output != Output || (!IgnoreInputChanges && this.Input != Input);
				this.Input = Input;
				this.Output = Output;
				return upd;
			}
		}

		static readonly char[] HEADER_SEP = new char[] {':'};
		
		const byte OP_TEXT = 0x1;
		const byte OP_PING = 0x9;
		const byte OP_PONG = 0xa;
		const byte OP_CLOSE = 0x8;

		
		TcpListener listener;
		byte[] sendBuf = new byte[256];
		Dictionary<Socket, List<Subscription>> subscriptions = new Dictionary<Socket, List<WebSocket.Subscription>>();
		List<Socket> deadSockets = new List<Socket>();
		
		public WebSocket(int port) {
			listener = new TcpListener(IPAddress.Any, 40000);
			listener.Start();
			listener.BeginAcceptSocket(acceptConnection, null);
		}
	
		void acceptConnection(IAsyncResult ar) {
			// accept another
			Socket sock = null;
			
			try {
				// accept another connection asynchronously
				listener.BeginAcceptSocket(acceptConnection, null);
				
				sock = listener.EndAcceptSocket(ar);
				
				if(negotiate(sock))
					receiveLoop(sock);
			}
			catch(Exception e) {
				System.Diagnostics.Debug.WriteLine(e.Message);
				if(sock != null) {
					removeSubscriptionsForSocket(sock);
					try {
						sock.Close();
					}
					catch(Exception) {}
				}
			}
			
		}
		
		/// <summary>
		/// Perform minimal WebSocket negotiation
		/// </summary>
		/// <param name="sock"></param>
		bool negotiate(Socket sock) {
			string sec = null;
        	
        	using(var ns = new NetworkStream(sock))
        	using(var tr = new StreamReader(ns))
        	using(var tw = new StreamWriter(ns)) {
        		string s;
        		while( !(s = tr.ReadLine()).Equals(string.Empty) ) {
					// read HTTP headers
        			var header = s.Split(HEADER_SEP, 2);
        			if(header[0].ToLower().Equals("sec-websocket-key"))
        			   sec = header[1].Trim();
        		}
        		
        		if(sec != null) {
        			sec += "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";	
        			sec = Convert.ToBase64String(SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(sec)));
        		
        			tw.Write("HTTP/1.1 101 Switching Protocols\r\n");
    				tw.Write("Upgrade: websocket\r\n");
    				tw.Write("Connection: Upgrade\r\n");
    				tw.Write("Sec-WebSocket-Accept: " + sec + "\r\n");
    				tw.Write("\r\n");
    				
    				return true;
        		}
        		
    			// invalid WebSocket request
    			sock.Close();
    			return false;
        	}
		}
		
		/// <summary>
		/// This is the receive loop. It reads from the socket and interprets
		/// the data as websocket frames.
		/// </summary>
		/// <param name="sock"></param>
		void receiveLoop(Socket sock) {
			var buf = new byte[256];
			
			while(true) {
        		if(sock.Receive(buf, 2, SocketFlags.None) != 2)
        			throw new Exception("Not enough data");
        		
        		// check FIN bit
        		if((buf[0] & 0x80) == 0)
        			throw new Exception("Fragmented frames not supported");
        		
        		// check MASK bit
        		if((buf[1] & 0x80) == 0)
        			throw new Exception("Client frames must be masked");
        		
        		// get length
        		var len = buf[1] & 0x7f;
        		if(len > 125)
        			throw new Exception("Unsupported length");
        		
        		var opcode = buf[0] & 0xF; 
        		
        		var fullLen = len + 4; // 4 mask bytes
        		if(sock.Receive(buf, fullLen, SocketFlags.None) != fullLen)
        			throw new Exception("Not enough data");
        		
        		// unmask 
				for(var i = 0; i<len; i++)
					buf[4+i] ^= buf[i%4];
        		
        		switch(opcode) {
        			case OP_TEXT:
    					var msg = Encoding.UTF8.GetString(buf, 4, len);
    					onMessage(sock, msg);
        				
						break;
						
        			case OP_PING:
    					// ping
    					// data to send with pong is the same as the one in buf,
    					// just prepare the two header bytes
    					buf[2] = 0x80 | OP_PONG;
						buf[3] = (byte)len;
						sock.Send(buf, 2, len+2, SocketFlags.None);
        				
        				break;
        				
        			case OP_CLOSE:
        				// close frame
        				sock.Close();
        				return;
        				
        			default:
        				throw new Exception("unsupported opcode "+opcode);
        		}
        	}
		}
		
		
		
		
		
		
		
		
		
		
		
		/// <summary>
		/// Can be called from any thread. Sends websocket messages for all
		/// subscriptions whose values have changed.
		/// </summary>
		public void  Dispatch() {
			lock(subscriptions) {
				foreach(var sock in subscriptions.Keys) {
					var l = subscriptions[sock];
					
					foreach(var sub in l) {
						var m = MainForm.Instance.MappingAt(sub.mappingIndex);
						
						if(m == null)
							// the mapping does not exists at this point
							continue;
						
						if(sub.Update(m.Input, m.Output)) {
							try {
								sendMappingData(sock, sub.mappingIndex+1, m);
							}
							catch(Exception e) {
								System.Diagnostics.Debug.WriteLine(e.Message);
								deadSockets.Add(sock);
								break;
							}
						}
					}
				}
				
				if(deadSockets.Count > 0) {
					foreach(var sock in deadSockets)
						removeSubscriptionsForSocket(sock);
					
					deadSockets.Clear();
				}
			}
		}
		
		
		/// <summary>
		/// Parses and executes commands coming from clients. Commands are:
		/// 
		/// get MAPPING_ID
		/// 	returns json message with the values of mapping MAPPING_ID
		/// 
		/// set_input MAPPING_ID VALUE
		/// 	sets the Input of mapping MAPPING_ID to VALUE
		///     The Output of the mapping will be set by the
		///     mapping itself.
		/// 
		/// set_output MAPPING_ID VALUE
		/// 	sets the Output of mapping MAPPING_ID to VALUE.
		/// 
		/// sub_input MAPPING_ID
		/// 	subscribes to changes in the Input of mapping MAPPING_ID
		/// 
		/// sub_output MAPPING_ID
		/// 	subscribes to changes in the Output of mapping MAPPING_ID
		/// 
		/// unsub MAPPING_ID
		/// 	unsubscribes to any changes of mapping MAPPING_ID
		/// 
		///
		/// All MAPPING_IDs start from 1
		/// 
		/// Subscriptions cause messages to be received upon changes, which 
		/// have the same format as the ones received from the `get` command
		/// </summary>
		/// <param name="sock"></param>
		/// <param name="msg"></param>
		void onMessage(Socket sock, string msg) {
			try {
				var parts = msg.ToLower().Split(null);
				if(parts.Length < 2)
					throw new Exception("Bad command");
				
				int idx = 0;
				
				var cmd = parts[0];
				
				if(!int.TryParse(parts[1], out idx))
					throw new Exception("Bad mapping index");
				
				switch(cmd) {
					case "get":
						assertPartsNum(parts, 2);
						var m = MainForm.Instance.MappingAt(idx - 1);
						if(m == null)
							throw new Exception("No such mapping");
						
						sendMappingData(sock, idx, m);
						
						break;
						
					case "set_input":
					case "set_output":
						assertPartsNum(parts, 3);
						m = MainForm.Instance.MappingAt(idx - 1);
						if(m == null)
							throw new Exception("No such mapping");
							
						float val;
						if(!float.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out val))
							throw new Exception("Bad value");
						
						if(cmd.Equals("set_input"))
							m.Input = (int)val;
						else
							m.Output = val;
						
						Dispatch();
							
						break;
						
					case "sub_input":
						assertPartsNum(parts, 2);
						subscribe(sock, true, idx-1);
						break;
						
					case "sub_output":
						assertPartsNum(parts, 2);
						subscribe(sock, false, idx-1);
						break;
						
					case "unsub":
						assertPartsNum(parts, 2);
						unsubscribe(sock, idx-1);
						break;
						
					default:
						sendError(sock, "Unknown command");
						break;
						
				}
			}
			catch(Exception e) {
				sendError(sock, e.Message);
			}
		}
		
		void assertPartsNum(string[] parts, int n) {
			if(parts.Length != n)
				throw new Exception("Incorrect number of arguments");
		}

		
		void sendError(Socket sock, String msg) {
			sendMessage(sock, "{\"error\":\""+msg.Replace("\"", "\\\"")+"\"}");
		}
		
		void sendMappingData(Socket sock, int mi, Mapping m) {
			sendMessage(sock, "{\"mapping\":"+mi+",\"input\":"+m.Input
			            +",\"output\":"+m.Output.ToString(CultureInfo.InvariantCulture)+"}");
		}
		
		void sendMessage(Socket sock, string msg) {;
			lock(sendBuf) {
				var slen = Encoding.UTF8.GetBytes(msg, 0, msg.Length, sendBuf, 2);
				sendBuf[0] = 0x81;
				sendBuf[1] = (byte)slen;
				sock.Send(sendBuf, slen+2, SocketFlags.None);
			}
		}
		
		void removeSubscriptionsForSocket(Socket sock) {
			lock(subscriptions) {
				if(subscriptions.ContainsKey(sock))
				   subscriptions.Remove(sock);
			}
		}
		
		void subscribe(Socket sock, bool subInput, int mIdx){
			lock(subscriptions) {
				var m = MainForm.Instance.MappingAt(mIdx);
				if(m == null)
					throw new Exception("No such mapping");
				
				if(!subscriptions.ContainsKey(sock))
					subscriptions[sock] = new List<Subscription>();
				var l = subscriptions[sock];
				
				foreach(var sub in l) {
					if(sub.mappingIndex == mIdx) {
						// already subscribed
						if(subInput)
							sub.IgnoreInputChanges = false;
						return;
					}
				}
				
				l.Add(new Subscription(mIdx, !subInput));
			}
		               	
		}
		
		void unsubscribe(Socket sock, int mIdx) {
			lock(subscriptions) {
				if(subscriptions.ContainsKey(sock)) {
					var l = subscriptions[sock];
					for(var i=0; i<l.Count; i++) {
						if(l[i].mappingIndex == mIdx) {
							l.RemoveAt(i);
							break; // there is only one sub for mIdx
						}
					}
				}
			}
		}
	}
}
