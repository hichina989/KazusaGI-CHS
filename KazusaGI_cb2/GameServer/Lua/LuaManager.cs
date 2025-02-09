using KazusaGI_cb2.Resource;
using KazusaGI_cb2.Resource.Excel;
using NLua;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaGI_cb2.GameServer.Lua;

public class LuaManager
{
    static Logger logger = new Logger("LuaManager");
    public static void executeTrigger(Session session, SceneTriggerLua trigger, ScriptArgs args, SceneGroupLua? group = null)
    {
        if (group == null)
        {
            group = session.player!.Scene.GetGroup(args.group_id);
        }

        if (group != null)
        {
            using (NLua.Lua groupLua = new NLua.Lua())
            {
                ScriptLib scriptLib = new(session);
                scriptLib.currentSession = session;
                scriptLib.currentGroupId = (int)session.player!.Scene.GetGroupIdFromGroupInfo(group);
                groupLua["ScriptLib"] = scriptLib;
                groupLua["context_"] = session;
                groupLua["evt_"] = args.toTable();

                ResourceLoader resourceLoader = MainApp.resourceManager.loader;
                string luaFile = File.ReadAllText(Path.Combine(resourceLoader._baseResourcePath, ResourceLoader.LuaSubPath, "Config", "Excel", "CommonScriptConfig.lua")) + "\n"
                        + File.ReadAllText(Path.Combine(resourceLoader._baseResourcePath, ResourceLoader.LuaSubPath, "Config", "Json", "ConfigEntityType.lua")) + "\n"
                        + File.ReadAllText(Path.Combine(resourceLoader._baseResourcePath, ResourceLoader.LuaSubPath, "Config", "Json", "ConfigEntity.lua")) + "\n"
                        + File.ReadAllText(MainApp.resourceManager.GetLuaStringFromGroupId((uint)args.group_id));

                groupLua.DoString(luaFile.Replace("ScriptLib.", "ScriptLib:"));

                string luaScript = @$"
                              
                                if {trigger.condition}(context_, evt_) then
                                    {trigger.action}(context_, evt_)
                                end
                            
                        ";
                try
                {
                    if (trigger.condition.Length == 0)
                    {
                        luaScript = @$"
                            {trigger.condition}(context_, evt_)
                            ";
                    }
                    groupLua.DoString(luaScript);
                    logger.LogSuccess($"Executed successfully LUA of type: {(TriggerEventType)trigger._event}");
                }
                catch (Exception ex)
                {
                    logger.LogError($"Error occured in executing Trigger Lua {ex.Message}");
                }
            }
        }
    }

    public static void executeTriggersLua(Session session, SceneGroupLua group, ScriptArgs args)
    {
        if (group == null) return;
        List<SceneTriggerLua> triggers = group.triggers.FindAll(t => t._event == args.eventTypeAsEnum());

        if (triggers.Count > 0)
        {
            foreach (SceneTriggerLua trigger in triggers)
            {

                executeTrigger(session, trigger, args, group);
            }
        }
    }
}
