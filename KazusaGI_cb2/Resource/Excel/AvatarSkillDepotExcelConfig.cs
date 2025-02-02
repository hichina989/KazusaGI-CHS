using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaGI_cb2.Resource.Excel;

public class AvatarSkillDepotExcelConfig
{
    public uint id;
    public uint energySkill;
    public List<uint> skills;
    public List<uint> subSkills;
    public uint attackModeSkill;
    public uint leaderTalent;
    public List<string> extraAbilities;
    public List<uint> talents;
    public string talentStarName;
    public uint coreProudSkillGroupId;
    public uint coreProudAvatarPromoteLevel;
    public List<ProudSkillOpenConfig> inherentProudSkillOpens;
}