using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaGI_cb2.Resource.Excel;

public class InvestigationMonsterConfig
{
    public uint id;
    public uint cityId;
    public uint monsterId;
    public List<uint> groupIdList;
    public List<uint> rewardList;
    public uint infoId;
    // maybe will add mapMarkCreateType and mapMarkCreateCondition in future
}