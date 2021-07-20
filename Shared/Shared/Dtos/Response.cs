using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Shared.Dtos
{
    public class Response<T> // no restrictions for type 
    {
        public T Data { get; set; }

        [JsonIgnore]
        public int StatusCode { get; set; }

        public List<string> ErrorList { get; set; }

        // Example of Static Factory Pattern - create an object with a static method
        public static Response<T> Success(T data, int statusCode)
        {
            return new Response<T> { Data = data, StatusCode = statusCode };
        }

        public static Response<T> Success( int statusCode)
        {
            return new Response<T> { Data = default(T), StatusCode = statusCode };
        }
        public static Response<T> Fail(List<string> errorList, int statusCode)
        {
            return new Response<T> { ErrorList = errorList, StatusCode = statusCode };
        }
        public static Response<T> Fail(string error, int statusCode)
        {
            return new Response<T> { ErrorList = new List<string> { error }, StatusCode = statusCode };
        }

    }
}
