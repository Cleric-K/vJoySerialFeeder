# vXbox

Emulation of Xbox 360 controller is supported on Windows with the ScpVBus driver.

Download it from [here](https://github.com/shauleiz/ScpVBus/releases) and follow
the installation instructions there.
> Pay attention that on Windows versions prior to 10, you must install
the [Xbox 360 drivers](https://www.microsoft.com/accessories/en-us/products/gaming/xbox-360-controller-for-windows/52a-00004#techspecs-connect)
before you install ScpVBus.

In vJoySerialFeeder you simply select one of the four joysticks starting with `vXbox.`.

Xbox controllers support 6 axes. Here's how they correspond with the standard
8 axes:

Axis #|1 | 2| 3| 4| 5| 6| 7| 8|
---|---|---|---|---|---|---|---|---
Standard Axis Name| X| Y| Z| Rx| Ry| Rz| Slider0| Slider1
Xbox Axis Name |Left-X |Left-Y |Right-Trigger|Right-X| Right-Y |Left-Trigger| N/A | N/A

10 buttons are supported:

1 |2| 3| 4| 5| 6| 7| 8| 9| 10
---|---|---|---|---|---|---|---|---|---
A |B |X |Y |Left-Shoulder|Right-Shoulder|Back| Start| Leftthumb|Rightthumb