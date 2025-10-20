using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GenderPayGap.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_ActionInActionPlan_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActionsinActionPlans",
                columns: table => new
                {
                    ActioninActionPlanId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActionPlanId = table.Column<long>(type: "bigint", nullable: false),
                    ActionId = table.Column<byte>(type: "smallint", nullable: false),
                    OldStatus = table.Column<byte>(type: "smallint", nullable: false),
                    NewStatus = table.Column<byte>(type: "smallint", nullable: false),
                    SupportingNarrative = table.Column<string>(type: "text", nullable: true),
                    EvaluationThreeYear = table.Column<string>(type: "text", nullable: true),
                    SupportingInformationThreeYear = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.ActionsinActionPlans", x => x.ActioninActionPlanId);
                    table.ForeignKey(
                        name: "FK_dbo.ActionsinActionPlans_dbo.ActionPlans_ActionPlanId",
                        column: x => x.ActionPlanId,
                        principalTable: "ActionPlans",
                        principalColumn: "ActionPlanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionsinActionPlans_ActionPlanId",
                table: "ActionsinActionPlans",
                column: "ActionPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionsinActionPlans_NewStatus",
                table: "ActionsinActionPlans",
                column: "NewStatus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionsinActionPlans");
        }
    }
}
