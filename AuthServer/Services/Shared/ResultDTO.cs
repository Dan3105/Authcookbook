namespace AuthServer.Services.Shared
{
    public class ResultDTO<T> where T : class
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public int StatusCode { get; set; }
        public T? Response { get; set; }

        public static ResultDTO<T> Success(T response, int statusCode = 200)
        {
            return new ResultDTO<T>
            {
                IsSuccess = true,
                Response = response,
                StatusCode = statusCode
            };
        }

        public static ResultDTO<T> Success(int statusCode = 200)
        {
            return new ResultDTO<T>
            {
                IsSuccess = true,
                StatusCode = statusCode
            };
        }

        public static ResultDTO<T> Failure(string message, int statusCode = 500)
        {
            return new ResultDTO<T>
            {
                IsSuccess = false,
                Message = message,
                StatusCode = statusCode
            };
        }

        public static ResultDTO<T> Failure(int statusCode = 404)
        {
            return new ResultDTO<T>
            {
                IsSuccess = false,
                StatusCode = statusCode
            };
        }
    }
}
