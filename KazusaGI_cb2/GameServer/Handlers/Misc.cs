using KazusaGI_cb2.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaGI_cb2.GameServer.Handlers;

public class Misc
{
    [Packet.PacketCmdId(PacketId.PingReq)]
    public static void HandlePingReq(Session session, Packet packet)
    {

        PingReq req = packet.GetDecodedBody<PingReq>();
        PingRsp rsp = new PingRsp()
        {
            ClientTime = req.ClientTime,
            Seq = req.Seq,
        };
        session.SendPacket(rsp);
    }

    [Packet.PacketCmdId(PacketId.MarkMapReq)]
    public static void HandleMarkMapReq(Session session, Packet packet)
    {
        MarkMapReq req = packet.GetDecodedBody<MarkMapReq>();
        MarkMapRsp rsp = new MarkMapRsp();
        if (req.Mark != null)
        {
            rsp.MarkLists.Add(req.Mark);
        }
        session.SendPacket(rsp);
    }
    
    [Packet.PacketCmdId(PacketId.PlayerSetPauseReq)]
    public static void HandlePlayerSetPauseReq(Session session, Packet packet)
    {
        PlayerSetPauseReq req = packet.GetDecodedBody<PlayerSetPauseReq>();
        PlayerSetPauseRsp rsp = new PlayerSetPauseRsp();
        session.SendPacket(rsp);
    }

    [Packet.PacketCmdId(PacketId.LogTalkNotify)]
    public static void HandleLogTalkNotify(Session session, Packet packet)
    {
        // meant for official server logs, useless for us
    }

    [Packet.PacketCmdId(PacketId.NpcTalkReq)]
    public static void HandleNpcTalkReq(Session session, Packet packet)
    {
        NpcTalkReq req = packet.GetDecodedBody<NpcTalkReq>();
        NpcTalkRsp rsp = new NpcTalkRsp()
        {
            NpcEntityId = req.NpcEntityId,
            TalkType = req.TalkType,
            CurTalkId = req.TalkId
        };
        session.SendPacket(rsp);
    }
}