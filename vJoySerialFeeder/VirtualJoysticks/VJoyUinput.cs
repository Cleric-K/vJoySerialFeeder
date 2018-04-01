using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace vJoySerialFeeder
{
    /// <summary>
    /// Virtual joystick is realized with the uinput device
    /// and the `libevdev` library.
    /// </summary>
    public class VJoyUinput : VJoyBase
    {
        const int NUM_AXES = 8;
        const int NUM_BUTTONS = 32;


        // from kernel input-event-codes.h

        // event types
        const int EV_SYN = 0x00;
        const int EV_KEY = 0x01;
        const int EV_ABS = 0x03;


        // event codes
        const int SYN_REPORT = 0x00;

        const int BTN_JOYSTICK = 0x120;

        const int ABS_X = 0x00;
        const int ABS_Y = 0x01;
        const int ABS_Z = 0x02;
        const int ABS_RX = 0x03;
        const int ABS_RY = 0x04;
        const int ABS_RZ = 0x05;
        const int ABS_THROTTLE = 0x06;
        const int ABS_RUDDER = 0x07;

        // `struct input_absinfo` from kernel input.h
        [StructLayout(LayoutKind.Sequential)]
        internal struct InputAbsinfo
        {
            internal Int32 value;
            internal Int32 minimum;
            internal Int32 maximum;
            internal Int32 fuzz;
            internal Int32 flat;
            internal Int32 resolution;
        };

        #region libevdev pinvoke

        [DllImport("libevdev.so.2")]
        static extern IntPtr libevdev_new();

        [DllImport("libevdev.so.2")]
        static extern void libevdev_set_name(IntPtr dev, IntPtr name);

        [DllImport("libevdev.so.2")]
        static extern int libevdev_enable_event_type(IntPtr dev, int ev);

        [DllImport("libevdev.so.2")]
        static extern int libevdev_enable_event_code(
            IntPtr dev,
            int type,
            int code,
            IntPtr data);

        [DllImport("libevdev.so.2")]
        static extern int libevdev_uinput_create_from_device(
            IntPtr dev, int fd, ref IntPtr uidev);

        [DllImport("libevdev.so.2")]
        static extern int libevdev_uinput_write_event(
            IntPtr dev,
            int type,
            int code,
            int value);

        [DllImport("libevdev.so.2")]
        static extern void libevdev_uinput_destroy(IntPtr dev);

		#endregion


		internal class State {
			internal int[] Axes = new int[NUM_AXES];
	        internal bool[] Buttons = new bool[NUM_BUTTONS];	
		}


        IntPtr dev, uidev;
        State prevState, state;

        public override void Acquire(string id)
        {
            if (uidev == IntPtr.Zero)
                _init();
        	
        	_reset();
        }

        public override List<string> GetJoysticks()
        {
            return new List<string>(new string[] { "1" });
        }



        public override void Release()
        {
            _reset();
        }

        public override void SetAxis(int axis, double value)
        {
            // map [0; 1] -> [-32767; 32767]
            var ival = (int)(Math.Round(2*(value-0.5)*0x7fff));
            if(axis >= 0 && axis < state.Axes.Length)
            	state.Axes[axis] = ival;
        }

        public override void SetButton(int btn, bool value)
        {
        	if(btn >= 0 && btn < state.Buttons.Length)
        		state.Buttons[btn] = value;
        }

        public override void SetState() {
            _setState(false);
        }


        void _init() {
            try
            {
                InputAbsinfo axisInfo = new InputAbsinfo()
                {
                    minimum = -32767,
                    maximum = 32767
                };

                // we give a const pointer to libevdev_enable_event_code()
                // so we have to allocate unmanaged memory and copy the struct
                IntPtr piai = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(InputAbsinfo)));
                Marshal.StructureToPtr(axisInfo, piai, false);

                dev = libevdev_new();
                libevdev_set_name(dev, Marshal.StringToHGlobalAnsi("vJoy"));

                if (libevdev_enable_event_type(dev, EV_ABS) != 0)
                    throw new VJoyException("libevdev_enable_event_type() failed");

                for (var ax = ABS_X; ax < ABS_X + NUM_AXES; ax++)
                    if (libevdev_enable_event_code(dev, EV_ABS, ax, piai) != 0)
                        throw new VJoyException("libevdev_enable_event_code() failed");

                if (libevdev_enable_event_type(dev, EV_KEY) != 0)
                    throw new VJoyException("libevdev_enable_event_type() failed");

                for (var btn = BTN_JOYSTICK; btn < BTN_JOYSTICK + NUM_BUTTONS; btn++)
                    if (libevdev_enable_event_code(dev, EV_KEY, btn, IntPtr.Zero) != 0)
                        throw new VJoyException("libevdev_enable_event_code() failed");

                if (libevdev_uinput_create_from_device(
                        dev, -2 /*LIBEVDEV_UINPUT_OPEN_MANAGED*/, ref uidev) != 0)
                    throw new VJoyException("libevdev_uinput_create_from_device() failed");
            }
            catch(Exception ex) {
                throw new VJoyException(ex.Message);
            }
        
        }
        
        void _setState(bool force)
        {
        	var needSyn = false;
        	
        	for(var i=0; i < state.Axes.Length; i++) {
        		if(force || state.Axes[i] != prevState.Axes[i]) {
        			libevdev_uinput_write_event(uidev, EV_ABS, ABS_X + i, state.Axes[i]);
        			
        			prevState.Axes[i] = state.Axes[i];
        			needSyn = true;
        		}
        	}
        	
        	for(var i=0; i < state.Buttons.Length; i++) {
        		if(force || state.Buttons[i] != prevState.Buttons[i]) {
        			libevdev_uinput_write_event(uidev,
                                        EV_KEY,
                                        BTN_JOYSTICK + i,
                                        state.Buttons[i] ? 1 : 0);
        			
        			prevState.Buttons[i] = state.Buttons[i];
        			needSyn = true;
        		}
        	}

            if (needSyn)
                libevdev_uinput_write_event(uidev, EV_SYN, SYN_REPORT, 0);
        }
        
        void _reset() {
        	prevState = new State();
        	state = new State();
        	_setState(true);
        }
    }
}
