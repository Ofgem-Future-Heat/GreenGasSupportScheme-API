using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ofgem.API.GGSS.Persistence.Migrations.ApplicationDb
{
    public partial class InitWithOrgSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    EntityId = table.Column<Guid>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    TableName = table.Column<string>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false),
                    OldValues = table.Column<string>(nullable: true),
                    NewValues = table.Column<string>(nullable: true),
                    AffectedColumns = table.Column<string>(nullable: true),
                    PrimaryKey = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organisations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Json = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organisations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProviderId = table.Column<string>(nullable: true),
                    Json = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    OrganisationId = table.Column<Guid>(nullable: false),
                    Json = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Applications_Organisations_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResponsiblePeople",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    OrganisationId = table.Column<Guid>(nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Json = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Organisation_ResponsiblePerson", x => new { x.OrganisationId, x.UserId });
                    table.ForeignKey(
                        name: "FK_Organisations_ResponsiblePerson",
                        column: x => x.OrganisationId,
                        principalTable: "Organisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_ResponsiblePerson",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationId = table.Column<Guid>(nullable: false),
                    Json = table.Column<string>(nullable: true),
                    ResponsiblePersonOrganisationId = table.Column<Guid>(nullable: true),
                    ResponsiblePersonUserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Documents_ResponsiblePeople_ResponsiblePersonOrganisationId_ResponsiblePersonUserId",
                        columns: x => new { x.ResponsiblePersonOrganisationId, x.ResponsiblePersonUserId },
                        principalTable: "ResponsiblePeople",
                        principalColumns: new[] { "OrganisationId", "UserId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Organisations",
                columns: new[] { "Id", "Json" },
                values: new object[] { new Guid("b141ac41-d6f7-47fd-b31e-847d77134fca"), "{\"RegisteredOfficeAddress\":{\"Postcode\":\"E14 4PU\",\"County\":null,\"Town\":\"London\",\"LineTwo\":\"Canary Wharf\",\"LineOne\":\"10 South Colonade\",\"Name\":null},\"Type\":\"Private\",\"ReferenceNumber\":\"3edae371-d14d-4cb9-be90-2d867e314ebc\",\"RegistrationNumber\":\"1234567\",\"Name\":\"Clydebiomass UK\"}" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Json", "ProviderId" },
                values: new object[] { new Guid("f3acf86b-9f44-44cd-82df-e10d0e7e7cf8"), "{\"EmailAddress\":\"james.anderson@ofgem.gov.uk\",\"Surname\":\"Anderson\",\"Name\":\"James\"}", "326FA974-7C05-4B37-A8E4-6D5FE6DEB63B" });

            migrationBuilder.InsertData(
                table: "ResponsiblePeople",
                columns: new[] { "OrganisationId", "UserId", "Id", "Json" },
                values: new object[] { new Guid("b141ac41-d6f7-47fd-b31e-847d77134fca"), new Guid("f3acf86b-9f44-44cd-82df-e10d0e7e7cf8"), new Guid("23d389d1-5a79-40e4-8171-d829b3244c6d"), "{\"TelephoneNumber\":\"01234567890\"}" });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_OrganisationId",
                table: "Applications",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ApplicationId",
                table: "Documents",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ResponsiblePersonOrganisationId_ResponsiblePersonUserId",
                table: "Documents",
                columns: new[] { "ResponsiblePersonOrganisationId", "ResponsiblePersonUserId" });

            migrationBuilder.CreateIndex(
                name: "IX_ResponsiblePeople_UserId_OrganisationId",
                table: "ResponsiblePeople",
                columns: new[] { "UserId", "OrganisationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProviderId",
                table: "Users",
                column: "ProviderId",
                unique: true,
                filter: "[ProviderId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "ResponsiblePeople");

            migrationBuilder.DropTable(
                name: "Organisations");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
