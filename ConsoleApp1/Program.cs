using System.Net;
using System.Text;
using System.Text.Json;

namespace ConsoleApp1;

class Program
{
    static async Task Main(string[] args)
    {
        var httpListener = new HttpListener();
        httpListener.Prefixes.Add("http://localhost:8080/");

        httpListener.Start();
        Console.WriteLine("Listening...");

        while (true)
        {
            var context = await httpListener.GetContextAsync();
            Console.WriteLine("Request received!");

            var request = context.Request;
            var response = context.Response;

            if (request is { HttpMethod: "GET", Url.AbsolutePath: "/hello" })
            {
                Console.WriteLine("GET /hello");
                await HandleGetHello(response);
            }
        }
    }

    static async Task HandleGetHello(HttpListenerResponse response)
    {
        var responseObj = new { message = "Hello, World!" };
        var responseJson = JsonSerializer.Serialize(responseObj);
        var buffer = Encoding.UTF8.GetBytes(responseJson);
        response.ContentType = "application/json";
        response.ContentLength64 = buffer.Length;

        await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
        response.OutputStream.Close();
    }
}
