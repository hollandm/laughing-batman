#include "netManager.h"

const std::string netManager::UDP_IP = "244.0.0.1";

netManager::netManager()
{
}


netManager::~netManager()
{
}

int netManager::openConnection() {

	//TODO: Open udp socket

	//TODO: While we are expecting more connections
	//TODO: Broadcast udp "handshake"
	//TODO: Listen for inbound tcp connections

	//TODO: Close udp socket

	return 0;
}

int netManager::recvConnection() {
	//TODO: Open udp socket

	//TODO: listen for udp handshake
	//TODO: open tcp connection to computer that sent handshake

	//TODO: Close udp socket
	return 0;
}