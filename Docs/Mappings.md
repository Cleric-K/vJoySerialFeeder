# More about Mappings

To use the advanced features of vJoySerialFeeder, some more details about the inner workings of
a Mapping is needed. Every Mapping has the following two properties:
* `Input` - This is a unsigned 16-bit integer value (0 - 65535). Normally `Input` is the last
value read from the serial port, _for the selected channel_. The actual
value range depends on the serial protocol.\
Channels are numbered from 1 onwards. If you need a Mapping that should _not_
receive data from the serial port, set its channel to 0. This is useful if you
intend to set the `Input` with Scripting or Interaction.

* `Output` - after the mapping gets its `Input`, it is transformed to an `Output`
value. The output values depend on the mapping type:
  * Axis Mapping - the output value is a floating point number in the range [0.0; 1.0].
  * Button Mapping - the output value is 0.0 for depressed button and 1.0 for pressed.
  * Bitmapped Button Mapping - unmapped bits are the same as the corresponding
    bits in the `Input` value. For mapped bits the bit in the `Output` represents
	the state of the virtual button that the bit commands -
	`1` if pressed and `0` is depressed. In many cases the `Output` bit will be
	the same as the corresponding `Input` bit, but when `Invert` or `Trigger`
	are used they may differ.
    > Note: When using in Scripting or Interaction the `Output` value of a
      Bitmapped Button Mapping might be read as floating point
      number. Simply cast it to integer before performing bitwise operations on it.

  `Output` is always calculated automatically anytime `Input` is modified. On the other
  hand if `Output` is modified, `Input` is unaffected (that is, there is no _inverse_ function
  to calculate the `Input` from the `Output`).

  In normal usage the `Output` value is fed to the virtual joystick's axes or buttons.
  If the `Output` is intended to be used solely for Scripting and
  Interaction you can set the axis to `None` for Axis Mapping or the button to `0`
  for Button Mappings.

  > Note: Mappings are processed in the order they appear on the screen - from
  top to bottom. For example, if by mistake you have
  Axis Mapping 1 which outputs to axis X and then later Axis Mapping 5
  which also outputs to axis X, that latter mapping will overwrite the output of Mapping 1.
  The _last_ mapping to set a given axis or button will be the effective one.