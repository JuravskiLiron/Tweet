using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tweet.Migrations
{
    /// <inheritdoc />
    public partial class Init5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ActiveAccount",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "GenderId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveAccount",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GenderId",
                table: "AspNetUsers");
        }
    }
}
