using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using ServerCore;

namespace DummyClient
{
    public class SeverSession : Session
    {
        public override void OnConnected(EndPoint endPoint)
        {
            System.Console.WriteLine($"OnConnected : {endPoint}");

            PlayerInfoReq packet = new PlayerInfoReq(){playerId = 1001, name = "ABCD"};
            packet.skills.Add(new PlayerInfoReq.Skill(){id = 101, level = 1, duration = 3.0f});
            packet.skills.Add(new PlayerInfoReq.Skill(){id = 201, level = 2, duration = 4.0f});
            packet.skills.Add(new PlayerInfoReq.Skill(){id = 301, level = 3, duration = 5.0f});
            packet.skills.Add(new PlayerInfoReq.Skill(){id = 401, level = 4, duration = 6.0f});

            

            //for (int i = 0; i < 5; i++)
            {
                ArraySegment<byte> s = packet.Write();

                if(s != null)
                    Send(s);
            }
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            System.Console.WriteLine($"OnDisconnected : {endPoint}");
        }

        public override int OnRecv(ArraySegment<byte> buffer)
        {
            string recvData = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
            System.Console.WriteLine($"[From Server] {recvData}");
            return buffer.Count;
        }

        public override void OnSend(int numOfBytes)
        {
            System.Console.WriteLine($"Transferred bytes: {numOfBytes}");
        }
    }
}