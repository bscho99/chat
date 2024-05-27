using TestChattingRoomServer.Server;

namespace TestChattingRoomServer.Packet;

public sealed record RequestWriteToBuffer(string Message);

public sealed record ResponseGetMessages(List<Message> Messages);