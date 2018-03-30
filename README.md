
# VJoySerialFeeder #
## What is it? ##
A program for feeding data from a serial port to the [vJoy](http://vjoystick.sourceforge.net) virtual joystick driver (vJoy version 2.x is required).

The data coming through the serial port should be structured in a specific way in order for the feeder to recognize it. Currently three protocols are supported - IBUS (used by FlySky radio controllers), MultiWii Serial Protocol (used by RC Flight Controllers running MultiWii, CleanFlight, BetaFlight, iNav, etc.) and SBUS (used by FrSky, Futaba radio controllers).

After data is received it can be _mapped_ to any vJoy axis or button in very flexible and configurable way.

![Screenshot](Docs/images/screenshot.png)

## Use cases ##
1. Use Arduino to read data from _any_ device and send it to your PC - basic sketch in the [Arduino](Arduino/Joystick) directory. See [example](Docs/Arduino.md) on using old RC controller for simulators.
2. Read RC controller (FlySky) directly from any IBUS capable receiver. [How-to](Docs/FlySky.md).
3. Read RC controller (FrSky, Futaba, etc.) directly from any SBUS receiver. [How-to](Docs/Sbus.md).
4. Use MultiWii compatible Flight Controller (MultiWii, CleanFlight, BetaFlight, etc.). You can use your actual RC model. [How-to](Docs/MultiWii.md).
5. Feed over network. You can use pairs of virtual serial ports provided by [com0com](http://com0com.sourceforge.net/) and [com2tcp](https://sourceforge.net/projects/com0com/files/com2tcp) for the TCP/IP transport. Another option is [HW VSP3](https://www.hw-group.com/products/hw_vsp/index_en.html) which combines the virtual serial port and the TCP/IP transport but the free version allows only one COM port.

## How to get it? ##
You can dowload binaries from the [releases](../../releases) section or you can build it yourself. Development is done with [SharpDevelop 4.4](http://www.icsharpcode.net/opensource/sd/)

## How to use it? ##
Check out the [Manual](Docs/README.md).

## Like it?
If this software brought a smile on your face, you may shine back if you feel like it: [![Donate](https://www.paypalobjects.com/en_US/i/btn/btn_donate_SM.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=L5789HZB5NAX4&lc=BG&currency_code=USD&bn=PP%2dDonationsBF%3abtn_donate_SM%2egif%3aNonHosted)\
Thank you!!!
