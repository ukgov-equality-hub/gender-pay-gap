using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GenderPayGap.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_ActionPlan_Table : Migration
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
                    ActionPlanType = table.Column<byte>(type: "smallint", nullable: true),
                    ProgressMade = table.Column<string>(type: "text", nullable: true),
                    MeasuringProgress = table.Column<string>(type: "text", nullable: true),
                    IsLateSubmission = table.Column<bool>(type: "boolean", nullable: false),
                    LateReason = table.Column<string>(type: "text", nullable: true)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionPlans");
        }
    }
}
