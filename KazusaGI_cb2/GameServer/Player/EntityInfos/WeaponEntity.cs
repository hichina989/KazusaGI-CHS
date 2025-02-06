using KazusaGI_cb2.GameServer.PlayerInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using KazusaGI_cb2.Protocol;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using KazusaGI_cb2.Resource.Excel;
using KazusaGI_cb2.Resource;

namespace KazusaGI_cb2.GameServer;

// created this one just so entity manager wont be confused
public class WeaponEntity : Entity
{
    public uint _gadgetId;
    public WeaponExcelConfig? weaponExcel;

    public WeaponEntity(Session session, uint gadgetId, uint? weaponId = null, Vector3? position = null)
        : base(session, position)
    {
        this._EntityId = session.GetEntityId(Protocol.ProtEntityType.ProtEntityWeapon);
        this._gadgetId = gadgetId;

        if (weaponId != null)
            this.weaponExcel = MainApp.resourceManager.WeaponExcel[gadgetId];
    }
}