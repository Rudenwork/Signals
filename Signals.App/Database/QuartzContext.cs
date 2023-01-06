using Microsoft.EntityFrameworkCore;

namespace Signals.App.Database
{
    public class QuartzContext : DbContext
    {
        public QuartzContext(DbContextOptions<QuartzContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { }
    }
}
