using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVA.Infrastructure.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AddPhotoProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "photo",
                table: "users",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "photo",
                table: "users");
        }
    }
}
