namespace ConsoleApp1;

class Program
{
    static async Task Main(string[] args)
    {
        var server = new Server.Server();
        await server.Run();
    }
}
