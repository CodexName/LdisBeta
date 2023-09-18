using LdisDirty.Services.GoogleOAuthServices;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
namespace LdisDirty.Services.RealizationServices
{
    public class SendRequest : IRequestSendService
    {
        public async Task<T> SendPostRequestAsync<T>(string endpoint, Dictionary<string, string> bodyParams)
        {
            var httpContent = new FormUrlEncodedContent(bodyParams);
            return await SendHttpRequestAsync<T>(HttpMethod.Post, endpoint, httpContent: httpContent);
        }

        public async Task<T> SendHttpRequestAsync<T>(HttpMethod httpMethod, string endpoint, string accessToken = null, Dictionary<string, string> queryParams = null, HttpContent httpContent = null)
        {
            var url = queryParams != null
                ? QueryHelpers.AddQueryString(endpoint, queryParams)
                : endpoint;

            var request = new HttpRequestMessage(httpMethod, url);

            if (accessToken != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            if (httpContent != null)
            {
                request.Content = httpContent;
            }

            using var httpClient = new HttpClient();
            using var response = await httpClient.SendAsync(request);

            var resultJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(resultJson);
            }

            var result = JsonConvert.DeserializeObject<T>(resultJson);
            return result;
        }

 
    }
}
