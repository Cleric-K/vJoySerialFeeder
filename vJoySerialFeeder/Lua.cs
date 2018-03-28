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
			
			public string Type { get { return this.GetType().ToString(); } }
			
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
				vjoy.SetAxis(axis, value);
			}
			
			public void SetButton(int button, double value) {
				vjoy.SetButton((button - 1), value > 0);
			}
		}
		
		
		string scriptSource;
		Script script;
		Closure update;
		
		public Lua(string src)
		{
			script = new Script(CoreModules.Preset_SoftSandbox);
			this.scriptSource = src;
		}
		
		public void Test() {
			script.DoString(scriptSource, null, "Script");
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
				script.Globals["AXIS_X"] = 0;
				script.Globals["AXIS_Y"] = 1;
				script.Globals["AXIS_Z"] = 2;
				script.Globals["AXIS_RX"] = 3;
				script.Globals["AXIS_RY"] = 4;
				script.Globals["AXIS_RZ"] = 5;
				script.Globals["AXIS_SL0"] = 6;
				script.Globals["AXIS_SL1"] = 7;
				
				script.Globals["VJoy"] = vjoy;
				script.Globals["Channel"] = (Func<int, int>)(index => channels[index-1]);
				script.Globals["Mapping"] = (Func<int, Mapping>)(index => MainForm.Instance.MappingAt(index - 1));
				
				// execute code
				script.DoString(scriptSource, null, "Script");
				
				// call init function if defined
				var initFunc = script.Globals.Get("init").Function;
				if(initFunc != null)
					initFunc.Call();
				
				// save update function
				update = script.Globals.Get("update").Function;
			}
			catch(InterpreterException ex) {
				throw ex;
			}
		}
		
		public void Update() {
			if(update != null) {
				try {
					update.Call();
				}
				catch(InterpreterException ex) {
					update = null;
					throw ex;
				}
			}
		}
		
		public void Stop() {
			update = null;
		}
		
		private Mapping mapping(int index) {
			return MainForm.Instance.MappingAt(index - 1);
		}
	}
}
