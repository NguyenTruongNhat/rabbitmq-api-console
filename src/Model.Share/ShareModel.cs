namespace Model.Share
{
    // Messages
    public record RequestMessage
    {
        public string Content { get; init; }
    }

    public record ResponseMessage
    {
        public string Content { get; init; }
        public DateTime Timestamp { get; init; }
    }


    // Message
    public record NotificationMessage
    {
        public string Title { get; init; }
        public string Content { get; init; }
        public DateTime Timestamp { get; init; } = DateTime.UtcNow;
    }


    // Message
    public record CommandMessage
    {
        public string Command { get; init; }
        public string Parameters { get; init; }
        public Guid CorrelationId { get; init; } = Guid.NewGuid();
    }
}
