using KazusaGI_cb2.Resource.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace KazusaGI_cb2.Resource;

public class ResourceLoader
{
    private static readonly string ExcelSubPath = "ExcelBinOutput";
    private string _baseResourcePath;

    private Dictionary<uint, AvatarExcelConfig> LoadAvatarExcel() =>
        JsonConvert.DeserializeObject<List<AvatarExcelConfig>>(
            File.ReadAllText(Path.Combine(_baseResourcePath, ExcelSubPath, "AvatarExcelConfigData.json"))
        )!.ToDictionary(data => data.id);
    private Dictionary<uint, AvatarSkillDepotExcelConfig> LoadAvatarSkillDepotExcel() =>
        JsonConvert.DeserializeObject<List<AvatarSkillDepotExcelConfig>>(
            File.ReadAllText(Path.Combine(_baseResourcePath, ExcelSubPath, "AvatarSkillDepotExcelConfigData.json"))
        )!.ToDictionary(data => data.id);
    private Dictionary<uint, AvatarSkillExcelConfig> LoadAvatarSkillExcel() =>
        JsonConvert.DeserializeObject<List<AvatarSkillExcelConfig>>(
            File.ReadAllText(Path.Combine(_baseResourcePath, ExcelSubPath, "AvatarSkillExcelConfigData.json"))
        )!.ToDictionary(data => data.id);
    private Dictionary<uint, Dictionary<uint, ProudSkillExcelConfig>> LoadProudSkillExcel() =>
        JsonConvert.DeserializeObject<List<ProudSkillExcelConfig>>(
            File.ReadAllText(Path.Combine(_baseResourcePath, ExcelSubPath, "ProudSkillExcelConfigData.json"))
        )!.GroupBy(data => data.proudSkillId)
        .ToDictionary(
            group => group.Key,
            group => group.ToDictionary(config => config.level)
        );

    private Dictionary<uint, WeaponExcelConfig> LoadWeaponExcel() =>
        JsonConvert.DeserializeObject<List<WeaponExcelConfig>>(
            File.ReadAllText(Path.Combine(_baseResourcePath, ExcelSubPath, "WeaponExcelConfigData.json"))
        )!.ToDictionary(data => data.id);


    public ResourceLoader(ResourceManager resourceManager, string baseResourcePath)
    {
        _baseResourcePath = baseResourcePath;
        resourceManager.AvatarExcel = this.LoadAvatarExcel();
        resourceManager.AvatarSkillDepotExcel = this.LoadAvatarSkillDepotExcel();
        resourceManager.AvatarSkillExcel = this.LoadAvatarSkillExcel();
        resourceManager.ProudSkillExcel = this.LoadProudSkillExcel();
        resourceManager.WeaponExcel = this.LoadWeaponExcel();
    }
}
