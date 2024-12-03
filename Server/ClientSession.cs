using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ServerCore;

namespace Server
{
    class Packet
    {
        public ushort size;
        public ushort packetId;
    }

    class PlayerInfoReq : Packet
    {
        public long playerId;
    }

    class PlayerInfoOk : Packet
    {
        public int hp;
        public int attack;
    }

    public enum packetID
    {
        PlayerInfoReq = 1,
        PlayerInfoOk = 2,
    }

    public class ClientSession : PacketSession
    {
        public override void OnConnected(EndPoint endPoint)
        {
            System.Console.WriteLine($"OnConnected : {endPoint}");

            // Packet packet = new Packet() { size = 100, packetId = 10 };

            // ArraySegment<byte> openSegment = SendBufferHelper.Open(4096);
            // byte[] buffer = BitConverter.GetBytes(packet.size);
            // byte[] buffer2 = BitConverter.GetBytes(packet.packetId);
            // Array.Copy(buffer, 0, openSegment.Array, openSegment.Offset, buffer2.Length);
            // Array.Copy(buffer2, 0, openSegment.Array, openSegment.Offset + buffer.Length, buffer2.Length);
            // ArraySegment<byte> sendBuff = SendBufferHelper.Close(buffer.Length + buffer2.Length);

            //Send(sendBuff);
            Thread.Sleep(500000);
            Disconnect();
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            ushort count = 0;
            ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
            count += 2;
            ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
            count += 2;

            switch((packetID)id)
            {
                case packetID.PlayerInfoReq:
                {
                    long playerId = BitConverter.ToInt64(buffer.Array, buffer.Offset + count);
                    count += 8;
                    System.Console.WriteLine($"PlayerInfoReq: {playerId}");
                }
                break;
            }

            System.Console.WriteLine($"RecvPacketId: {id}, Size {size}");
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