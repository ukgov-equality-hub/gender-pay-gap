using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GenderPayGap.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddNumberOfEmployeesToOrganisationScope : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfEmployees",
                table: "OrganisationScopes",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NumberOfEmployeesSkipped",
                table: "OrganisationScopes",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfEmployees",
                table: "OrganisationScopes");

            migrationBuilder.DropColumn(
                name: "NumberOfEmployeesSkipped",
                table: "OrganisationScopes");
        }
    }
}
