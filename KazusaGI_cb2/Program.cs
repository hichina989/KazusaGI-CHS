using KazusaGI_cb2.WebServer;
using KazusaGI_cb2.Resource;
using System.Threading;

namespace KazusaGI_cb2;

public class MainApp
{
    public static Config config = Config.Load();
    public static ResourceManager resourceManager = new("resources");
    public static void Main(string[] args)
    {
        Logger.DoLogUselessInfo = true;
        Logger logger = new("MainApp");
        logger.LogKazusa();
        Thread webServerThread = new Thread(() => WebProgram.Main(config.WebServer.ServerIP, config.WebServer.ServerPort));
        webServerThread.Start();
        Thread gameServerThread = new Thread(() => GameServer.GameServerManager.StartLoop());
        gameServerThread.Start();
    }
}