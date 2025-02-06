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

public class GadgetEntity : Entity
{
    public GadgetLua? _gadgetLua;
    public uint _gadgetId;
    public GadgetExcelConfig gadgetExcel;
    public uint level;

    // because some gadgets can be damaged, this is a placeholder for now
    public float Hp = 1;
    public float MaxHp = 1;

    public GadgetEntity(Session session, uint gadgetId, GadgetLua? gadgetInfo, Vector3? position = null)
        : base(session, position)
    {
        this._EntityId = session.GetEntityId(Protocol.ProtEntityType.ProtEntityGadget);
        this._gadgetId = gadgetId;
        this._gadgetLua = gadgetInfo;
        this.level = gadgetInfo != null ? gadgetInfo.level : 1;
        this.gadgetExcel = MainApp.resourceManager.GadgetExcel[gadgetId];
    }

    public SceneEntityInfo ToSceneEntityInfo(Session session)
    {
        SceneEntityInfo ret = new SceneEntityInfo()
        {
            EntityType = ProtEntityType.ProtEntityGadget,
            EntityId = this._EntityId,
            Name = this.gadgetExcel.jsonName,
            MotionInfo = new MotionInfo()
            {
                Pos = _gadgetLua != null
                    ? Session.Vector3ToVector(_gadgetLua.pos) : Session.Vector3ToVector(this.Position),
                Rot = _gadgetLua != null
                    ? Session.Vector3ToVector(_gadgetLua.rot) : Session.Vector3ToVector(this.Position),
                Speed = new Protocol.Vector(),
                State = MotionState.MotionNone
            },
            LifeState = this.Hp > 0 ? (uint)1 : 0,
            AiInfo = new SceneEntityAiInfo()
            {
                IsAiOpen = true,
                BornPos = _gadgetLua != null
                    ? Session.Vector3ToVector(_gadgetLua.pos) : Session.Vector3ToVector(this.Position),
            },
        };
        SceneGadgetInfo sceneGadgetInfo = new SceneGadgetInfo()
        {
            AuthorityPeerId = 1,
            GadgetState = 0, // todo: implement states
            IsEnableInteract = this.gadgetExcel.isInteractive,
            ConfigId = _gadgetLua != null ? _gadgetLua.config_id : 0,
            GadgetId = this._gadgetId,
            BornType = GadgetBornType.GadgetBornGadget,
            // todo: gadget type
        };
        ret.PropMaps.Add((uint)PropType.PROP_LEVEL, new PropValue() { Type = (uint)PropType.PROP_LEVEL, Ival = this.level, Val = this.level });
        // ret.PropMaps.Add((uint)PropType.PROP_EXP, new PropValue() { Type = (uint)PropType.PROP_EXP, Ival = 1, Val = this.level });
        foreach (var prop in this.GetFightProps())
        {
            ret.FightPropMaps.Add(prop.Key, prop.Value);
        }
        ret.Gadget = sceneGadgetInfo;
        return ret;
    }

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
}