using LdisDirty.DataBaseContext;
using LdisDirty.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace LdisDirty.SignalREngine
{
    public class ChatsHandler : Hub
    {
        private IHttpContextAccessor _httpContextAccess;
        private DbContextApplication _Context;
        public ChatsHandler(IHttpContextAccessor contexthttp,DbContextApplication context)
        {
            _httpContextAccess = contexthttp;
            _Context = context;
        }
        public async Task Send(string message,string group,string name)
        {
            var IdGroup = _Context.Chats.AsNoTracking().FirstOrDefault(x => x.Name == group);

            DateTime timesendMessage = DateTime.Now;
            var messageInstance = new Message
            { 
                Text = message,
                DateMessage = timesendMessage,
                ForeignChatId = 
            };

            Clients.Group(group).SendAsync("receive",message,name);
        }
        public async Task Enter(string username,string groupname)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupname);
            await Clients.All.SendAsync("EnterInGroupNotify", $"{username} вступил в группу");
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
