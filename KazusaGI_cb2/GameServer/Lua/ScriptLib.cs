using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazusaGI_cb2.GameServer.Lua;

public class ScriptLib
{
    public int currentGroupId;
    public Session currentSession;

    public int GetGroupMonsterCount(Session session)
    {
        return currentSession.entityMap.Values
            .Where(e => e is MonsterEntity monster &&
                        monster._monsterInfo != null &&
                        monster._monsterInfo.group_id == currentGroupId)
            .Count();
    }

    public int GetGroupMonsterCountByGroupId(Session session, int groupId)
    {
        return currentSession.entityMap.Values
            .Where(e => e is MonsterEntity monster &&
                        monster._monsterInfo != null &&
                        monster._monsterInfo.group_id == groupId)
            .Count();
    }


    public ScriptLib(Session session)
    {
        currentSession = session;
    }
}
