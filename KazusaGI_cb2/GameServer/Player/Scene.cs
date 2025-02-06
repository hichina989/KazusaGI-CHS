using KazusaGI_cb2.Protocol;
using KazusaGI_cb2.Resource.Excel;
using KazusaGI_cb2.Resource;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace KazusaGI_cb2.GameServer;

// idk when ill fully implement this, too lazy kek
public class Scene
{
    public Session session { get; private set; }
    public Player player { get; private set; }
    public SceneLua sceneLua { get; private set; }
    public SceneBlockLua? sceneBlockLua { get; private set; }
    private static ResourceManager resourceManager { get; } = MainApp.resourceManager;
    private static Logger logger = new("SceneManager");
    public bool isFinishInit { get; set; } = false;
    public float defaultRange { get; private set; } = 100f;
    public List<MonsterLua> alreadySpawnedMonsters { get; private set; } = new();
    public List<GadgetLua> alreadySpawnedGadgets { get; private set; } = new();

    public Scene(Session _session, Player _player)
    {
        this.session = _session;
        this.player = _player;
        this.sceneLua = MainApp.resourceManager.SceneLuas[_player.SceneId];
    }

    // todo: make a timeout or something, so it wont be spammed every single frame
    public void UpdateOnMove()
    {
        if (!isFinishInit)
            return;
        Vector3 playerPos = this.player.Pos;
        for (int i = 0; i < this.sceneLua.block_rects.Count; i++)
        {
            BlockRect blockRect = this.sceneLua.block_rects[i];
            SceneBlockLua blockLua = this.sceneLua.scene_blocks[this.sceneLua.blocks[i]];

            if (!isInRange(blockRect, playerPos))
            {
                if (blockLua == sceneBlockLua)
                {
                    this.UnloadSceneBlock(blockLua);
                }
                continue;
            }

            if (blockLua != sceneBlockLua)
            {
                session.c.LogWarning($"Player {player.Uid} is in range of block {this.sceneLua.blocks[i]}");
                this.LoadSceneBlock(blockLua);
            } else
            {
                UpdateBlock();
            }

            // check entities isInRange later
            // same with regions
        }
    }

    public void UpdateBlock()
    {
        Vector3 playerPos = this.player.Pos;
        if (this.sceneBlockLua!.scene_groups != null)
        {
            foreach (var group in this.sceneBlockLua.scene_groups)
            {
                UpdateGroup(group.Value);
            }
        }
    }

    public void UpdateGroup(SceneGroupLua sceneGroupLua)
    {
        List<SceneEntityAppearNotify> sceneEntityAppearNotifies = new List<SceneEntityAppearNotify>();

        SceneEntityAppearNotify currentSceneEntityAppearNotify = new SceneEntityAppearNotify()
        {
            AppearType = VisionType.VisionMeet,
        };

        // Process monsters
        foreach (MonsterLua monsterLua in sceneGroupLua.monsters)
        {
            if (isInRange(monsterLua.pos, player.Pos, defaultRange) && !this.alreadySpawnedMonsters.Contains(monsterLua))
            {
                uint MonsterId = monsterLua.monster_id;
                MonsterExcelConfig monster = resourceManager.MonsterExcel[MonsterId];
                MonsterEntity monsterEntity = new MonsterEntity(session, MonsterId, monsterLua, monsterLua.pos);
                session.entityMap.Add(monsterEntity._EntityId, monsterEntity);
                currentSceneEntityAppearNotify.EntityLists.Add(monsterEntity.ToSceneEntityInfo(session));
                alreadySpawnedMonsters.Add(monsterLua);

                // If there are more than 5 entities, push current notify and start a new one
                if (currentSceneEntityAppearNotify.EntityLists.Count >= 10)
                {
                    sceneEntityAppearNotifies.Add(currentSceneEntityAppearNotify);
                    currentSceneEntityAppearNotify = new SceneEntityAppearNotify()
                    {
                        AppearType = VisionType.VisionMeet,
                    };
                }
            } else
            {
                if (!isInRange(monsterLua.pos, player.Pos, defaultRange) && this.alreadySpawnedMonsters.Contains(monsterLua))
                {
                    DespawnMonster(monsterLua);
                    this.alreadySpawnedMonsters.Remove(monsterLua); // so it can respawn when we come back to the are
                }
            }
        }


        // Process gadgets
        foreach (GadgetLua gadgetLua in sceneGroupLua.gadgets)
        {
            if (isInRange(gadgetLua.pos, player.Pos, defaultRange) && !this.alreadySpawnedGadgets.Contains(gadgetLua))
            {
                uint GadgetID = gadgetLua.gadget_id;
                Vector3 pos = gadgetLua.pos;
                GadgetEntity gadgetEntity = new GadgetEntity(session, GadgetID, gadgetLua, pos);
                session.entityMap.Add(gadgetEntity._EntityId, gadgetEntity);
                currentSceneEntityAppearNotify.EntityLists.Add(gadgetEntity.ToSceneEntityInfo(session));

                // If there are more than 5 entities, push current notify and start a new one
                if (currentSceneEntityAppearNotify.EntityLists.Count >= 10)
                {
                    sceneEntityAppearNotifies.Add(currentSceneEntityAppearNotify);
                    currentSceneEntityAppearNotify = new SceneEntityAppearNotify()
                    {
                        AppearType = VisionType.VisionMeet,
                    };
                }
            } else
            {
                if (!isInRange(gadgetLua.pos, player.Pos, defaultRange) && this.alreadySpawnedGadgets.Contains(gadgetLua))
                {
                    DespawnGadget(gadgetLua);
                    this.alreadySpawnedGadgets.Remove(gadgetLua); // so it can respawn when we come back to the are
                }
            }
        }

        // Add the last notify if it contains any entities, we don't want to send empty notifies
        if (currentSceneEntityAppearNotify.EntityLists.Count > 0)
        {
            sceneEntityAppearNotifies.Add(currentSceneEntityAppearNotify);
        }

        // Send each batch of SceneEntityAppearNotify
        foreach (var notify in sceneEntityAppearNotifies)
        {
            session.SendPacket(notify);
        }
    }

    public void LoadSceneBlock(SceneBlockLua blockLua)
    {
        if (this.sceneBlockLua == blockLua) return;
        this.sceneBlockLua = blockLua;

        // todo: validate group distance from player pos
        foreach (SceneGroupLua sceneGroupLua in blockLua.scene_groups.Values)
        {
            this.LoadSceneGroup(sceneGroupLua);
        }
    }

    public void UnloadSceneBlock(SceneBlockLua blockLua)
    {
        this.sceneBlockLua = null;
        foreach (SceneGroupLua sceneGroupLua in blockLua.scene_groups.Values)
        {
            this.UnloadSceneGroup(sceneGroupLua);
        }
    }
    public void LoadSceneGroup(SceneGroupLua sceneGroupLua)
    {
        List<SceneEntityAppearNotify> sceneEntityAppearNotifies = new List<SceneEntityAppearNotify>();

        SceneEntityAppearNotify currentSceneEntityAppearNotify = new SceneEntityAppearNotify()
        {
            AppearType = VisionType.VisionMeet,
        };

        // Process monsters
        foreach (MonsterLua monsterLua in sceneGroupLua.monsters)
        {
            if (isInRange(monsterLua.pos, player.Pos, 50f))
            {
                uint MonsterId = monsterLua.monster_id;
                MonsterExcelConfig monster = resourceManager.MonsterExcel[MonsterId];
                MonsterEntity monsterEntity = new MonsterEntity(session, MonsterId, monsterLua, monsterLua.pos);
                session.entityMap.Add(monsterEntity._EntityId, monsterEntity);
                currentSceneEntityAppearNotify.EntityLists.Add(monsterEntity.ToSceneEntityInfo(session));
                alreadySpawnedMonsters.Add(monsterLua);

                // If there are more than 5 entities, push current notify and start a new one
                if (currentSceneEntityAppearNotify.EntityLists.Count >= 10)
                {
                    sceneEntityAppearNotifies.Add(currentSceneEntityAppearNotify);
                    currentSceneEntityAppearNotify = new SceneEntityAppearNotify()
                    {
                        AppearType = VisionType.VisionMeet,
                    };
                }
            }
            
        }


        // Process gadgets
        foreach (GadgetLua gadgetLua in sceneGroupLua.gadgets)
        {
            if (isInRange(gadgetLua.pos, player.Pos, 50f))
            {
                uint GadgetID = gadgetLua.gadget_id;
                Vector3 pos = gadgetLua.pos;
                GadgetEntity gadgetEntity = new GadgetEntity(session, GadgetID, gadgetLua, pos);
                session.entityMap.Add(gadgetEntity._EntityId, gadgetEntity);
                currentSceneEntityAppearNotify.EntityLists.Add(gadgetEntity.ToSceneEntityInfo(session));
                alreadySpawnedGadgets.Add(gadgetLua);

                // If there are more than 5 entities, push current notify and start a new one
                if (currentSceneEntityAppearNotify.EntityLists.Count >= 10)
                {
                    sceneEntityAppearNotifies.Add(currentSceneEntityAppearNotify);
                    currentSceneEntityAppearNotify = new SceneEntityAppearNotify()
                    {
                        AppearType = VisionType.VisionMeet,
                    };
                }
            }
        }

        // Add the last notify if it contains any entities, we don't want to send empty notifies
        if (currentSceneEntityAppearNotify.EntityLists.Count > 0)
        {
            sceneEntityAppearNotifies.Add(currentSceneEntityAppearNotify);
        }

        // Send each batch of SceneEntityAppearNotify
        foreach (var notify in sceneEntityAppearNotifies)
        {
            session.SendPacket(notify);
        }
    }


    public void UnloadSceneGroup(SceneGroupLua sceneGroupLua)
    {
        foreach (MonsterLua monsterLua in sceneGroupLua.monsters)
        {
            this.DespawnMonster(monsterLua);
        }
        foreach (GadgetLua gadgetLua in sceneGroupLua.gadgets)
        {
            this.DespawnGadget(gadgetLua);
        }
    }

    public void DespawnMonster(MonsterLua monsterLua)
    {
        MonsterEntity? monsterEntity = session.entityMap.Values.OfType<MonsterEntity>().FirstOrDefault(x => x._monsterInfo == monsterLua);
        if (monsterEntity == null)
        {
            // logger.LogError($"Monster {monsterLua.monster_id} not found in current entity map (???)");
            return;
        }
        monsterEntity.Die(VisionType.VisionMiss);
    }

    public void DespawnGadget(GadgetLua gadgetLua)
    {
        GadgetEntity? gadgetEntity = session.entityMap.Values.OfType<GadgetEntity>().FirstOrDefault(x => x._gadgetLua == gadgetLua);
        if (gadgetEntity == null)
        {
            logger.LogError($"Gadget {gadgetLua.gadget_id} not found in current entity map (???)");
            return;
        }
        this.session!.SendPacket(new SceneEntityDisappearNotify()
        {
            EntityLists = { gadgetEntity._EntityId },
            DisappearType = VisionType.VisionMiss
        });
        session.entityMap.Remove(gadgetEntity._EntityId);
    }

    public static bool isInRange(Vector3 pos1, Vector3 pos2, float range)
    {
        return Vector3.Distance(pos1, pos2) < range;
    }

    public bool isInRange(BlockRect blockPos, Vector3 playerPos)
    {
        float minX = Math.Min(blockPos.min.X, blockPos.max.X);
        float maxX = Math.Max(blockPos.min.X, blockPos.max.X);
        float minY = Math.Min(blockPos.min.Z, blockPos.max.Z);
        float maxY = Math.Max(blockPos.min.Z, blockPos.max.Z);
        return playerPos.X >= minX && playerPos.X <= maxX && playerPos.Z >= minY && playerPos.Z <= maxY;
    }
}
