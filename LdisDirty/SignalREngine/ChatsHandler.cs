using LdisDirty.DataBaseContext;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Runtime.CompilerServices;

namespace LdisDirty.SignalREngine
{
    public class ChatsHandler : Hub
    {
        private IHttpContextAccessor _httpContextAccess;
        public ChatsHandler(IHttpContextAccessor contexthttp)
        {
            _httpContextAccess = contexthttp;
        }
        private DbContextApplication _Context;
        public ChatsHandler(DbContextApplication context)
        {
            _Context = context;
        }
        public async Task Send(string message)
        {
          
            Clients.Group("d").SendAsync("receive",message);
        }
        public async Task OnConnectedAsync()
        {
            var User = 
        }
    }
}
