/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 28.3.2018 г.
 * Time: 16:01 ч.
 */
using System;
using System.Collections.Generic;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Dummy vjoy that does nothing
	/// </summary>
	public class VJoyNone : VJoyBase
	{
		public VJoyNone()
		{
		}
		
		public override void SetState()
		{
		}
		
		public override void SetButton(int btn, bool value)
		{
		}
		
		public override void SetAxis(int axis, double value)
		{
		}

		public override void SetDiscPov(int pov, int value)
		{
		}

		public override void SetContPov(int pov, double value)
		{
		}

		public override void Release()
		{
		}
		
		public override List<string> GetJoysticks()
		{
			return null;
		}
		
		public override void Acquire(string id)
		{
		}
	}
}
