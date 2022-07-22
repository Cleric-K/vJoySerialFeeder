#include "ibus.h"


// //////////////////
// Edit here to customize

// How often to send data?
#define UPDATE_INTERVAL 10 // milliseconds

// 1. Analog channels. Data can be read with the Arduino's 10-bit ADC.
// This gives values from 0 to 1023.
// Specify below the analog pin numbers (as for analogRead()) you would like to use.
// Every analog input is sent as a single channel. The ADC 0-1023 range is mapped to the 1000-2000 range to make it more compatible with RC standards.

// example:
// byte analogPins[] = {A0, A1, A2, A3};
// This will send four analog channels for A0, A1, A2, A3 respectively

byte analogPins[] = {};


// 2. Digital channels. Data can be read from Arduino's digital pins.
// They could be either LOW or HIGH. The pins are set to use the internal pull-ups so your hardware buttons/switches should short them to GND.
// Specify below the digital pin numbers (as for digitalRead()) you would like to use.
// Every pin is sent as a single channel. LOW is the active state (when you short the pin to GND) and is sent as channel value 2000.
// HIGH is the pulled-up state when the button is open. It is sent as channel velue 1000.

// example:
// byte digitalPins[] = {2, 3};
// This will send two channels for pins D2 and D3 respectively

byte digitalPins[] = {};


// 3. Digital bit-mapped channels. Sending a single binary state as a 16-bit
// channel is pretty wasteful. Instead, we can encode one digital input
// in each of the 16 channel bits.
// Make sure you activete "use 16-bit channels" in the Setup dialog of the IBUS protocol in vJoySerialFeeder
// Specify below the digital pins (as for digitalRead()) you would like to send as
// bitmapped channel data. Data will be automatically organized in channels.
// The first 16 pins will go in one channel (the first pin goes into the LSB of the channel).
// The next 16 pins go in another channel and so on
// LOW pins are encoded as 1 bit, HIGH - as 0.

// example:
// byte digitalBitmappedPins[] = {4, 5, 6, 7, 8, 9};
// This will pack D4, D5, D6, D7, D8, D9 into one channel

byte digitalBitmappedPins[] = {};


// Define the appropriate analog reference source. See
// https://www.arduino.cc/reference/en/language/functions/analog-io/analogreference/
// Based on your device voltage, you may need to modify this definition
#define ANALOG_REFERENCE DEFAULT

// Define the baud rate
#define BAUD_RATE 115200

// /////////////////




#define ANALOG_INPUTS_COUNT sizeof(analogPins)
#define DIGITAL_INPUTS_COUNT sizeof(digitalPins)
#define DIGITAL_BITMAPPED_INPUTS_COUNT sizeof(digitalBitmappedPins)
#define NUM_CHANNELS ( (ANALOG_INPUTS_COUNT) + (DIGITAL_INPUTS_COUNT) + (15 + (DIGITAL_BITMAPPED_INPUTS_COUNT))/16 )


IBus ibus(NUM_CHANNELS);

void setup()
{
   for(int i=0; i < DIGITAL_INPUTS_COUNT; i++) {
      pinMode(digitalPins[i], INPUT_PULLUP);
   }

   for(int i=0; i < DIGITAL_BITMAPPED_INPUTS_COUNT; i++) {
      pinMode(digitalBitmappedPins[i], INPUT_PULLUP);
   }

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
      ibus.write(1000 + (uint32_t)analogRead(analogPins[i])*1000/1023); // map 0-1023 to 1000-2000

   // read digital pins - one per channel
   for(i=0; i < DIGITAL_INPUTS_COUNT; i++)
      ibus.write(digitalRead(digitalPins[i]) == LOW ? 2000 : 1000);

   // read digital bit-mapped pins - 16 pins go in one channel
   for(i=0; i < DIGITAL_BITMAPPED_INPUTS_COUNT; i++) {
      int bit = i%16;
      if(digitalRead(digitalBitmappedPins[i]) == LOW)
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
