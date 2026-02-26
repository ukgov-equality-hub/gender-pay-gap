using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GenderPayGap.Database.Migrations
{
    /// <inheritdoc />
    public partial class ActionPlansAddResponsiblePerson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResponsiblePersonFirstName",
                table: "ActionPlans",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponsiblePersonJobTitle",
                table: "ActionPlans",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponsiblePersonLastName",
                table: "ActionPlans",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResponsiblePersonFirstName",
                table: "ActionPlans");

            migrationBuilder.DropColumn(
                name: "ResponsiblePersonJobTitle",
                table: "ActionPlans");

            migrationBuilder.DropColumn(
                name: "ResponsiblePersonLastName",
                table: "ActionPlans");
        }
    }
}
