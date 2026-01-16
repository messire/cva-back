using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVA.Infrastructure.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class RemoveYearsOfExperience : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "years_of_experience",
                table: "developer_profiles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "years_of_experience",
                table: "developer_profiles",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
