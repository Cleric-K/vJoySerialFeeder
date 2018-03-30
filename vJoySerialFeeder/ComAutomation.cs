/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 13.3.2018 г.
 * Time: 22:30 ч.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Windows.Forms;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Implements COM OLE Automation
	/// </summary>
	/// 
	public class ComAutomation
	{
		const string PROGID = "vJoySerialFeeder.1";
		const string CLSID = "abc3f69e-8a95-4f6c-975a-0a99338c2433";
		
		
		/// 
		/// IClassFactory declaration
		/// 
		/// We need this to support COM CreateInstance
		/// 
		[ComImport()]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("00000001-0000-0000-C000-000000000046")]
		internal interface IClassFactory
		{
			[PreserveSig]
			int CreateInstance(IntPtr pUnkOuter, ref Guid riid, out IntPtr ppvObject);
			[PreserveSig]
			int LockServer(bool fLock);
		}
		
		
		
		/// <summary>
		/// This is the main Com Object.
		/// </summary>
		[ComVisible(true)]
		[ProgId(PROGID)]
		[Guid(CLSID)]
		[ClassInterface(ClassInterfaceType.AutoDispatch)]
		public class Application : IClassFactory {
			/// <summary>
			/// The Mappings Array
			/// </summary>
			public Mappings Mappings {get; private set;}
			
			internal Application() {
				Mappings = new Mappings();
			}
			
			
			/// <summary>
			/// Detach handler `h`
			/// </summary>
			/// <param name="h"></param>
			public void DetachHandler(dynamic h) {
				var handlers = ComAutomation.instance.handlers;
				lock(handlers) {
					for(int i = handlers.Count-1; i >= 0; i--) {
						try {
							if(handlers[i].handlerObject == h)
								handlers.RemoveAt(i);
						}
						catch(COMException) {
							// if a handler COM object is no longer active
							// the comparison will throw exception - remove it
							handlers.RemoveAt(i);
						}
					}
				}
			}
			
			
			// IClassFactory implementation. The object is a singleton so CreateInstance always 
			// return the same object
			public int CreateInstance(IntPtr pUnkOuter, ref Guid riid, out IntPtr ppvObject)
			{
				return Marshal.QueryInterface(Marshal.GetIUnknownForObject(this), ref riid, out ppvObject);
			}
			
			public int LockServer(bool fLock)
			{
				return 0;
			}
		}
		
		/// <summary>
		/// This class implemets a Mapping Array with Item() and Count
		/// </summary>
		[ComVisible(true)]
		public class Mappings {
			public MappingWrapper Item(int index) {
				var m = MainForm.Instance.MappingAt(index-1);
				if(m == null)
					return null;
				return new MappingWrapper(m);
			}
			
			public int Count { get { return MainForm.Instance.MappingCount; } }
		}
		
		
		/// <summary>
		/// Wraps a Mapping
		/// </summary>
		[ComVisible(true)]
		public class MappingWrapper {
			internal Mapping m;
			
			internal MappingWrapper(Mapping m) {
				this.m = m;
			}
			
			void attachHandler(dynamic h, bool oo) {
				lock(ComAutomation.instance.handlers)
					ComAutomation.instance.handlers.Add(new Handler(m, h, oo));
			}
			
			public int Input {
				get { return m.Input; }
				set { m.Input = value; }
			}
			
			public float Output {
				get { return m.Output; }
				set { m.Output = value; }
			}
			
			public string Type() {
				return m.GetType().Name;
			}
			
			public void AttachOutputHandler(dynamic h) {
				attachHandler(h, true);
			}
			
			public void AttachInputHandler(dynamic h) {
				attachHandler(h, false);
			}
			
			public void DetachHandler(dynamic h) {
				var handlers = ComAutomation.instance.handlers;
				lock(handlers) {
					for(int i = handlers.Count-1; i >= 0; i--) {
						var hi = handlers[i];
						try{
							if(hi.handlerObject == h && hi.Mapping == m)
								handlers.RemoveAt(i);
						}
						catch(COMException) {
							// if a handler COM object is no longer active
							// the comparison will throw exception - remove it
							handlers.RemoveAt(i);
						}
					}
				}
			}
		}
		
		
		
		/// <summary>
		/// This class encapsulates a user provided handler to be called on events
		/// </summary>
		class Handler {
			internal int? Input;
			internal float? Output;
	
			internal readonly Mapping Mapping;
			internal readonly bool IgnoreInputChanges;
			
			internal dynamic handlerObject;

			int numFails;
			
			
			internal Handler(Mapping m, dynamic h, bool ignInput) {
				handlerObject = h;
				Mapping = m;
				IgnoreInputChanges = ignInput;
			}
			
			/// <summary>
			/// Updates the values of the handler and return true
			/// if change was detected
			/// </summary>
			/// <param name="i"></param>
			/// <param name="o"></param>
			/// <returns></returns>
			internal bool SetValues(int i, float o) {
				var r = Output != o ||
							(!IgnoreInputChanges && Input != i);
								
				Input = i;
				Output = o;
				
				return r;
			}
			
			internal void Execute() {
				try {
					// COM call
					handlerObject.OnUpdate(Input, Output);
					numFails = 0;
				}
				catch(Exception e) {
					System.Diagnostics.Debug.WriteLine("Handler execution failed: "+e.Message);
					if(++numFails == 5)
						// too many consecutive errors,
						// this handler will be removed
						throw e;
				}
				
			}
		}
		
		
		
		
		
		
		
		
		
		static ComAutomation instance;
		
		ManualResetEvent dispatchLoopWait = new ManualResetEvent(false);
		List<Handler> handlers = new List<Handler>();
		
		
		
		
		public static ComAutomation GetInstance() {
			if(instance == null) {
				var lck = new object();
				
				lock(lck) {
					var t = new Thread(() => {
						instance = new ComAutomation();
						
						lock(lck)
							Monitor.Pulse(lck);
						
						instance.dispatchLoop();
					});
					t.Name = "ComEventsDispatcher";
					t.IsBackground = true;
					t.Start();
					
					
					Monitor.Wait(lck);
				}
			}
			
			return instance;
		}
		
		[DllImport("ole32.dll")]
	    private static extern int CoRegisterClassObject(
		    [MarshalAs(UnmanagedType.LPStruct)] Guid rclsid,
		    [MarshalAs(UnmanagedType.IUnknown)] object pUnk,
		    uint dwClsContext,
		    uint flags,
		    out uint lpdwRegister);
		
		[DllImport("ole32.dll")]
		private static extern int CreateFileMoniker([MarshalAs(UnmanagedType.LPWStr)] string
		   lpszPathName, out IMoniker ppmk);
		
		[DllImport("ole32.dll")]
	    private static extern int GetRunningObjectTable(uint reserved,
	       out System.Runtime.InteropServices.ComTypes.IRunningObjectTable pprot);
	        	
	    [DllImport("oleaut32.dll")]
		private static extern int RegisterActiveObject
		    ([MarshalAs(UnmanagedType.IUnknown)] object punk,
		    ref Guid rclsid, 
		    uint dwFlags, 
		    out int pdwRegister);	
		
		
		
		
		
		private ComAutomation()
		{
			var comObj = new Application();
			IMoniker mon;
			IRunningObjectTable rot;
			int areg;
			uint creg;
			int hr;
			
			var clsid = new Guid(CLSID);
			
			// register the object in the Running Object Table with a File Moniker
			// This allows for example from VBScript to do: GetObject('vJoySerialFeeder.1')
			hr = CreateFileMoniker(PROGID, out mon);
			hr = GetRunningObjectTable(0, out rot);
			rot.Register(1 /*ROTFLAGS_REGISTRATIONKEEPSALIVE*/, comObj, mon);
			
			// register the object in the Running Object Table by CLSID.
			hr = RegisterActiveObject(comObj, ref clsid, 0 /*ACTIVEOBJECT_STRONG*/, out areg);
			
			// register the object as Out-of-Proces object, which allows go do CreateObject()
			hr = CoRegisterClassObject(clsid, comObj,
			                      4 /*CLSCTX_LOCAL_SERVER*/,
			                      1 /*REGCLS_MULTIPLEUSE*/,
			                      out creg);
			
		}
		
		
		
		
		
		/// <summary>
		/// This method can be called from any thread.
		/// It sends signal to the `dispatchLoop` that
		/// events need processing
		/// </summary>
		public void Dispatch() {
			dispatchLoopWait.Set();
		}
		
		
		
		
		/// <summary>
		/// This is the COM event dispatcher.
		/// It waits for another thread to call `Dispatch`.
		/// </summary>
		private void dispatchLoop() {
			List<Handler> dispatchList = new List<Handler>();
			
			while(true) {
				// wait for signal
				dispatchLoopWait.WaitOne();
				
				// do not block the `handlers` list for too long -
				// try get quickly the handlers that need to be
				// executed and push them on a list
				lock(handlers) {
					for(var i = handlers.Count-1; i >= 0; i--) {
						// iterate backwards to support removal
						var h = handlers[i];
						var m = h.Mapping;
						
						if(m.Removed) {
							// handler mapping does not exist anymore
							handlers.RemoveAt(i);
						}
						else if(h.SetValues(m.Input, m.Output)) {
						    // handler needs to be executed
							dispatchList.Add(h);
						}
					}
				}
				
				// now execute the handlers. This may take unknown time depending
				// on the handlers implementation
				if(dispatchList.Count > 0) {
					foreach(var h in dispatchList) {
						try {
							h.Execute();
						}
						catch(Exception e) {
							// this handler misbehaves - remove it
							System.Diagnostics.Debug.WriteLine("Removing dead handler: "+e.Message);
							lock(handlers)
								handlers.Remove(h);
						}
					}
					
					dispatchList.Clear();
				}
				
				dispatchLoopWait.Reset();
			}
		}
	}
}
