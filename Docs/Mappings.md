# More about Mappings

To use the Advanced features, some more details about the inner workings of
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
  * Bitmapped Button Mapping - the value is the same as `Input`, except for bits
  which have been set as inverted buttons - for such buttons, their bits in `Output`
  are flipped compared to the same bits in the `Input`.
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