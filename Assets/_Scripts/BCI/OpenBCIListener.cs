using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class OpenBCIListener : MonoBehaviour {
    public float curBetaValue = 5;
    public float curBetaAverage = 5;
    public float secondsToAverage = 5;
    public List<EEGData> cachedEEGData;

    private float lastTime = 0;


    public class EEGData
    {
        public string type;
        public List<List<double>> data;
        public float timestamp;
        // {"type":"bandPower",        //  "data":[[0.67796624,1.9630938,2.5061078,14.015687,25.454866],[0.8223976,3.18445,3.152183,17.37749,26.149632],        //          [6.943687,34.68595,16.464489,35.98638,34.788563],[6.409976,32.323486,16.830719,36.632137,34.20752]]}

        public float GetBeta()
        {
            //if (data.Count == 0 || data[0].Count < 5)
            //    return -1;
            //// gets the average of all the beta waves 
            //var avg = 0.0f;
            //for (int i = 0; i < data.Count; i++)
            //{
            //    avg += (float)data[i][3];
            //}
            //avg /= data.Count;
            //return avg;
            // gives relative beta
            // alpha / (beta + theta) for relaxation
            return (float)(data[0][3] / (data[0][1] + data[0][2]));
            // beta / (alpha + theta) for attentiveness
            //return (float)(data[0][2] / (data[0][3] + data[0][1]));
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
    private void Awake()
    {
        cachedEEGData = new List<OpenBCIListener.EEGData>();
    }

    // Use this for initialization
    void Start () {
        cachedEEGData = new List<EEGData>();
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
        RemoveOldCachedData();
        lock (msgLock)
        {
            if(messageReceived)
            {
                Debug.Log("RECIEVED msg: " + msg);
                // 0 to 30
                var data = JsonConvert.DeserializeObject<EEGData>(msg);
                curBetaValue = data.GetBeta();
                data.timestamp = Time.time;
                lastTime = data.timestamp;

                // add the data to the cached list
                cachedEEGData.Add(data);

                // get the avg of the most recent beta values
                curBetaAverage = cachedEEGData.Average(d => d.GetBeta());

                //var t = GetComponent<Text>();
                //t.text = b.ToString();
                //t.color = new Color(b / 5.0f, 0, 0);
                //Debug.Log("RECIEVED Beta wave: " + data.getBeta());
                messageReceived = false;
                receivingUdpClient.BeginReceive(new AsyncCallback(ReceiveCallback), s);
            }
        }
    }

    private void RemoveOldCachedData()
    {
        var dataBuffer = new List<EEGData>();
        for(var i=0; i<cachedEEGData.Count; i++)
        {
            var time = cachedEEGData[i].timestamp;
            if (time > lastTime - secondsToAverage)
            {
                dataBuffer.Add(cachedEEGData[i]);
            }
        }
        // update the data
        cachedEEGData = dataBuffer;
    }
}
