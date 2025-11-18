using Docker.DotNet.Models;
using Newtonsoft.Json.Linq;

namespace AuthServer.Services.Shared
{
    public class ResultDTO
    {
        public bool IsSuccess { get; private set; }
        public string? Message { get; private set; }
        public int StatusCode { get; private set; }

        protected internal ResultDTO(bool isSuccess, int code)
        {
            IsSuccess = isSuccess;
            StatusCode = code;
        }
        protected internal ResultDTO(bool isSuccess, int code, string message)
        {
            IsSuccess = isSuccess;
            StatusCode = code;
            Message = message;
        }

        public static ResultDTO Success(int statusCode = 200) => new(true, statusCode);
        public static ResultDTO Success(string message, int statusCode = 200) => new(true, statusCode, message);
        public static ResultDTO Failure(int statusCode = 500) => new(false, statusCode);
        public static ResultDTO Failure(string message, int statusCode = 500) => new(false, statusCode, message);
    }

    public class ResultDTO<T> : ResultDTO
    {
        public T? Response { get; private set; }

        protected internal ResultDTO(T? value, bool isSuccess, int statusCode)
            : base(isSuccess, statusCode)
        {
            Response = value;
        }

        protected internal ResultDTO(T? value, bool isSuccess, int statusCode, string message)
            : base(isSuccess, statusCode, message)
        {
            Response = value;
        }

        public static new ResultDTO<T> Success(int statusCode = 200) => new(default, true, statusCode);
        public static new ResultDTO<T> Success(string message, int statusCode = 200) => new(default, true, statusCode, message);
        public static new ResultDTO<T> Failure(int statusCode = 500) => new(default, false, statusCode);
        public static new ResultDTO<T> Failure(string message, int statusCode = 500) => new(default, false, statusCode, message);


        public static ResultDTO<T> Success(T? response, int statusCode = 200) => new(response, true, statusCode);
        public static ResultDTO<T> Success(T? response, string message, int statusCode = 200) => new(response, true, statusCode, message);
        public static ResultDTO<T> Failure(T? response, int statusCode = 500) => new(response, false, statusCode);
        public static ResultDTO<T> Failure(T? response, string message, int statusCode = 500) => new(response, false, statusCode, message);
    }
}
