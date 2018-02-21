# VJoySerialFeeder using FlySky receiver #

If you have a FlySky controller with a spare receiver it very easy to use it with VJoySerialFeeder, since the IBUS pin of the receiver is in fact a UART TX pin working at 115200 bps.

One way to way to connect the receiver to the PC is by using UART USB cable or FTDI adapter:
![UART](images/flysky.jpg)

The connections should be as follows:

UART USB Cable | FlySky Receiver
-----          | ----
GND            | GND
+5V            | +5V
RX             | IBUS

In the picture above, FS82 micro receiver has been used which is cheap and perfect for this use.

After connecting the receiver to the PC through the cable/adapter, you should bind it to your radio controller, if not already bound. It is convenient to define a dedicated model in your controller for this receiver.

Then you should select the correct COM port, choose baud rate 115200, select IBUS protocol and hit **connect**.

Map your channels and enjoy your wireless controller with your favorite sims.
