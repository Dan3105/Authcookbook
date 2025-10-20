namespace AuthCookbook.Core.Shared
{
    public class ResponseDTO
    {
        public int code { get; set; }
        public string message { get; set; } = string.Empty;
        public object? data { get; set; }
    }
}
