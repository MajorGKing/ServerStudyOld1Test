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
        public string name;
        
        public PlayerInfoReq()
        {
            this.packetId = (ushort)PacketID.PlayerInfoReq;
        }

        public override void Read(ArraySegment<byte> segment)
        {
            ushort count = 0;

            ReadOnlySpan<byte> s = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);

            //ushort size = BitConverter.ToUInt16(s.Array, s.Offset);
            count += sizeof(ushort);
            //ushort id = BitConverter.ToUInt16(s.Array, s.Offset + count);
            count += sizeof(ushort);
            this.playerId = BitConverter.ToInt64(s.Slice(count, s.Length - count));
            count += sizeof(long);

            // string
            ushort nameLen = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
            count += sizeof(ushort);
            this.name = Encoding.Unicode.GetString(s.Slice(count, nameLen));
        }

        public override ArraySegment<byte> Write()
        {
            // byte[] sendBuff = Encoding.UTF8.GetBytes($"Hello World! {i}");

            ArraySegment<byte> segment = SendBufferHelper.Open(4096);

            ushort count = 0;
            bool success = true;

            Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count);

            //success &= BitConverter.TryWriteBytes(new Span<byte>(s.Array, s.Offset + count, s.Count - count), packet.size);
            count += sizeof(ushort);
            //success &= BitConverter.TryWriteBytes(new Span<byte>(s.Array, s.Offset + count, s.Count - count), (ushort)PacketID.PlayerInfoReq);
            success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), this.packetId);
            count += sizeof(ushort);
            success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), this.playerId);
            count += sizeof(long);
            

            // string
            // string len [2]
            // byte []
            // ushort nameLen = (ushort)Encoding.Unicode.GetByteCount(this.name);
            // success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), nameLen);
            // count += sizeof(ushort);
            // Array.Copy(Encoding.Unicode.GetBytes(this.name), 0, segment.Array, count, nameLen);
            // count += nameLen;

            ushort nameLen = (ushort) Encoding.Unicode.GetBytes(this.name, 0, name.Length, segment.Array, segment.Offset + count + sizeof(ushort));
            success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), nameLen);
            count += sizeof(ushort);
            count += nameLen;
            
            success &= BitConverter.TryWriteBytes(s, count);

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

            if(success == false)
                return null;

            return SendBufferHelper.Close(count);
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

            PlayerInfoReq packet = new PlayerInfoReq(){packetId = (ushort)PacketID.PlayerInfoReq, playerId = 1001, name = "ABCD"};

            

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