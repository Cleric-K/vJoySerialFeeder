# Running on Linux

## Get it
To run vJoySerialFeeder on Linux you can build it yourself with Mono-Develop or
download it from the [releases](../../../releases). You have two options:
* If you already have `mono` installed you can download the binaries ending with `..._requires_mono".
* Otherwise download one of the bundles (32bit or 64bit). They are larger because they include all the
mono libraries packed in them.

## Installing Dependencies

### uinput
uinput is used for simulating a virtual joystick. You probably have this running already. Check out
if `/dev/uinput` device exists. If it does not try `sudo modprobe uinput`.

To use that device you need to have read/write permissions.
* If you are eager to test things out you
can simply `sudo chmod a+rw /dev/uinput`, but permissions will be reset after restart.

* If you want to do the things in the proper way do this:
  1. Create a `uinput` group:\
     `sudo groupdadd -f uinput`

  2. Add yourself to the group\
     `sudo gpasswd -a USER uinput`\
     where you replace `USER` with your username

  3. Create file `/etc/udev/rules.d/99-input.rules` (as root) with the following contents:\
    `SUBSYSTEM=="misc", KERNEL=="uinput", MODE="0660", GROUP="uinput"`

  4. Reboot

  Optional:

  5. Check with `ls -l /dev/uinput` you should see something like\
     `crw-rw---- 1 root uinput 10, 223 Nov 11 15:35 /dev/uinput`

  6. Check if you are added to the `uinput` group with the `groups` command in the console

### libevdev2
Install with `sudo apt-get install libevdev2` or whatever your package manager is.

## Running
* If you are using the version that needs mono, go in a console to wherever you unpacked vJoySerialFeeder
and execute `mono vJoySerialFeeder.exe`
* If you are using the bundles you can execute them directly.

## Limitations

* The nice syntax highlighting Lua editor is not available on Linux. The editor is a simple TextBox