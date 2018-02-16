
# VJoySerialFeeder #
## What is it? ##
A program for feeding data from a serial port to the [vJoy](http://vjoystick.sourceforge.net) virtual joystick driver.

The data coming through the serial port should be structured in a specific way in order for the feeder to recognize it. Currently two protocols are supported - IBUS (used by FlySky radios) and MultiWii Serial Protocol (MSP).

After data is received it can be _mapped_ to any vJoy axis or button in very flexible and configurable way.

![Screenshot](docs/images/screenshot.png)

## Use cases ##
1. Use Arduino to read data from any device and send it to your PC - basic sketch in the [Arduino](Arduino/Joystick) directory. See [example](docs/Arduino.md) on using old RC controller for simulators.
2. Read controller directly from any IBUS capable FlySky receiver. [Example](docs/FlySky.md)
3. Use MultiWii compatible Flight Controller (MultiWii, CleanFlight, BetaFlight, etc.). Connect your RC model to your PC, choose the MultiWii protocol and open the USB serial port (the one you would normally use for your Configurator utility).
4. Feed over network. You can use pairs of virtual serial ports - for example [com0com](http://com0com.sourceforge.net/) - and tool like [socat](http://www.dest-unreach.org/socat/doc/socat.html) or [netcat](https://eternallybored.org/misc/netcat/), to achieve almost anything you need.

## How to get it? ##
You can dowload binaries from the [releases](../../releases) section or you can build it yourself. Development is done with [SharpDevelop 4.4](http://www.icsharpcode.net/opensource/sd/)

## How to use it? ##
Check ot the [Manual](docs/MANUAL.md).
