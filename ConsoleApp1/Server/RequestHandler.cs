using System.Net;
using System.Text.Json;
using ConsoleApp1.Util;

namespace ConsoleApp1.Server;

public sealed class RequestHandler
{
    private string scratchPaper;

    public RequestHandler(string scratchPaper)
    {
        this.scratchPaper = scratchPaper;
    }

    public async Task HandleRequest(HttpListenerContext context)
    {
        var request = context.Request;
        var response = context.Response;

        if (request.HttpMethod == "GET" && request.Url!.AbsolutePath == "/hello")
        {
            await HandleGetHello(response);
        }

        if (request.HttpMethod == "POST" && request.Url!.AbsolutePath == "/say")
        {
            await HandleSaySomething(request, response);
        }
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

    private async Task HandleSaySomething(HttpListenerRequest request, HttpListenerResponse response)
    {
        // Deserialize JSON to string
        var buffer = new byte[request.ContentLength64];
        _ = await request.InputStream.ReadAsync(buffer, 0, buffer.Length);
        var saySomething = JsonSerializer.Deserialize<SaySomething>(buffer);

        // Append message to scratchPaper
        scratchPaper += saySomething?.Something;

        // Serialize string to JSON and to byte array eventually
        var responseString = scratchPaper;
        var outputBuffer = responseString.ToJsonUtf8ByteArray();

        // Decorate HttpListenerResponse object
        response.ContentType = "application/json";
        response.ContentLength64 = outputBuffer.Length;

        // Write the response to the output stream
        await response.OutputStream.WriteAsync(outputBuffer, 0, outputBuffer.Length);
        response.OutputStream.Close();
    }
}

public sealed record SaySomething(string Something);