using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthorizationDB.Migrations
{
    public partial class TypeRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Users",
                type: "TEXT",
                nullable: true);
        }
    }
}
