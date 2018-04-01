/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 28.3.2018 г.
 * Time: 23:20 ч.
 */
using System;
using System.Collections.Generic;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Description of VJoyCollectionLinux.
	/// </summary>
	public class VJoyCollectionLinux : VJoyCollectionBase
	{
		const string PREFIX_UINPUT = "uinput";
		const string PREFIX_NONE = "None";
		
		VJoyUinput vju;
		VJoyNone vjNone;
		
		public VJoyCollectionLinux()
		{
			vju = new VJoyUinput();
			vjNone = new VJoyNone();
		}
		
		public override VJoyBase GetVJoy(string name)
		{
			var t = name.Split('.');
			
			switch(t[0]) {
				case PREFIX_UINPUT:
					vju.Acquire(t[1]);
					return vju;
				case PREFIX_NONE:
					return vjNone;
			}
			return null;
		}
		
		public override string[] GetJoysticks()
		{
			var list = new List<string>();
			foreach(var n in vju.GetJoysticks()) {
				list.Add(PREFIX_UINPUT + "." + n);
			}
			list.Add("None");
			
			return list.ToArray();
		}
	}
}
