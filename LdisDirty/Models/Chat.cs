namespace LdisDirty.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int QuantityUser {get;set;} 
        public ICollection<User> Users { get; set; }
    }
}
