#include <string>
#include <vector>

#include "sensor.h"
#include "netManager.h"

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
	static const int DEVICE_ID_FEL = 1;		//Sending data using this device id will send it to the front end loader
	static const int DEVICE_ID_CMD = 2;		//Sending data using this device id will send it to the command console

	/**
	 * Human Readable Names for devices
	 * 0 - ALL
	 * 1 - FEL
	 * 2 - CMD
	 */ 
	static const std::string DEVICE_NAMES[];


//Member Variables

	//What the robot should be doing (Standbye, Manual, Autonamous)
	int deviceMode;

	//The id of the device
	int deviceId;

	//List of sensor components opened by this program
	std::vector<sensor> sensors;

	//TODO: Add actuators vector

	netManager netManager;

//Constructors & Destructors
	device(int deviceId);
	~device();

};

