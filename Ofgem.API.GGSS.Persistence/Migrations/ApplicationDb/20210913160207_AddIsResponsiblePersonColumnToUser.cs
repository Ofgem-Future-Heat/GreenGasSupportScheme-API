using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ofgem.API.GGSS.Persistence.Migrations.ApplicationDb
{
    public partial class AddIsResponsiblePersonColumnToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsResponsiblePerson",
                table: "Users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Organisations",
                keyColumn: "Id",
                keyValue: new Guid("b141ac41-d6f7-47fd-b31e-847d77134fca"),
                column: "Json",
                value: "{\"RegisteredOfficeAddress\":{\"Postcode\":\"E14 4PU\",\"County\":null,\"Town\":\"London\",\"LineTwo\":\"Canary Wharf\",\"LineOne\":\"10 South Colonade\",\"Name\":null},\"LegalDocument\":null,\"Type\":\"Private\",\"Error\":null,\"ReferenceNumber\":\"67d3e29f-565b-41f3-b1fa-38bee6f1dfa5\",\"RegistrationNumber\":\"1234567\",\"Name\":\"Clydebiomass UK\"}");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsResponsiblePerson",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Organisations",
                keyColumn: "Id",
                keyValue: new Guid("b141ac41-d6f7-47fd-b31e-847d77134fca"),
                column: "Json",
                value: "{\"RegisteredOfficeAddress\":{\"Postcode\":\"E14 4PU\",\"County\":null,\"Town\":\"London\",\"LineTwo\":\"Canary Wharf\",\"LineOne\":\"10 South Colonade\",\"Name\":null},\"Type\":\"Private\",\"ReferenceNumber\":\"3edae371-d14d-4cb9-be90-2d867e314ebc\",\"RegistrationNumber\":\"1234567\",\"Name\":\"Clydebiomass UK\"}");
        }
    }
}
