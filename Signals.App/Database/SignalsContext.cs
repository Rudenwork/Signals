using Microsoft.EntityFrameworkCore;
using Signals.App.Database.Entities;

namespace Signals.App.Database
{
    public class SignalsContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }

        public SignalsContext(DbContextOptions<SignalsContext> options) : base(options) { }
    }
}
