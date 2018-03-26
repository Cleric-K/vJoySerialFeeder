/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 14.6.2017 г.
 * Time: 22:44 ч.
 */
using System;
using System.Collections;
using System.Windows.Forms;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Description of VJoy.
	/// </summary>
	public abstract class VJoy
	{
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


        public abstract string Init();

        public abstract string Acquire(uint id);

        public abstract void Release();

        public abstract void SetAxis(double value, int axis);

        public abstract void SetButton(bool value, uint btn);

        public abstract void SetState();

        public abstract object[] GetJoysticks();
	}
}
