using Microsoft.Extensions.Primitives;

namespace LdisDirty.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public string ImageLink { get; set; }
        public string Status { get; set; }
        public ICollection<Chat> Chats { get; set; }
    }
}
