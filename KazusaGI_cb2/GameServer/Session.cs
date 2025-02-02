using KazusaGI_cb2.Protocol;
using System;
using KazusaGI_cb2.Utils;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using Newtonsoft.Json;
using KazusaGI_cb2.GameServer.PlayerInfos;

namespace KazusaGI_cb2.GameServer;

public class Session
{
    public readonly Logger c;
    private readonly ENetClient _client;
    public Player? player;
    public Dictionary<ulong, Entity> entityMap;
    public IntPtr _peer;
    public byte[]? key;
    private ulong lastGuid = 0;
    private uint lastEntityId = 0;

    public Session(ENetClient client, IntPtr peer)
    {
        _client = client;
        _peer = peer;
        entityMap = new();
        c = new Logger($"Session {_peer}");
    }

    private string PacketToJson(Packet packet)
    {
        try
        {
            PacketId cmd = (PacketId)packet.CmdId;
            string protoName = $"{cmd}";
            Type protoType = Type.GetType($"KazusaGI_cb2.Protocol.{protoName}")!;
            MethodInfo method = typeof(Packet).GetMethod(nameof(packet.GetDecodedBody))!.MakeGenericMethod(protoType);
            string jsonBody = Newtonsoft.Json.JsonConvert.SerializeObject(method.Invoke(packet, null)!);
            return jsonBody;
        } 
        catch (Exception e)
        {
            c.LogError($"{e.Message}, {e.InnerException}, {e.Source}");
            return String.Empty;
        }
    }

    public SceneEntityInfo CreateSceneEntityInfoFromPlayerAvatar(Session session, PlayerAvatar playerAvatar)
    {
        AvatarEntity avatarEntity = new AvatarEntity(session, playerAvatar);
        this.entityMap.Add(avatarEntity.EntityId, avatarEntity);
        return avatarEntity.ToSceneEntityInfo(session);
    }

    public ulong GetGuid()
    {
        lastGuid++;
        return lastGuid;
    }
    public uint GetEntityId(ProtEntityType type)
    {
        lastEntityId++;
        return ((uint)type << 24) + lastEntityId;
    }

    public void onMessage(Packet packet)
    {
        if (packet == null)
        {
            return;
        }
        c.LogPacket($"Received {(PacketId)packet.CmdId} {PacketToJson(packet)}");
        var handler = HandlerFactory.GetHandler((PacketId)packet.CmdId);
        if (handler == null)
        {
            c.LogError($"No handler for {(PacketId)packet.CmdId}");
            return;
        }

        handler?.Invoke(this, packet);
    }


    public bool SendPacket(IExtensible protoMessage)
    {
        try
        {
            string protoName = protoMessage.ToString()!.Split("KazusaGI_cb2.Protocol.").Last();
            PacketId packetId = (PacketId)Enum.Parse(typeof(PacketId), protoName);
            IntPtr packet = Packet.EncodePacket(this, (ushort)packetId, protoMessage);
            ENet.enet_peer_send(_peer, 0, packet);
            c.LogInfo($"Sent {protoName} {JsonConvert.SerializeObject(protoMessage)}");
            return true;
        }
        catch (Exception e)
        {
            c.LogError($"{e.Message}");
            return false;
        }
    }
}