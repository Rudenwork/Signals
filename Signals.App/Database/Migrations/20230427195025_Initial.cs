using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Signals.App.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Indicators",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Interval = table.Column<string>(type: "varchar", nullable: true),
                    LoopbackPeriod = table.Column<int>(type: "integer", nullable: true),
                    Symbol = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Indicators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    IsAdmin = table.Column<bool>(type: "boolean", nullable: false),
                    IsDisabled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Indicators-BollingerBands",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BandType = table.Column<string>(type: "varchar", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Indicators-BollingerBands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Indicators-BollingerBands_Indicators_Id",
                        column: x => x.Id,
                        principalTable: "Indicators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Indicators-Candle",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ParameterType = table.Column<string>(type: "varchar", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Indicators-Candle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Indicators-Candle_Indicators_Id",
                        column: x => x.Id,
                        principalTable: "Indicators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Indicators-Constant",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Indicators-Constant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Indicators-Constant_Indicators_Id",
                        column: x => x.Id,
                        principalTable: "Indicators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Indicators-ExponentialMovingAverage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Indicators-ExponentialMovingAverage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Indicators-ExponentialMovingAverage_Indicators_Id",
                        column: x => x.Id,
                        principalTable: "Indicators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Indicators-MovingAverage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Indicators-MovingAverage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Indicators-MovingAverage_Indicators_Id",
                        column: x => x.Id,
                        principalTable: "Indicators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Indicators-RelativeStrengthIndex",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Indicators-RelativeStrengthIndex", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Indicators-RelativeStrengthIndex_Indicators_Id",
                        column: x => x.Id,
                        principalTable: "Indicators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Channels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "varchar", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExternalId = table.Column<long>(type: "bigint", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Destination = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Channels_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Signals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Schedule = table.Column<string>(type: "text", nullable: true),
                    IsDisabled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Signals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Signals_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Stages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SignalId = table.Column<Guid>(type: "uuid", nullable: false),
                    Index = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stages_Signals_SignalId",
                        column: x => x.SignalId,
                        principalTable: "Signals",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Executions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SignalId = table.Column<Guid>(type: "uuid", nullable: false),
                    StageId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Executions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Executions_Signals_SignalId",
                        column: x => x.SignalId,
                        principalTable: "Signals",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Executions_Stages_StageId",
                        column: x => x.StageId,
                        principalTable: "Stages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Stages-Notification",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChannelId = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stages-Notification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stages-Notification_Channels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Stages-Notification_Stages_Id",
                        column: x => x.Id,
                        principalTable: "Stages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stages-Waiting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Unit = table.Column<string>(type: "varchar", nullable: false),
                    Length = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stages-Waiting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stages-Waiting_Stages_Id",
                        column: x => x.Id,
                        principalTable: "Stages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Blocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentBlockId = table.Column<Guid>(type: "uuid", nullable: true),
                    Index = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blocks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Blocks-Change",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IndicatorId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "varchar", nullable: false),
                    Operator = table.Column<string>(type: "varchar", nullable: false),
                    Target = table.Column<decimal>(type: "numeric", nullable: false),
                    IsPercentage = table.Column<bool>(type: "boolean", nullable: false),
                    PeriodUnit = table.Column<string>(type: "varchar", nullable: false),
                    PeriodLength = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blocks-Change", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Blocks-Change_Blocks_Id",
                        column: x => x.Id,
                        principalTable: "Blocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Blocks-Change_Indicators_IndicatorId",
                        column: x => x.IndicatorId,
                        principalTable: "Indicators",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Blocks-Group",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "varchar", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blocks-Group", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Blocks-Group_Blocks_Id",
                        column: x => x.Id,
                        principalTable: "Blocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Blocks-Value",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LeftIndicatorId = table.Column<Guid>(type: "uuid", nullable: false),
                    RightIndicatorId = table.Column<Guid>(type: "uuid", nullable: false),
                    Operator = table.Column<string>(type: "varchar", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blocks-Value", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Blocks-Value_Blocks_Id",
                        column: x => x.Id,
                        principalTable: "Blocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Blocks-Value_Indicators_LeftIndicatorId",
                        column: x => x.LeftIndicatorId,
                        principalTable: "Indicators",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Blocks-Value_Indicators_RightIndicatorId",
                        column: x => x.RightIndicatorId,
                        principalTable: "Indicators",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Stages-Condition",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RetryCount = table.Column<int>(type: "integer", nullable: true),
                    RetryDelayUnit = table.Column<string>(type: "varchar", nullable: true),
                    RetryDelayLength = table.Column<int>(type: "integer", nullable: true),
                    BlockId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stages-Condition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stages-Condition_Blocks_BlockId",
                        column: x => x.BlockId,
                        principalTable: "Blocks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Stages-Condition_Stages_Id",
                        column: x => x.Id,
                        principalTable: "Stages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blocks_ParentBlockId",
                table: "Blocks",
                column: "ParentBlockId");

            migrationBuilder.CreateIndex(
                name: "IX_Blocks-Change_IndicatorId",
                table: "Blocks-Change",
                column: "IndicatorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Blocks-Value_LeftIndicatorId",
                table: "Blocks-Value",
                column: "LeftIndicatorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Blocks-Value_RightIndicatorId",
                table: "Blocks-Value",
                column: "RightIndicatorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Channels_UserId",
                table: "Channels",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Executions_SignalId",
                table: "Executions",
                column: "SignalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Executions_StageId",
                table: "Executions",
                column: "StageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Signals_UserId",
                table: "Signals",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Stages_SignalId",
                table: "Stages",
                column: "SignalId");

            migrationBuilder.CreateIndex(
                name: "IX_Stages-Condition_BlockId",
                table: "Stages-Condition",
                column: "BlockId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stages-Notification_ChannelId",
                table: "Stages-Notification",
                column: "ChannelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blocks_Blocks-Group_ParentBlockId",
                table: "Blocks",
                column: "ParentBlockId",
                principalTable: "Blocks-Group",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blocks_Blocks-Group_ParentBlockId",
                table: "Blocks");

            migrationBuilder.DropTable(
                name: "Blocks-Change");

            migrationBuilder.DropTable(
                name: "Blocks-Value");

            migrationBuilder.DropTable(
                name: "Executions");

            migrationBuilder.DropTable(
                name: "Indicators-BollingerBands");

            migrationBuilder.DropTable(
                name: "Indicators-Candle");

            migrationBuilder.DropTable(
                name: "Indicators-Constant");

            migrationBuilder.DropTable(
                name: "Indicators-ExponentialMovingAverage");

            migrationBuilder.DropTable(
                name: "Indicators-MovingAverage");

            migrationBuilder.DropTable(
                name: "Indicators-RelativeStrengthIndex");

            migrationBuilder.DropTable(
                name: "Stages-Condition");

            migrationBuilder.DropTable(
                name: "Stages-Notification");

            migrationBuilder.DropTable(
                name: "Stages-Waiting");

            migrationBuilder.DropTable(
                name: "Indicators");

            migrationBuilder.DropTable(
                name: "Channels");

            migrationBuilder.DropTable(
                name: "Stages");

            migrationBuilder.DropTable(
                name: "Signals");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Blocks-Group");

            migrationBuilder.DropTable(
                name: "Blocks");
        }
    }
}
