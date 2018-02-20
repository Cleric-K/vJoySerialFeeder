#include "ibus.h"


// //////////////////
// Edit here to customize

// How often to send data?
#define UPDATE_INTERVAL 10 // milliseconds


// 1. Analog channels. Data can be read with the Arduino's 10-bit ADC.
// This gives values from 0 to 1023.
// Specify below the analog pin numbers (as for analogRead) you would like to use.
// Every analog input is sent as a single channel.
#define NUM_ANALOG_INPUTS 4
byte analogPins[] = {0, 1, 2, 3}; // element count MUST be == NUM_ANALOG_INPUTS


// 2. Digital channels. Data can be read from Arduino's digital pins.
// They could be either LOW or HIGH.
// Specify below the digital pin numbers (as for digitalRead) you would like to use.
// Every pin is sent as a single channel. LOW is encoded as 0, HIGH - as 1023
#define NUM_DIGITAL_INPUTS 2
byte digitalPins[] = {6, 7}; // element count MUST be == NUM_DIGITAL_INPUTS


// 3. Digital bit-mapped channels. Sending a single binary state as a 16-bit
// channel is pretty wasteful. Instead, we can encode one digital input
// in each of the 16 channel bits.
// Specify below the digital pins (as for digitalRead) you would like to send as
// bitmapped channel data. Data will be automatically organized in channels.
// The first 16 pins will go in one channel (the first pin goes into the LSB of the channel).
// The next 16 pins go in another channel and so on
// LOW pins are encoded as 0 bit, HIGH - as 1.
#define NUM_DIGITAL_BITMAPPED_INPUTS 3
byte digitalBitmappedPins[] = {8, 9, 10}; // element count MUST be == NUM_DIGITAL_BITMAPPED_INPUTS


// Define the appropriate analog reference source. See
// https://www.arduino.cc/reference/en/language/functions/analog-io/analogreference/
#define ANALOG_REFERENCE EXTERNAL

// Define the baud rate
#define BAUD_RATE 115200

// /////////////////





#define NUM_CHANNELS ( (NUM_ANALOG_INPUTS) + (NUM_DIGITAL_INPUTS) + (15 + (NUM_DIGITAL_BITMAPPED_INPUTS))/16 )


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
  for(i=0; i < NUM_ANALOG_INPUTS; i++)
    ibus.write(analogRead(analogPins[i]));

  // read digital pins - one per channel
  for(i=0; i < NUM_DIGITAL_INPUTS; i++)
    ibus.write(digitalRead(digitalPins[i]) == HIGH ? 1023 : 0);

  // read digital bit-mapped pins - 16 pins go in one channel
  for(i=0; i < NUM_DIGITAL_BITMAPPED_INPUTS; i++) {
  	int bit = i%16;
  	if(digitalRead(digitalBitmappedPins[i]) == HIGH)
  		bm_ch |= 1 << bit;

  	if(bit == 15 || i == NUM_DIGITAL_BITMAPPED_INPUTS-1) {
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
