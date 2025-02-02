using KazusaGI_cb2.Protocol;
using System;
using KazusaGI_cb2.Utils;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KazusaGI_cb2.GameServer;

public class ENetClient
{
    public bool Connected { get; set; }
    public ENetClient(IntPtr id)
    {
    }
}