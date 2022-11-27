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
        public DbSet<StageExecutionEntity> StageExecutions { get; set; }

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

            modelBuilder.Entity<StageEntity>()
                .HasOne<StageEntity>()
                .WithMany()
                .HasForeignKey(x => x.PreviousStageId);

            modelBuilder.Entity<StageEntity>()
                .HasOne<StageEntity>()
                .WithMany()
                .HasForeignKey(x => x.NextStageId);

            //Stage Parameter
            modelBuilder.Entity<StageParameterEntity>()
                .HasOne<StageEntity>()
                .WithMany(x => x.Parameters)
                .HasForeignKey(x => x.StageId);

            //Stage Execution
            modelBuilder.Entity<StageExecutionEntity>()
                .HasOne<StageEntity>()
                .WithOne()
                .HasForeignKey<StageExecutionEntity>(x => x.StageId);

            modelBuilder.Entity<StageExecutionEntity>()
                .HasOne<SignalEntity>()
                .WithOne()
                .HasForeignKey<StageExecutionEntity>(x => x.SignalId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
