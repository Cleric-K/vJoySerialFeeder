/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 28.3.2018 г.
 * Time: 15:02 ч.
 */
using System;
using System.Collections.Generic;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Currently Windows support vJoy and vXbox
	/// </summary>
	public class VJoyCollectionWindows : VJoyCollectionBase
	{
		const string PREFIX_VJOY = "vJoy";
		const string PREFIX_VXBOX = "vXbox";
		const string PREFIX_NONE = "None";
		
		VJoyVJoy vjoy;
		VJoyVXBox vjx;
		VJoyNone vjNone;
		
		public VJoyCollectionWindows()
		{
			vjoy = new VJoyVJoy();
			vjx = new VJoyVXBox();
			vjNone = new VJoyNone();
		}
		
		public override VJoyBase GetVJoy(string name)
		{
			var t = name.Split('.');
			
			switch(t[0]) {
				case PREFIX_NONE:
					return vjNone;
					
				case PREFIX_VJOY:
					vjoy.Acquire(t[1]);
					return vjoy;
					
				case PREFIX_VXBOX:
					vjx.Acquire(t[1]);
					return vjx;
			}
			
			return null;
		}
		
		public override string[] GetJoysticks()
		{
			var list = new List<string>();
			
			foreach(var s in vjoy.GetJoysticks()) {
				list.Add(PREFIX_VJOY + "." + s);
			}
			
			foreach(var s in vjx.GetJoysticks()) {
				list.Add(PREFIX_VXBOX + "." + s);
			}
			
			list.Add("None");
			
			return list.ToArray();
		}
	}
}
