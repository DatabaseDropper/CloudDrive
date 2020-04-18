using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudDrive.Miscs
{
    public class Result<T>
    {
        public Result(bool success, T data, List<string> errors)
        {
            Success = success;
            Data = data;
            Errors = errors;
        }      
        
        public Result(bool success, T data, string error)
        {
            Success = success;
            Data = data;
            Errors = new List<string> { error };
        }

        public Result(bool success, T data)
        {
            Success = success;
            Data = data;
        }

        public List<string> Errors = new List<string>();

        public bool Success { get; set; }

        public T Data { get; set; }
    }
}
