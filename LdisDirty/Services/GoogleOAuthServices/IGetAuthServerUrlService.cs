namespace LdisDirty.Services.GoogleOAuthServices
{
    public interface IGetAuthServerUrlService
    {
       string GeneratedUrlAuthServer(string scope, string redirectUrl, string codeChallenge);

    }
}
