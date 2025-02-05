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
using NLua;

namespace KazusaGI_cb2.GameServer;

public class MonsterEntity : Entity
{
    public MonsterLua? _monsterInfo;
    public MonsterExcelConfig excelConfig;
    public uint _monsterId;
    public uint level;
    public float Hp;
    public float MaxHp;

    public MonsterEntity(Session session, uint MonsterId, MonsterLua? monsterInfo, Vector3? position)
        : base(session, position)
    {
        this._EntityId = session.GetEntityId(ProtEntityType.ProtEntityMonster);
        _monsterInfo = monsterInfo;
        _monsterId = MonsterId;
        this.level = monsterInfo != null ? monsterInfo.level : 1;
        this.excelConfig = MainApp.resourceManager.MonsterExcel[MonsterId];
        this.Hp = excelConfig.hpBase; // todo: curve
        this.MaxHp = this.Hp;
    }

    public SceneEntityInfo ToSceneEntityInfo(Session session)
    {
        SceneEntityInfo ret = new SceneEntityInfo()
        {
            EntityType = ProtEntityType.ProtEntityMonster,
            EntityId = this._EntityId,
            MotionInfo = new MotionInfo()
            {
                Pos = _monsterInfo != null
                    ? Session.Vector3ToVector(_monsterInfo.pos) : Session.Vector3ToVector(this.Position),
                Rot = _monsterInfo != null
                    ? Session.Vector3ToVector(_monsterInfo.rot) : Session.Vector3ToVector(this.Position),
                Speed = new Protocol.Vector(),
                State = MotionState.MotionFallOnGround
            },
            LifeState = this.Hp > 0 ? (uint)1 : 0,
            AiInfo = new SceneEntityAiInfo()
            {
                IsAiOpen = true,
                BornPos = _monsterInfo != null
                    ? Session.Vector3ToVector(_monsterInfo.pos) : Session.Vector3ToVector(this.Position),
            },
        };
        SceneMonsterInfo sceneMonsterInfo = new SceneMonsterInfo()
        {
            AuthorityPeerId = 1,
            MonsterId = _monsterId,
            IsElite = _monsterInfo != null ? _monsterInfo.isElite : false,
            ConfigId = _monsterInfo != null ? _monsterInfo.config_id : 0,
            BornType = MonsterBornType.MonsterBornDefault,
            PoseId = _monsterInfo != null ? _monsterInfo.pose_id : 0,
        };
        ret.PropMaps.Add((uint)PropType.PROP_LEVEL, new PropValue() { Type = (uint)PropType.PROP_LEVEL, Ival = this.level, Val = this.level });
        ret.PropMaps.Add((uint)PropType.PROP_EXP, new PropValue() { Type = (uint)PropType.PROP_EXP, Ival = 1, Val = this.level });
        foreach (uint affixId in this.excelConfig.affix)
        {
            if (affixId == 0 && (this._monsterInfo != null && !this._monsterInfo.affix.Contains(affixId)))
            {
                continue;
            }
        }
        foreach (var prop in this.GetFightProps())
        {
            ret.FightPropMaps.Add(prop.Key, prop.Value);
        }
        ret.Monster = sceneMonsterInfo;
        return ret;
    }

    public void Damage(float dmg)
    {
        this.Hp -= dmg;
        session.c.LogWarning($"Monster {this._EntityId} damaged by {dmg}, current hp: {this.Hp}");
        bool isDie = false;
        if (this.Hp <= 0)
        {
            isDie = true;
            this.Hp = 0;
        }
        EntityFightPropUpdateNotify entityFightPropUpdateNotify = new EntityFightPropUpdateNotify()
        {
            EntityId = this._EntityId
        };
        foreach(var prop in this.GetFightProps())
        {
            entityFightPropUpdateNotify.FightPropMaps.Add(prop.Key, prop.Value);
        }
        this.session!.SendPacket(entityFightPropUpdateNotify);
        this.session!.SendPacket(new EntityFightPropChangeReasonNotify()
        {
            EntityId = this._EntityId,
            PropType = (uint)FightPropType.FIGHT_PROP_CUR_HP,
            PropDelta = this.Hp,
            Reason = PropChangeReason.PropChangeAbility
        });
        if (isDie)
        {
            this.Die();
        }
    }

    public void Die()
    {
        this.Hp = 0; // for safety
        this.session!.SendPacket(new LifeStateChangeNotify()
        {
            EntityId = this._EntityId,
            LifeState = 2,
            DieType = PlayerDieType.PlayerDieNone,
        });
        this.session!.SendPacket(new SceneEntityDisappearNotify()
        {
            EntityLists = { this._EntityId },
            DisappearType = VisionType.VisionDie
        });
        this.session!.entityMap.Remove(this._EntityId);
    }

    
    public Dictionary<uint, float> GetFightProps()
    {
        Dictionary<uint, float> ret = new Dictionary<uint, float>();
        foreach (FightPropType propType in Enum.GetValues(typeof(FightPropType)))
        {
            switch (propType)
            {
                case FightPropType.FIGHT_PROP_BASE_HP:
                    ret.Add((uint)propType, this.excelConfig.hpBase);
                    break;
                case FightPropType.FIGHT_PROP_CUR_ATTACK: // todo: curve
                case FightPropType.FIGHT_PROP_ATTACK: // todo: curve
                case FightPropType.FIGHT_PROP_BASE_ATTACK:
                    ret.Add((uint)propType, this.excelConfig.attackBase);
                    break;
                case FightPropType.FIGHT_PROP_DEFENSE: // todo: curve
                case FightPropType.FIGHT_PROP_BASE_DEFENSE:
                    ret.Add((uint)propType, this.excelConfig.defenseBase);
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