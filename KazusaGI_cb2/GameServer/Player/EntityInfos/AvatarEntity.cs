using KazusaGI_cb2.GameServer.PlayerInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using KazusaGI_cb2.Protocol;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace KazusaGI_cb2.GameServer;

public class AvatarEntity : Entity
{
    public PlayerAvatar DbInfo { get; set; }

    public AvatarEntity(Session session, PlayerAvatar playerAvatar, Vector3? position = null)
        : base(session, position)
    {
        this.DbInfo = playerAvatar;
    }

    public SceneEntityInfo ToSceneEntityInfo(Session session)
    {
        AvatarInfo asAvatarInfo = this.DbInfo.ToAvatarInfo(session);
        SceneEntityInfo ret = new SceneEntityInfo()
        {
            EntityType = ProtEntityType.ProtEntityAvatar,
            EntityId = this.EntityId,
            Avatar = DbInfo.ToSceneAvatarInfo(session),
            MotionInfo = new MotionInfo()
            {
                Pos = session.player!.Vector3ToVector(session.player.Pos),
                Rot = new Protocol.Vector(),
                Speed = new Protocol.Vector()
            },
            LifeState = DbInfo.Hp > 0 ? (uint)1 : 0,
            AiInfo = new SceneEntityAiInfo()
            {
                IsAiOpen = true
            },
        };
        foreach (KeyValuePair<uint, PropValue> kvp in asAvatarInfo.PropMaps)
        {
            ret.PropMaps.Add(kvp.Key, kvp.Value);
        };
        foreach (KeyValuePair<uint, float> kvp in asAvatarInfo.FightPropMaps)
        {
            ret.FightPropMaps.Add(kvp.Key, kvp.Value);
        };
        return ret;
    }
}