using Common.DTOs;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace Common.Http
{
    public static class HttpRequest
    {
        /// <summary>
        /// Http Post to an URI
        /// </summary>
        /// <typeparam name="Tout">Object type to return, the response will be descerializad to this type</typeparam>
        /// <typeparam name="Tin">Content tipe to send to the URI, this object will be serialized to send in the request</typeparam>
        /// <param name="apiUrl">Uri to post the request</param>
        /// <param name="content">Content of the request</param>
        /// <param name="headers">Headers to add to the request</param>
        /// <param name="authHeaders">Authorization headers to add to the request</param>
        /// <returns>Result object of Tout value</returns>
        public static async Task<Result<Tout>> HttpPostAsync<Tout,Tin>(string apiUrl, Tin content, IDictionary<string,string>? authHeaders = null, IDictionary<string,string>? headers = null)
        {
            try
            {
                using var client = new HttpClient();

                if(authHeaders != null)
                {
                    foreach (var header in authHeaders)
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(header.Key, header.Value);
                    }
                }

                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }

                HttpResponseMessage response = await client.PostAsJsonAsync(apiUrl, content);

                if (!response.IsSuccessStatusCode)
                    return new Result<Tout>(response.StatusCode, response.ReasonPhrase);

                var responseBody = await response.Content.ReadAsStringAsync();
                return new Result<Tout>(HttpStatusCode.OK,string.Empty, JsonConvert.DeserializeObject<Tout>(responseBody));
            }
            catch (Exception ex)
            {
                return new Result<Tout>(HttpStatusCode.InternalServerError, $"{ex.GetType().Name}. {ex.Message}");
            }
        }
    }
}
