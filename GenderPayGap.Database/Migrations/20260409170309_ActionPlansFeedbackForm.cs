using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GenderPayGap.Database.Migrations
{
    /// <inheritdoc />
    public partial class ActionPlansFeedbackForm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DifficultyActionPlan",
                table: "Feedback",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "WhoAreYou_EmployeeInterestedInOrganisationActionPlan",
                table: "Feedback",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "WhoAreYou_EmployeeResponsibleForSubmittingActionPlan",
                table: "Feedback",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "WhyVisitSite_CreateAnActionPlanForMyOrganisation",
                table: "Feedback",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "WhyVisitSite_FindOutMoreAboutCreatingAnActionPlan",
                table: "Feedback",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "WhyVisitSite_LookAtActionPlansForOrganisationsOrSectors",
                table: "Feedback",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YourName",
                table: "Feedback",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DifficultyActionPlan",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "WhoAreYou_EmployeeInterestedInOrganisationActionPlan",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "WhoAreYou_EmployeeResponsibleForSubmittingActionPlan",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "WhyVisitSite_CreateAnActionPlanForMyOrganisation",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "WhyVisitSite_FindOutMoreAboutCreatingAnActionPlan",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "WhyVisitSite_LookAtActionPlansForOrganisationsOrSectors",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "YourName",
                table: "Feedback");
        }
    }
}
