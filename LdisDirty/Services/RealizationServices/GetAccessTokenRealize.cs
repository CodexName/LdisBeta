using LdisDirty.Services.GoogleOAuthServices;

namespace LdisDirty.Services.RealizationServices
{
    public class GetAccessTokenRealize : IgetAccessTokenService
    {
        private IRequestSendService _SendRequest;
        public GetAccessTokenRealize()
        {
            _SendRequest = new SendRequest();
        }
        private const string ClientId = "16188761563-v6v8qr4n6t8at58tpl3cadniu43pgsaf.apps.googleusercontent.com";
        private const string ClientSecret = "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";
        public async Task<AccessTokenResult> ExchangeOnAccessTokenAsync(string codeAuthorization, string codeVerifier, string Redirecturl)
        {
            var urlServerauth = "https://oauth2.googleapis.com/token";
            var requestParametrs = new Dictionary<string, string>
            {
               { "client_id", ClientId },
                { "client_secret", ClientSecret },
                { "code", codeAuthorization },
                { "code_verifier", codeVerifier },
                { "grant_type", "authorization_code" },
                { "redirect_uri", Redirecturl }
            };
            var AccessToken = await _SendRequest.SendPostRequestAsync<AccessTokenResult>(urlServerauth,requestParametrs);
            return AccessToken;
        }
    }
}