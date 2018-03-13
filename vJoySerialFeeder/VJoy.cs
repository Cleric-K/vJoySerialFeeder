/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 14.6.2017 г.
 * Time: 22:44 ч.
 */
using System;
using System.Collections;
using System.Windows.Forms;
using vJoyInterfaceWrap;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Description of VJoy.
	/// </summary>
	public class VJoy
	{
        public enum Axes {
			X,
			Y,
			Z,
			Rx,
			Ry,
			Rz,
			Sl0,
			Sl1
		};


		private vJoy joystick;
		private vJoy.JoystickState state;
        private uint id;

		public VJoy()
		{
			// Create one joystick object
            joystick = new vJoy();
            state = new vJoy.JoystickState();
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
		
		public void SetAxis(float value, int axis) {
			// vjoy accepts axis values from 0 to 32767 (0x7fff)
			int v = (int)(value*0x7fff); 

			switch((Axes)axis) {
				case Axes.X:
				   state.AxisX = v;
				   break;
				case Axes.Y:
				   state.AxisY = v;
				   break;
				case Axes.Z:
				   state.AxisZ = v;
				   break;
				case Axes.Rx:
				   state.AxisXRot = v;
				   break;
				case Axes.Ry:
				   state.AxisYRot = v;
				   break;
				case Axes.Rz:
				   state.AxisZRot = v;
				   break;
				case Axes.Sl0:
				   state.Slider = v;
				   break;
				case Axes.Sl1:
				   state.Dial = v;
				   break;
			}
		}
		
		public void SetButton(bool value, uint btn) {
			if(value)
				state.Buttons |= (uint)1<<(int)btn;
			else
				state.Buttons &= ~((uint)1<<(int)btn);
		}

		public void SetState() {
			joystick.UpdateVJD(id, ref state);
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
