#include "device.h"

const std::string device::DEVICE_NAMES[] = { "ALL", "FEL", "CMD" };

device::device(int deviceId)
{
	this->deviceMode = device::MODE_STANDBY;
	this->deviceId = deviceId;
}


device::~device()
{
}

