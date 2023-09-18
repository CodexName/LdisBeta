namespace LdisDirty.Services.GoogleOAuthServices
{
    public interface IRequestSendService
    {
        Task<T> SendPostRequestAsync<T>(string endpoint, Dictionary<string, string> bodyParams);
        Task<T> SendHttpRequestAsync<T>(HttpMethod httpMethod, string endpoint, string accessToken, Dictionary<string, string> queryParams, HttpContent httpContent);
    }
}
