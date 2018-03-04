#define SBUS_PIN 3
#define TX_PIN 1

/*
	Simple sketch to read the digital level at pin SBUS_PIN and
	write the inverse to TX_PIN. Since SBUS works at 100 000 bps
	we have to be quick.  digitalRead/Write are not fast enough.
	We pre-cache the port registers' addresses and bit masks
	and run a simple `read -> write inverted` loop.
*/

uint8_t sbus_bit, tx_bit, tx_bit_inv;
volatile uint8_t *sbus_reg, *tx_reg;
  
void setup() {
  sbus_bit = digitalPinToBitMask(SBUS_PIN);
  sbus_reg = portInputRegister(digitalPinToPort(SBUS_PIN));
  
  tx_bit = digitalPinToBitMask(TX_PIN);
  tx_bit_inv = ~tx_bit;
  tx_reg = portOutputRegister(digitalPinToPort(TX_PIN));

  pinMode(TX_PIN, OUTPUT);
}

void loop() {
  cli(); // disable interrupts
  while(1) {
    // use our own loop

    if(*sbus_reg & sbus_bit)
      // sbus is high, so write low to tx
      *tx_reg &= tx_bit_inv;
    else
     // sbus low -> tx high
      *tx_reg |= tx_bit;
  }
}