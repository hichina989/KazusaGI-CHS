using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaGI_cb2.Resource.Excel;

public class ShopPlanExcelConfig
{
    public uint Id;
    public ShopType shopType;
    public uint goodsId;
    public uint minPlayerLevel;
    public uint maxPlayerLevel;
    public uint sortLevel;
    public uint minShowLevel;
}