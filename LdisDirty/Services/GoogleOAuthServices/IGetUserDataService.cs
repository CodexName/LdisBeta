namespace LdisDirty.Services.GoogleOAuthServices
{
    public interface IGetUserDataService
    {
        Task<Dictionary<string, string>> GetUserData(string accesToken);
    }
}
