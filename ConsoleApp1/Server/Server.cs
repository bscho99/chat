using System.Net;

namespace ConsoleApp1.Server;

public class Server
{
    private readonly HttpListener listener;
    private string scratchPaper;
    private RequestHandler requestHandler;

    public Server()
    {
        scratchPaper = "";

        listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:8080/");

        requestHandler = new RequestHandler(scratchPaper);

        listener.Start();
        Console.WriteLine("Listening...");
    }

    public async Task Run()
    {
        while (true)
        {
            var context = await listener.GetContextAsync();
            Console.WriteLine("Request received!");

            await requestHandler.HandleRequest(context);
        }
    }
}