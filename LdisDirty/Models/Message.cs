namespace LdisDirty.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int ForeignChatId { get; set; }
        public string Text { get; set; }
        public DateTime DateMessage { get; set; }
        public Chat Chats { get; set; }
    }
}
