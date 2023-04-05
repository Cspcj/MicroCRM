using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroCRM.Data.Migrations
{
    public partial class ClientandProjectTablesmodified1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "dbo",
                table: "Projets",
                newName: "Owner");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "dbo",
                table: "Clients",
                newName: "Owner");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Owner",
                schema: "dbo",
                table: "Projets",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Owner",
                schema: "dbo",
                table: "Clients",
                newName: "Id");
        }
    }
}
