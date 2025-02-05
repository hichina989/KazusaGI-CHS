using KazusaGI_cb2.Protocol;
using KazusaGI_cb2.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using static KazusaGI_cb2.Utils.Crypto;

namespace KazusaGI_cb2.GameServer.Handlers;

public class Scene
{
    [Packet.PacketCmdId(PacketId.SceneEntitiesMovesReq)]
    public static void HandleSceneEntitiesMovesReq(Session session, Packet packet)
    {
        SceneEntitiesMovesReq req = packet.GetDecodedBody<SceneEntitiesMovesReq>();
        SceneEntitiesMovesRsp rsp = new SceneEntitiesMovesRsp();
        foreach (EntityMoveInfo move in req.EntityMoveInfoLists)
        {
            if (session.entityMap.ContainsKey(move.EntityId))
            {
                session.entityMap[move.EntityId].Position = Session.VectorProto2Vector3(move.MotionInfo.Pos);
                if (session.entityMap[move.EntityId] is AvatarEntity)
                {
                    session.player!.TeleportToPos(session, Session.VectorProto2Vector3(move.MotionInfo.Pos), true);
                    session.player!.SetRot(Session.VectorProto2Vector3(move.MotionInfo.Rot));
                    // session.c.LogWarning($"Player {session.player.Uid} moved to {move.MotionInfo.Pos.X}, {move.MotionInfo.Pos.Y}, {move.MotionInfo.Pos.Z}");
                }
            }
        }
        session.SendPacket(rsp);
    }

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

    [Packet.PacketCmdId(PacketId.SceneTransToPointReq)]
    public static void HandleSceneTransToPointReq(Session session, Packet packet)
    {
        SceneTransToPointReq req = packet.GetDecodedBody<SceneTransToPointReq>();

        ConfigScenePoint scenePoint = MainApp.resourceManager.ScenePoints[req.SceneId].points[req.PointId];

        session.player!.SetRot(scenePoint.tranRot);
        session.player.TeleportToPos(session, scenePoint.tranPos, true);
        session.player.EnterScene(session, req.SceneId, EnterType.EnterGoto);

        SceneTransToPointRsp rsp = new SceneTransToPointRsp()
        {
            PointId = req.PointId,
            SceneId = req.SceneId
        };
        session.SendPacket(rsp);
    }

    [Packet.PacketCmdId(PacketId.GetScenePointReq)]
    public static void HandleGetScenePointReq(Session session, Packet packet)
    {
        GetScenePointReq req = packet.GetDecodedBody<GetScenePointReq>();
        GetScenePointRsp rsp = new GetScenePointRsp();
        foreach (KeyValuePair<uint, ConfigScenePoint> kvp in MainApp.resourceManager.ScenePoints[req.SceneId].points)
        {
            rsp.UnlockedPointLists.Add(kvp.Key);
            rsp.BelongUid = req.BelongUid;

            if (!rsp.UnlockAreaLists.Contains(kvp.Value.areaId) && kvp.Value.areaId != 0)
                rsp.UnlockAreaLists.Add(kvp.Value.areaId);
        }
        session.SendPacket(rsp);
        ScenePointUnlockNotify scenePointUnlockNotify = new ScenePointUnlockNotify() { SceneId = session.player!.SceneId };

        for (uint i = 1; i < 200; i++)
        {
            scenePointUnlockNotify.PointLists.Add(i);
        }
        session.SendPacket(scenePointUnlockNotify);
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
            SceneId = req.SceneId,
        };
        foreach (ConfigScenePoint scenePoint in MainApp.resourceManager.ScenePoints[session.player!.SceneId].points.Values)
        {
            if (!rsp.AreaIdLists.Contains(scenePoint.areaId) && scenePoint.areaId != 0)
                rsp.AreaIdLists.Add(scenePoint.areaId);
        }
        session.SendPacket(rsp);
    }
}
