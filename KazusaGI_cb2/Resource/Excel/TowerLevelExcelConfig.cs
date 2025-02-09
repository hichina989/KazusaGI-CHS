using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaGI_cb2.Resource.Excel;

public class TowerLevelExcelConfig
{
    public uint levelId;
    public uint levelGroupId;
    public uint levelIndex;
    public uint dungeonId;
    public List<TowerCondition> conds;
    public List<string> towerBuffConfigStrList;
    public uint dailyRewardId;
    public uint firstPassRewardId;
    public uint monsterLevel;
    public List<uint> firstMonsterList;
    public List<uint> secondMonsterList;
}