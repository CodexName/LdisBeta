using LdisDirty.DataBaseContext;
using LdisDirty.Models;
using LdisDirty.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace LdisDirty.SignalREngine
{
    public class ChatHandlerRealize : Hub, IChatHandlerService
    {
        private IHttpContextAccessor _httpContextAccess;
        private DbContextApplication _Context;
        private IMemoryCache _Cache;
        public ChatHandlerRealize(IHttpContextAccessor contexthttp, DbContextApplication context, IMemoryCache cache)
        {
            _httpContextAccess = contexthttp;
            _Context = context;
            _Cache = cache;
        }
        public async Task Enter(string username, string groupname)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupname);
            await Clients.All.SendAsync("EnterInGroupNotify", $"{username} вступил в группу");
        }

        public async Task Send(string message, string group, string name)
        {
            int Id = (int)_Cache.Get("GroupKeyId");
            Chat Group = null;
            if (Id == null)
            {
                Group = _Context.Chats.AsNoTracking().FirstOrDefault(x => x.Name == group);
                int IdGroup = Group.Id;
                _Cache.Set("GroupKeyId", IdGroup, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(4)));
                Send(message, group, name);
            }
            DateTime timesendMessage = DateTime.Now;
            var messageInstance = new Message
            {
                Text = message,
                DateMessage = timesendMessage,
                ForeignChatId = Id
            };
            _Context.Add(messageInstance);
            _Context.SaveChanges();
            Clients.Group(group).SendAsync("receive", message, name);
        }
        public override async Task OnConnectedAsync()
        {
            var userData = _httpContextAccess.HttpContext.Request.Cookies["userkey"];
            var UserJsonDeserealize = JsonSerializer.Deserialize<User>(userData);
            var User = _Context.Users.AsNoTracking().FirstOrDefault(x => x.Email == UserJsonDeserealize.Email);
            User.Status = "Online";
            _Context.SaveChanges();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userData = _httpContextAccess.HttpContext.Request.Cookies["userkey"];
            var UserJsonDeserealize = JsonSerializer.Deserialize<User>(userData);
            var User = _Context.Users.AsNoTracking().FirstOrDefault(x => x.Email == UserJsonDeserealize.Email);
            User.Status = "Offline";
            _Context.SaveChanges();
        }
    }
}
