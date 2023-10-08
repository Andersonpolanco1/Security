using Common.DTOs;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace Common.Http
{
    public static class HttpRequest
    {
        public static async Task<Result<T>> TryGetAsync<T>(LoginRequestModel userCredentials)
        {
            try
            {
                //using var client = new HttpClient();
                //HttpResponseMessage response = await client.PostAsJsonAsync(
                //"https://localhost:8089/api/users/credentials", userCredentials,);
                //response.EnsureSuccessStatusCode();


                using var httpClient = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, new Uri("https://localhost:8089/api/users/credentials"));
                string jsonContent = JsonConvert.SerializeObject(userCredentials);
                request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("ApiKey", "pgH7QzFHJx4w46fI~5Uzi4RvtTwlEXp");
                HttpResponseMessage response = await httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                    return new Result<T>(response.StatusCode, response.ReasonPhrase);

                var responseBody = await response.Content.ReadAsStringAsync();
                return new Result<T>(HttpStatusCode.OK,string.Empty, JsonConvert.DeserializeObject<T>(responseBody));

            }
            catch (Exception ex)
            {
                return new Result<T>(HttpStatusCode.InternalServerError, $"{ex.GetType().Name}. {ex.Message}");
            }
        }
    }
}
