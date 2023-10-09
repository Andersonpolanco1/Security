using Auth.Services.Interfaces;
using Common.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Common.Http
{
    public class AppHttpClient:IAppHttpClient
    {
        public static readonly string UsersHttpClientName = "UsersHttpClient";
        private readonly IHttpClientFactory _httpClientFactory;

        public AppHttpClient(IHttpClientFactory httpClientFactory) =>
            _httpClientFactory = httpClientFactory;

        /// <summary>
        /// Http post request to user manager service using the client settings configured in appsettings
        /// </summary>
        /// <typeparam name="Tout">Type of object to return</typeparam>
        /// <typeparam name="Tin">Type of object that receibe as param</typeparam>
        /// <param name="content"></param>
        /// <returns>An <see cref="Result{T}
        /// "/></returns>
        public async Task<Result<Tout>> HttpPostAsync<Tout, Tin>(Tin content)
        {
            try
            {
                var client = _httpClientFactory.CreateClient(UsersHttpClientName);
                HttpResponseMessage response = await client.PostAsJsonAsync(new Uri(client.BaseAddress,"auth/"), content);

                if (!response.IsSuccessStatusCode)
                    return new Result<Tout>(response.StatusCode, response.ReasonPhrase);

                var responseBody = await response.Content.ReadAsStringAsync();
                    return new Result<Tout>(HttpStatusCode.OK, string.Empty, JsonConvert.DeserializeObject<Tout>(responseBody));
            }
            catch (Exception ex)
            {
                return new Result<Tout>(HttpStatusCode.InternalServerError, $"{ex.GetType().Name}. {ex.Message}");
            }
        }
    }
}
