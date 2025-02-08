using KazusaGI_cb2.Protocol;
using KazusaGI_cb2.Resource;
using KazusaGI_cb2.Resource.Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaGI_cb2.GameServer.Handlers;

public class Dungeon
{
    [Packet.PacketCmdId(PacketId.DungeonEntryInfoReq)]
    public static void HandleDungeonEntryInfoReq(Session session, Packet packet)
    {
        DungeonEntryInfoReq req = packet.GetDecodedBody<DungeonEntryInfoReq>();
        ConfigScenePoint scenePoint = MainApp.resourceManager.ScenePoints[session.player!.SceneId].points[req.PointId];
        List<uint> ids = scenePoint.dungeonIds.Count > 0 ? scenePoint.dungeonIds : scenePoint.dungeonRandomList;
        DungeonEntryInfoRsp rsp = new DungeonEntryInfoRsp()
        {
            PointId = req.PointId,
        };
        DungeonExcelConfig recommmendDungeon = MainApp.resourceManager.DungeonExcel[ids.First()];
        foreach (uint dungeonId in ids)
        {
            DungeonExcelConfig dungeonExcel = MainApp.resourceManager.DungeonExcel[dungeonId];
            DungeonEntryInfo entryInfo = new DungeonEntryInfo()
            {
                DungeonId = dungeonId,
                BossChestNum = 0,
                EndTime = 1999999999,
                IsPassed = false,
                LeftTimes = 3,
                MaxBossChestNum = 3, // idk where they are in game data
                StartTime = 0,
            };
            if (dungeonExcel.levelRevise > recommmendDungeon.levelRevise && dungeonExcel.levelRevise <= session.player.Level)
            {
                recommmendDungeon = dungeonExcel;
            }
            rsp.DungeonEntryLists.Add(entryInfo);
        }
        rsp.RecommendDungeonId = recommmendDungeon.id;
        session.SendPacket(rsp);
    }

    // todo: implement properly
    [Packet.PacketCmdId(PacketId.GetDailyDungeonEntryInfoReq)]
    public static void HandleGetDailyDungeonEntryInfoReq(Session session, Packet packet)
    {
        GetDailyDungeonEntryInfoReq req = packet.GetDecodedBody<GetDailyDungeonEntryInfoReq>();
        GetDailyDungeonEntryInfoRsp rsp = new GetDailyDungeonEntryInfoRsp();
        foreach (InvestigationDungeonConfig investigationDungeonExcel in MainApp.resourceManager.InvestigationDungeonExcel.Values)
        {
            foreach (uint dungeonid in investigationDungeonExcel.dungeonIdList)
            {
                DailyDungeonEntryInfo entryInfo = new DailyDungeonEntryInfo()
                {
                    DungeonEntryConfigId = dungeonid,
                    DungeonEntryId = investigationDungeonExcel.entranceId,
                    RecommendDungeonId = dungeonid,
                };
                rsp.DailyDungeonInfoLists.Add(entryInfo);
            }
        }
        session.SendPacket(rsp);
    }

}