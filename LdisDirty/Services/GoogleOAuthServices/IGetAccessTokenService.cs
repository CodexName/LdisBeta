namespace LdisDirty.Services.GoogleOAuthServices
{
    public interface IgetAccessTokenService
    {
        Task<AccessTokenResult> ExchangeOnAccessTokenAsync(string codeAuthorization,string codeVerifier,string Redirecturl);
    }
}
