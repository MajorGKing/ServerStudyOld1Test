using System.Net;
using System.Text;
using ServerCore;

namespace Server
{
    class Packet
    {
        public ushort size;
        public ushort packetId;
    }

    class GameSession : PacketSession
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
            ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
            ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + 2);
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
    class Program
    {
        static Listener _listener = new Listener();

        static void Main(string[] args)
        {
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            _listener.Init(endPoint, () => { return new GameSession(); });
            System.Console.WriteLine("Listening...");

            while (true)
            {
                ;
            }
        }
    }
}