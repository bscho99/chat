using System.Net;
using ConsoleApp1.Util;

namespace ConsoleApp1.Server;

public sealed class RequestHandler
{
    public Task HandleRequest(HttpListenerContext context)
    {
        var request = context.Request;
        var response = context.Response;

        if (request.HttpMethod == "GET" && request.Url!.AbsolutePath == "/hello")
        {
            return HandleGetHello(response);
        }

        return Task.CompletedTask;
    }

    private async Task HandleGetHello(HttpListenerResponse response)
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
}