using LdisDirty.DataBaseContext;
using LdisDirty.Services.GoogleOAuthServices;
using LdisDirty.Services.RealizationServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace LdisDirty.GoogleOauthHandler
{
    public class GoogleOauth : Controller
    {
        private IS256CoderService _S256;
        private IGetAuthServerUrlService _GetUrl;
        private IgetAccessTokenService _GetToken;
        private IGetUserDataService _GetUserData;
        private IHttpContextAccessor _HttpAccsess;
        private string EmailUser;
        private string ImageAvatarLink;
        private DbContextApplication _Context;
        public GoogleOauth(DbContextApplication context)
        {
            _Context = context;
            _S256 = new S256Realize();
            _GetToken = new GetAccessTokenRealize();
            _GetUrl = new GetUrlAuthServer();
            _GetUserData = new GetUserDataRealize();
        }
        private const string Scope = "https://www.googleapis.com/auth/userinfo.email";
        private const string RedirectUrl = "https://localhost:7005/GoogleOauth/GetAccessTokenHandler";
        public IActionResult RedirectToOauthServer()
        {
            string codeVerifier = Guid.NewGuid().ToString();
            var codeChallenge = _S256.ComputeHash(codeVerifier);
            HttpContext.Session.SetString("Verifier", codeVerifier);
            string url = _GetUrl.GeneratedUrlAuthServer(Scope,RedirectUrl,codeChallenge);
            return Redirect(url);
        }
        private void CookieUserAdd(string? jsonFile)
        {
            HttpContext.Response.Cookies.Append("userkey",jsonFile);
        }
        public async Task<IActionResult> GetAccessTokenHandler(string code)
        {
            string codeVerifierSession = HttpContext.Session.GetString("Verifier");
            var TokenResult = await _GetToken.ExchangeOnAccessTokenAsync(code,codeVerifierSession,RedirectUrl);
            string AccessToken = TokenResult.AccessToken;
            var User = await _GetUserData.GetUserData(AccessToken);
            foreach (var item in User)
            {
                if (item.Key == "email")
                {
                    EmailUser = item.Value;
                }
                if (item.Key == "picture")
                {
                    ImageAvatarLink = item.Value;
                }
            }

            var UserFind = _Context.Users.AsNoTracking().FirstOrDefault(x => x.Email == EmailUser);
            if (UserFind != null)
            {
                /*Устанавливаем юзера в куки для минимизации запросов в бд */
                var JsonSerializeUserData = JsonSerializer.Serialize(UserFind);
                CookieUserAdd(JsonSerializeUserData);

                var claims = new List<Claim>
                {
                   new Claim(ClaimTypes.Email , EmailUser)
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(claimsPrincipal);

            }
            else
            {
                return View();
            }
            return Ok();
        }
     
    }
}
