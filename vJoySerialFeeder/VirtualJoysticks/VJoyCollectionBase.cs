/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 28.3.2018 г.
 * Time: 15:00 ч.
 */
using System;

namespace vJoySerialFeeder
{
	/// <summary>
	/// VJoyCollection base.
	/// </summary>
	public abstract class VJoyCollectionBase
	{
		public abstract string[] GetJoysticks();
			
		public abstract VJoyBase GetVJoy(string name);
	}
}
