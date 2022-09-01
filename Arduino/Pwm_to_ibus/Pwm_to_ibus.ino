#include "ibus.h"


// //////////////////
// Edit here the pins (as for digitalRead()) that you want to read the PWM signals from.
// IMPORTANT! The pinst MUST belong to the same port. Search 'pinout' in google for
// your Arduino model and choose pins from the same port (for example PD, PB, etc.)
// On Arduino Uno/Nano you can safely use pins 2 through 7, as they all use the same port PD.
byte pins[] = {2, 3, 4, 5};


// //////////////////





#define BAUD_RATE 115200

struct {
  uint8_t mask;
  unsigned long time;
  byte state;
} pinData[sizeof(pins)];

struct {
  unsigned long time;
  uint8_t state;
} state_history[2*sizeof(pins)];

#define FOR_PINS for(int i=0; i < sizeof(pins); i++)
#define PIN pinData[i]

IBus ibus(sizeof(pins));
volatile uint8_t *port; // store the address of the port register
uint8_t mask; // mask for the pins of interest

void setup()
{
  Serial.begin(BAUD_RATE);           // setup serial

  // take the port register from the first pin. The other pins MUST use the same port
  port = portInputRegister(digitalPinToPort(pins[0]));
  mask = 0;

  FOR_PINS {
    PIN.mask = digitalPinToBitMask(pins[i]); // mask for the individual pin
    mask |= PIN.mask; // accumulate all masks
  }
}

enum {
  STATE_INIT, STATE_NORMAL
} program_state = STATE_INIT;



void loop()
{
  if (program_state == STATE_INIT) {
    // wait until all pins are down
    while(*port & mask);

    program_state = STATE_NORMAL;
  }

  uint8_t prev_state = 0, new_state;
  int history_index = 0;


  // Arduinos like Uno/Nano don't have interrupts or timers for all pins so we need to poll for changes and 
  // measure the PWM pulse lengths manually. The problem is that we have to be very quick since we're dealing
  // with microsecond timescales and every instruction counts. The method employed here is to have a tight loop
  // which records all the changes to the port and the time of occurence. After all pins are down
  // we have plenty of time to process the list and send the data over IBUS.
  for(;;) {
    while(prev_state == (new_state = *port & mask)); // wait for change in state

    state_history[history_index++] = { micros(), prev_state = new_state };

    if(!new_state || history_index == sizeof(state_history))
      break; // all pins are down (we have captured all pulses) OR we have gone beyond the size of state_history
  }

  if(new_state) {
    program_state = STATE_INIT; // something's wrong. We didn't reach a state where all pins are down. Start over.
    return;
  }

  // Process the states
  for(int ih = 0; ih < history_index; ih++) {
    uint8_t state = state_history[ih].state;
    unsigned long time = state_history[ih].time;

    FOR_PINS {
      int s = (bool)(state & PIN.mask);

      if (!PIN.state && s) { // low to high
        PIN.time = time; // save pulse starting time;
      }
      else if (PIN.state && !s) { // high to low
        PIN.time = time - PIN.time; // calculate pulse width
      }
      PIN.state = s;
    }
  }

  // send the data
  ibus.begin();
  FOR_PINS {
    ibus.write(PIN.time);
  }
  ibus.end();
}
