using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GenderPayGap.Database.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOrganisationScopeSnapshotDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrganisationScopes_SnapshotDate",
                table: "OrganisationScopes");

            migrationBuilder.DropColumn(
                name: "SnapshotDate",
                table: "OrganisationScopes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SnapshotDate",
                table: "OrganisationScopes",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "('1900-01-01T00:00:00.000')");

            migrationBuilder.CreateIndex(
                name: "IX_OrganisationScopes_SnapshotDate",
                table: "OrganisationScopes",
                column: "SnapshotDate");
        }
    }
}
