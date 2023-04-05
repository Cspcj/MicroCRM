using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroCRM.Data.Migrations
{
    public partial class ClientandProjectTablesmodified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomerName",
                schema: "dbo",
                table: "Clients",
                newName: "ClientName");

            migrationBuilder.RenameColumn(
                name: "CustomerCompany",
                schema: "dbo",
                table: "Clients",
                newName: "ClientCompany");

            migrationBuilder.AlterColumn<string>(
                name: "ClientOtherInformation",
                schema: "dbo",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ClientAdress",
                schema: "dbo",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                schema: "dbo",
                table: "Clients",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                schema: "dbo",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "ClientName",
                schema: "dbo",
                table: "Clients",
                newName: "CustomerName");

            migrationBuilder.RenameColumn(
                name: "ClientCompany",
                schema: "dbo",
                table: "Clients",
                newName: "CustomerCompany");

            migrationBuilder.AlterColumn<string>(
                name: "ClientOtherInformation",
                schema: "dbo",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClientAdress",
                schema: "dbo",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
