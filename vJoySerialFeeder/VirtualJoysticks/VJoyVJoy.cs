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
        	if(btn >=0 && btn < 32)
        		_setButton(ref state.Buttons, btn, value);
        	else if(btn >= 32 && btn < 64)
        		_setButton(ref state.ButtonsEx1, btn-32, value);
        	else if(btn >= 64 && btn < 96)
        		_setButton(ref state.ButtonsEx2, btn-64, value);
        	else if(btn >= 96 && btn < 128)
        		_setButton(ref state.ButtonsEx3, btn-96, value);
        }
        
        private void _setButton(ref uint buttons, int btn, bool value) {
        	if (value)
                buttons |= (uint)1 << (int)btn;
            else
                buttons &= ~((uint)1 << (int)btn);
        }
        
        /// <summary>
        /// From: http://vjoystick.sourceforge.net/site/includes/SDK_ReadMe.pdf
        /// When setting discrete POV bHats contains four nibble for four possible discrete hats
        /// The lowest nibble is used for switch #1, the second nibble
		/// for switch #2, the third nibble for switch #3 and the highest nibble for switch #4
		/// 
		/// Each nibble supports one of the following values:
		/// 	0x0 North (forward)
		/// 	0x1 East (right)
		/// 	0x2 South (backwards)
		/// 	0x3 West (Left)
		/// 	0xF Neutral
        /// </summary>
        /// <param name="pov"></param>
        /// <param name="value"></param>
        public override void SetDiscPov(int pov, int value)
        {
        	uint hv = value < 0 ? 0xf : (uint)value & 0xf;
        	var h = state.bHats;
        	h &= (uint)~(0xf << (pov*4)); // zero out the nibble
        	h |= (uint)((hv & 0xf) << (pov*4)); // set the nibble
        	state.bHats = h;
        }
        
        /// <summary>
        /// From: http://vjoystick.sourceforge.net/site/includes/SDK_ReadMe.pdf
        /// Valid value for POV Hat Switch member is either 0xFFFFFFFF (neutral) or in the range of 0 to 35999 .
        /// </summary>
        /// <param name="pov"></param>
        /// <param name="value"></param>
        public override void SetContPov(int pov, double value)
        {
        	uint hv = value < 0 ?  ~0U : ((uint)((value%360.0)*100));

        	switch(pov)
        	{
        		case 0:
        			state.bHats = hv;
        			break;
        		case 1:
        			state.bHatsEx1 = hv;
        			break;
    			case 2:
        			state.bHatsEx2 = hv;
        			break;
				case 3:
    				state.bHatsEx3 = hv;
        			break;
        	}
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
        	
        	state.bHats = ~0U;
        	state.bHatsEx1 = ~0U;
        	state.bHatsEx2 = ~0U;
        	state.bHatsEx3 = ~0U;
        	
        	SetState();
        }
    }
}
