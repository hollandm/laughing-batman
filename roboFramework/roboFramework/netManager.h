#include <sys/types.h>
//#include <sys/socket.h>
#include <string>

#pragma once
class netManager
{
public:

	//Multicast broadcast address, to send connection messages to every machine on the network
	static const std::string UDP_IP;
	
	//The port to send udp commands on
	static const int UDP_PORT = 6000;
	
	//Size of udp packets
	static const int UDP_MAX_PACKET_SUZE = 1024;

	//tcp buffer size
	static const int TCP_BUFFER_SZIE = 1024;

	//tcp port
	static const int TCP_PORT = 6001;

	//TODO: socket list

	netManager();
	~netManager();

	/**
	 * openConnection
	 *
	 * Description: 
	 *
	 * Parameters:
	 *		
	 * Returns: 
	 */
	int openConnection();

	/**
	 * recvConnection
	 *
	 * Description: 
	 *
	 * Parameters:
	 *		
	 * Returns: 
	 */
	int recvConnection();


};

