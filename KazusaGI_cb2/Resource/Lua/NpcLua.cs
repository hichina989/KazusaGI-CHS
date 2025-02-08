using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace KazusaGI_cb2.Resource.Excel;

public class NpcLua
{
    public uint config_id;
    public uint npc_id;
    public Vector3 pos;
    public Vector3 rot;

    // custom fields
    public uint block_id;
    public uint group_id;
}