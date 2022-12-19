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

        //Stages
        public DbSet<StageEntity> Stages { get; set; }
        public DbSet<WaitingStageEntity> WaitingStages { get; set; }
        public DbSet<ConditionStageEntity> ConditionStages { get; set; }
        public DbSet<NotificationStageEntity> NotificationStages { get; set; }

        //Blocks
        public DbSet<BlockEntity> Blocks { get; set; }
        public DbSet<GroupBlockEntity> GroupBlocks { get; set; }
        public DbSet<ValueBlockEntity> ValueBlocks { get; set; }
        public DbSet<ChangeBlockEntity> ChangeBlocks { get; set; }

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

            //Block
            modelBuilder.Entity<BlockEntity>()
                .HasOne<StageEntity>()
                .WithMany()
                .HasForeignKey(x => x.StageId);

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
