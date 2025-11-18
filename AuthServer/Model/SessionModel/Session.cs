namespace AuthServer.Model.SessionModel
{
    public class SessionDTO
    {
        public required string SessionId { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }

    public static class SessionConfiguration
    {
        public static string SessionPrefix = "authserver:sessions:";
        public static TimeSpan SessionDurationInHour = TimeSpan.FromHours(1); // Session duration
    }
}
