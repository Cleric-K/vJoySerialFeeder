# Dummy serial protocol

The Dummy serial protocol does not need a serial port at all - the serial port and
its settings are completely ignored.

There are two main uses for it:

* If you want to test something without having a real serial device connected.\
  The protocol provides a single channel on which a sinusoidal signal is simulated.

* If you want to use vJoySerialFeeder as general virtual joystick _feeder_ application,
  not necessarily taking data from serial port. In this case you'll have to provide
  the mappings' `Input`s/`Output`s through Interaction and Scripting
  (dont'f forget to set the mappings' channels to `0`).\
  Through the Dummy protocol settings (the `Setup` button) you can set the `Update Rate`.
  This serves as "heart-beat" for your feeder - this is the update rate at which,
  scripts will be executed, events will be dispatched and
  mappings' outputs will be fed to the virtual joystick.
