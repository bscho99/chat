using System.Net;
using System.Text;
using System.Text.Json;

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
        var responseObj = new { message = "Hello, World!" };
        var responseJson = JsonSerializer.Serialize(responseObj);
        var buffer = Encoding.UTF8.GetBytes(responseJson);

        // Decorate HttpListenerResponse object
        response.ContentType = "application/json";
        response.ContentLength64 = buffer.Length;

        // Write the response to the output stream
        await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
        response.OutputStream.Close();
    }
}