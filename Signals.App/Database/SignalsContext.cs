using Microsoft.EntityFrameworkCore;
using Signals.App.Database.Entities;
using Signals.App.Database.Entities.Blocks;
using Signals.App.Database.Entities.Stages;

namespace Signals.App.Database
{
    public class SignalsContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ChannelEntity> Channels { get; set; }
        public DbSet<SignalEntity> Signals { get; set; }
        public DbSet<StageEntity> Stages { get; set; }
        public DbSet<BlockEntity> Blocks { get; set; }
        public DbSet<ExecutionEntity> Executions { get; set; }

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

            //Condition Stage
            modelBuilder.Entity<ConditionStageEntity>()
                .ToTable($"{nameof(Stages)}-Condition");

            //Waiting Stage
            modelBuilder.Entity<WaitingStageEntity>()
                .ToTable($"{nameof(Stages)}-Waiting");

            //Notification Stage
            modelBuilder.Entity<NotificationStageEntity>()
                .ToTable($"{nameof(Stages)}-Notification");

            //Block
            modelBuilder.Entity<BlockEntity>()
                .HasOne<StageEntity>()
                .WithMany()
                .HasForeignKey(x => x.StageId);

            //Group Block
            modelBuilder.Entity<GroupBlockEntity>()
                .ToTable($"{nameof(Blocks)}-Group");

            //Value Block
            modelBuilder.Entity<ValueBlockEntity>()
                .ToTable($"{nameof(Blocks)}-Value");

            //Change Block
            modelBuilder.Entity<ChangeBlockEntity>()
                .ToTable($"{nameof(Blocks)}-Change");

            //Execution
            modelBuilder.Entity<ExecutionEntity>()
                .HasOne<SignalEntity>()
                .WithOne()
                .HasForeignKey<ExecutionEntity>(x => x.SignalId);

            modelBuilder.Entity<ExecutionEntity>()
                .HasOne<StageEntity>()
                .WithOne()
                .HasForeignKey<ExecutionEntity>(x => x.StageId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
