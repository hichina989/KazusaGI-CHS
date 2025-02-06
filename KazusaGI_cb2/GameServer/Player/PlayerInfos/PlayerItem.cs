using KazusaGI_cb2.Resource.Excel;
using KazusaGI_cb2.Resource;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KazusaGI_cb2.Protocol;

namespace KazusaGI_cb2.GameServer.PlayerInfos;

public class PlayerItem
{
    private Session Session { get; set; } // just in case ill need it
    private MaterialExcelConfig MaterialExcel { get; set; }
    public ulong Guid { get; set; }
    public uint ItemId { get; set; }
    public uint Count { get; set; }

    public PlayerItem(Session session, uint materialId)
    {
        this.Session = session;
        this.MaterialExcel = MainApp.resourceManager.MaterialExcel[materialId];
        this.Guid = session.GetGuid();
        this.ItemId = materialId;
        this.Count = this.MaterialExcel.stackLimit;
    }

    // more stuff later ig?
}
