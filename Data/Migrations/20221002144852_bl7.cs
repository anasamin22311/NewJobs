using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jobs.Data.Migrations
{
    public partial class bl7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Job");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Job",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
