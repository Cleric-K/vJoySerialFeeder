# MultiWii protocol 

The MultiWii Serial Protocol (MSP) was designed as integral part of the MultiWii software which initiated the first generation of Arduino based Flight Controllers (FC). MSP is used mainly for the communication between the configurator utility and the FC, but OSD, telemetry modules, etc. also use it to get data from the FC.

New FC software that evolved from MultiWii (CleanFlight, BetaFlight, iNav) still use MSP.

MSP allows us to read the RC channels that the FC sees (same data you see on the Receiver tab in your FC configurator utility). This is slightly slower method than reading directly from a receiver with UART converter, but has the benefit that it works with _any_ controller that your FC understands.

## How to use
Simply plug your RC model's USB cable but instead of opening the configurator utility, open vJoySerialFeeder. Choose the correct COM port (the one you would use for the configurator), select `MultiWii` protocol and click `Connect`. Map your channels as usual and enjoy!

Drone Mesh has a video tutorial on this method [here](https://www.youtube.com/watch?v=6RsGqLJqsD4).