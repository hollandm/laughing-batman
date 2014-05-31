#include <iostream>
#include <string>

#include "device.h"

using namespace std;

int main() {
	
	device a = device();

	cout << "Hello World!" << endl;

	cout << "Device mode manual: " << a.MODE_MANUAL << endl;
	cout << "Device mode standbye: " << a.MODE_STANDBY << endl;
	cout << "Device mode autonamous: " << a.MODE_AUTONOMOUS << endl;



	string line;
	getline(cin, line);

	return 0;
}