using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common.Http
{
    public class Result<T>
    {
        public Result(HttpStatusCode statusCode, string mesage)
        {
            this.statusCode = statusCode;
            this.message = message;
        }

        public Result(HttpStatusCode statusCode, T data)
        {
            this.statusCode = statusCode;
            this.Data = data;
        }

        public Result(HttpStatusCode statusCode, string message, T data) : this(statusCode, message)
        {
            Data = data;
        }

        public HttpStatusCode statusCode { get; set; }
        public string message { get; set; }
        public T Data { get; set; }
    }
}
