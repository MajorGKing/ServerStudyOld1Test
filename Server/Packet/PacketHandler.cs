using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServerCore;


class PacketHandler
{
    public static void C_PlayerInfoReqHandler(PacketSession session, IPacket packet)
    {
        C_PlayerInfoReq p = packet as C_PlayerInfoReq;

        System.Console.WriteLine($"PlayerInfoReq: {p.playerId} {p.name}");

        foreach (C_PlayerInfoReq.Skill skill in p.skills)
        {
            System.Console.WriteLine($"Skill ({skill.id}) ({skill.level}) ({skill.duration})");
        }
    }
}