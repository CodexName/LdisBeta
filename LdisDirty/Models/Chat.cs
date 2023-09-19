namespace LdisDirty.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int QuantityUser {get;set;} 
        public int ForeiginMessageId { get; set; }
        public string AvatarChatImageLink { get; set; }
        public Message Messages { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
