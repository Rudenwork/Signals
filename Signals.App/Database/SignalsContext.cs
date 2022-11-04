using Microsoft.EntityFrameworkCore;
using Signals.App.Database.Entities;

namespace Signals.App.Database
{
    public class SignalsContext : DbContext
    {
        public SignalsContext(DbContextOptions<SignalsContext> options) : base(options) { }

        public DbSet<TestEntity> Tests { get; set; }
    }
}
