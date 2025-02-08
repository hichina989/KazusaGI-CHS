using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaGI_cb2.Resource.Excel;

public class InvestigationConfig
{
    public uint id;
    public uint cityId;
    public List<uint> nextInvestigationIdList;
    public OpenStateType unlockOpenStateType;
    public uint rewardId;
    public uint titleTextMapHash;
}