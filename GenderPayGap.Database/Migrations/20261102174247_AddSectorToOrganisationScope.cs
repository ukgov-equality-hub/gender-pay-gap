using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GenderPayGap.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddSectorToOrganisationScope : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Sector",
                table: "OrganisationScopes",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SectorStatus",
                table: "OrganisationScopes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sector",
                table: "OrganisationScopes");

            migrationBuilder.DropColumn(
                name: "SectorStatus",
                table: "OrganisationScopes");
        }
    }
}
