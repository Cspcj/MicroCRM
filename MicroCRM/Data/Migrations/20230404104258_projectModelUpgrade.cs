using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroCRM.Data.Migrations
{
    public partial class projectModelUpgrade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Region",
                schema: "dbo",
                table: "Projets",
                newName: "ProjectLoceationRegion");

            migrationBuilder.RenameColumn(
                name: "ProjectLocationCity",
                schema: "dbo",
                table: "Projets",
                newName: "ProjectLoceationCity");

            migrationBuilder.AlterColumn<string>(
                name: "ProjectName",
                schema: "dbo",
                table: "Projets",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                schema: "dbo",
                table: "Projets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "dbo",
                table: "Projets",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsArchived",
                schema: "dbo",
                table: "Projets");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "dbo",
                table: "Projets");

            migrationBuilder.RenameColumn(
                name: "ProjectLoceationRegion",
                schema: "dbo",
                table: "Projets",
                newName: "Region");

            migrationBuilder.RenameColumn(
                name: "ProjectLoceationCity",
                schema: "dbo",
                table: "Projets",
                newName: "ProjectLocationCity");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectName",
                schema: "dbo",
                table: "Projets",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
