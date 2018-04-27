# Running on Linux

## Get it
To run vJoySerialFeeder on Linux you can build it yourself with Mono-Develop or
download it from the [releases](../../../releases).

## Requirements

### Mono
vJoySerialFeeder requires Mono version 5 or higher. Follow the installation
instructions [here](https://www.mono-project.com/download/stable/).
You need the `mono-complete` package.

### Serial port permissions

To be able to use the serial ports without `root` privileges you must be member
of the `dialout` group (if you are not already).
1. `sudo gpasswd -a USER dialout`\
   where you replace `USER` with your username
2. You might need to restart your session (log out and in again)

### uinput
uinput is used for simulating a virtual joystick. You probably have this running already. Check out
if `/dev/uinput` device exists. If it does not try `sudo modprobe uinput`.

To use that device you need to have read/write permissions.
* If you are eager to test things out you
can simply `sudo chmod a+rw /dev/uinput`, but permissions will be reset after reboot.

* If you want to do the things in the proper way do this:
   1. Create a `uinput` group:\
      `sudo groupdadd -f uinput`

   2. Add yourself to the group\
      `sudo gpasswd -a USER uinput`\
      where you replace `USER` with your username

   4. Create file `/etc/udev/rules.d/99-input.rules` (as root) with the following contents:\
     `SUBSYSTEM=="misc", KERNEL=="uinput", MODE="0660", GROUP="uinput"`

   5. Reboot

   Optional:

   6. Check with `ls -l /dev/uinput` you should see something like\
      `crw-rw---- 1 root uinput 10, 223 Nov 11 15:35 /dev/uinput`

   7. Check if you are added to the `uinput` group with the `groups` command in the console

### libevdev2
Install with `sudo apt-get install libevdev2` or whatever your package manager is.

## Running

Open a console, go to wherever you unpacked vJoySerialFeeder
and execute `mono vJoySerialFeeder.exe`.

## Limitations

* The nice syntax highlighting Lua editor is not available on Linux. The editor is a simple TextBox
* Microsoft COM Interaction is not available on Linux