using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;


public class UdpState
{
    public UdpClient udpClient;
    public IPEndPoint ipEndPoint;
    public const int BufferSize = 1024;
    public byte[] buffer = new byte[BufferSize];
    public int counter = 0;
}
