using KazusaGI_cb2.Resource.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using NLua;
using System.Numerics;
using KazusaGI_cb2.Protocol;
using System.Resources;

namespace KazusaGI_cb2.Resource;

public class ResourceLoader
{
    private static readonly string ExcelSubPath = "ExcelBinOutput";
    private static readonly string JsonSubPath = "BinOutput";
    private static readonly string LuaSubPath = "Lua";
    private string _baseResourcePath;
    private ResourceManager _resourceManager;

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

    private Dictionary<uint, ScenePoint> LoadScenePoints()
    {
        var scenePoints = new Dictionary<uint, ScenePoint>();
        string scenePath = Path.Combine(_baseResourcePath, LuaSubPath, "Scene");
        foreach (var sceneDir in Directory.GetDirectories(scenePath))
        {
            string sceneIdStr = Path.GetFileName(sceneDir);
            if (!uint.TryParse(sceneIdStr, out uint sceneId))
                continue;
            string jsonPath = Path.Combine(sceneDir, $"scene{sceneId}_point.json");
            if (File.Exists(jsonPath))
            {
                var scenePoint = JsonConvert.DeserializeObject<ScenePoint>(File.ReadAllText(jsonPath));
                if (scenePoint != null)
                    scenePoints[sceneId] = scenePoint;
            }
            LoadSceneLua(sceneDir, sceneId);
        }
        return scenePoints;
    }

    // TODO: load blocks later
    private void LoadSceneLua(string sceneDir, uint sceneId)
    {
        string luaPath = Path.Combine(sceneDir, $"scene{sceneId}.lua");
        using (Lua luaContent = new Lua())
        {
            luaContent.DoString(File.ReadAllText(luaPath));
            SceneLua sceneLuaConfig = new SceneLua();
            LuaTable blocks = (LuaTable)luaContent["blocks"];
            LuaTable scene_config = (LuaTable)luaContent["scene_config"];
            LuaTable dummy_points = (LuaTable)luaContent["dummy_points"];
            LuaTable routes_config = (LuaTable)luaContent["routes_config"];
            Vector3 begin_pos = Table2Vector3(scene_config["begin_pos"]);
            Vector3 born_pos = Table2Vector3(scene_config["born_pos"]);
            Vector3 born_rot = Table2Vector3(scene_config["born_rot"]);
            Vector3 size = Table2Vector3(scene_config["size"]);

            LuaTable block_rects = (LuaTable)luaContent["block_rects"];

            SceneConfig sceneConfig = new SceneConfig()
            {
                begin_pos = begin_pos,
                size = size,
                born_pos = born_pos,
                born_rot = born_rot,
                die_y = Convert.ToInt32(scene_config["die"]),
            };

            sceneLuaConfig.scene_config = sceneConfig;

            sceneLuaConfig.blocks = blocks.Keys.Count > 0
                ? blocks.Values.Cast<object>().Select(block => Convert.ToInt32(block)).ToList()
                : new List<int>();

            sceneLuaConfig.block_rects = new List<BlockRect>();
            if (block_rects != null)
            {
                foreach (LuaTable c in block_rects.Values.Cast<LuaTable>())
                {
                    sceneLuaConfig.block_rects.Add(new BlockRect()
                    {
                        min = Table2Vector3(c["min"]),
                        max = Table2Vector3(c["max"])
                    });
                }
            }

            if (dummy_points != null)
            {
                sceneLuaConfig.dummy_points = dummy_points.Values.Count > 0
                ? dummy_points.Values.Cast<string>().ToList()
                : new List<string>();
            }

            if (routes_config != null)
            {
                sceneLuaConfig.routes_config = routes_config.Values.Count > 0
                    ? routes_config.Values.Cast<string>().ToList()
                    : new List<string>();
            }

            _resourceManager.SceneLuas[sceneId] = sceneLuaConfig;
        }
    }

    private Vector3 Table2Vector3(object vectorTable)
    {
        LuaTable _vectorTable = (LuaTable)vectorTable;
        return new Vector3()
        {
            X = Convert.ToSingle(Convert.ToDouble(_vectorTable["x"])),
            Y = _vectorTable["y"] != null ? Convert.ToSingle(Convert.ToDouble(_vectorTable["y"])) : 0.0F,
            Z = Convert.ToSingle(Convert.ToDouble(_vectorTable["z"]))
        };
    }

    public ResourceLoader(ResourceManager resourceManager, string baseResourcePath)
    {
        _baseResourcePath = baseResourcePath;
        this._resourceManager = resourceManager;
        _resourceManager.SceneLuas = new Dictionary<uint, SceneLua>();
        _resourceManager.AvatarExcel = this.LoadAvatarExcel();
        _resourceManager.AvatarSkillDepotExcel = this.LoadAvatarSkillDepotExcel();
        _resourceManager.AvatarSkillExcel = this.LoadAvatarSkillExcel();
        _resourceManager.ProudSkillExcel = this.LoadProudSkillExcel();
        _resourceManager.WeaponExcel = this.LoadWeaponExcel();
        _resourceManager.ScenePoints = this.LoadScenePoints();
    }
}
