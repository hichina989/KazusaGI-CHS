using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaGI_cb2.Resource.Excel;

public class GadgetExcelConfig
{
    // type
    public string jsonName;
    public bool hasAudio;
    public bool isInteractive;
    // visionLevel
    public List<string> tags;
    public string itemJsonName;
    public string inteeIconName;
    public uint id;
    public uint nameTextMapHash;
    public uint campID;
    public string LODPatternName;
}