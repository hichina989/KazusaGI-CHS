using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaGI_cb2.Resource.Excel;

public class InvestigationDungeonConfig
{
    public uint entranceId;
    public uint cityId;
    public List<uint> dungeonIdList;
}