using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ServerCore;

namespace Server
{
    public class ClientSession : PacketSession
    {
        public override void OnConnected(EndPoint endPoint)
        {
            System.Console.WriteLine($"OnConnected : {endPoint}");

            Thread.Sleep(500000);
            Disconnect();
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            PacketManager.Instance.OnRecvPacket(this, buffer);

            
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            System.Console.WriteLine($"OnDisconnected : {endPoint}");
        }



        // public override int OnRecv(ArraySegment<byte> buffer)
        // {
        //     string recvData = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
        //     System.Console.WriteLine($"[From Client] {recvData}");
        //     return buffer.Count;
        // }

        public override void OnSend(int numOfBytes)
        {
            System.Console.WriteLine($"Transferred bytes: {numOfBytes}");
        }
    }
}