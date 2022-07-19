using Microsoft.EntityFrameworkCore.Migrations;

namespace KundenVerzeichnis.Migrations
{
    public partial class AddNotesToBills : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Bills",
                type: "varchar(250)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Bills");
        }
    }
}
