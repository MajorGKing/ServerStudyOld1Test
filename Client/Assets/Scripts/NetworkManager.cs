using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using DummyClient;
using ServerCore;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    ServerSession _session = new ServerSession();

    // Start is called before the first frame update
    void Start()
    {
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            Connector connector = new Connector();

            connector.Connect(endPoint, 
            () => { return _session;},
            1);

            //PacketManager.Instance.Register();
    }

    public void Send(ArraySegment<byte> sendBuff)
    {
        _session.Send(sendBuff);
    }

    // Update is called once per frame
    void Update()
    {
        List<IPacket> list = PacketQueue.Instane.PopAll();
        //Debug.Log(list.Count);
        foreach(IPacket packet in list)
            PacketManager.Instance.HandlePacket(_session, packet);
        
    }
}
