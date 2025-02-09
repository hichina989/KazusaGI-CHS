using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace KazusaGI_cb2.Resource.Excel;

public class SceneBlockLua
{
    public List<SceneGroupBasicLua> groups;

    // custom, inlined
    public Dictionary<uint, SceneGroupLua> scene_groups;
}

public class SceneGroupBasicLua
{
    public uint id;
    public uint refresh_id;
    public uint area;
    public Vector3 pos;
    public bool dynamic_load;
    public bool unload_when_disconnect;
    // life_cycle, will be used later if at all
}

public class SceneGroupLua
{
    public List<MonsterLua> monsters;
    public List<NpcLua> npcs;
    public List<GadgetLua> gadgets;
    // regions
    public List<SceneTriggerLua> triggers;
    // variables -> important for later (logic)
    public SceneGroupLuaInitConfig init_config;
    public List<SceneGroupLuaSuite> suites;

    // functions later ??
}

public class SceneGroupLuaInitConfig
{
    public uint suite; // the suit we load the scene with
    public uint end_suite; // last index of the suits 
    public uint rand_suite; // no idea what it does
}

public class SceneGroupLuaSuite
{
    public List<uint> monsters;
    public List<uint> gadgets;
    public List<uint> regions;
    public List<string> triggers;
    public uint rand_weight;
}