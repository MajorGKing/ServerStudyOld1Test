using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServerCore;

namespace Server
{
    class PacketHandler
    {
        public static void PlayerInfoReqHandler(PacketSession session, IPacket packet)
        {
            PlayerInfoReq p = packet as PlayerInfoReq;

            System.Console.WriteLine($"PlayerInfoReq: {p.playerId} {p.name}");

            foreach (PlayerInfoReq.Skill skill in p.skills)
            {
                System.Console.WriteLine($"Skill ({skill.id}) ({skill.level}) ({skill.duration})");
            }
        }
    }
}