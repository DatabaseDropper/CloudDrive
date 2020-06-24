using System.Collections.Generic;

namespace CloudDrive.Miscs
{
    public class Result<T>
    {
        public Result(bool success, T data, Dictionary<string, List<string>> errors, ErrorType type)
        {
            Success = success;
            Data = data;
            Errors = errors;
            Error = type;
        }       
        
        public Result(bool success, T data, List<string> errors, ErrorType type)
        {
            Success = success;
            Data = data;
            Errors = new Dictionary<string, List<string>>
            {
                { "message", errors }
            };
            Error = type;
        }      
        
        public Result(bool success, T data, string error, ErrorType type)
        {
            Success = success;
            Data = data;
            Errors =
            new Dictionary<string, List<string>>
            {
                { "message", new List<string> { error }}
            };
            
            Error = type;
        }

        public Result(bool success, T data)
        {
            Success = success;
            Data = data;
        }

        public Dictionary<string, List<string>> Errors = new Dictionary<string, List<string>>();

        public bool Success { get; set; }

        public T Data { get; set; }

        public ErrorType Error { get; set; } = ErrorType.None;
    }
}
