using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GenderPayGap.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddOrganisationScopeReportingYear : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReportingYear",
                table: "OrganisationScopes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
            
            migrationBuilder.Sql("UPDATE \"OrganisationScopes\" SET \"ReportingYear\" = EXTRACT(YEAR FROM \"SnapshotDate\")");

            migrationBuilder.CreateIndex(
                name: "IX_OrganisationScopes_ReportingYear",
                table: "OrganisationScopes",
                column: "ReportingYear");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrganisationScopes_ReportingYear",
                table: "OrganisationScopes");

            migrationBuilder.DropColumn(
                name: "ReportingYear",
                table: "OrganisationScopes");
        }
    }
}
