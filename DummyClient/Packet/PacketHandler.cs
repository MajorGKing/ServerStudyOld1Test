using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DummyClient;
using ServerCore;


class PacketHandler
{
    public static void S_ChatHandler(PacketSession session, IPacket packet)
    {
        S_Chat chatPacket = packet as S_Chat;
        ServerSession serverSession = session as ServerSession;

        //if(chatPacket.playerId == 1)
            //System.Console.WriteLine(chatPacket.chat);
    }
}