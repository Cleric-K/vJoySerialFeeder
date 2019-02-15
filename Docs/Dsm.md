Connecting DSM is similar to connecting [FlySky/IBUS](FlySky.md). You can check it out for details.
The most important difference is:
> DSM Receivers are powered with 3.3 Volts !!! If you connect it to a 5V supply you will damage it!<br>Double check the voltage you are using before connecting.

Most Spektrum receivers use standard colors for the connection wires:

Pin | Color | Purpose
---- | --- | ---
1 | Orange | 3.3VDC +/-5%, 20mA max
2 | Black | GND
3 | Gray | DATA

If you are referring to the [FlySky How-to](FlySky.md) just think of `IBUS` as `DATA` and `5V` as `3.3V`.

When starting VJSF you'll have to select `DSM` as protocol.

> You can not use VJSF for binding the receiver to your radio controller. Your receiver have to be already bound.
