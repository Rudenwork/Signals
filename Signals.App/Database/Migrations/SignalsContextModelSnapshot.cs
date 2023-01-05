﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Signals.App.Database;

#nullable disable

namespace Signals.App.Database.Migrations
{
    [DbContext(typeof(SignalsContext))]
    partial class SignalsContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Signals.App.Database.Entities.BlockEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ParentBlockId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ParentBlockId");

                    b.ToTable("Blocks");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("Signals.App.Database.Entities.ChannelEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsVerified")
                        .HasColumnType("bit");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Channels");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("Signals.App.Database.Entities.ExecutionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SignalId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("StageId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SignalId")
                        .IsUnique();

                    b.HasIndex("StageId")
                        .IsUnique()
                        .HasFilter("[StageId] IS NOT NULL");

                    b.ToTable("Executions");
                });

            modelBuilder.Entity("Signals.App.Database.Entities.IndicatorEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Interval")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LoopbackPeriod")
                        .HasColumnType("int");

                    b.Property<string>("Symbol")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Indicators");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("Signals.App.Database.Entities.SignalEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDisabled")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Schedule")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Signals");
                });

            modelBuilder.Entity("Signals.App.Database.Entities.StageEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("NextStageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("PreviousStageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SignalId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SignalId");

                    b.ToTable("Stages");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("Signals.App.Database.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDisabled")
                        .HasColumnType("bit");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Blocks.ChangeBlockEntity", b =>
                {
                    b.HasBaseType("Signals.App.Database.Entities.BlockEntity");

                    b.Property<Guid>("IndicatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsPercentage")
                        .HasColumnType("bit");

                    b.Property<string>("Operator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("Period")
                        .HasColumnType("time");

                    b.Property<decimal>("Target")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasIndex("IndicatorId")
                        .IsUnique()
                        .HasFilter("[IndicatorId] IS NOT NULL");

                    b.ToTable("Blocks-Change", (string)null);
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Blocks.GroupBlockEntity", b =>
                {
                    b.HasBaseType("Signals.App.Database.Entities.BlockEntity");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Blocks-Group", (string)null);
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Blocks.ValueBlockEntity", b =>
                {
                    b.HasBaseType("Signals.App.Database.Entities.BlockEntity");

                    b.Property<Guid>("LeftIndicatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Operator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RightIndicatorId")
                        .HasColumnType("uniqueidentifier");

                    b.HasIndex("LeftIndicatorId")
                        .IsUnique()
                        .HasFilter("[LeftIndicatorId] IS NOT NULL");

                    b.HasIndex("RightIndicatorId")
                        .IsUnique()
                        .HasFilter("[RightIndicatorId] IS NOT NULL");

                    b.ToTable("Blocks-Value", (string)null);
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Channels.EmailChannelEntity", b =>
                {
                    b.HasBaseType("Signals.App.Database.Entities.ChannelEntity");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Channels-Email", (string)null);
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Channels.TelegramChannelEntity", b =>
                {
                    b.HasBaseType("Signals.App.Database.Entities.ChannelEntity");

                    b.Property<long?>("ChatId")
                        .HasColumnType("bigint");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Channels-Telegram", (string)null);
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Indicators.BollingerBandsIndicatorEntity", b =>
                {
                    b.HasBaseType("Signals.App.Database.Entities.IndicatorEntity");

                    b.Property<string>("BandType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Indicators-BollingerBands", (string)null);
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Indicators.CandleIndicatorEntity", b =>
                {
                    b.HasBaseType("Signals.App.Database.Entities.IndicatorEntity");

                    b.Property<string>("ParameterType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Indicators-Candle", (string)null);
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Indicators.ConstantIndicatorEntity", b =>
                {
                    b.HasBaseType("Signals.App.Database.Entities.IndicatorEntity");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(18,2)");

                    b.ToTable("Indicators-Constant", (string)null);
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Indicators.ExponentialMovingAverageIndicatorEntity", b =>
                {
                    b.HasBaseType("Signals.App.Database.Entities.IndicatorEntity");

                    b.ToTable("Indicators-ExponentialMovingAverage", (string)null);
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Indicators.RelativeStrengthIndexIndicatorEntity", b =>
                {
                    b.HasBaseType("Signals.App.Database.Entities.IndicatorEntity");

                    b.ToTable("Indicators-RelativeStrengthIndex", (string)null);
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Indicators.SimpleMovingAverageIndicatorEntity", b =>
                {
                    b.HasBaseType("Signals.App.Database.Entities.IndicatorEntity");

                    b.ToTable("Indicators-MovingAverage", (string)null);
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Stages.ConditionStageEntity", b =>
                {
                    b.HasBaseType("Signals.App.Database.Entities.StageEntity");

                    b.Property<Guid>("BlockId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("RetryCount")
                        .HasColumnType("int");

                    b.Property<TimeSpan?>("RetryDelay")
                        .HasColumnType("time");

                    b.HasIndex("BlockId")
                        .IsUnique()
                        .HasFilter("[BlockId] IS NOT NULL");

                    b.ToTable("Stages-Condition", (string)null);
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Stages.NotificationStageEntity", b =>
                {
                    b.HasBaseType("Signals.App.Database.Entities.StageEntity");

                    b.Property<Guid>("ChannelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasIndex("ChannelId");

                    b.ToTable("Stages-Notification", (string)null);
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Stages.WaitingStageEntity", b =>
                {
                    b.HasBaseType("Signals.App.Database.Entities.StageEntity");

                    b.Property<TimeSpan>("Period")
                        .HasColumnType("time");

                    b.ToTable("Stages-Waiting", (string)null);
                });

            modelBuilder.Entity("Signals.App.Database.Entities.BlockEntity", b =>
                {
                    b.HasOne("Signals.App.Database.Entities.Blocks.GroupBlockEntity", null)
                        .WithMany("Children")
                        .HasForeignKey("ParentBlockId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Signals.App.Database.Entities.ChannelEntity", b =>
                {
                    b.HasOne("Signals.App.Database.Entities.UserEntity", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Signals.App.Database.Entities.ExecutionEntity", b =>
                {
                    b.HasOne("Signals.App.Database.Entities.SignalEntity", null)
                        .WithOne()
                        .HasForeignKey("Signals.App.Database.Entities.ExecutionEntity", "SignalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Signals.App.Database.Entities.StageEntity", null)
                        .WithOne()
                        .HasForeignKey("Signals.App.Database.Entities.ExecutionEntity", "StageId")
                        .OnDelete(DeleteBehavior.NoAction);
                });

            modelBuilder.Entity("Signals.App.Database.Entities.SignalEntity", b =>
                {
                    b.HasOne("Signals.App.Database.Entities.UserEntity", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Signals.App.Database.Entities.StageEntity", b =>
                {
                    b.HasOne("Signals.App.Database.Entities.SignalEntity", null)
                        .WithMany("Stages")
                        .HasForeignKey("SignalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Blocks.ChangeBlockEntity", b =>
                {
                    b.HasOne("Signals.App.Database.Entities.BlockEntity", null)
                        .WithOne()
                        .HasForeignKey("Signals.App.Database.Entities.Blocks.ChangeBlockEntity", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Signals.App.Database.Entities.IndicatorEntity", "Indicator")
                        .WithOne()
                        .HasForeignKey("Signals.App.Database.Entities.Blocks.ChangeBlockEntity", "IndicatorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Indicator");
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Blocks.GroupBlockEntity", b =>
                {
                    b.HasOne("Signals.App.Database.Entities.BlockEntity", null)
                        .WithOne()
                        .HasForeignKey("Signals.App.Database.Entities.Blocks.GroupBlockEntity", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Blocks.ValueBlockEntity", b =>
                {
                    b.HasOne("Signals.App.Database.Entities.BlockEntity", null)
                        .WithOne()
                        .HasForeignKey("Signals.App.Database.Entities.Blocks.ValueBlockEntity", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Signals.App.Database.Entities.IndicatorEntity", "LeftIndicator")
                        .WithOne()
                        .HasForeignKey("Signals.App.Database.Entities.Blocks.ValueBlockEntity", "LeftIndicatorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Signals.App.Database.Entities.IndicatorEntity", "RightIndicator")
                        .WithOne()
                        .HasForeignKey("Signals.App.Database.Entities.Blocks.ValueBlockEntity", "RightIndicatorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("LeftIndicator");

                    b.Navigation("RightIndicator");
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Channels.EmailChannelEntity", b =>
                {
                    b.HasOne("Signals.App.Database.Entities.ChannelEntity", null)
                        .WithOne()
                        .HasForeignKey("Signals.App.Database.Entities.Channels.EmailChannelEntity", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Channels.TelegramChannelEntity", b =>
                {
                    b.HasOne("Signals.App.Database.Entities.ChannelEntity", null)
                        .WithOne()
                        .HasForeignKey("Signals.App.Database.Entities.Channels.TelegramChannelEntity", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Indicators.BollingerBandsIndicatorEntity", b =>
                {
                    b.HasOne("Signals.App.Database.Entities.IndicatorEntity", null)
                        .WithOne()
                        .HasForeignKey("Signals.App.Database.Entities.Indicators.BollingerBandsIndicatorEntity", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Indicators.CandleIndicatorEntity", b =>
                {
                    b.HasOne("Signals.App.Database.Entities.IndicatorEntity", null)
                        .WithOne()
                        .HasForeignKey("Signals.App.Database.Entities.Indicators.CandleIndicatorEntity", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Indicators.ConstantIndicatorEntity", b =>
                {
                    b.HasOne("Signals.App.Database.Entities.IndicatorEntity", null)
                        .WithOne()
                        .HasForeignKey("Signals.App.Database.Entities.Indicators.ConstantIndicatorEntity", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Indicators.ExponentialMovingAverageIndicatorEntity", b =>
                {
                    b.HasOne("Signals.App.Database.Entities.IndicatorEntity", null)
                        .WithOne()
                        .HasForeignKey("Signals.App.Database.Entities.Indicators.ExponentialMovingAverageIndicatorEntity", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Indicators.RelativeStrengthIndexIndicatorEntity", b =>
                {
                    b.HasOne("Signals.App.Database.Entities.IndicatorEntity", null)
                        .WithOne()
                        .HasForeignKey("Signals.App.Database.Entities.Indicators.RelativeStrengthIndexIndicatorEntity", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Indicators.SimpleMovingAverageIndicatorEntity", b =>
                {
                    b.HasOne("Signals.App.Database.Entities.IndicatorEntity", null)
                        .WithOne()
                        .HasForeignKey("Signals.App.Database.Entities.Indicators.SimpleMovingAverageIndicatorEntity", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Stages.ConditionStageEntity", b =>
                {
                    b.HasOne("Signals.App.Database.Entities.BlockEntity", "Block")
                        .WithOne()
                        .HasForeignKey("Signals.App.Database.Entities.Stages.ConditionStageEntity", "BlockId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Signals.App.Database.Entities.StageEntity", null)
                        .WithOne()
                        .HasForeignKey("Signals.App.Database.Entities.Stages.ConditionStageEntity", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Block");
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Stages.NotificationStageEntity", b =>
                {
                    b.HasOne("Signals.App.Database.Entities.ChannelEntity", null)
                        .WithMany()
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Signals.App.Database.Entities.StageEntity", null)
                        .WithOne()
                        .HasForeignKey("Signals.App.Database.Entities.Stages.NotificationStageEntity", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Stages.WaitingStageEntity", b =>
                {
                    b.HasOne("Signals.App.Database.Entities.StageEntity", null)
                        .WithOne()
                        .HasForeignKey("Signals.App.Database.Entities.Stages.WaitingStageEntity", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Signals.App.Database.Entities.SignalEntity", b =>
                {
                    b.Navigation("Stages");
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Blocks.GroupBlockEntity", b =>
                {
                    b.Navigation("Children");
                });
#pragma warning restore 612, 618
        }
    }
}
