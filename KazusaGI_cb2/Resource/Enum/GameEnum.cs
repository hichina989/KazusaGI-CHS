﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaGI_cb2.Resource;

public enum AvatarUseType
{
    AVATAR_TEST = 0,
    AVATAR_SYNC_TEST = 1,
    AVATAR_FORMAL = 2,
    AVATAR_ABANDON = 3
}

public enum BodyType
{
	BODY_NONE = 0,
	BODY_BOY = 1,
	BODY_GIRL = 2,
	BODY_LADY = 3,
	BODY_MALE = 4,
	BODY_LOLI = 5
}

public enum QualityType
{
	QUALITY_NONE = 0,
	QUALITY_WHITE = 1,
	QUALITY_GREEN = 2,
	QUALITY_BLUE = 3,
	QUALITY_PURPLE = 4,
	QUALITY_ORANGE = 5
}

public enum WeaponType
{
	WEAPON_SWORD_ONE_HAND = 1,
	WEAPON_CROSSBOW = 2,
	WEAPON_STAFF = 3,
	WEAPON_DOUBLE_DAGGER = 4,
	WEAPON_KATANA = 5,
	WEAPON_SHURIKEN = 6,
	WEAPON_STICK = 7,
	WEAPON_SPEAR = 8,
	WEAPON_SHIELD_SMALL = 9,
	WEAPON_CATALYST = 10,
	WEAPON_CLAYMORE = 11,
	WEAPON_BOW = 12,
	WEAPON_POLE = 13
}

public enum AvatarIdentityType
{
    AVATAR_IDENTITY_MASTER = 0,
    AVATAR_IDENTITY_NORMAL = 1
}

public enum ScenePointType
{
    NORMAL = 0,
    TOWER = 1,
    PORTAL = 2,
    Other = 3
}

public enum MonsterType
{
    MONSTER_NONE = 0,
    MONSTER_ORDINARY = 1,
    MONSTER_BOSS = 2,
    MONSTER_ENV_ANIMAL = 3,
    MONSTER_LITTLE_MONSTER = 4,
}

public enum FightPropType
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
	FIGHT_PROP_CUR_SPEED = 2003
}

public enum GrowCurveType
{
	GROW_CURVE_NONE = 0,
	GROW_CURVE_HP = 1,
	GROW_CURVE_ATTACK = 2,
	GROW_CURVE_STAMINA = 3,
	GROW_CURVE_STRIKE = 4,
	GROW_CURVE_ANTI_STRIKE = 5,
	GROW_CURVE_ANTI_STRIKE1 = 6,
	GROW_CURVE_ANTI_STRIKE2 = 7,
	GROW_CURVE_ANTI_STRIKE3 = 8,
	GROW_CURVE_STRIKE_HURT = 9,
	GROW_CURVE_ELEMENT = 10,
	GROW_CURVE_KILL_EXP = 11,
	GROW_CURVE_DEFENSE = 12,
	GROW_CURVE_ATTACK_BOMB = 13,
	GROW_CURVE_HP_LITTLEMONSTER = 14,
	GROW_CURVE_ELEMENT_MASTERY = 15,
	GROW_CURVE_PROGRESSION = 16,
	GROW_CURVE_DEFENDING = 17,
	GROW_CURVE_HP_S5 = 21,
	GROW_CURVE_HP_S4 = 22,
	GROW_CURVE_ATTACK_S5 = 31,
	GROW_CURVE_ATTACK_S4 = 32,
	GROW_CURVE_ATTACK_S3 = 33,
	GROW_CURVE_STRIKE_S5 = 34,
	GROW_CURVE_DEFENSE_S5 = 41,
	GROW_CURVE_DEFENSE_S4 = 42,
	GROW_CURVE_ATTACK_101 = 1101,
	GROW_CURVE_ATTACK_102 = 1102,
	GROW_CURVE_ATTACK_103 = 1103,
	GROW_CURVE_ATTACK_104 = 1104,
	GROW_CURVE_ATTACK_105 = 1105,
	GROW_CURVE_ATTACK_201 = 1201,
	GROW_CURVE_ATTACK_202 = 1202,
	GROW_CURVE_ATTACK_203 = 1203,
	GROW_CURVE_ATTACK_204 = 1204,
	GROW_CURVE_ATTACK_205 = 1205,
	GROW_CURVE_ATTACK_301 = 1301,
	GROW_CURVE_ATTACK_302 = 1302,
	GROW_CURVE_ATTACK_303 = 1303,
	GROW_CURVE_ATTACK_304 = 1304,
	GROW_CURVE_ATTACK_305 = 1305,
	GROW_CURVE_CRITICAL_101 = 2101,
	GROW_CURVE_CRITICAL_102 = 2102,
	GROW_CURVE_CRITICAL_103 = 2103,
	GROW_CURVE_CRITICAL_104 = 2104,
	GROW_CURVE_CRITICAL_105 = 2105,
	GROW_CURVE_CRITICAL_201 = 2201,
	GROW_CURVE_CRITICAL_202 = 2202,
	GROW_CURVE_CRITICAL_203 = 2203,
	GROW_CURVE_CRITICAL_204 = 2204,
	GROW_CURVE_CRITICAL_205 = 2205,
	GROW_CURVE_CRITICAL_301 = 2301,
	GROW_CURVE_CRITICAL_302 = 2302,
	GROW_CURVE_CRITICAL_303 = 2303,
	GROW_CURVE_CRITICAL_304 = 2304,
	GROW_CURVE_CRITICAL_305 = 2305
}

public enum GachaItemType
{
    GACHA_ITEM_INVALID = 0,
    GACHA_ITEM_AVATAR_S5 = 11,
    GACHA_ITEM_AVATAR_S4 = 12,
    GACHA_ITEM_AVATAR_S3 = 13,
    GACHA_ITEM_WEAPON_S5 = 21,
    GACHA_ITEM_WEAPON_S4 = 22,
    GACHA_ITEM_WEAPON_S3 = 23,
    GACHA_ITEM_COMMON_MATERIAL = 31
}

public enum MaterialType
{
    MATERIAL_NONE = 0,
    MATERIAL_FOOD = 1,
    MATERIAL_QUEST = 2,
    MATERIAL_EXCHANGE = 4,
    MATERIAL_CONSUME = 5,
    MATERIAL_EXP_FRUIT = 6,
    MATERIAL_AVATAR = 7,
    MATERIAL_ADSORBATE = 8,
    MATERIAL_CRICKET = 9,
    MATERIAL_ELEM_CRYSTAL = 10,
    MATERIAL_WEAPON_EXP_STONE = 11,
    MATERIAL_CHEST = 12,
    MATERIAL_RELIQUARY_MATERIAL = 13,
    MATERIAL_AVATAR_MATERIAL = 14,
    MATERIAL_NOTICE_ADD_HP = 15,
    MATERIAL_SEA_LAMP = 16
}

public enum ItemType
{
    ITEM_NONE = 0,
    ITEM_VIRTUAL = 1,
    ITEM_MATERIAL = 2,
    ITEM_RELIQUARY = 3,
    ITEM_WEAPON = 4,
    ITEM_DISPLAY = 5
}

public enum ArithType
{
    ARITH_NONE = 0,
    ARITH_ADD = 1,
    ARITH_MULTI = 2,
    ARITH_SUB = 3,
    ARITH_DIVIDE = 4
}

public enum ShopType
{
    SHOP_TYPE_NONE = 0,
    SHOP_TYPE_PAIMON = 1001,
    SHOP_TYPE_CITY = 1002,
    SHOP_TYPE_BLACKSMITH = 1003,
    SHOP_TYPE_GROCERY = 1004,
    SHOP_TYPE_FOOD = 1005,
    SHOP_TYPE_SEA_LAMP = 1006,
    SHOP_TYPE_VIRTUAL_SHOP = 1007,
    SHOP_TYPE_LIYUE_GROCERY = 1008,
    SHOP_TYPE_LIYUE_SOUVENIR = 1009,
    SHOP_TYPE_LIYUE_RESTAURANT = 1010,
}

public enum OpenStateType
{
    OPEN_STATE_NONE = 0,
    OPEN_STATE_PAIMON = 1,
    OPEN_STATE_PAIMON_NAVIGATION = 2,
    OPEN_STATE_AVATAR_PROMOTE = 3,
    OPEN_STATE_AVATAR_TALENT = 4,
    OPEN_STATE_WEAPON_PROMOTE = 5,
    OPEN_STATE_WEAPON_AWAKEN = 6,
    OPEN_STATE_QUEST_REMIND = 7,
    OPEN_STATE_GAME_GUIDE = 8,
    OPEN_STATE_COOK = 9,
    OPEN_STATE_WEAPON_UPGRADE = 10,
    OPEN_STATE_RELIQUARY_UPGRADE = 11,
    OPEN_STATE_RELIQUARY_PROMOTE = 12,
    OPEN_STATE_WEAPON_PROMOTE_GUIDE = 13,
    OPEN_STATE_WEAPON_CHANGE_GUIDE = 14,
    OPEN_STATE_PLAYER_LVUP_GUIDE = 15,
    OPEN_STATE_FRESHMAN_GUIDE = 16,
    OPEN_STATE_SKIP_FRESHMAN_GUIDE = 17,
    OPEN_STATE_GUIDE_MOVE_CAMERA = 18,
    OPEN_STATE_GUIDE_SCALE_CAMERA = 19,
    OPEN_STATE_GUIDE_KEYBOARD = 20,
    OPEN_STATE_GUIDE_MOVE = 21,
    OPEN_STATE_GUIDE_JUMP = 22,
    OPEN_STATE_GUIDE_SPRINT = 23,
    OPEN_STATE_GUIDE_MAP = 24,
    OPEN_STATE_GUIDE_ATTACK = 25,
    OPEN_STATE_GUIDE_FLY = 26,
    OPEN_STATE_GUIDE_TALENT = 27,
    OPEN_STATE_GUIDE_RELIC = 28,
    OPEN_STATE_GUIDE_RELIC_PROM = 29,
    OPEN_STATE_COMBINE = 30,
    OPEN_STATE_GACHA = 31,
    OPEN_STATE_GUIDE_GACHA = 32,
    OPEN_STATE_GUIDE_TEAM = 33,
    OPEN_STATE_GUIDE_PROUD = 34,
    OPEN_STATE_GUIDE_AVATAR_PROMOTE = 35,
    OPEN_STATE_GUIDE_ADVENTURE_CARD = 36,
    OPEN_STATE_FORGE = 37,
    OPEN_STATE_GUIDE_BAG = 38,
    OPEN_STATE_EXPEDITION = 39,
    OPEN_STATE_GUIDE_ADVENTURE_DAILYTASK = 40,
    OPEN_STATE_GUIDE_ADVENTURE_DUNGEON = 41,
    OPEN_STATE_TOWER = 42,
    OPEN_STATE_WORLD_STAMINA = 43,
    OPEN_STATE_TOWER_FIRST_ENTER = 44,
    OPEN_STATE_RESIN = 45,
    OPEN_STATE_WORLD_RESIN = 46,
    OPEN_STATE_LIMIT_REGION_FRESHMEAT = 47,
    OPEN_STATE_LIMIT_REGION_GLOBAL = 48,
    OPEN_STATE_MULTIPLAYER = 49,
    OPEN_STATE_GUIDE_MOUSEPC = 50,
    OPEN_STATE_GUIDE_MULTIPLAYER = 51,
    OPEN_STATE_GUIDE_DUNGEONREWARD = 52,
    OPEN_STATE_SHOP_TYPE_PAIMON = 1001,
    OPEN_STATE_SHOP_TYPE_CITY = 1002,
    OPEN_STATE_SHOP_TYPE_BLACKSMITH = 1003,
    OPEN_STATE_SHOP_TYPE_GROCERY = 1004,
    OPEN_STATE_SHOP_TYPE_FOOD = 1005,
    OPEN_STATE_SHOP_TYPE_SEA_LAMP = 1006,
    OPEN_STATE_SHOP_TYPE_VIRTUAL_SHOP = 1007,
    OPEN_STATE_SHOP_TYPE_LIYUE_GROCERY = 1008,
    OPEN_STATE_SHOP_TYPE_LIYUE_SOUVENIR = 1009,
    OPEN_STATE_SHOP_TYPE_LIYUE_RESTAURANT = 1010,
    OPEN_ADVENTURE_MANUAL = 1100,
    OPEN_ADVENTURE_MANUAL_CITY_MENGDE = 1101,
    OPEN_ADVENTURE_MANUAL_CITY_LIYUE = 1102,
    OPEN_ADVENTURE_MANUAL_MONSTER = 1103,
    OPEN_STATE_ACTIVITY_SEALAMP = 1200,
    OPEN_STATE_ACTIVITY_SEALAMP_TAB2 = 1201,
    OPEN_STATE_ACTIVITY_SEALAMP_TAB3 = 1202,
}