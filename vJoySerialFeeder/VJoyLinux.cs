using System;
using System.Runtime.InteropServices;

namespace vJoySerialFeeder
{
    /// <summary>
    /// Virtual joystick is realized with the uinput device
    /// and the `libevdev` library.
    /// </summary>
    public class VJoyLinux : VJoy
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







        IntPtr dev, uidev;
        int[] axes = new int[NUM_AXES];
        bool[] buttons = new bool[NUM_BUTTONS];
        bool needSyn;


        public override string Init()
        {
            InputAbsinfo axisInfo = new InputAbsinfo() {
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
                return "libevdev_enable_event_type() failed";

            for (var ax = ABS_X; ax < ABS_X + NUM_AXES; ax++)
                if (libevdev_enable_event_code(dev, EV_ABS, ax, piai) != 0)
                    return "libevdev_enable_event_code() failed";

            if (libevdev_enable_event_type(dev, EV_KEY) != 0)
                return "libevdev_enable_event_type() failed";

            for (var btn = BTN_JOYSTICK; btn < BTN_JOYSTICK + NUM_BUTTONS; btn++)
                if (libevdev_enable_event_code(dev, EV_KEY, btn, IntPtr.Zero) != 0)
                    return "libevdev_enable_event_code() failed";
            
            if (libevdev_uinput_create_from_device(
                dev, -2 /*LIBEVDEV_UINPUT_OPEN_MANAGED*/, ref uidev) != 0)
                return "libevdev_uinput_create_from_device() failed";

            return null;
        }

        public override string Acquire(uint id)
        {
            return null;
        }

        public override object[] GetJoysticks()
        {
            if(uidev != IntPtr.Zero)
                return new object[] { "1" };
            return new object[] { };
        }



        public override void Release()
        {
            if (uidev == IntPtr.Zero) return;

            // center all axes and turn off all buttons
            for (var i = 0; i < axes.Length; i++)
                SetAxis(0.5, i);
            for (var i = 0; i < buttons.Length; i++)
                SetButton(false, (uint)i);
            SetState();
            
        }

        public override void SetAxis(double value, int axis)
        {
            if (uidev == IntPtr.Zero) return;

            // map [0; 1] -> [-32767; 32767]
            var ival = (int)(2*(value - 0.5) * 32767);

            if (axis < axes.Length && axes[axis] != ival) {
                axes[axis] = ival;
                needSyn = true;
                libevdev_uinput_write_event(uidev, EV_ABS, ABS_X + axis, ival);
            }


        }

        public override void SetButton(bool value, uint btn)
        {
            if (uidev == IntPtr.Zero) return;

            if (btn < buttons.Length && buttons[btn] != value) {
                buttons[btn] = value;
                needSyn = true;
                libevdev_uinput_write_event(uidev,
                                        EV_KEY,
                                        BTN_JOYSTICK + (int)btn,
                                        value ? 1 : 0);
            }


        }

        public override void SetState()
        {
            if (uidev == IntPtr.Zero) return;

            if (needSyn) {
                libevdev_uinput_write_event(uidev, EV_SYN, SYN_REPORT, 0);
                needSyn = false;
            }
        }
    }
}
