package com.nerfAmbush;
import java.awt.Canvas;
import java.awt.Color;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.PrintWriter;

import gnu.io.CommPortIdentifier;
import gnu.io.SerialPort;
import gnu.io.SerialPortEvent;
import gnu.io.SerialPortEventListener;

import java.util.Enumeration;

import java.net.*;

import javax.swing.JFrame;

public class SerialTest implements SerialPortEventListener {
    SerialPort serialPort;
    public boolean armed = true;

    /** The port we're normally going to use. */
    private static final String PORT_NAMES[] = {
//            "COM6", // Windows
//		"COM4",
		"COM3"
    };
    /**
     * A BufferedReader which will be fed by a InputStreamReader
     * converting the bytes into characters
     * making the displayed results codepage independent
     */
    private BufferedReader input;
    /** The output stream to the port */
    private OutputStream output;
    /** Milliseconds to block while waiting for port open */
    private static final int TIME_OUT = 2000;
    /** Default bits per second for COM port. */
    private static final int DATA_RATE = 9600;

    MP3 triggeredSound= new MP3("C:/Users/IN/Desktop/Stupid/triggered.mp3");
    MP3 armedSound= new MP3("C:/Users/IN/Desktop/Stupid/armed.mp3");



    public void initialize() {

        CommPortIdentifier portId = null;
        Enumeration portEnum = CommPortIdentifier.getPortIdentifiers();

        //First, Find an instance of serial port as set in PORT_NAMES.
        while (portEnum.hasMoreElements()) {
            CommPortIdentifier currPortId = (CommPortIdentifier) portEnum.nextElement();
            for (String portName : PORT_NAMES) {
                if (currPortId.getName().equals(portName)) {
                    portId = currPortId;
                    break;
                }
            }
        }
        if (portId == null) {
            System.out.println("Could not find COM port.");
            return;
        }

        try {
            // open serial port, and use class name for the appName.
            serialPort = (SerialPort) portId.open(this.getClass().getName(),
                    TIME_OUT);

            // set port parameters
            serialPort.setSerialPortParams(DATA_RATE,
                    SerialPort.DATABITS_8,
                    SerialPort.STOPBITS_1,
                    SerialPort.PARITY_NONE);

            // open the streams
            input = new BufferedReader(new InputStreamReader(serialPort.getInputStream()));
            output = serialPort.getOutputStream();

            // add event listeners
            serialPort.addEventListener(this);
            serialPort.notifyOnDataAvailable(true);
        } catch (Exception e) {
            System.err.println(e.toString());
        }
    }

    /**
     * This should be called when you stop using the port.
     * This will prevent port locking on platforms like Linux.
     */
    public synchronized void close() {
        if (serialPort != null) {
            serialPort.removeEventListener();
            serialPort.close();
        }
    }

    /**
     * Handle an event on the serial port. Read the data and print it.
     */
    public synchronized void serialEvent(SerialPortEvent oEvent) {
        if (oEvent.getEventType() == SerialPortEvent.DATA_AVAILABLE) {
            try {
                String inputLine=input.readLine();
                System.out.println(inputLine);

                if (inputLine.equals("armed")) {
                    this.armed = true;
                    //					armedSound.close();
                    //					triggeredSound.close();
                    armedSound.play();
//					System.out.println(inputLine);
                }

                if (inputLine.equals("triggered")) {
                    this.armed = false;
//					System.out.println(inputLine);

                    //					armedSound.close();
                    //					triggeredSound.close();


                    Canvas c = new Canvas();
                    c.setSize(1000,1000);


                    JFrame frame = new JFrame ();
                    frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
                    frame.setLocationRelativeTo(null);
                    frame.add(c);
                    frame.setSize(2000,2000);
                    frame.setLocation(0, 0);
                    frame.setVisible(true);



                    boolean ever = true;

                    triggeredSound.play();
                    int flashTime = 100;

                    int songTime = 180*1000;
                    int usedTime = 0;

                    int h,s,v;



                    for (;ever;) {

                        if (usedTime > songTime) {
                            frame.setVisible(false);
                            frame.dispose();
                            break;
                        }

                        h =(int)Math.floor(Math.random()*255);
                        s =(int)Math.floor(Math.random()*255);
                        v =(int)Math.floor(Math.random()*255);

                        usedTime += flashTime;
                        try {
                            Thread.sleep(flashTime);
                            c.setBackground(Color.getHSBColor(h,s,v));
                        } catch (Exception e) {}
                    }
                    //					System.out.println(inputLine);

                }
            } catch (Exception e) {
                System.err.println(e.toString());
            }
        }
        // Ignore all the other eventTypes, but you should consider the other ones.
    }

    public static void main(String[] args) throws Exception {
        final SerialTest main = new SerialTest();
        main.initialize();
        Thread t=new Thread() {
            public void run() {
                //the following line will keep this app alive for 1000 seconds,
                //waiting for events to occur and responding to them (printing incoming messages to console).
                //				try {Thread.sleep(1000000);} catch (InterruptedException ie) {}


                try {
                    ServerSocket server = new ServerSocket(1337);


                    while (true) {
                        Socket recv = server.accept();
                        PrintWriter out = new PrintWriter(recv.getOutputStream());

                        if (main.armed) {
                            out.println("Recieved Disarm Command");
                            System.out.println("Recieved Disarm Command");
                            try {
                                main.output.write(("disarm").getBytes());
                            } catch (IOException e) {}

                        } else {
                            out.println("Recieved Arm Command");
                            main.triggeredSound.close();
                            main.triggeredSound = new MP3("C:/Users/IN/Desktop/Stupid/triggered.mp3");
                            System.out.println("Recieved Arm Command");
                            try {
                                main.output.write(("arm").getBytes());
                            } catch (IOException e) {}
                        }
                        main.armed = !main.armed;
                        out.close();
//						BufferedReader br = new BufferedReader(new InputStreamReader(recv.getInputStream()));
//						String command = br.readLine().trim();

//						System.out.println(command);
//						
//						if (command.equals("arm")) {
//							System.out.println("Recieved Arm Command");
//							try {
//								main.output.write(("arm").getBytes());
//							} catch (IOException e) {}
//						}
//						
//						if (command.equals("disarm")) {
//							System.out.println("Recieved Disarm Command");
//							try {
//								main.output.write(("disarm").getBytes());
//							} catch (IOException e) {}
//							
//						}
//						br.close();
                        recv.close();
                    }
                } catch (IOException e) {
                    System.out.println("1");
                }

            }
        };
        t.start();
        System.out.println("Started");
    }
}
