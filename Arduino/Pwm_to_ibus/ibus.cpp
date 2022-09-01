#include "Arduino.h"
#include "ibus.h"

/*
 * See IbusReader.cs for details about the IBUS protocol
 */

#define COMMAND40 0x40

IBus::IBus(int numChannels) {
  len = 4 + numChannels*2;
}

void IBus::begin() {
  checksum = 0xffff - len - COMMAND40;
  Serial.write(len);
  Serial.write(COMMAND40);
}

void IBus::end() {
  // write checksum
  Serial.write(checksum & 0xff);
  Serial.write(checksum >> 8);
}

/*
 * Write 16bit value in little endian format and update checksum
 */
void IBus::write(unsigned short val) {
  byte b = val & 0xff;
  Serial.write(b);
  checksum -= b;
  b = val >> 8;
  Serial.write(b);
  checksum -= b;
}
