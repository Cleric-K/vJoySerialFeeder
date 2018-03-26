using System;
using System.Collections;
using System.Windows.Forms;
using vJoyInterfaceWrap;

namespace vJoySerialFeeder
{
    public class VJoyWindows : VJoy
    {
        private vJoy joystick;
        private vJoy.JoystickState state;
        private uint id;

        public VJoyWindows()
        {
            // Create one joystick object
            joystick = new vJoy();
            state = new vJoy.JoystickState();
        }

        public override string Init()
        {
            // Get the driver attributes (Vendor ID, Product ID, Version Number)
            if (!joystick.vJoyEnabled())
                return "vJoy driver not enabled: Failed Getting vJoy attributes.\n";

            UInt32 DllVer = 0, DrvVer = 0;

            bool match = joystick.DriverMatch(ref DllVer, ref DrvVer);
            if (!match)
                return String.Format("Version of Driver ({0:X}) does NOT match DLL Version ({1:X})\n", DrvVer, DllVer);


            return null;
        }

        public override string Acquire(uint id)
        {
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


        public override void Release()
        {
        	// reset state
        	state = new vJoy.JoystickState();
        	
        	// center all axes
        	foreach(var a in Enum.GetValues(typeof(Axes)))
        		SetAxis(0.5, (int)a);
        	
            joystick.RelinquishVJD(id);
        }

        public override void SetAxis(double value, int axis)
        {
             // vjoy accepts axis values from 0 to 32767 (0x7fff)
            int v = (int)(value * 0x7fff);

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

        public override void SetButton(bool value, uint btn)
        {
            if (value)
                state.Buttons |= (uint)1 << (int)btn;
            else
                state.Buttons &= ~((uint)1 << (int)btn);
        }

        public override void SetState()
        {
            joystick.UpdateVJD(id, ref state);
        }

        public override object[] GetJoysticks()
        {
            ArrayList list = new ArrayList();
            for (uint i = 1; i <= 16; i++)
            {
                if (joystick.isVJDExists(i))
                    list.Add(i.ToString());
            }
            return list.ToArray();
        }
    }
}
