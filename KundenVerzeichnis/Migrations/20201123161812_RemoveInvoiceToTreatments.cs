using Microsoft.EntityFrameworkCore.Migrations;

namespace KundenVerzeichnis.Migrations
{
    public partial class RemoveInvoiceToTreatments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Invoice",
                table: "Treatments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Invoice",
                table: "Treatments");
        }
    }
}
