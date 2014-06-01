#include <iostream>
#include <string>

#include "device.h"

using namespace std;

int main() {
	
	device a = device(device::DEVICE_ID_FEL);

	cout << "Hello World!" << endl;

	cout << "Device mode manual: " << device::MODE_MANUAL << endl;
	cout << "Device mode standbye: " << device::MODE_STANDBY << endl;
	cout << "Device mode autonamous: " << device::MODE_AUTONOMOUS << endl;


	//cout << a->DEVICE_NAMES[0] << endl;
	cout << "DEVICE_NAMES[" << device::DEVICE_ID_ALL << "] = " << device::DEVICE_NAMES[device::DEVICE_ID_ALL] << endl;
	cout << "DEVICE_NAMES[" << device::DEVICE_ID_FEL << "] = " << device::DEVICE_NAMES[device::DEVICE_ID_FEL] << endl;
	cout << "DEVICE_NAMES[" << device::DEVICE_ID_CMD << "] = " << device::DEVICE_NAMES[device::DEVICE_ID_CMD] << endl;

	string line;
	getline(cin, line);

	return 0;
}