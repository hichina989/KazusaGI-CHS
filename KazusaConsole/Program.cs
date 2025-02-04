using KazusaConsole;
using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;

class Client
{
    static Logger clientLogger = new Logger("KazusaConsole");
    static Logger serverLogger = new Logger("Server");

    static void Main()
    {
        while (true) // Always attempt reconnection
        {
            try
            {
                using (var pipeClient = new NamedPipeClientStream(".", "KazusaGI", PipeDirection.InOut))
                {
                    clientLogger.LogWarning("Attempting to connect to pipe...");
                    pipeClient.Connect();
                    clientLogger.LogSuccess("Connected to pipe.");

                    using (StreamReader reader = new StreamReader(pipeClient))
                    using (StreamWriter writer = new StreamWriter(pipeClient) { AutoFlush = true })
                    {
                        serverLogger.LogSuccess(reader.ReadLine()!);

                        while (true) // Keep sending messages
                        {
                            serverLogger.LogInfo("Enter message: ");
                            string input = Console.ReadLine()!;
                            if (string.IsNullOrEmpty(input)) break;

                            writer.WriteLine(input);

                            string? response = reader.ReadLine();
                            if (response == null) break; // If server disconnects

                            clientLogger.LogSuccess(response);
                        }
                    }
                }
            }
            catch (IOException)
            {
                clientLogger.LogError("Server disconnected. Retrying in 2 seconds...");
                Thread.Sleep(2000);
            }
        }
    }
}
