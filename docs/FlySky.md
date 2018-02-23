# VJoySerialFeeder using FlySky receiver #

If you have a FlySky controller with a spare receiver it is very easy to use it with VJoySerialFeeder, since the IBUS pin of the receiver is in fact a UART TX pin working at 115200 bps.

To make it work you need some kind of 5V UART<->COM converter. It is most convenient to use one of those UART USB cables or FTDI adapter. 

If you don't have UART adapter but you have some Arduino board with USB port - you can use it. Basically you would use it just for its UART converter and you don't need the microcontroller at all (but you **MUST** make sure that the Arduino pin labeled TX is **NOT SET AS OUTPUT**, or you can burn a pin on the Arduino or in the receiver). Drone Mesh has made a video explaining it: [youtu.be/TRnu2_TI9Vk](https://youtu.be/TRnu2_TI9Vk)

Here is an example how to connect the receiver to the PC by using a UART USB cable (it would be practically the same for a FTDI adapter):
![UART](images/flysky.jpg)

In the picture above, FS82 micro receiver has been used which is cheap and perfect for this use. A three pin header strip is soldered at the end of its wires for easy connection to the USB cable. The connections should be as follows:

UART USB Cable/FTDI adapter | FlySky Receiver
-----          | ----
GND            | GND
+5V            | +5V
RX             | IBUS

Please note that in some rarer FTDI adapters RX and TX labels are reversed. Always make sure you connect the IBUS pin to the pin of the FTDI where it expects the signal to go IN (towards the PC) - almost always that is RX

After connecting the receiver to the PC through the cable/adapter, you should bind it to your radio controller, if not already bound. It is convenient to define a dedicated model in your controller for this receiver.

Then you should select the correct COM port, choose baud rate 115200, select IBUS protocol and hit **connect**.

Map your channels and enjoy your wireless controller with your favorite sims.
