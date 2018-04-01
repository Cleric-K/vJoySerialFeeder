using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

using vJoyInterfaceWrap;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Implements vJoy
	/// </summary>
    public class VJoyVJoy : VJoyBase
    {
        private vJoy joystick;
        private vJoy.JoystickState state;
        private int id;

        public VJoyVJoy()
        {
            // Create one joystick object
            joystick = new vJoy();
            state = new vJoy.JoystickState();
        }

        public override void Acquire(string id)
        {
        	// Get the driver attributes (Vendor ID, Product ID, Version Number)
            if (!joystick.vJoyEnabled())
            	throw new VJoyException("vJoy driver not enabled: Failed Getting vJoy attributes.");

            UInt32 DllVer = 0, DrvVer = 0;

            bool match = joystick.DriverMatch(ref DllVer, ref DrvVer);
            if (!match)
            	throw new VJoyException(String.Format("Version of Driver ({0:X}) does NOT match DLL Version ({1:X})", DrvVer, DllVer));
            
            
            if(!int.TryParse(id, out this.id))
            	throw new VJoyException(String.Format("Invalid joystcik id {0}", id));

            // Get the state of the requested device
            VjdStat status = joystick.GetVJDStatus((uint)this.id);
            switch (status)
            {
                case VjdStat.VJD_STAT_OWN:
                    //MessageBox.Show(String.Format("vJoy Device {0} is already owned by this feeder\n", id));
                    break;
                case VjdStat.VJD_STAT_FREE:
                    //MessageBox.Show(String.Format("vJoy Device {0} is free\n", id));
                    break;
                case VjdStat.VJD_STAT_BUSY:
                    throw new VJoyException(String.Format("vJoy Device {0} is already owned by another feeder\nCannot continue", id));
                case VjdStat.VJD_STAT_MISS:
                    throw new VJoyException(String.Format("vJoy Device {0} is not installed or disabled\nCannot continue", id));
                default:
                    throw new VJoyException(String.Format("vJoy Device {0} general error\nCannot continue", id));
            };

            // Acquire the target
            if ((status == VjdStat.VJD_STAT_OWN) || ((status == VjdStat.VJD_STAT_FREE) && (!joystick.AcquireVJD((uint)this.id))))
            	throw new VJoyException(String.Format("Failed to acquire vJoy device number {0}.", id));

            _reset();
        }


        public override void Release()
        {
        	_reset();
        	joystick.RelinquishVJD((uint)id);
        }

        public override void SetAxis(int axis, double value)
        {
        	// there are conflicting sources but it seems
            // vjoy accepts axis values from 1 to 32768 (0x8000)
            int v = 1 + (int)(value * 0x7fff);

            switch ((Axes)axis)
            {
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

        public override void SetButton(int btn, bool value)
        {
            if (value)
                state.Buttons |= (uint)1 << (int)btn;
            else
                state.Buttons &= ~((uint)1 << (int)btn);
        }

        public override void SetState()
        {
        	joystick.UpdateVJD((uint)id, ref state);
        }

        public override List<string> GetJoysticks()
        {
            var list = new List<string>();
            for (uint i = 1; i <= 16; i++)
            {
                if (joystick.isVJDExists(i))
                    list.Add(i.ToString());
            }
            return list;
        }
        
        void _reset() {
        	// reset state
        	state = new vJoy.JoystickState();
        	
        	// center all axes
        	foreach(var a in Enum.GetValues(typeof(Axes)))
        		SetAxis((int)a, 0.5);
        	
        	SetState();
        }
    }
}
