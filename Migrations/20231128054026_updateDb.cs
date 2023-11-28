using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProniaWebApp.Migrations
{
    /// <inheritdoc />
    public partial class updateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastUpdatedDate",
                table: "Tags",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedDate",
                table: "Sliders",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedDate",
                table: "Products",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedDate",
                table: "ProductImages",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedDate",
                table: "Categories",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedDate",
                table: "BlogsImages",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedDate",
                table: "Blogs",
                newName: "UpdatedDate");

            migrationBuilder.AlterColumn<string>(
                name: "ImgUrl",
                table: "Sliders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "Tags",
                newName: "LastUpdatedDate");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "Sliders",
                newName: "LastUpdatedDate");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "Products",
                newName: "LastUpdatedDate");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "ProductImages",
                newName: "LastUpdatedDate");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "Categories",
                newName: "LastUpdatedDate");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "BlogsImages",
                newName: "LastUpdatedDate");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "Blogs",
                newName: "LastUpdatedDate");

            migrationBuilder.AlterColumn<string>(
                name: "ImgUrl",
                table: "Sliders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
