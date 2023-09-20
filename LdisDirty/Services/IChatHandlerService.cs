using Microsoft.Identity.Client;

namespace LdisDirty.Services
{
    public interface IChatHandlerService
    {
        Task Send(string message, string group, string name);
        Task Enter(string username, string groupname);
    }
}
