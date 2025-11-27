using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GenderPayGap.Database.Migrations
{
    /// <inheritdoc />
    public partial class ActionPlansMVP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActionPlans",
                columns: table => new
                {
                    ActionPlanId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrganisationId = table.Column<long>(type: "bigint", nullable: false),
                    ReportingYear = table.Column<int>(type: "integer", nullable: false),
                    DraftCreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    SubmittedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Status = table.Column<byte>(type: "smallint", nullable: false),
                    ActionPlanType = table.Column<byte>(type: "smallint", nullable: false),
                    SupportingNarrative = table.Column<string>(type: "text", nullable: true),
                    LinkToReport = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.ActionPlans", x => x.ActionPlanId);
                    table.ForeignKey(
                        name: "FK_dbo.ActionPlans_dbo.Organisations_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisations",
                        principalColumn: "OrganisationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActionsInActionPlans",
                columns: table => new
                {
                    ActionInActionPlanId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActionPlanId = table.Column<long>(type: "bigint", nullable: false),
                    Action = table.Column<int>(type: "integer", nullable: false),
                    OldStatus = table.Column<byte>(type: "smallint", nullable: true),
                    NewStatus = table.Column<byte>(type: "smallint", nullable: false),
                    SupportingText = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.ActionsInActionPlans", x => x.ActionInActionPlanId);
                    table.ForeignKey(
                        name: "FK_dbo.ActionsInActionPlans_dbo.ActionPlans_ActionPlanId",
                        column: x => x.ActionPlanId,
                        principalTable: "ActionPlans",
                        principalColumn: "ActionPlanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionPlans_OrganisationId",
                table: "ActionPlans",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionPlans_OrganisationId_ReportingYear",
                table: "ActionPlans",
                columns: new[] { "OrganisationId", "ReportingYear" });

            migrationBuilder.CreateIndex(
                name: "IX_ActionPlans_Status",
                table: "ActionPlans",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ActionsInActionPlans_ActionPlanId",
                table: "ActionsInActionPlans",
                column: "ActionPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionsInActionPlans_NewStatus",
                table: "ActionsInActionPlans",
                column: "NewStatus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionsInActionPlans");

            migrationBuilder.DropTable(
                name: "ActionPlans");
        }
    }
}
