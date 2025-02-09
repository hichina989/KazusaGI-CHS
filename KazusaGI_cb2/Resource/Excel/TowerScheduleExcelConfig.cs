using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaGI_cb2.Resource.Excel;

public class TowerScheduleExcelConfig
{
    public uint scheduleId;
    public List<TowerSchedule> schedules;
    public string closeTime;
    public List<TowerStarReward> scheduleRewards;
    public uint commemorativeReward;
    public uint descTextMapHash;
}