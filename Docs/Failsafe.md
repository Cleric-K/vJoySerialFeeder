# Failsafe

The purpose of Failsafe is _not_ to protect your PC if it flies out of range.
Instead it provides you with a way to specify joystick behavior if you _deliberately_
turn off your serial source. This is useful if you use a RC controller with a game
and the game does not require joystick input all the time. To save battery you
may want to turn off your controller when it is not needed. This is where Failsafe
settings come into play - they allow you to specify concrete behavior when
the serial data is unavailable. See [Quickstart](Quickstart.md) for details on the specific
settings. In summary they are two kinds:
* Use the _last_ values before the signal was lost
* Use specified Failsafe value

> Note: Failsafe is _not_ activated if you use USB serial converter and the
converted itself is unplugged. In that case vJoySerialFeeder will be disconnected.
Failsafe is activated if your serial port is connected but it does not
receive proper serial data or if the protocol supports some way to signal
that failsafe should be activated.