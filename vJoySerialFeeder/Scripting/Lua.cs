/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 27.3.2018 г.
 * Time: 11:26 ч.
 */
using System;
using System.Windows.Forms;
using MoonSharp.Interpreter;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Description of Lua.
	/// </summary>
	public class Lua
	{
		internal class MappingProxy {
			Mapping mapping;
			
			internal MappingProxy(Mapping m) {
				mapping = m;
			}
			
			public string Type { get { return mapping.GetType().Name; } }
			
			public int Input {
				get { return mapping.Input; }
				set { mapping.Input = value; }
			}

			public float Output {
				get { return mapping.Output; }
				set { mapping.Output = value; }
			}
		}
		
		internal class VJoyProxy {
			VJoyBase vjoy;
			
			internal VJoyProxy(VJoyBase vj) {
				vjoy = vj;
			}
			
			public void SetAxis(int axis, double value) {
				vjoy.SetAxis(axis - 1, value);
			}
			
			public void SetButton(int button, double value) {
				vjoy.SetButton(button - 1, value > 0);
			}
			
			public void SetDiscPov(int pov, double value) {
				vjoy.SetDiscPov(pov - 1, (int)value);
			}
			
			public void SetContPov(int pov, double value) {
				vjoy.SetContPov(pov - 1, value);
			}
		}
		
		
		string scriptSource;
		Script script;
		bool initted;
		Closure update;
		
		public Lua(string src)
		{
			script = new Script(CoreModules.Preset_SoftSandbox);
			this.scriptSource = src;
		}
		
		public void Test() {
			//script.DoString(scriptSource, null, "Script");
			script.LoadString(scriptSource, null, "Script");
		}
		
		public void Init(VJoyBase vjoy, int[] channels) {
			update = null;
			if(scriptSource == null)
				return;
			
			try {
				// setup proxies
				UserData.RegisterProxyType<MappingProxy, Mapping>(m => new MappingProxy(m));
				UserData.RegisterProxyType<VJoyProxy, VJoyBase>(vj => new VJoyProxy(vj));
			
				// setup globals
				script.Globals["AXIS_X"] = 1;
				script.Globals["AXIS_Y"] = 2;
				script.Globals["AXIS_Z"] = 3;
				script.Globals["AXIS_RX"] = 4;
				script.Globals["AXIS_RY"] = 5;
				script.Globals["AXIS_RZ"] = 6;
				script.Globals["AXIS_SL0"] = 7;
				script.Globals["AXIS_SL1"] = 8;
				
				script.Globals["VJoy"] = vjoy;
				script.Globals["Channel"] = (Func<int, int>)(index => {
				                                              	if(index < 1 || index > channels.Length)
				                                              		throw new ScriptRuntimeException("Invalid Channel index");
				                                             	return channels[index-1];
				                                             });
				script.Globals["Mapping"] = (Func<int, Mapping>)(index => {
				                                                 	var m = MainForm.Instance.MappingAt(index - 1);
				                                                 	if(m == null)
				                                                 		throw new ScriptRuntimeException("Invalid Mapping index");
				                                                 	return m;
				                                                 });
				
				script.Globals["Failsafe"] = false;
				
				// execute code
				script.DoString(scriptSource, null, "Script");
				script.Options.DebugPrint = print;
				
				// save update function
				update = script.Globals.Get("update").Function;
				
				initted = true;
			}
			catch(InterpreterException ex) {
				throw ex;
			}
		}
		
		public void Update(VJoyBase vjoy, int[] channels, bool failsafe) {
			if(!initted) {
				Init(vjoy, channels);
			}
			
			if(update != null) {
				try {
					script.Globals["Failsafe"] = failsafe;
					update.Call();
				}
				catch(InterpreterException ex) {
					update = null;
					throw ex;
				}
			}
		}
		
		private Mapping mapping(int index) {
			return MainForm.Instance.MappingAt(index - 1);
		}
		
		private void print(string s) {
			LuaOutputForm.Write(s + "\n");
		}
	}
}
