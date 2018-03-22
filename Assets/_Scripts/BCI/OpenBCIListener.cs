using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class OpenBCIListener : MonoBehaviour {
    public float curBetaValue = 5;


    public class EEGData
    {
        public string type;
        public List<List<double>> data;


        public float getBeta()
        {
            if (data.Count == 0 || data[0].Count < 5)
                return -1;
            // gets the average of all the beta waves 
            var avg = 0.0f;
            for (int i = 0; i < data.Count; i++)
            {
                avg += (float)data[i][3];
            }
            avg /= data.Count;
            return avg;
        }
    }
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
                //Debug.Log("RECIEVED msg: " + msg);
                // 0 to 30
                var data = JsonConvert.DeserializeObject<EEGData>(msg);
                curBetaValue = data.getBeta();
                //var t = GetComponent<Text>();
                //t.text = b.ToString();
                //t.color = new Color(b / 5.0f, 0, 0);
                //Debug.Log("RECIEVED Beta wave: " + data.getBeta());
                messageReceived = false;
                receivingUdpClient.BeginReceive(new AsyncCallback(ReceiveCallback), s);
            }
        }
    }
}
