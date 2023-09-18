namespace LdisDirty.Services.GoogleOAuthServices
{
    public interface IS256CoderService
    {
        string ComputeHash(string codeverifier);
    }
}
