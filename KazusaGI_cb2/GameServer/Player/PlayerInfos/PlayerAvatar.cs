using KazusaGI_cb2.Resource.Excel;
using KazusaGI_cb2.Resource;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KazusaGI_cb2.Protocol;
using System.Xml.Serialization;

namespace KazusaGI_cb2.GameServer.PlayerInfos;

public class PlayerAvatar
{
    private static ResourceManager resourceManager = MainApp.resourceManager;
    private AvatarExcelConfig avatarExcel { get; set; }
    private AvatarSkillDepotExcelConfig avatarSkillDepotExcel { get; set; }
    public ulong Guid { get; set; } // critical
    public uint AvatarId { get; set; } // critical
    public uint Level { get; set; }
    public uint Exp { get; set; }
    public float Hp { get; set; }
    public float MaxHp { get; set; }
    public float Def { get; set; }
    public float Atk { get; set; }
    public uint PromoteLevel { get; set; }
    public uint BreakLevel { get; set; }
    public float CurElemEnergy { get; set; }
    public uint SkillDepotId { get; set; }
    public uint UltSkillId { get; set; }
    public ulong EquipGuid { get; set; }
    public Dictionary<uint, uint> SkillLevels { get; set; }
    public List<uint> UnlockedTalents { get; set; }
    public List<uint> ProudSkills { get; set; }

    public PlayerAvatar(Session session, uint AvatarId)
    {
        this.avatarExcel = resourceManager.AvatarExcel[AvatarId];
        this.SkillDepotId = this.isTraveler() ? avatarExcel.candSkillDepotIds[3] : avatarExcel.skillDepotId;
        this.avatarSkillDepotExcel = resourceManager.AvatarSkillDepotExcel[this.SkillDepotId];
        this.UltSkillId = avatarSkillDepotExcel.energySkill;
        this.SkillLevels = new Dictionary<uint, uint>();
        this.UnlockedTalents = new List<uint>();
        this.ProudSkills = new List<uint>();
        this.Guid = session.GetGuid();
        this.AvatarId = AvatarId;
        this.Level = 90;
        this.Exp = 0;
        this.BreakLevel = 4;
        this.Hp = avatarExcel.hpBase;
        this.MaxHp = avatarExcel.hpBase;
        this.Def = avatarExcel.defenseBase;
        this.Atk = avatarExcel.attackBase;
        this.PromoteLevel = 6;
        this.CurElemEnergy = 0;
        uint initialWeapon = avatarExcel.initialWeapon;
        if (initialWeapon != 0)
        {
            PlayerWeapon weapon = new(session, initialWeapon);
            weapon.EquipOnAvatar(this);
        }
        this.SkillLevels.Add(this.UltSkillId, 1); // todo: get from resources
        foreach (uint skillId in avatarSkillDepotExcel.skills)
        {
            if (skillId == 0) continue;
            this.SkillLevels.Add(skillId, 1); // todo: get from resources
        }
        foreach (uint talentId in avatarSkillDepotExcel.talents)
        {
            if (talentId == 0) continue;
            this.UnlockedTalents.Add(talentId);
        }
        foreach(ProudSkillOpenConfig proudSkillOpenConfig in this.avatarSkillDepotExcel.inherentProudSkillOpens)
        {
            if (this.PromoteLevel < proudSkillOpenConfig.needAvatarPromoteLevel)
                continue;
            uint proudSkillGroupId = proudSkillOpenConfig.proudSkillGroupId;
            ProudSkillExcelConfig? proudSkillExcel = resourceManager.ProudSkillExcel
                .SelectMany(kv => kv.Value.Values) // Flatten the nested dictionaries
                .FirstOrDefault(config => config.proudSkillGroupId == proudSkillGroupId);

            if (proudSkillExcel != null)
            {
                this.ProudSkills.Add(proudSkillExcel.proudSkillId);
            }
        }
    }

    public SceneAvatarInfo ToSceneAvatarInfo(Session session)
    {
        SceneAvatarInfo sceneAvatarInfo = new SceneAvatarInfo()
        {
            PeerId = 1,
            Guid = this.Guid,
            AvatarId = this.AvatarId,
            Uid = session.player!.Uid,
            SkillDepotId = this.SkillDepotId,
            CoreProudSkillLevel = 1,
            Weapon = session.player!.weaponDict[this.EquipGuid].ToSceneWeaponInfo(session),
        };
        foreach (uint unlockedTalentId in this.UnlockedTalents)
        {
            sceneAvatarInfo.TalentIdLists.Add(unlockedTalentId);
        }
        foreach (uint proudSkillId in this.ProudSkills)
        {
            sceneAvatarInfo.InherentProudSkillLists.Add(proudSkillId);
        }
        foreach (KeyValuePair<uint, uint> skillLevelMapKvp in this.SkillLevels)
        {
            uint skillId = skillLevelMapKvp.Key;
            uint level = skillLevelMapKvp.Value;

            sceneAvatarInfo.SkillLevelMaps.Add(skillId, level);
        }
        sceneAvatarInfo.EquipIdLists.Add(session.player!.weaponDict[this.EquipGuid].WeaponId);
        return sceneAvatarInfo;
    }

    public bool isTraveler()
    {
        return this.avatarExcel.candSkillDepotIds.Count > 0;
    }

    public AvatarInfo ToAvatarInfo(Session session)
    {
        AvatarInfo avatarInfo = new AvatarInfo()
        {
            Guid = this.Guid,
            AvatarId = this.AvatarId,
            LifeState = this.Hp > 0 ? (uint)1 : 0,
            SkillDepotId = this.SkillDepotId,
            AvatarType = (uint)AvatarType.AvatarTypeFormal,
            CoreProudSkillLevel = 1,
            FetterInfo = new AvatarFetterInfo()
            {
                ExpLevel = 10,
                ExpNumber = 10
            }
        };
        foreach (uint unlockedTalentId in this.UnlockedTalents)
        {
            avatarInfo.TalentIdLists.Add(unlockedTalentId);
        }
        foreach (uint proudSkillId in this.ProudSkills)
        {
            avatarInfo.InherentProudSkillLists.Add(proudSkillId);
        }
        foreach (KeyValuePair<uint, uint> skillLevelMapKvp in this.SkillLevels)
        {
            uint skillId = skillLevelMapKvp.Key;
            uint level = skillLevelMapKvp.Value;

            avatarInfo.SkillLevelMaps.Add(skillId, level);
        }
        AddPropMap(PropType.PROP_LEVEL, this.Level, avatarInfo.PropMaps);
        AddPropMap(PropType.PROP_EXP, this.Exp, avatarInfo.PropMaps);
        AddPropMap(PropType.PROP_BREAK_LEVEL, this.BreakLevel, avatarInfo.PropMaps);

        AddAllFightProps(avatarInfo.FightPropMaps);

        avatarInfo.EquipGuidLists.Add(this.EquipGuid);
        return avatarInfo;
    }

    public void AddAllFightProps(Dictionary<uint, float> keyValuePairs)
    {
        AvatarExcelConfig avatarExcel = MainApp.resourceManager.AvatarExcel[this.AvatarId];
        AddFightPropMap(FightProp.FIGHT_PROP_MAX_HP, this.MaxHp, keyValuePairs);
        AddFightPropMap(FightProp.FIGHT_PROP_HP_PERCENT, 0.0f, keyValuePairs);
        AddFightPropMap(FightProp.FIGHT_PROP_CUR_SPEED, 0.0f, keyValuePairs);
        AddFightPropMap(FightProp.FIGHT_PROP_BASE_DEFENSE, avatarExcel.defenseBase, keyValuePairs);
        AddFightPropMap(FightProp.FIGHT_PROP_BASE_ATTACK, avatarExcel.attackBase, keyValuePairs);
        AddFightPropMap(FightProp.FIGHT_PROP_CRITICAL, avatarExcel.critical, keyValuePairs);
        AddFightPropMap(FightProp.FIGHT_PROP_CRITICAL_HURT, avatarExcel.criticalHurt, keyValuePairs);
        AddFightPropMap(FightProp.FIGHT_PROP_CHARGE_EFFICIENCY, 2.0f, keyValuePairs); // its a ps, let people have fun
        AddFightPropMap(FightProp.FIGHT_PROP_MAX_ROCK_ENERGY, 100.0f, keyValuePairs);
        AddFightPropMap(FightProp.FIGHT_PROP_MAX_ICE_ENERGY, 100.0f, keyValuePairs);
        AddFightPropMap(FightProp.FIGHT_PROP_MAX_WATER_ENERGY, 100.0f, keyValuePairs);
        AddFightPropMap(FightProp.FIGHT_PROP_MAX_FIRE_ENERGY, 100.0f, keyValuePairs);
        AddFightPropMap(FightProp.FIGHT_PROP_MAX_ELEC_ENERGY, 100.0f, keyValuePairs);
        AddFightPropMap(FightProp.FIGHT_PROP_MAX_GRASS_ENERGY, 100.0f, keyValuePairs);
        AddFightPropMap(FightProp.FIGHT_PROP_MAX_WIND_ENERGY, 100.0f, keyValuePairs);
        AddFightPropMap(FightProp.FIGHT_PROP_CUR_ROCK_ENERGY, 100.0f, keyValuePairs);
        AddFightPropMap(FightProp.FIGHT_PROP_CUR_ICE_ENERGY, 100.0f, keyValuePairs);
        AddFightPropMap(FightProp.FIGHT_PROP_CUR_WATER_ENERGY, 100.0f, keyValuePairs);
        AddFightPropMap(FightProp.FIGHT_PROP_CUR_ELEC_ENERGY, 100.0f, keyValuePairs);
        AddFightPropMap(FightProp.FIGHT_PROP_CUR_FIRE_ENERGY, 100.0f, keyValuePairs);
        AddFightPropMap(FightProp.FIGHT_PROP_CUR_WIND_ENERGY, 100.0f, keyValuePairs);
        AddFightPropMap(FightProp.FIGHT_PROP_CUR_GRASS_ENERGY, 100.0f, keyValuePairs);
        AddFightPropMap(FightProp.FIGHT_PROP_CUR_HP, this.Hp, keyValuePairs);
        AddFightPropMap(FightProp.FIGHT_PROP_BASE_HP, avatarExcel.hpBase, keyValuePairs);
        AddFightPropMap(FightProp.FIGHT_PROP_CUR_DEFENSE, this.Def, keyValuePairs);
        AddFightPropMap(FightProp.FIGHT_PROP_CUR_ATTACK, this.Atk, keyValuePairs);
        AddFightPropMap(FightProp.FIGHT_PROP_SKILL_CD_MINUS_RATIO, 2.0f, keyValuePairs); // its a ps, let people have fun
    }

    private void AddPropMap(PropType propType, uint ival, Dictionary<uint,PropValue> keyValuePairs)
    {
        keyValuePairs.Add((uint)propType, new PropValue()
        {
            Type = (uint)propType,
            Ival = ival,
            Val = ival
        });
    }

    private void AddFightPropMap(FightProp propType, float val, Dictionary<uint, float> keyValuePairs)
    {
        keyValuePairs.Add((uint)propType, val);
    }
}


public enum PropType
{
    PROP_NONE = 0,
    PROP_EXP = 1001,
    PROP_BREAK_LEVEL = 1002,
    PROP_SMALL_TALENT_POINT = 1004,
    PROP_BIG_TALENT_POINT = 1005,
    PROP_GEAR_START_VAL = 2001,
    PROP_GEAR_STOP_VAL = 2002,
    PROP_LEVEL = 4001,
    PROP_LAST_CHANGE_AVATAR_TIME = 10001,
    PROP_MAX_SPRING_VOLUME = 10002,
    PROP_CUR_SPRING_VOLUME = 10003,
    PROP_IS_SPRING_AUTO_USE = 10004,
    PROP_SPRING_AUTO_USE_PERCENT = 10005,
    PROP_IS_FLYABLE = 10006,
    PROP_IS_WEATHER_LOCKED = 10007,
    PROP_IS_GAME_TIME_LOCKED = 10008,
    PROP_IS_TRANSFERABLE = 10009,
    PROP_MAX_STAMINA = 10010,
    PROP_CUR_PERSIST_STAMINA = 10011,
    PROP_CUR_TEMPORARY_STAMINA = 10012,
    PROP_PLAYER_LEVEL = 10013,
    PROP_PLAYER_EXP = 10014,
    PROP_PLAYER_HCOIN = 10015,
    PROP_PLAYER_SCOIN = 10016,
    PROP_PLAYER_MP_SETTING_TYPE = 10017,
    PROP_IS_MP_MODE_AVAILABLE = 10018,
    PROP_PLAYER_LEVEL_LOCK_ID = 10019,
    PROP_PLAYER_RESIN = 10020,
    PROP_PLAYER_WORLD_RESIN = 10021,
    PROP_PLAYER_WAIT_SUB_HCOIN = 10022,
    PROP_PLAYER_WAIT_SUB_SCOIN = 10023,
}

public enum FightProp
{
    FIGHT_PROP_NONE = 0,
    FIGHT_PROP_BASE_HP = 1,
    FIGHT_PROP_HP = 2,
    FIGHT_PROP_HP_PERCENT = 3,
    FIGHT_PROP_BASE_ATTACK = 4,
    FIGHT_PROP_ATTACK = 5,
    FIGHT_PROP_ATTACK_PERCENT = 6,
    FIGHT_PROP_BASE_DEFENSE = 7,
    FIGHT_PROP_DEFENSE = 8,
    FIGHT_PROP_DEFENSE_PERCENT = 9,
    FIGHT_PROP_BASE_SPEED = 10,
    FIGHT_PROP_SPEED_PERCENT = 11,
    FIGHT_PROP_HP_MP_PERCENT = 12,
    FIGHT_PROP_ATTACK_MP_PERCENT = 13,
    FIGHT_PROP_CRITICAL = 20,
    FIGHT_PROP_ANTI_CRITICAL = 21,
    FIGHT_PROP_CRITICAL_HURT = 22,
    FIGHT_PROP_CHARGE_EFFICIENCY = 23,
    FIGHT_PROP_ADD_HURT = 24,
    FIGHT_PROP_SUB_HURT = 25,
    FIGHT_PROP_HEAL_ADD = 26,
    FIGHT_PROP_HEALED_ADD = 27,
    FIGHT_PROP_ELEMENT_MASTERY = 28,
    FIGHT_PROP_PHYSICAL_SUB_HURT = 29,
    FIGHT_PROP_PHYSICAL_ADD_HURT = 30,
    FIGHT_PROP_DEFENCE_IGNORE_RATIO = 31,
    FIGHT_PROP_DEFENCE_IGNORE_DELTA = 32,
    FIGHT_PROP_FIRE_ADD_HURT = 40,
    FIGHT_PROP_ELEC_ADD_HURT = 41,
    FIGHT_PROP_WATER_ADD_HURT = 42,
    FIGHT_PROP_GRASS_ADD_HURT = 43,
    FIGHT_PROP_WIND_ADD_HURT = 44,
    FIGHT_PROP_ROCK_ADD_HURT = 45,
    FIGHT_PROP_ICE_ADD_HURT = 46,
    FIGHT_PROP_HIT_HEAD_ADD_HURT = 47,
    FIGHT_PROP_FIRE_SUB_HURT = 50,
    FIGHT_PROP_ELEC_SUB_HURT = 51,
    FIGHT_PROP_WATER_SUB_HURT = 52,
    FIGHT_PROP_GRASS_SUB_HURT = 53,
    FIGHT_PROP_WIND_SUB_HURT = 54,
    FIGHT_PROP_ROCK_SUB_HURT = 55,
    FIGHT_PROP_ICE_SUB_HURT = 56,
    FIGHT_PROP_EFFECT_HIT = 60,
    FIGHT_PROP_EFFECT_RESIST = 61,
    FIGHT_PROP_FREEZE_RESIST = 62,
    FIGHT_PROP_TORPOR_RESIST = 63,
    FIGHT_PROP_DIZZY_RESIST = 64,
    FIGHT_PROP_FREEZE_SHORTEN = 65,
    FIGHT_PROP_TORPOR_SHORTEN = 66,
    FIGHT_PROP_DIZZY_SHORTEN = 67,
    FIGHT_PROP_MAX_FIRE_ENERGY = 70,
    FIGHT_PROP_MAX_ELEC_ENERGY = 71,
    FIGHT_PROP_MAX_WATER_ENERGY = 72,
    FIGHT_PROP_MAX_GRASS_ENERGY = 73,
    FIGHT_PROP_MAX_WIND_ENERGY = 74,
    FIGHT_PROP_MAX_ICE_ENERGY = 75,
    FIGHT_PROP_MAX_ROCK_ENERGY = 76,
    FIGHT_PROP_SKILL_CD_MINUS_RATIO = 80,
    FIGHT_PROP_SHIELD_COST_MINUS_RATIO = 81,
    FIGHT_PROP_CUR_FIRE_ENERGY = 1000,
    FIGHT_PROP_CUR_ELEC_ENERGY = 1001,
    FIGHT_PROP_CUR_WATER_ENERGY = 1002,
    FIGHT_PROP_CUR_GRASS_ENERGY = 1003,
    FIGHT_PROP_CUR_WIND_ENERGY = 1004,
    FIGHT_PROP_CUR_ICE_ENERGY = 1005,
    FIGHT_PROP_CUR_ROCK_ENERGY = 1006,
    FIGHT_PROP_CUR_HP = 1010,
    FIGHT_PROP_MAX_HP = 2000,
    FIGHT_PROP_CUR_ATTACK = 2001,
    FIGHT_PROP_CUR_DEFENSE = 2002,
    FIGHT_PROP_CUR_SPEED = 2003,
}