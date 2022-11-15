using Microsoft.EntityFrameworkCore;
using Signals.App.Database.Entities;

namespace Signals.App.Database
{
    public class SignalsContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ChannelEntity> Channels { get; set; }

        public SignalsContext(DbContextOptions<SignalsContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChannelEntity>()
                .HasOne<UserEntity>()
                .WithMany()
                .HasForeignKey(c => c.UserId);
        }
    }
}
