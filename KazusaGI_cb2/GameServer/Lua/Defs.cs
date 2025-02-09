using KazusaGI_cb2.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaGI_cb2.GameServer.Lua;

public class Variable
{
    public string name;
    public int value;
}

public class ScriptArgs
{
    public int param1;
    public int param2;
    public int param3;
    public int source_eid; // Source entity
    public int target_eid;
    public int group_id;
    public string source; // source string, used for timers
    public int type;

    public EventTriggerType eventTypeAsEnum()
    {
        return (EventTriggerType)type;
    }

    public ScriptArgs(int groupId, int eventType, int param1, int param2)
    {
        this.group_id = groupId;
        this.type = eventType;
        this.param1 = param1;
        this.param2 = param2;
    }
    public ScriptArgs(int groupId, int eventType)
    {
        this.group_id = groupId;
        this.type = eventType;
    }
    public ScriptArgs(int groupId, int eventType, int param1)
    {
        this.group_id = groupId;
        this.type = eventType;
        this.param1 = param1;
    }

    public object toTable()
    {
        return new { param1 = param1, param2 = param2, param3 = param3, source_eid = source_eid, target_eid = target_eid, type = type, group_id = group_id, source = source };
    }
}

public enum TriggerEventType
{
    EVENT_NONE = 0, // 无
    EVENT_ANY_MONSTER_DIE = 1, // 任何怪物死亡
    EVENT_ANY_GADGET_DIE = 2, // 任何物件死亡
    EVENT_VARIABLE_CHANGE = 3, // 变量改变
    EVENT_ENTER_REGION = 4, // 进入区域
    EVENT_LEAVE_REGION = 5, // 离开区域
    EVENT_GADGET_CREATE = 6, // 物件创建
    EVENT_GADGET_STATE_CHANGE = 7, // 物件状态改变
    EVENT_DUNGEON_SETTLE = 8, // 副本结算
    EVENT_SELECT_OPTION = 9, // 选择选项
    EVENT_CLIENT_EXECUTE = 10, // 客户端请求执行trigger
    EVENT_ANY_MONSTER_LIVE = 11, // 任何怪物创生
    EVENT_SPECIFIC_MONSTER_HP_CHANGE = 12, // 指定config_id的怪物血量变化
    EVENT_AREA_LEVELUP_UNLOCK_DUNGEON_ENTRY = 13, // area升级后执行UNLOCK_DUNGEON_ENTRY的动作
    EVENT_DUNGEON_BROADCAST_ONTIMER = 14, // 副本group定时事件，广播给所有group
    EVENT_TIMER_EVENT = 15, // 延时调用事件
    EVENT_CHALLENGE_SUCCESS = 16, // 区域挑战成功
    EVENT_CHALLENGE_FAIL = 17, // 区域挑战失败
    EVENT_SEAL_BATTLE_BEGIN = 18, // 封印战斗开始
    EVENT_SEAL_BATTLE_END = 19, // 封印战斗结束
    EVENT_GATHER = 20, // 采集事件
    EVENT_QUEST_FINISH = 21, // 任务完成
    EVENT_MONSTER_BATTLE = 22, // 怪物入战
    EVENT_AREA_LEVELUP = 23, // 一级区域升级
    EVENT_CUTSCENE_END = 24, // 所有客户端播放cutscene结束
    EVENT_AVATAR_NEAR_PLATFORM = 25, // 角色靠近移动平台，这时移动平台已停止(客户端发送)
    EVENT_PLATFORM_REACH_POINT = 26,
}