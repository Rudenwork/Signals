using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;
using Signals.App.Database.Entities;
using Signals.App.Database.Entities.Blocks;
using Signals.App.Database.Entities.Indicators;
using Signals.App.Database.Entities.Stages;

namespace Signals.App.Database
{
    public class SignalsContext : DbContextWithTriggers
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ChannelEntity> Channels { get; set; }
        public DbSet<SignalEntity> Signals { get; set; }
        public DbSet<ExecutionEntity> Executions { get; set; }
        public DbSet<StageEntity> Stages { get; set; }
        public DbSet<BlockEntity> Blocks { get; set; }
        public DbSet<IndicatorEntity> Indicators { get; set; }

        static SignalsContext()
        {
            Triggers<UserEntity>.Deleting += entry =>
            {
                var user = entry.Entity;
                var context = entry.Context as SignalsContext;

                var signals = context.Signals
                    .Where(x => x.UserId == user.Id)
                    .ToList();

                var channels = context.Channels
                    .Where(x => x.UserId == user.Id)
                    .ToList();

                context.Signals.RemoveRange(signals);
                context.Channels.RemoveRange(channels);
            };

            Triggers<SignalEntity>.Deleting += entry =>
            {
                var signal = entry.Entity;
                var context = entry.Context as SignalsContext;

                var execution = context.Executions.FirstOrDefault(x => x.SignalId == signal.Id);

                var stages = context.Stages
                    .Where(x => x.SignalId == signal.Id)
                    .ToList();

                if (execution is not null)
                {
                    context.Executions.Remove(execution);
                }

                context.Stages.RemoveRange(stages);
            };

            Triggers<ConditionStageEntity>.Deleting += entry =>
            {
                var stage = entry.Entity;
                var context = entry.Context as SignalsContext;

                var block = context.Blocks.Find(stage.BlockId);

                context.Blocks.Remove(block);
            };

            Triggers<GroupBlockEntity>.Deleting += entry =>
            {
                var block = entry.Entity;
                var context = entry.Context as SignalsContext;

                var children = context.Blocks
                    .Where(x => x.ParentBlockId == block.Id)
                    .ToList();

                context.Blocks.RemoveRange(children);
            };

            Triggers<ChangeBlockEntity>.Deleting += entry =>
            {
                var block = entry.Entity;
                var context = entry.Context as SignalsContext;

                var indicator = context.Indicators.Find(block.IndicatorId);

                context.Indicators.Remove(indicator);
            };

            Triggers<ValueBlockEntity>.Deleting += entry =>
            {
                var block = entry.Entity;
                var context = entry.Context as SignalsContext;

                var leftIndicator = context.Indicators.Find(block.LeftIndicatorId);
                var rightIndicator = context.Indicators.Find(block.RightIndicatorId);

                context.Indicators.Remove(leftIndicator);
                context.Indicators.Remove(rightIndicator);
            };
        }

        public SignalsContext(DbContextOptions<SignalsContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Channel
            modelBuilder.Entity<ChannelEntity>()
                .HasOne<UserEntity>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            //Signal
            modelBuilder.Entity<SignalEntity>()
                .HasOne<UserEntity>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            //Execution
            modelBuilder.Entity<ExecutionEntity>()
                .HasOne<SignalEntity>()
                .WithOne(x => x.Execution)
                .HasForeignKey<ExecutionEntity>(x => x.SignalId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ExecutionEntity>()
                .HasOne(x => x.Stage)
                .WithOne()
                .HasForeignKey<ExecutionEntity>(x => x.StageId)
                .OnDelete(DeleteBehavior.NoAction);

            //Stage
            modelBuilder.Entity<StageEntity>()
                .HasOne<SignalEntity>()
                .WithMany(x => x.Stages)
                .HasForeignKey(x => x.SignalId)
                .OnDelete(DeleteBehavior.NoAction);

            //Condition Stage
            modelBuilder.Entity<ConditionStageEntity>()
                .ToTable($"{nameof(Stages)}-Condition");

            modelBuilder.Entity<ConditionStageEntity>()
                .HasOne(x => x.Block)
                .WithOne()
                .HasForeignKey<ConditionStageEntity>(x => x.BlockId)
                .OnDelete(DeleteBehavior.NoAction);

            //Waiting Stage
            modelBuilder.Entity<WaitingStageEntity>()
                .ToTable($"{nameof(Stages)}-Waiting");

            //Notification Stage
            modelBuilder.Entity<NotificationStageEntity>()
                .ToTable($"{nameof(Stages)}-Notification");

            modelBuilder.Entity<NotificationStageEntity>()
                .HasOne<ChannelEntity>()
                .WithMany()
                .HasForeignKey(x => x.ChannelId)
                .OnDelete(DeleteBehavior.NoAction);

            //Block
            modelBuilder.Entity<BlockEntity>()
                .HasOne<GroupBlockEntity>()
                .WithMany(x => x.Children)
                .HasForeignKey(x => x.ParentBlockId)
                .OnDelete(DeleteBehavior.NoAction);

            //Group Block
            modelBuilder.Entity<GroupBlockEntity>()
                .ToTable($"{nameof(Blocks)}-Group");

            //Value Block
            modelBuilder.Entity<ValueBlockEntity>()
                .ToTable($"{nameof(Blocks)}-Value");

            modelBuilder.Entity<ValueBlockEntity>()
                .HasOne(x => x.LeftIndicator)
                .WithOne()
                .HasForeignKey<ValueBlockEntity>(x => x.LeftIndicatorId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ValueBlockEntity>()
                .HasOne(x => x.RightIndicator)
                .WithOne()
                .HasForeignKey<ValueBlockEntity>(x => x.RightIndicatorId)
                .OnDelete(DeleteBehavior.NoAction);

            //Change Block
            modelBuilder.Entity<ChangeBlockEntity>()
                .ToTable($"{nameof(Blocks)}-Change");

            modelBuilder.Entity<ChangeBlockEntity>()
                .HasOne(x => x.Indicator)
                .WithOne()
                .HasForeignKey<ChangeBlockEntity>(x => x.IndicatorId)
                .OnDelete(DeleteBehavior.NoAction);

            //Bollinger Bands Indicator
            modelBuilder.Entity<BollingerBandsIndicatorEntity>()
                .ToTable($"{nameof(Indicators)}-BollingerBands");

            //Candle Indicator
            modelBuilder.Entity<CandleIndicatorEntity>()
                .ToTable($"{nameof(Indicators)}-Candle");

            //Constant Indicator
            modelBuilder.Entity<ConstantIndicatorEntity>()
                .ToTable($"{nameof(Indicators)}-Constant");

            //Exponential Moving Average Indicator
            modelBuilder.Entity<ExponentialMovingAverageIndicatorEntity>()
                .ToTable($"{nameof(Indicators)}-ExponentialMovingAverage");

            //Moving Average Indicator
            modelBuilder.Entity<SimpleMovingAverageIndicatorEntity>()
                .ToTable($"{nameof(Indicators)}-MovingAverage");

            //Relative Strength Index Indicator
            modelBuilder.Entity<RelativeStrengthIndexIndicatorEntity>()
                .ToTable($"{nameof(Indicators)}-RelativeStrengthIndex");
        }
    }
}
