namespace LdisDirty.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int QuantityUser {get;set;} 
        public int ForeiginMessageId { get; set; }
        public Message Messages { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
