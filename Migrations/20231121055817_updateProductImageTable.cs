using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProniaWebApp.Migrations
{
    /// <inheritdoc />
    public partial class updateProductImageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPrime",
                table: "ProductImages",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPrime",
                table: "ProductImages");
        }
    }
}
