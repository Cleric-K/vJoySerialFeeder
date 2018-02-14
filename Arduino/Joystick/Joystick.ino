#include "ibus.h"


// //////////////////
// Edit here to customize

#define UPDATE_INTERVAL 10 // milliseconds

// The number of analog channels you would like to read
#define NUM_ANALOG_CHANNELS 4

// Enter the the analog pins you would like to read. Their count must be NUM_ANALOG_CHANNELS
byte analogPins[] = {0, 1, 2, 3};

// The number of digital channels you would like to red
#define NUM_DIGITAL_CHANNELS 2

// Enter the the digital pins you would like to read. Their count must be NUM_DIGITAL_CHANNELS
byte digitalPins[] = {6, 7};

// Define the appropriate analog reference source. See
// https://www.arduino.cc/reference/en/language/functions/analog-io/analogreference/
#define ANALOG_REFERENCE EXTERNAL

// Define the baud rate
#define BAUD_RATE 115200

// /////////////////






#define NUM_CHANNELS (NUM_ANALOG_CHANNELS + NUM_DIGITAL_CHANNELS)


IBus ibus(NUM_CHANNELS);

void setup()
{
  analogReference(ANALOG_REFERENCE);     // using external voltage reference
  Serial.begin(BAUD_RATE);          // setup serial
}

void loop()
{
  int i;
  unsigned long time = millis();
  
  ibus.begin();
  
  for(i=0; i < NUM_ANALOG_CHANNELS; i++)
    ibus.write(analogRead(analogPins[i]));
  for(i=0; i < NUM_DIGITAL_CHANNELS; i++)
    ibus.write(digitalRead(digitalPins[i]) == HIGH ? 1023 : 0);

  ibus.end();

  time = millis() - time; // time elapsed in reading the inputs
  if(time < UPDATE_INTERVAL)
    // sleep till it is time for the next update
    delay(UPDATE_INTERVAL  - time);
}
