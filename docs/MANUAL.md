# VJoySerialFeeder Manual #
## Basic concepts ##

* Channel.\
A channel is a stream of integer values coming through the serial port. There can be one or more channels multiplexed over the same serial line.\
_Tech details:_ Serial data arrives in _frames_. Different protocols use different frame structures but in most cases they all can be decoded as an array of integers. The stream consisting of the _first_ values from the arrays of each frame can be thought of as _channel 1_. The second values in the arrays represent _channel 2_ and so on.

* Mapping.\
A mapping is a rule which transforms the integer channel data in a value that can be fed to vJoy. There are two types of mappings:
  1. Axis Mapping - used to transform a channel to a joystick axis (X, Y, Slider, etc.).
  2. Button Mapping - used to transform channel value to a binary button state - pressed/depressed - using threshold(s).
  3. Bit-mapped Button Mapping - takes the channel values as 16-bit integer and allows each bit to command one button (16 buttons total).
  
## How to use ##
1. First you have to decide how you are going to get the serial data you need. Refer to [Use cases](../README.md).

2. After your data provider is ready you should select the COM port and baud rate.

3. Select the correct serial protocol.

4. Select the vJoy joystick instance (if there are more than one).

5. Click the **Connect** button. If everything is OK you should see in the status bar something like:
![status](images/statusbar.png)

6. Open the **Channel Monitor** and see if everything seems to be working OK. Try changing your inputs and confirm that channels are changing value.

7. Start mapping!\
Use the **Add Axis**,  **Add Button** and **Add Bit-mapped Button** buttons to add mappings to the interface.\
All mappings can take data from only one channel. Use the **Channel** input box to select it. What is done with the data depends on the mapping type.
  * In Axis Mappings, there is a **Axis** dropdown menu from which you can select the joystick axis which you would like to command with the selected channel. Please note that your vJoy configuration may or may not have some of the axes enabled.\
Axis Mappings can be thought of as a function which take the input channel value and returns axis value between 0% and 100%.\
  In the **Setup** dialog there are various parameters which can be tweaked to make that function do what you need.
 
 ![axis-setup](images/axis-setup.png)
  
   **Symmetric** tells if the axis has a center point. For example: symmetric axes are the X, Y of a joystick, while an asymmetric axis is the throttle. **Center** and **Deadband** parameters are available only in symmetric mode.\
  **Invert** simply inverts the function output - 0% becomes 100% and vice versa.\
  The **Minimum** parameter determines the channel value which will transform to 0% axis position (100% in inverted). Channel values _less_ than **Minimum** are ignored and axis position will still be 0%.\
  The **Maximum** parameter is analogous.\
  The **Center** parameter determines the channel value which will translate to 50% axis position.\
  The **Deadband** defines a range around the center point which will always translate to 50% axis position.\
  **Expo** makes the channel value transformation a non linear function. The most common use is to make your controls less sensitive around the center and fully responsive near the endpoints.\
  The **Calibrate** button allows easy setting of the **Minimum**, **Maximum** and **Center** parameters. Just click it and follow the instructions.
  
  * Button Mappings work by defining one or two thresholds. First you have to select the vJoy **Button** (numbered from 1 to 128) that you would like to command with this channel. Please note that the actual number of buttons available depends on the vJoy configuration.\
  Click **Setup** for more parameters:
  
  ![button-setup](images/button-setup.png)
  
  In **one threshold** mode the button state is determined depending on the value being _lower_ or _higher_ than the threshold.\
  In **two thresholds** mode the button is in one state if the value is _between_ the two thresholds and in the other state if it is _outside_ them.\
  The **Invert** parameter inverts the button logic.\
  The **Calibrate** buttons allows for easy setup of the threshold. It only works in **one threshold** mode.
  
  * Bit-mapped Button Mappings take the channel's 16-bit value and allows each bit to be mapped to a button. The interface show representation of the channel 16 bits as boxes:
  
  ![bitmap](images/bitmap.png)
  
  _Tech details:_ The LSB is displayed on the _left_. The usual way to represent the bits of a number is to place the LSB to the _right_, but for our purposes LSB to the left makes more sense.
  
  Initially the bits are not mapped to anything. Click a bit box to set it up:
  
  ![bitmap-setup](images/bitmap-setup.png)
  
  **Enable** tells that you want to map this bit.\
  **Button** selects the vJoy button you want to command with this bit.\
  Normally, a zero bit results in depressed button and one bit - in pressed. If you want the reverse - check **Invert**.\
  After a button has been mapped its vJoy button id is shown in the box. If the button is inverted an overbar is shown above the number.\
  For unmapped bits (no number shown) the color of the box represents the _input_ bit. For mapped bits the color represents the vJoy button state.
  
8. Save your your configuration as a **Profile**. Enter a profile name in the text box and click **Save**. The last used profile is automatically loaded on next run.
