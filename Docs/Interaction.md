# vJoySerialFeeder Process Interaction

Sometimes it may be useful to use the serial data available to vJoySerialFeeder
for other purposes. In other cases you may want to command some axes or buttons
from outside vJoySerialFeeder.

The classic example would be to develop a dashboard software. Let's say you built a cockpit
with some sticks, levers, switches and you use Arduino to feed the data to
vJoySerialFeeder. Using the Interaction API you can create a dashboard (for example as
a web page opened on a tablet) which visualizes
some of the data coming through the serial channels. Furthermore you can make
"soft" switches, buttons, axes on your dashboard which can be fed back to
vJoySerialFeeder and from there to the virtual joystick.

## General concepts

The Interaction capability allows data to be read and written from a _running_
vJoySerialFeeder. Data is exchanged on _per mapping_ basis.

### Reading and Writing
Each mapping can be read and/or written to.

#### Reading
In general there are two ways to read data from a mapping:
* Polling - you can request the `Input` and `Output` values anytime you need them
* Events - you can subscribe to changes in the values of a mapping. There are
two types of events:
   * Input events - these occur when the `Input` value of the mapping has changed.
   * Output events - these occur when the `Output` value of the mapping has changed.

   You can also subscribe to _both_ kind of events at the same time.\
   In most cases we care for the "output" type. For example, in Button Mappings
   we want to know when the button changes state (`Output` jumps to either 0.0 or 1.0)
   and we don't care about the actual `Input` which may be varying slightly all the time.


#### Writing
You can set the `Input` and `Output` values of a Mapping yourself. If you modify the `Input`, `Output`
will be automatically updated based on the Mapping's transformation rules.
> Note: If you write to a mapping that is currently getting its input from a serial
channel, your changes will be quickly overwritten. If you want to use a Mapping
for writing you _must_ assign it to channel `0` (see [More about Mappings](Mappings.md)).


## Interaction options

vJoySerialFeeder gives two options for process interaction:
1. Microsoft COM\
   Not to be confused with serial COM ports, this technology
allows you to interact vJoySerialFeeder from any COM enabled application -
for example: VBScript, AutoHotKey, etc. [How-to](COM.md).
2. WebSocket\
   Since web browsers are available on any PC you can use this
option to quickly develop a HTML based dashboard. [How-to](WebSocket.md).