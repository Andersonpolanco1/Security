using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Http
{
    public static class HttpUtils
    {
        public static HttpRequestMessage BuildRequest(HttpMethod method, string? requestUri) =>
            new HttpRequestMessage(method, requestUri);



    }
}
