using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class OpenBCIListener : MonoBehaviour {


    public static bool messageReceived = false;
    public static string msg = "";
    private static System.Object msgLock = new System.Object();

    UdpClient receivingUdpClient;
    UdpState s;
    IPEndPoint ipEndPoint;

    static void ReceiveCallback(IAsyncResult ar)
    {
        UdpClient u = (UdpClient)((UdpState)(ar.AsyncState)).udpClient;
        IPEndPoint e = (IPEndPoint)((UdpState)(ar.AsyncState)).ipEndPoint;

        Byte[] receiveBytes = u.EndReceive(ar, ref e);
        string receiveString = Encoding.ASCII.GetString(receiveBytes);

        //Debug.Log("Received: " + receiveString);
        lock (msgLock)
        {
            messageReceived = true;
            msg = receiveString;
        }
    }

    // Use this for initialization
    void Start () {
        //Creates a UdpClient for reading incoming data.
        ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
        receivingUdpClient = new UdpClient(ipEndPoint);

        s = new UdpState();
        s.ipEndPoint = ipEndPoint;
        s.udpClient = receivingUdpClient;

        receivingUdpClient.BeginReceive(new AsyncCallback(ReceiveCallback), s);
    }

    // Update is called once per frame
    void Update ()
    {
        lock (msgLock)
        {
            if(messageReceived)
            {
                Debug.Log("RECIEVED: " + msg);
                messageReceived = false;
                receivingUdpClient.BeginReceive(new AsyncCallback(ReceiveCallback), s);
            }
        }
    }
}
