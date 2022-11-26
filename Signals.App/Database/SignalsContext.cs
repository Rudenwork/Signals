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
        public DbSet<StageExecution> StageExecutions { get; set; }

        public SignalsContext(DbContextOptions<SignalsContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Channel Entity
            modelBuilder.Entity<ChannelEntity>()
                .HasOne<UserEntity>()
                .WithMany()
                .HasForeignKey(c => c.UserId);

            //Signal Entity
            modelBuilder.Entity<SignalEntity>()
                .HasOne<UserEntity>()
                .WithMany()
                .HasForeignKey(x => x.UserId);

            //Stage Entity
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

            //Notification Stage Entity
            modelBuilder.Entity<NotificationStageEntity>()
                .ToTable("NotificationStages");

            //Condition Stage Entity
            modelBuilder.Entity<ConditionStageEntity>()
                .ToTable("ConditionStages");

            //Waiting Stage Entity
            modelBuilder.Entity<WaitingStageEntity>()
                .ToTable("WaitingStages");
        }
    }
}
