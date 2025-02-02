using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace KazusaGI_cb2.GameServer;

public class Entity
{
    public uint EntityId { get; private set; }
    public Vector3 Position { get; set; }

    public Entity(Session session, Vector3? position)
    {
        this.EntityId = session.GetEntityId(Protocol.ProtEntityType.ProtEntityAvatar);
        this.Position = position ?? new Vector3(0, 0, 0);

    }

}
