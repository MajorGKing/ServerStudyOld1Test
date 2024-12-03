using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ServerCore;

namespace DummyClient
{
    public abstract class Packet
    {
        public ushort size;
        public ushort packetId;
        public abstract ArraySegment<byte> Write();
        public abstract void Read(ArraySegment<byte> s);
    }

    class PlayerInfoReq : Packet
    {
        public long playerId;

        public override void Read(ArraySegment<byte> s)
        {
            throw new NotImplementedException();
        }

        public override ArraySegment<byte> Write()
        {
            throw new NotImplementedException();
        }
    }

    // class PlayerInfoOk : Packet
    // {
    //     public int hp;
    //     public int attack;
    // }

    public enum PacketID
    {
        PlayerInfoReq = 1,
        PlayerInfoOk = 2,
    }

    public class SeverSession : Session
    {
        public override void OnConnected(EndPoint endPoint)
        {
            System.Console.WriteLine($"OnConnected : {endPoint}");

            PlayerInfoReq packet = new PlayerInfoReq(){packetId = (ushort)PacketID.PlayerInfoReq, playerId = 1001};

            

            //for (int i = 0; i < 5; i++)
            {
                // byte[] sendBuff = Encoding.UTF8.GetBytes($"Hello World! {i}");

                ArraySegment<byte> s = SendBufferHelper.Open(4096);

                short count = 0;
                bool success = true;
                //success &= BitConverter.TryWriteBytes(new Span<byte>(s.Array, s.Offset + count, s.Count - count), packet.size);
                count += 2;
                success &= BitConverter.TryWriteBytes(new Span<byte>(s.Array, s.Offset + count, s.Count - count), packet.packetId);
                count += 2;
                success &= BitConverter.TryWriteBytes(new Span<byte>(s.Array, s.Offset + count, s.Count - count), packet.playerId);
                count += 8;
                success &= BitConverter.TryWriteBytes(new Span<byte>(s.Array, s.Offset, s.Count), count);

                // byte[] size = BitConverter.GetBytes(packet.size);
                // byte[] packetId = BitConverter.GetBytes(packet.packetId);
                // byte[] playerId = BitConverter.GetBytes(packet.playerId);
                
                // ushort count = 0;

                // Array.Copy(size, 0, s.Array, s.Offset + count, 2);
                // count += 2;
                // Array.Copy(packetId, 0, s.Array, s.Offset + count, 2);
                // count += 2;
                // Array.Copy(playerId, 0, s.Array, s.Offset + count, 8);
                // count += 8;

                ArraySegment<byte> sendBuff = SendBufferHelper.Close(count);

                if(success)
                    Send(sendBuff);
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