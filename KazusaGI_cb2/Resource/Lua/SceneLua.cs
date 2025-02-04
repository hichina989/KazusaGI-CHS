using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace KazusaGI_cb2.Resource.Excel;

public class SceneLua
{
    public SceneConfig scene_config;
    public List<int> blocks;
    public List<BlockRect> block_rects;
    public List<string> dummy_points;
    public List<string> routes_config;
}

public class SceneConfig
{
    public Vector3 begin_pos;
    public Vector3 size;
    public Vector3 born_pos;
    public Vector3 born_rot;
    public int die_y;
}

public class BlockRect
{
    public Vector3 min;
    public Vector3 max;
}