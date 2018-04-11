# SBUS protocol

SBUS protocol sends data over a UART pin configured at 100 000 bps and using 8-E-2 framing. It also uses inverted signal levels. This makes it impossible for most USB UART converters to read the signal directly - the signal must first be inverted.

There are several options:
1. Use Arduino. This is by far the most convenient method since it combines the signal
   inversion with the USB UART converter in the Arduino itself. Arduino Nano is perfect
   for the job - it is cheap and compact.\
   Simply flash [this](../Arduino/Sbus_invert/Sbus_invert.ino) sketch,
   power your receiver from the Arduino board, make the connections shown below
   and that's it. Start vJoySerialFeeder, choose the Arduino's COM port,
   select `SBUS` protocol and click `Connect`.

   SBUS Receiver Pin | Arduino Pin
   --- | ---
   +5V | +5V
   GND | GND
   SBUS | D3

   [Snille](https://github.com/Snille) has created a 3D printable box for the Arduino Nano and FrSky R-XSR Receiver. Check it out: [https://www.thingiverse.com/thing:2852557](https://www.thingiverse.com/thing:2852557)

2. Most receivers have a dedicated inverter component on their boards. If we can tap at the signal _before_ inversion, we can send it to the UART converter. [OscarLiang.com](https://oscarliang.com/uninverted-sbus-smart-port-frsky-receivers/) has info about the location of the uninverted signal for different receivers. See [here](https://oscarliang.com/uninverted-sbus-smart-port-frsky-receivers/).\
Once you have found the uninverted pad, you can refer to the [FlySky/IBUS How-to](FlySky.md) with the only difference being that you use the uninverted SBUS signal instead of IBUS and you have to select the `SBUS` protocol in vJoySerialFeeder.

3. Use hardware inverter ([example](https://imgur.com/a/dDPaZ)) between the receiver the UART converter. Other than that, everything is the same as [FlySky/IBUS How-to](FlySky.md) with the only difference being that you use the hardware inverted SBUS signal instead of IBUS and you have to select the `SBUS` protocol in vJoySerialFeeder.
