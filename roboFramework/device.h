#include <string>

#pragma once
class device
{
public:
//Constants
	//In the standby mode the device will ignore all manual or autonomous movement commands.
	static const int MODE_STANDBY = 0;

	//While under manual control, the device receives movement commands via a network connection
	static const int MODE_MANUAL = 1;

	//While in autonomous mode the device will make decisions about what movements it should make
	static const int MODE_AUTONOMOUS = 2;

	//Device ID Constants
	static const int DEVICE_ID_ALL = 0;		//Sending data using this device id will send it to all devices
	static const int DEVICE_ID_FEL = 2;		//Sending data using this device id will send it to the front end loader
	static const int DEVICE_ID_CMD = 2;		//Sending data using this device id will send it to the command console

	//Human Readable Names for devices
	static const std::string DEVICE_NAMES[];

//Constructors & Destructors
	device();
	~device();
};

