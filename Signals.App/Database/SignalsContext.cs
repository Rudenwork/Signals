using Microsoft.EntityFrameworkCore;
using Signals.App.Database.Entities;

namespace Signals.App.Database
{
    public class SignalsContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ChannelEntity> Channels { get; set; }
        public DbSet<SignalEntity> Signals { get; set; }
        public DbSet<StageEntity> Stages { get; set; }
        public DbSet<StageParameterEntity> StageParameters { get; set; }
        public DbSet<BlockEntity> Blocks { get; set; }
        public DbSet<BlockParameterEntity> BlockParameters { get; set; }
        public DbSet<SignalExecutionEntity> SignalExecutions { get; set; }

        public SignalsContext(DbContextOptions<SignalsContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Channel
            modelBuilder.Entity<ChannelEntity>()
                .HasOne<UserEntity>()
                .WithMany()
                .HasForeignKey(x => x.UserId);

            //Signal
            modelBuilder.Entity<SignalEntity>()
                .HasOne<UserEntity>()
                .WithMany()
                .HasForeignKey(x => x.UserId);

            //Stage
            modelBuilder.Entity<StageEntity>()
                .HasOne<SignalEntity>()
                .WithMany()
                .HasForeignKey(x => x.SignalId);

            //Stage Parameter
            modelBuilder.Entity<StageParameterEntity>()
                .HasOne<StageEntity>()
                .WithMany(x => x.Parameters)
                .HasForeignKey(x => x.StageId);

            //Block
            modelBuilder.Entity<BlockEntity>()
                .HasOne<StageEntity>()
                .WithMany()
                .HasForeignKey(x => x.StageId);

            //Block Parameter
            modelBuilder.Entity<BlockParameterEntity>()
                .HasOne<BlockEntity>()
                .WithMany(x => x.Parameters)
                .HasForeignKey(x => x.BlockId);

            //Signal Execution
            modelBuilder.Entity<SignalExecutionEntity>()
                .HasOne<SignalEntity>()
                .WithOne()
                .HasForeignKey<SignalExecutionEntity>(x => x.SignalId);

            modelBuilder.Entity<SignalExecutionEntity>()
                .HasOne<StageEntity>()
                .WithOne()
                .HasForeignKey<SignalExecutionEntity>(x => x.StageId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
