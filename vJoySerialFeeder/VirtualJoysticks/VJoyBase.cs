/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 14.6.2017 г.
 * Time: 22:44 ч.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace vJoySerialFeeder
{
	/// <summary>
	/// This class should be derived by any
	/// Virtual Joystick Driver implementation
	/// </summary>
	public abstract class VJoyBase
	{
		// Standard 8 joystick axes
        public enum Axes {
			X,
			Y,
			Z,
			Rx,
			Ry,
			Rz,
			Sl0,
			Sl1,
			None
		};
		
		/// <summary>
		/// Any errors should be thrown with this exception
		/// </summary>
		public class VJoyException : Exception {
			public VJoyException(string msg) : base(msg) { }
		}

		/// <summary>
		/// Open the requested virtual joystick
		/// </summary>
		/// <param name="id"></param>
        public abstract void Acquire(string id);

        /// <summary>
        /// Should release the virtual joystick
        /// </summary>
        public abstract void Release();

        /// <summary>
        /// Should set an axis state.
        /// 
        /// This method should *not* update the virtual joystick
        /// immediately. Instead it should keep local state which
        /// is sent to the virtual joystick driver with SetState()
        /// </summary>
        /// <param name="axis">zero based axis index</param>
        /// <param name="value">number in range [0.0; 1.0]</param>
        public abstract void SetAxis(int axis, double value);

        /// <summary>
        /// Should set a button status
        /// 
        /// /// This method should *not* update the virtual joystick
        /// immediately. Instead it should keep local state which
        /// is sent to the virtual joystick driver with SetState()
        /// </summary>
        /// <param name="btn">zero based button index</param>
        /// <param name="value">true/false - pressed/depressed</param>
        public abstract void SetButton(int btn, bool value);
        
        /// <summary>
        /// Set discrete POV Hat. Value depends on driver
        /// </summary>
        /// <param name="pov"></param>
        /// <param name="value"></param>
        public abstract void SetDiscPov(int pov, int value);
        
        /// <summary>
        /// Set continuous POV Hat. Value depends on driver
        /// </summary>
        /// <param name="pov"></param>
        /// <param name="value"></param>
        public abstract void SetContPov(int pov, double value);

        /// <summary>
        /// Should send the accumulated local state to the virtual
        /// joystick driver. Ideally only axes and buttons that have
        /// changed values should be updated.
        /// </summary>
        public abstract void SetState();

        /// <summary>
        /// Must return list of strings, any of which can be used with
        /// Acquire()
        /// </summary>
        /// <returns></returns>
        public abstract List<string> GetJoysticks();
	}
}
