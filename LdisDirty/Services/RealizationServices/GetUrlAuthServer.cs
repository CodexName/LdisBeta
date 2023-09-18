using LdisDirty.Services.GoogleOAuthServices;
using Microsoft.AspNetCore.WebUtilities;

namespace LdisDirty.Services.RealizationServices
{
    public class GetUrlAuthServer : IGetAuthServerUrlService
    {

        private const string ClientId = "16188761563-v6v8qr4n6t8at58tpl3cadniu43pgsaf.apps.googleusercontent.com";
        private const string ClientSecret = "GGOCSPX-7WDAQEsRfE9Wr3NNbP3yZXtm8TRE";
        public string GeneratedUrlAuthServer(string scope, string redirectUrl, string codeChallenge)
        {
            var endpointServerUrl = "https://accounts.google.com/o/oauth2/v2/auth";
            var requestParams = new Dictionary<string, string>
            {
                {"client_id", ClientId},
                { "redirect_uri", redirectUrl },
                { "response_type", "code" },
                { "scope", scope },
                { "code_challenge", codeChallenge },
                { "code_challenge_method", "S256" },
                { "access_type", "offline" }
            };
            var url = QueryHelpers.AddQueryString(endpointServerUrl,requestParams);
            return url;
        }
    }
}
