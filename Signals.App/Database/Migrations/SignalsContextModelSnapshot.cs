﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
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
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Signals.App.Database.Entities.BlockEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int?>("Index")
                        .HasColumnType("integer");

                    b.Property<Guid?>("ParentBlockId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ParentBlockId");

                    b.ToTable("Blocks");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("Signals.App.Database.Entities.ChannelEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Destination")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long?>("ExternalId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsVerified")
                        .HasColumnType("boolean");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("varchar");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Channels");
                });

            modelBuilder.Entity("Signals.App.Database.Entities.ExecutionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("SignalId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("StageId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("SignalId")
                        .IsUnique();

                    b.HasIndex("StageId")
                        .IsUnique();

                    b.ToTable("Executions");
                });

            modelBuilder.Entity("Signals.App.Database.Entities.IndicatorEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Interval")
                        .HasColumnType("varchar");

                    b.Property<int?>("LoopbackPeriod")
                        .HasColumnType("integer");

                    b.Property<string>("Symbol")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Indicators");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("Signals.App.Database.Entities.SignalEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDisabled")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Schedule")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Signals");
                });

            modelBuilder.Entity("Signals.App.Database.Entities.StageEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Index")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("SignalId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("SignalId");

                    b.ToTable("Stages");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("Signals.App.Database.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDisabled")
                        .HasColumnType("boolean");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Blocks.ChangeBlockEntity", b =>
                {
                    b.HasBaseType("Signals.App.Database.Entities.BlockEntity");

                    b.Property<Guid>("IndicatorId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsPercentage")
                        .HasColumnType("boolean");

                    b.Property<string>("Operator")
                        .IsRequired()
                        .HasColumnType("varchar");

                    b.Property<TimeSpan>("Period")
                        .HasColumnType("interval");

                    b.Property<decimal>("Target")
                        .HasColumnType("numeric");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("varchar");

                    b.HasIndex("IndicatorId")
                        .IsUnique();

                    b.ToTable("Blocks-Change", (string)null);
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Blocks.GroupBlockEntity", b =>
                {
                    b.HasBaseType("Signals.App.Database.Entities.BlockEntity");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("varchar");

                    b.ToTable("Blocks-Group", (string)null);
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Blocks.ValueBlockEntity", b =>
                {
                    b.HasBaseType("Signals.App.Database.Entities.BlockEntity");

                    b.Property<Guid>("LeftIndicatorId")
                        .HasColumnType("uuid");

                    b.Property<string>("Operator")
                        .IsRequired()
                        .HasColumnType("varchar");

                    b.Property<Guid>("RightIndicatorId")
                        .HasColumnType("uuid");

                    b.HasIndex("LeftIndicatorId")
                        .IsUnique();

                    b.HasIndex("RightIndicatorId")
                        .IsUnique();

                    b.ToTable("Blocks-Value", (string)null);
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Indicators.BollingerBandsIndicatorEntity", b =>
                {
                    b.HasBaseType("Signals.App.Database.Entities.IndicatorEntity");

                    b.Property<string>("BandType")
                        .IsRequired()
                        .HasColumnType("varchar");

                    b.ToTable("Indicators-BollingerBands", (string)null);
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Indicators.CandleIndicatorEntity", b =>
                {
                    b.HasBaseType("Signals.App.Database.Entities.IndicatorEntity");

                    b.Property<string>("ParameterType")
                        .IsRequired()
                        .HasColumnType("varchar");

                    b.ToTable("Indicators-Candle", (string)null);
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Indicators.ConstantIndicatorEntity", b =>
                {
                    b.HasBaseType("Signals.App.Database.Entities.IndicatorEntity");

                    b.Property<decimal>("Value")
                        .HasColumnType("numeric");

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
                        .HasColumnType("uuid");

                    b.Property<int?>("RetryCount")
                        .HasColumnType("integer");

                    b.Property<TimeSpan?>("RetryDelay")
                        .HasColumnType("interval");

                    b.HasIndex("BlockId")
                        .IsUnique();

                    b.ToTable("Stages-Condition", (string)null);
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Stages.NotificationStageEntity", b =>
                {
                    b.HasBaseType("Signals.App.Database.Entities.StageEntity");

                    b.Property<Guid>("ChannelId")
                        .HasColumnType("uuid");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasIndex("ChannelId");

                    b.ToTable("Stages-Notification", (string)null);
                });

            modelBuilder.Entity("Signals.App.Database.Entities.Stages.WaitingStageEntity", b =>
                {
                    b.HasBaseType("Signals.App.Database.Entities.StageEntity");

                    b.Property<TimeSpan>("Period")
                        .HasColumnType("interval");

                    b.ToTable("Stages-Waiting", (string)null);
                });

            modelBuilder.Entity("Signals.App.Database.Entities.BlockEntity", b =>
                {
                    b.HasOne("Signals.App.Database.Entities.Blocks.GroupBlockEntity", null)
                        .WithMany("Children")
                        .HasForeignKey("ParentBlockId")
                        .OnDelete(DeleteBehavior.NoAction);
                });

            modelBuilder.Entity("Signals.App.Database.Entities.ChannelEntity", b =>
                {
                    b.HasOne("Signals.App.Database.Entities.UserEntity", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("Signals.App.Database.Entities.ExecutionEntity", b =>
                {
                    b.HasOne("Signals.App.Database.Entities.SignalEntity", null)
                        .WithOne("Execution")
                        .HasForeignKey("Signals.App.Database.Entities.ExecutionEntity", "SignalId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Signals.App.Database.Entities.StageEntity", "Stage")
                        .WithOne()
                        .HasForeignKey("Signals.App.Database.Entities.ExecutionEntity", "StageId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Stage");
                });

            modelBuilder.Entity("Signals.App.Database.Entities.SignalEntity", b =>
                {
                    b.HasOne("Signals.App.Database.Entities.UserEntity", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("Signals.App.Database.Entities.StageEntity", b =>
                {
                    b.HasOne("Signals.App.Database.Entities.SignalEntity", null)
                        .WithMany("Stages")
                        .HasForeignKey("SignalId")
                        .OnDelete(DeleteBehavior.NoAction)
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
                        .OnDelete(DeleteBehavior.NoAction)
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
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Signals.App.Database.Entities.IndicatorEntity", "RightIndicator")
                        .WithOne()
                        .HasForeignKey("Signals.App.Database.Entities.Blocks.ValueBlockEntity", "RightIndicatorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("LeftIndicator");

                    b.Navigation("RightIndicator");
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
                        .OnDelete(DeleteBehavior.NoAction)
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
                        .OnDelete(DeleteBehavior.NoAction)
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
                    b.Navigation("Execution");

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
