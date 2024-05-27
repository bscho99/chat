using TestChattingRoomServer.Server;

namespace TestChattingRoomServer;

class Program
{
    static async Task Main(string[] args)
    {
        var socket = "http://localhost:8080/";
        if (args.Length >= 1)
        {
            socket = args[0];
        }

        ServerBuilder.CreateServer(socket);
        var server = ServerBuilder.GetInstance();
        await server.Run();
    }
}
