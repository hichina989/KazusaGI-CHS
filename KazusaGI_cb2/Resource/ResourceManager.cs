using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KazusaGI_cb2.Resource.Excel;
using KazusaGI_cb2.Utils;

namespace KazusaGI_cb2.Resource;

public class ResourceManager
{
    private ResourceLoader loader;
    public Dictionary<uint, AvatarExcelConfig> AvatarExcel { get; set; }
    public Dictionary<uint, AvatarSkillDepotExcelConfig> AvatarSkillDepotExcel { get; set; }
    public Dictionary<uint, AvatarSkillExcelConfig> AvatarSkillExcel { get; set; }
    public Dictionary<uint, Dictionary<uint, ProudSkillExcelConfig>> ProudSkillExcel { get; set; }
    public Dictionary<uint, WeaponExcelConfig> WeaponExcel { get; set; }
    public Dictionary<uint, MonsterExcelConfig> MonsterExcel { get; set; }
    public Dictionary<uint, GadgetExcelConfig> GadgetExcel { get; set; }
    public Dictionary<uint, ScenePoint> ScenePoints { get; set; }
    public Dictionary<uint, SceneLua> SceneLuas { get; set; }

    public ResourceManager(string baseResourcePath = "resources")
    {
        // Init Logger
        Logger c = new("ExcelLoader");

        // :3
        c.LogInfo("Loading Excels...");

        // Load all resources here
        this.loader = new(this, baseResourcePath);

        // Log SUCCESS
        c.LogSuccess("Loaded Excels");
    }
}
