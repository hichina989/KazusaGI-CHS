using KazusaGI_cb2.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static KazusaGI_cb2.Utils.Crypto;

namespace KazusaGI_cb2.GameServer.Handlers;

public class Scene
{
    [Packet.PacketCmdId(PacketId.SceneGetAreaExplorePercentReq)]
    public static void HandleSceneGetAreaExplorePercentReq(Session session, Packet packet)
    {
        SceneGetAreaExplorePercentReq req = packet.GetDecodedBody<SceneGetAreaExplorePercentReq>();
        SceneGetAreaExplorePercentRsp rsp = new SceneGetAreaExplorePercentRsp()
        {
            AreaId = req.AreaId,
            ExplorePercent = 100
        };
        session.SendPacket(rsp);
    }

    [Packet.PacketCmdId(PacketId.GetScenePointReq)] // TODO: get from res
    public static void HandleGetScenePointReq(Session session, Packet packet)
    {
        GetScenePointReq req = packet.GetDecodedBody<GetScenePointReq>();
        GetScenePointRsp rsp = new GetScenePointRsp();
        for (uint i = 0; i < 200; i++)
        {
            rsp.UnlockedPointLists.Add(i);
            rsp.UnlockAreaLists.Add(i);
        }
        session.SendPacket(rsp);
    }

    [Packet.PacketCmdId(PacketId.EnterWorldAreaReq)]
    public static void HandleEnterWorldAreaReq(Session session, Packet packet)
    {
        EnterWorldAreaReq req = packet.GetDecodedBody<EnterWorldAreaReq>();
        EnterWorldAreaRsp rsp = new EnterWorldAreaRsp()
        {
            AreaId = req.AreaId,
            AreaType = req.AreaType
        };
        session.SendPacket(rsp);
    }

    [Packet.PacketCmdId(PacketId.GetSceneAreaReq)]
    public static void HandleGetSceneAreaReq(Session session, Packet packet)
    {
        GetSceneAreaReq req = packet.GetDecodedBody<GetSceneAreaReq>();
        GetSceneAreaRsp rsp = new GetSceneAreaRsp()
        {
            SceneId = session.player!.SceneId,
        };
        for (uint i = 0; i < 200; i++)
        {
            rsp.AreaIdLists.Add(i);
        }
        session.SendPacket(rsp);
    }
}
