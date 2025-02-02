﻿using KazusaGI_cb2.GameServer.PlayerInfos;
using KazusaGI_cb2.Protocol;
using KazusaGI_cb2.Resource.Excel;
using System;
using System.Numerics;

namespace KazusaGI_cb2.GameServer;

public class Player
{
    public string Name { get; set; }
    public int Level { get; set; }
    public uint Uid { get; set; }
    public Dictionary<ulong, PlayerAvatar> avatarDict { get; set; }
    public Dictionary<ulong, PlayerWeapon> weaponDict { get; set; }
    public List<PlayerTeam> teamList { get; set; }
    public uint TeamIndex { get; set; } = 0;
    public uint SceneId { get; set; } = 3;
    public uint WorldLevel { get; set; } = 1;
    public Scene Scene { get; set; }
    public Vector3 Pos { get; private set; }
    public Gender PlayerGender { get; private set; } = Gender.Female;

    public Player(uint uid)
    {
        Name = "KazusaPS";
        Level = 1;
        Uid = uid;

        // Initialize the dictionaries, todo: automatically add everyhing
        this.avatarDict = new();
        this.weaponDict = new();
        this.teamList = new();
        this.Pos = new()
        {
            X = 2747.6F,
            Y = 194.6F,
            Z = -1719.4F
        };
    }

    public void AddAllAvatars(Session session)
    {
        foreach (KeyValuePair<uint, AvatarExcelConfig> avatarExcelRow in MainApp.resourceManager.AvatarExcel)
        {
            if (avatarExcelRow.Key >= 11000000) continue;
            PlayerAvatar playerAvatar = new(session, avatarExcelRow.Key);
            if (avatarExcelRow.Key == 10000007)
            {
                this.teamList.Add(new PlayerTeam(session, playerAvatar));
            }
            AvatarEntity avatarEntity = new AvatarEntity(session, playerAvatar);
            session.entityMap.Add(avatarEntity.EntityId, avatarEntity);
            session.player!.avatarDict.Add(playerAvatar.Guid, playerAvatar);
        }
    }
    
    public AvatarEntity? FindEntityByPlayerAvatar(Session session, PlayerAvatar playerAvatar)
    {
        List<AvatarEntity> avatarEntities = session.entityMap.Values
            .OfType<AvatarEntity>()
            .ToList();
        return avatarEntities.FirstOrDefault(c => c.DbInfo == playerAvatar);
    }

    public void SendPlayerEnterSceneInfoNotify(Session session)
    {
        
        PlayerEnterSceneInfoNotify notify = new PlayerEnterSceneInfoNotify()
        {
            CurAvatarEntityId = FindEntityByPlayerAvatar(session, GetCurrentLineup().Leader)!.EntityId,
            TeamEnterInfo = new TeamEnterSceneInfo()
            {
                TeamAbilityInfo = new(),
                TeamEntityId = session.GetEntityId(ProtEntityType.ProtEntityTeam)
            },
            MpLevelEntityInfo = new()
            {
                EntityId = session.GetEntityId(ProtEntityType.ProtEntityMpLevel),
                AuthorityPeerId = 1,
                AbilityInfo = new()
            }
        };
        foreach (PlayerAvatar playerAvatar in GetCurrentLineup().Avatars)
        {
            notify.AvatarEnterInfoes.Add(new AvatarEnterSceneInfo()
            {
                AvatarGuid = playerAvatar.Guid,
                AvatarEntityId = FindEntityByPlayerAvatar(session, playerAvatar)!.EntityId,
                WeaponGuid = playerAvatar.EquipGuid,
                WeaponEntityId = weaponDict[playerAvatar.EquipGuid].WeaponEntityId
            });
        }
        session.SendPacket(notify);
    }

    public void SendSceneTeamUpdateNotify(Session session)
    {
        SceneTeamUpdateNotify notify = new SceneTeamUpdateNotify();
        foreach(PlayerAvatar playerAvatar in GetCurrentLineup().Avatars)
        {
            List<AvatarEntity> avatarEntities = session.entityMap.Values
                .OfType<AvatarEntity>()
                .ToList();
            notify.SceneTeamAvatarLists.Add(new SceneTeamAvatar()
            {
                AvatarGuid = playerAvatar.Guid,
                EntityId = avatarEntities.First(c => c.DbInfo == playerAvatar).EntityId,
                AvatarInfo = playerAvatar.ToAvatarInfo(session),
                PlayerUid = this.Uid,
                SceneId = session.player!.SceneId,
                // SceneAvatarInfo = playerAvatar.ToSceneAvatarInfo(session),
            });
        }
        session.SendPacket(notify);
    }

    public Protocol.Vector Vector3ToVector(Vector3 pos)
    {
        return new Protocol.Vector()
        {
            X = pos.X,
            Y = pos.Y,
            Z = pos.Z
        };
    }

    public void EnterScene(Session session, uint sceneId)
    {
        this.SceneId = sceneId;
        this.Scene = new(sceneId); // will write later
        PlayerEnterSceneNotify enterSceneNotify = new()
        {
            SceneId = sceneId,
            Pos = Vector3ToVector(this.Pos),
            SceneBeginTime = (ulong)DateTimeOffset.Now.ToUnixTimeMilliseconds(),
            Type = EnterType.EnterSelf,
            EnterSceneToken = 69,
            WorldLevel = 1,
            TargetUid = this.Uid
        };
        session.SendPacket(enterSceneNotify);
    }

    public void SendAvatarDataNotify(Session session)
    {
        AvatarDataNotify dataNotify = new AvatarDataNotify()
        {
            CurAvatarTeamId = this.TeamIndex,
            ChooseAvatarGuid = GetCurrentLineup().Leader.Guid
        };
        for (uint i = 0; i < this.teamList.Count; i++)
        {
            PlayerTeam playerTeam = this.teamList[(int)i];
            AvatarTeam avatarTeam = new AvatarTeam()
            {
                TeamName = $"KazusaGI team {i + 1}"
            };
            foreach (PlayerAvatar playerAvatar in playerTeam.Avatars)
            {
                avatarTeam.AvatarGuidLists.Add(playerAvatar.Guid);
            }

            dataNotify.AvatarTeamMaps.Add(i, avatarTeam);
        }
        foreach (KeyValuePair<ulong, PlayerAvatar> pair in this.avatarDict)
        {
            PlayerAvatar avatar = pair.Value;
            dataNotify.AvatarLists.Add(avatar.ToAvatarInfo(session));
        }
        session.SendPacket(dataNotify);
    }

    public PlayerTeam GetCurrentLineup()
    {
        return this.teamList[(int)this.TeamIndex];
    }

    public enum Gender
    {
        All = 0,
        Female = 1,
        Male = 2,
        Others = 3
    }
}
