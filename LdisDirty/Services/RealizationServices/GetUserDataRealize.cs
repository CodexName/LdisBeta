using LdisDirty.Services.GoogleOAuthServices;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace LdisDirty.Services.RealizationServices
{
    public class GetUserDataRealize : IGetUserDataService
    {
        public async Task<Dictionary<string, string>> GetUserData(string accesToken)
        {
            string googleServerUri = "https://www.googleapis.com/oauth2/v2/userinfo";
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",accesToken);
                HttpResponseMessage responce = await httpClient.GetAsync(googleServerUri);
                if (responce.IsSuccessStatusCode)
                {
                    var responseContent = await responce.Content.ReadAsStringAsync();
                    var userinfo = JsonConvert.DeserializeObject<Dictionary<string,string>>(responseContent);
                    return userinfo;
                }
                else
                {
                    throw new Exception("Error");
                }
            }
        }
    }
}
