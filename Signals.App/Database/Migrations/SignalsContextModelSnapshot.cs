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

                    b.Property<Guid>("StageId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("StageId");

                    b.ToTable("Blocks");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("Signals.App.Database.Entities.ChannelEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Destination")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsVerified")
                        .HasColumnType("bit");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(25)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Channels");
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

                    b.Property<int>("Interval")
                        .HasColumnType("int");

                    b.Property<int>("LoopbackPeriod")
                        .HasMaxLength(100)
                        .HasColumnType("int");

                    b.Property<string>("Symbol")
                        .IsRequired()
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
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Schedule")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

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
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

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
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

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

                    b.Property<int>("Operator")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("Period")
                        .HasColumnType("time");

                    b.Property<decimal>("Target")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasIndex("IndicatorId")
                        .IsUnique()
                        .HasFilter("[IndicatorId] IS NOT NULL");

                    b.ToTable("Blocks-Change", (string)null);
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Blocks.GroupBlockEntity", b =>
                {
                    b.HasBaseType("Signals.App.Database.Entities.BlockEntity");

                    b.Property<int>("GroupType")
                        .HasColumnType("int");

                    b.ToTable("Blocks-Group", (string)null);
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Blocks.ValueBlockEntity", b =>
                {
                    b.HasBaseType("Signals.App.Database.Entities.BlockEntity");

                    b.Property<Guid>("LeftIndicatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Operator")
                        .HasColumnType("int");

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

            modelBuilder.Entity("Signals.App.Database.Entities.Indicators.BollingerBandsIndicatorEntity", b =>
                {
                    b.HasBaseType("Signals.App.Database.Entities.IndicatorEntity");

                    b.Property<int>("BandType")
                        .HasColumnType("int");

                    b.ToTable("Indicators-BollingerBands", (string)null);
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Indicators.CandleIndicatorEntity", b =>
                {
                    b.HasBaseType("Signals.App.Database.Entities.IndicatorEntity");

                    b.Property<int>("ParameterType")
                        .HasColumnType("int");

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

            modelBuilder.Entity("Signals.App.Database.Entities.Indicators.MovingAverageIndicatorEntity", b =>
                {
                    b.HasBaseType("Signals.App.Database.Entities.IndicatorEntity");

                    b.ToTable("Indicators-MovingAverage", (string)null);
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Indicators.RelativeStrengthIndexIndicatorEntity", b =>
                {
                    b.HasBaseType("Signals.App.Database.Entities.IndicatorEntity");

                    b.ToTable("Indicators-RelativeStrengthIndex", (string)null);
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Stages.ConditionStageEntity", b =>
                {
                    b.HasBaseType("Signals.App.Database.Entities.StageEntity");

                    b.Property<int?>("RetryCount")
                        .HasColumnType("int");

                    b.Property<TimeSpan?>("RetryDelay")
                        .HasColumnType("time");

                    b.ToTable("Stages-Condition", (string)null);
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Stages.NotificationStageEntity", b =>
                {
                    b.HasBaseType("Signals.App.Database.Entities.StageEntity");

                    b.Property<Guid>("ChannelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

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
                    b.HasOne("Signals.App.Database.Entities.StageEntity", null)
                        .WithMany()
                        .HasForeignKey("StageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
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
                        .WithMany()
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

                    b.HasOne("Signals.App.Database.Entities.IndicatorEntity", null)
                        .WithOne()
                        .HasForeignKey("Signals.App.Database.Entities.Blocks.ChangeBlockEntity", "IndicatorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
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

                    b.HasOne("Signals.App.Database.Entities.IndicatorEntity", null)
                        .WithOne()
                        .HasForeignKey("Signals.App.Database.Entities.Blocks.ValueBlockEntity", "LeftIndicatorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Signals.App.Database.Entities.IndicatorEntity", null)
                        .WithOne()
                        .HasForeignKey("Signals.App.Database.Entities.Blocks.ValueBlockEntity", "RightIndicatorId")
                        .OnDelete(DeleteBehavior.NoAction)
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

            modelBuilder.Entity("Signals.App.Database.Entities.Indicators.MovingAverageIndicatorEntity", b =>
                {
                    b.HasOne("Signals.App.Database.Entities.IndicatorEntity", null)
                        .WithOne()
                        .HasForeignKey("Signals.App.Database.Entities.Indicators.MovingAverageIndicatorEntity", "Id")
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

            modelBuilder.Entity("Signals.App.Database.Entities.Stages.ConditionStageEntity", b =>
                {
                    b.HasOne("Signals.App.Database.Entities.StageEntity", null)
                        .WithOne()
                        .HasForeignKey("Signals.App.Database.Entities.Stages.ConditionStageEntity", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Stages.NotificationStageEntity", b =>
                {
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
#pragma warning restore 612, 618
        }
    }
}
