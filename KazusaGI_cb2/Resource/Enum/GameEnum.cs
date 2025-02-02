using System;
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