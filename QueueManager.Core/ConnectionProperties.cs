namespace QueueManager.Core
{
    public record ConnectionProperties
    {
        public string UserName { get; init; } = "guest";
        public string Password { get; init; }= "guest";
        public string HostName { get; init; } = "localhost";
    }
}