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
using vJoyInterfaceWrap;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Description of VJoy.
	/// </summary>
	public class VJoy
	{
		vJoy joystick;
		public vJoy.JoystickState jState { get; private set;}
        uint id;

		public VJoy()
		{
			// Create one joystick object
            joystick = new vJoy();
            jState = new vJoy.JoystickState();
		}
		
		public bool Init() {
            // Get the driver attributes (Vendor ID, Product ID, Version Number)
            if (!joystick.vJoyEnabled())
            {
                MessageBox.Show("vJoy driver not enabled: Failed Getting vJoy attributes.\n");
                return false;
            }
            
            UInt32 DllVer = 0, DrvVer = 0;

            bool match = joystick.DriverMatch(ref DllVer, ref DrvVer);
            if (!match)
            {
            	
            	MessageBox.Show(String.Format("Version of Driver ({0:X}) does NOT match DLL Version ({1:X})\n", DrvVer, DllVer));
            	return false;
            }
            
            return true;
		}
		
		public string Acquire(uint id) {
			this.id = id;
			
			// Get the state of the requested device
            VjdStat status = joystick.GetVJDStatus(id);
            switch (status)
            {
                case VjdStat.VJD_STAT_OWN:
            		//MessageBox.Show(String.Format("vJoy Device {0} is already owned by this feeder\n", id));
                    break;
                case VjdStat.VJD_STAT_FREE:
                    //MessageBox.Show(String.Format("vJoy Device {0} is free\n", id));
                    break;
                case VjdStat.VJD_STAT_BUSY:
                    return String.Format("vJoy Device {0} is already owned by another feeder\nCannot continue\n", id);
                case VjdStat.VJD_STAT_MISS:
                    return String.Format("vJoy Device {0} is not installed or disabled\nCannot continue\n", id);
                default:
                    return String.Format("vJoy Device {0} general error\nCannot continue\n", id);
            };
            
            // Acquire the target
            if ((status == VjdStat.VJD_STAT_OWN) || ((status == VjdStat.VJD_STAT_FREE) && (!joystick.AcquireVJD(id))))
            	return String.Format("Failed to acquire vJoy device number {0}.\n", id);
                
            return null;
		}
			
		
		public void Release() {
			joystick.RelinquishVJD(id);
		}
		
		public void SetAxis(double value, HID_USAGES axis) {
			joystick.SetAxis((int)(value*0x7fff), id, axis);
		}
		
		public void SetButton(bool value, uint btn) {
			joystick.SetBtn(value, id, btn);
		}
		
		public object[] GetJoysticks() {
			ArrayList list = new ArrayList();
			for(uint i = 1; i <= 16; i++) {
				if(joystick.isVJDExists(i))
					list.Add(i.ToString());
			}
			return list.ToArray();
		}
	}
}
