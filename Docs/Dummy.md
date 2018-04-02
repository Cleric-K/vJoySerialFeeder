# Dummy serial protocol

The Dummy serial protocol does not need a serial port at all - the COM port and
its settings are completely ignored.

There are two main uses for it:

* If you want to test something without having a real serial device connected.\
  The protocol provides a single channel on which a sinusoidal signal is simulated.

* If you use the advanced features of vJoySerialFeeder - Interaction and Scripting.\
  Sometimes you may need to use vJoySerialFeeder as virtual joystick feeder application
  but you don't really need or have a serial device to give you channel data. In 
  such case you can use the Dummy protocol. You'll have to add the needed Mappings
  with channel set to `0` and set their `Input`/`Output` values through the Interaction
  features.\
  Through the Dummy protocol settings (the `Setup` button) you can set the `Update Rate`.
  This servers as "heart-beat" of your feeder - this is the update rate at which
  mappings' outputs will be fed to the virtual joystick.
  