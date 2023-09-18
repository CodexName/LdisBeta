using IdentityModel;
using LdisDirty.Services.GoogleOAuthServices;
using System.Security.Cryptography;
using System.Text;

namespace LdisDirty.Services.RealizationServices
{
    public class S256Realize : IS256CoderService
    {
        public string ComputeHash(string codeverifier)
        {
            using var sha256 = SHA256.Create();
            var challengeBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeverifier));
            var codeChallenge = Base64Url.Encode(challengeBytes);
            return codeChallenge;
        }
    }
}
