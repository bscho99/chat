using System.Text;
using System.Text.Json;

namespace ConsoleApp1.Util;

public static class StringExtension
{
    public static Byte[] ToJsonUtf8ByteArray(this string input)
    {
        var messageObject = new { message = input };
        var messageJson = JsonSerializer.Serialize(messageObject);
        var buffer = Encoding.UTF8.GetBytes(messageJson);

        return buffer;
    }
}