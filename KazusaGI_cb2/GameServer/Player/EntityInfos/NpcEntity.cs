using KazusaGI_cb2.GameServer.PlayerInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using KazusaGI_cb2.Protocol;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using KazusaGI_cb2.Resource.Excel;
using KazusaGI_cb2.Resource;

namespace KazusaGI_cb2.GameServer;

public class NpcEntity : Entity
{
    public NpcLua? _npcInfo;
    public uint _npcId;

    public NpcEntity(Session session, uint npcId, NpcLua? npcInfo, Vector3? position = null)
        : base(session, position)
    {
        this._EntityId = session.GetEntityId(Protocol.ProtEntityType.ProtEntityNpc);
        this._npcId = npcId;
        this._npcInfo = npcInfo;
        this.Position = _npcInfo != null ? _npcInfo.pos : this.Position;
    }

    public SceneEntityInfo ToSceneEntityInfo()
    {
        SceneEntityInfo ret = new SceneEntityInfo()
        {
            EntityType = ProtEntityType.ProtEntityNpc,
            EntityId = this._EntityId,
            // maybe more excel info in future
            MotionInfo = new MotionInfo()
            {
                Pos = Session.Vector3ToVector(this.Position),
                Rot = _npcInfo != null
                    ? Session.Vector3ToVector(_npcInfo.rot) : Session.Vector3ToVector(this.Position),
                Speed = new Protocol.Vector(),
                State = MotionState.MotionNone
            },
            LifeState = 1,
            AiInfo = new SceneEntityAiInfo()
            {
                IsAiOpen = true,
                BornPos = Session.Vector3ToVector(this.Position),
            },
        };
        SceneNpcInfo sceneNpcInfo = new SceneNpcInfo()
        {
            NpcId = this._npcId,
            RoomId = 0, // for quests later
            ParentQuestId = 0, // again, for quests
            BlockId = _npcInfo != null ? this._npcInfo.block_id : 0
        };
        ret.Npc = sceneNpcInfo;
        return ret;
    }

    /*
    public Dictionary<uint, float> GetFightProps()
    {
        Dictionary<uint, float> ret = new Dictionary<uint, float>();
        foreach (FightPropType propType in Enum.GetValues(typeof(FightPropType)))
        {
            switch (propType)
            {
                // todo: implement from binoutput
                case FightPropType.FIGHT_PROP_BASE_HP:
                case FightPropType.FIGHT_PROP_CUR_ATTACK:
                case FightPropType.FIGHT_PROP_ATTACK:
                case FightPropType.FIGHT_PROP_BASE_ATTACK:
                case FightPropType.FIGHT_PROP_BASE_DEFENSE:
                case FightPropType.FIGHT_PROP_DEFENSE:
                    ret.Add((uint)propType, 1f);
                    break;
                case FightPropType.FIGHT_PROP_CUR_HP:
                case FightPropType.FIGHT_PROP_HP:
                    ret.Add((uint)propType, this.Hp);
                    break;
                case FightPropType.FIGHT_PROP_MAX_HP:
                    ret.Add((uint)propType, this.MaxHp);
                    break;
                case FightPropType.FIGHT_PROP_HP_PERCENT:
                case FightPropType.FIGHT_PROP_CUR_SPEED:
                    ret.Add((uint)propType, 0.0f);
                    break;
                case FightPropType.FIGHT_PROP_CUR_FIRE_ENERGY:
                case FightPropType.FIGHT_PROP_CUR_ELEC_ENERGY:
                case FightPropType.FIGHT_PROP_CUR_WATER_ENERGY:
                case FightPropType.FIGHT_PROP_CUR_GRASS_ENERGY:
                case FightPropType.FIGHT_PROP_CUR_WIND_ENERGY:
                case FightPropType.FIGHT_PROP_CUR_ICE_ENERGY:
                case FightPropType.FIGHT_PROP_CUR_ROCK_ENERGY:
                case FightPropType.FIGHT_PROP_MAX_FIRE_ENERGY:
                case FightPropType.FIGHT_PROP_MAX_ELEC_ENERGY:
                case FightPropType.FIGHT_PROP_MAX_WATER_ENERGY:
                case FightPropType.FIGHT_PROP_MAX_GRASS_ENERGY:
                case FightPropType.FIGHT_PROP_MAX_WIND_ENERGY:
                case FightPropType.FIGHT_PROP_MAX_ICE_ENERGY:
                case FightPropType.FIGHT_PROP_MAX_ROCK_ENERGY:
                    ret.Add((uint)propType, 100.0f);
                    break;
                default:
                    break;
            }
        }
        return ret;
    }
    */
}