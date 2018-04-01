/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 28.3.2018 г.
 * Time: 14:52 ч.
 */
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Implements vXbox joystick using the vXboxInterface.dll from
	/// https://github.com/shauleiz/vXboxInterface
	/// and ScpVbus https://github.com/shauleiz/ScpVBus
	/// </summary>
	public class VJoyVXBox : VJoyBase
	{
		internal class State {
			internal short[] Axes = new short[6];
			internal bool[] Buttons = new bool[11];
		}
		
		#region vXboxInterface.dll functions
		// control functions
		[DllImport("vXboxInterface.dll")]
		static extern bool isVBusExists();
		
		[DllImport("vXboxInterface.dll")]
		static extern bool PlugIn(uint idx);
		
		[DllImport("vXboxInterface.dll")]
		static extern bool UnPlug(uint idx);
		
		[DllImport("vXboxInterface.dll")]
		static extern bool isControllerExists(uint idx);
		
		// axes
		[DllImport("vXboxInterface.dll")]
		static extern bool SetAxisX(uint idx, short val);
		
		[DllImport("vXboxInterface.dll")]
		static extern bool SetAxisY(uint idx, short val);
		
		[DllImport("vXboxInterface.dll")]
		static extern bool SetAxisRx(uint idx, short val);
		
		[DllImport("vXboxInterface.dll")]
		static extern bool SetAxisRy(uint idx, short val);
		
		[DllImport("vXboxInterface.dll")]
		static extern bool SetTriggerL(uint idx, byte val);
		
		[DllImport("vXboxInterface.dll")]
		static extern bool SetTriggerR(uint idx, byte val);
		
		// buttons
		[DllImport("vXboxInterface.dll")]
		static extern bool SetBtnA(uint idx, bool val);
		
		[DllImport("vXboxInterface.dll")]
		static extern bool SetBtnB(uint idx, bool val);
		
		[DllImport("vXboxInterface.dll")]
		static extern bool SetBtnX(uint idx, bool val);
		
		[DllImport("vXboxInterface.dll")]
		static extern bool SetBtnY(uint idx, bool val);
		
		[DllImport("vXboxInterface.dll")]
		static extern bool SetBtnStart(uint idx, bool val);
		
		[DllImport("vXboxInterface.dll")]
		static extern bool SetBtnBack(uint idx, bool val);
		
		[DllImport("vXboxInterface.dll")]
		static extern bool SetBtnLT(uint idx, bool val);
		
		[DllImport("vXboxInterface.dll")]
		static extern bool SetBtnRT(uint idx, bool val);
		
		[DllImport("vXboxInterface.dll")]
		static extern bool SetBtnLB(uint idx, bool val);
		
		[DllImport("vXboxInterface.dll")]
		static extern bool SetBtnRB(uint idx, bool val);
		
		[DllImport("vXboxInterface.dll")]
		static extern bool SetBtnGD(uint idx, bool val);
		
		#endregion
		
		
		delegate bool AxisSetter(uint idx, short val);
		delegate bool ButtonSetter(uint idx, bool val);
		
		// helpers to find the function calls by index
		static AxisSetter[] axisSetters = new AxisSetter[] {
			SetAxisX, SetAxisY,
			(idx, val) => SetTriggerL(idx, (byte)val), // trigger has to cast to byte
			SetAxisRx, SetAxisRy,
			(idx, val) => SetTriggerR(idx, (byte)val) // trigger has to cast to byte
		};
		
		static ButtonSetter[] buttonSetters = new ButtonSetter[] {
			SetBtnA, SetBtnB, SetBtnX, SetBtnY,
			SetBtnLB, SetBtnRB,
			SetBtnBack, SetBtnStart,
			SetBtnLT, SetBtnRT,
			SetBtnGD
		};
		
		uint id;
		State prevState, state;
		
		public override void SetState()
		{
			_setState(false);
		}
		
		/// <summary>
		/// Call Set functions only if needed or force is requested
		/// </summary>
		/// <param name="force"></param>
		void _setState(bool force) {
			// axes
			for(var i = 0; i < state.Axes.Length; i++) {
				if(force || state.Axes[i] != prevState.Axes[i]) {
					axisSetters[i](id, state.Axes[i]);
					prevState.Axes[i] = state.Axes[i];
				}
			}
			
			// buttons
			for(var i = 0; i < state.Buttons.Length; i++) {
				if(force || state.Buttons[i] != prevState.Buttons[i]) {
					buttonSetters[i](id, state.Buttons[i]);
					prevState.Buttons[i] = state.Buttons[i];
				}
			}
		}
		
		public override void SetButton(int btn, bool value)
		{
			if(btn < 0 || btn > 10)
				return;
			state.Buttons[btn] = value;
		}
		
		public override void SetAxis(int axis, double value)
		{
			if(axis < 0 || axis > 5)
				return;
			
			short val;
			
			if(axis == 2 /*left trigger*/ || axis == 5 /*right trigger*/)
				// triggers have lower resolution
				// trigger range 0 to 255
				val = (short)Math.Round(value*0xff); 
			else
				// regular axes range -32768 to 32767
				val = (short)(Math.Round(value*0xffff) - 0x8000);
			
			state.Axes[axis] = val;
		}
		
		public override void Release()
		{
			// set everything in zero state
			state = new State();
			_setState(true);
			
			UnPlug(id);
		}
		
		public override List<string> GetJoysticks()
		{
			return isVBusExists() ?
				new List<string>(new string[] {"1","2","3","4"})
				:
				new List<string>();
		}
		
		public override void Acquire(string id)
		{
			if(!isVBusExists())
				throw new VJoyException("ScpVBus driver not installed");
			
			if(!uint.TryParse(id, out this.id))
				throw new VJoyException(String.Format("Invalid joystick id {0}", id));
			
			if(isControllerExists(this.id))
				throw new VJoyException(String.Format("Joystick id {0} already in use", id));
			
			if(!PlugIn(this.id))
				throw new VJoyException(String.Format("Could not open joystick id {0}", id));
			
			prevState = new State();
			state = new State();
			
			// set everything in zero state
			_setState(true);
		}
	}
}
