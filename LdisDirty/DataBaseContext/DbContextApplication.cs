using LdisDirty.Models;
using Microsoft.EntityFrameworkCore;

namespace LdisDirty.DataBaseContext
{
    public class DbContextApplication : DbContext
    {
        public DbContextApplication(DbContextOptions<DbContextApplication> options) : base (options)
        {
            Database.Migrate();
        }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Ldis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chat>().HasOne(x => x.Messages).WithOne(x => x.Chats).HasForeignKey<Message>(x => x.ForeignChatId);
            modelBuilder.Entity<Message>().HasOne(x => x.Chats).WithOne(x => x.Messages);
        }
    }
}
