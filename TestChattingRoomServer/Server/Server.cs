using System.Net;
using System.Text.Json;
using TestChattingRoomServer.Packet;
using TestChattingRoomServer.Util;

namespace TestChattingRoomServer.Server;

public static class ServerBuilder
{
    private static Server serverInstance;

    public static void CreateServer(string socket)
    {
        serverInstance = new Server(socket);
    }

    public static Server GetInstance()
    {
        return serverInstance;
    }
}

public sealed class Server
{
    private readonly HttpListener _listener;
    private string _textBuffer;
    private Dictionary<string, int> _ipToUserId;
    private int _maxUserId;

    public Server(string socket)
    {
        _listener = new HttpListener();
        _listener.Prefixes.Add(socket);

        _textBuffer = String.Empty;

        _ipToUserId = new Dictionary<string, int>();

        _listener.Start();
        Console.WriteLine($"Listening from {socket}...");
    }

    public async Task Run()
    {
        while (true)
        {
            var context = await _listener.GetContextAsync();
            Console.WriteLine("Request received!");

            try
            {
                await HandleRequest(context);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    private async Task HandleRequest(HttpListenerContext context)
    {
        var request = context.Request;
        var response = context.Response;

        if (request.HttpMethod == "GET" && request.Url!.AbsolutePath == "/hello")
        {
            await Hello(response);
        }

        if (request.HttpMethod == "POST" && request.Url!.AbsolutePath == "/write")
        {
            await WriteToBuffer(request, response);
        }
    }

    private async Task Hello(HttpListenerResponse response)
    {
        // Serialize string to JSON and to byte array eventually
        var helloString = "Hello, World!";
        var buffer = helloString.ToJsonUtf8ByteArray();

        // Decorate HttpListenerResponse object
        response.ContentType = "application/json";
        response.ContentLength64 = buffer.Length;

        // Write the response to the output stream
        await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
        response.OutputStream.Close();
    }

    private async Task WriteToBuffer(HttpListenerRequest request, HttpListenerResponse response)
    {
        // Deserialize JSON to string
        var buffer = new byte[request.ContentLength64];
        _ = await request.InputStream.ReadAsync(buffer, 0, buffer.Length);
        var message = JsonSerializer.Deserialize<RequestWriteToBuffer>(buffer);

        // Append message to scratchPaper
        _textBuffer += message?.Message;

        // Serialize string to JSON and to byte array eventually
        var responseString = _textBuffer;
        var outputBuffer = responseString.ToJsonUtf8ByteArray();

        // Decorate HttpListenerResponse object
        response.ContentType = "application/json";
        response.ContentLength64 = outputBuffer.Length;

        // Write the response to the output stream
        await response.OutputStream.WriteAsync(outputBuffer, 0, outputBuffer.Length);
        response.OutputStream.Close();
    }

    private int GetUserIdFromIpAddress(string ipAddress)
    {
        if (_ipToUserId.ContainsKey(ipAddress))
        {
            return _ipToUserId[ipAddress];
        }
        _ipToUserId.Add(ipAddress, _maxUserId);
        _maxUserId += 1;
        return _ipToUserId[ipAddress];
    }
}