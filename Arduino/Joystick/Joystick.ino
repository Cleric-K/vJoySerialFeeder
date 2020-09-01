#include "ibus.h"


// //////////////////
// Edit here to customize

// How often to send data?
#define UPDATE_INTERVAL 10 // milliseconds

// To disable reading a specific type of pin, set the count to 0 and remove all items from the pins list

// 1. Analog channels. Data can be read with the Arduino's 10-bit ADC.
// This gives values from 0 to 1023.
// Specify below the analog pin numbers (as for analogRead) you would like to use.
// Every analog input is sent as a single channel.
// Arduino Mega has 16 analog pins, however if your device has fewer you'll need to modify the count and pin list below

#define ANALOG_INPUTS_COUNT 16
byte analogPins[] = {A0, A1, A2, A3, A4, A5, A6, A7, A8, A9, A10, A11, A12, A13, A14, A15}; // element count MUST be == ANALOG_INPUTS_COUNT


// 2. Digital channels. Data can be read from Arduino's digital pins.
// They could be either LOW or HIGH.
// Specify below the digital pin numbers (as for digitalRead) you would like to use.
// Every pin is sent as a single channel. LOW is encoded as 0, HIGH - as 1023
// Arduino Mega has 54 digital only pins and the ability to read the analog pins as digital via pin numbers 55-68. If your device has fewer you'll need to modify the count and pin list below
#define DIGITAL_INPUTS_COUNT 54
byte digitalPins[] = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53}; // element count MUST be == DIGITAL_INPUTS_COUNT


// 3. Digital bit-mapped channels. Sending a single binary state as a 16-bit
// channel is pretty wasteful. Instead, we can encode one digital input
// in each of the 16 channel bits.
// Specify below the digital pins (as for digitalRead) you would like to send as
// bitmapped channel data. Data will be automatically organized in channels.
// The first 16 pins will go in one channel (the first pin goes into the LSB of the channel).
// The next 16 pins go in another channel and so on
// LOW pins are encoded as 0 bit, HIGH - as 1.
#define DIGITAL_BITMAPPED_INPUTS_COUNT 0
byte digitalBitmappedPins[] = {}; // element count MUST be == DIGITAL_BITMAPPED_INPUTS_COUNT


// Define the appropriate analog reference source. See
// https://www.arduino.cc/reference/en/language/functions/analog-io/analogreference/
// Based on your device voltage, you may need to modify this definition
#define ANALOG_REFERENCE DEFAULT

// Define the baud rate
#define BAUD_RATE 115200

// /////////////////





#define NUM_CHANNELS ( (ANALOG_INPUTS_COUNT) + (DIGITAL_INPUTS_COUNT) + (15 + (DIGITAL_BITMAPPED_INPUTS_COUNT))/16 )


IBus ibus(NUM_CHANNELS);

void setup()
{
  analogReference(ANALOG_REFERENCE); // use the defined ADC reference voltage source
  Serial.begin(BAUD_RATE);           // setup serial
}

void loop()
{
  int i, bm_ch = 0;
  unsigned long time = millis();

  ibus.begin();

  // read analog pins - one per channel
  for(i=0; i < ANALOG_INPUTS_COUNT; i++)
    ibus.write(analogRead(analogPins[i]));

  // read digital pins - one per channel
  for(i=0; i < DIGITAL_INPUTS_COUNT; i++)
    ibus.write(digitalRead(digitalPins[i]) == HIGH ? 1023 : 0);

  // read digital bit-mapped pins - 16 pins go in one channel
  for(i=0; i < DIGITAL_BITMAPPED_INPUTS_COUNT; i++) {
  	int bit = i%16;
  	if(digitalRead(digitalBitmappedPins[i]) == HIGH)
  		bm_ch |= 1 << bit;

  	if(bit == 15 || i == DIGITAL_BITMAPPED_INPUTS_COUNT-1) {
  		// data for one channel ready
  		ibus.write(bm_ch);
  		bm_ch = 0;
  	}
  }

  ibus.end();

  time = millis() - time; // time elapsed in reading the inputs
  if(time < UPDATE_INTERVAL)
    // sleep till it is time for the next update
    delay(UPDATE_INTERVAL  - time);
}
