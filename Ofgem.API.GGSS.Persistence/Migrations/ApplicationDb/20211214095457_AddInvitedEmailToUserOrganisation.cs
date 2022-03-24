using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ofgem.API.GGSS.Persistence.Migrations.ApplicationDb
{
    public partial class AddInvitedEmailToUserOrganisation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InvitedEmail",
                table: "UserOrganisations",
                nullable: true,
                defaultValue: null);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsResponsiblePerson",
                table: "Users");
        }
    }
}
