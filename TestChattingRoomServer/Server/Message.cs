namespace TestChattingRoomServer.Server;

public sealed record Message(int UserId, string MessageBody, DateTime SentAt);
