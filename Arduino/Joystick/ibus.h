#pragma once

class IBus {
  private:
    int len;
    int checksum;
  public:
    IBus(int numChannels);
  
    void begin();
    void end();
    void write(unsigned short);
    
};

