﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Ofgem.API.GGSS.Persistence;

namespace Ofgem.API.GGSS.Persistence.Migrations.ApplicationDb
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20211110144907_MakeUserNullableInUserOrganisation")]
    partial class MakeUserNullableInUserOrganisation
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.17")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Ofgem.API.GGSS.Application.Entities.Application", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Json")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("OrganisationId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("OrganisationId");

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("Ofgem.API.GGSS.Application.Entities.Audit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AffectedColumns")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("EntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("NewValues")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OldValues")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PrimaryKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TableName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("AuditLogs");
                });

            modelBuilder.Entity("Ofgem.API.GGSS.Application.Entities.Document", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ApplicationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Json")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ResponsiblePersonOrganisationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ResponsiblePersonUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.HasIndex("ResponsiblePersonOrganisationId", "ResponsiblePersonUserId");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("Ofgem.API.GGSS.Application.Entities.Organisation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Json")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Organisations");

                    b.HasData(
                        new
                        {
                            Id = new Guid("b141ac41-d6f7-47fd-b31e-847d77134fca"),
                            Json = "{\"RegisteredOfficeAddress\":{\"Postcode\":\"E14 4PU\",\"County\":null,\"Town\":\"London\",\"LineTwo\":\"Canary Wharf\",\"LineOne\":\"10 South Colonade\",\"Name\":null},\"LegalDocument\":null,\"LetterOfAuthorisation\":null,\"PhotoId\":null,\"ProofOfAddress\":null,\"Type\":\"Private\",\"LastModified\":null,\"OrganisationStatus\":\"Not verified\",\"Error\":null,\"ReferenceNumber\":\"a6423c5f-1b19-47dc-b102-e7f9271931aa\",\"RegistrationNumber\":\"1234567\",\"Name\":\"Clydebiomass UK\"}"
                        });
                });

            modelBuilder.Entity("Ofgem.API.GGSS.Application.Entities.ResponsiblePerson", b =>
                {
                    b.Property<Guid>("OrganisationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Json")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("OrganisationId", "UserId")
                        .HasName("Organisation_ResponsiblePerson");

                    b.HasIndex("UserId", "OrganisationId")
                        .IsUnique();

                    b.ToTable("ResponsiblePeople");

                    b.HasData(
                        new
                        {
                            OrganisationId = new Guid("b141ac41-d6f7-47fd-b31e-847d77134fca"),
                            UserId = new Guid("00000000-0000-0000-0000-000000000000"),
                            Id = new Guid("23d389d1-5a79-40e4-8171-d829b3244c6d"),
                            Json = "{\"PhotoId\":null,\"BankStatement\":null,\"LetterOrAuthority\":null,\"DateOfBirth\":\"08/17/1965 00:00:00\",\"TelephoneNumber\":\"01234567890\"}"
                        });
                });

            modelBuilder.Entity("Ofgem.API.GGSS.Application.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsResponsiblePerson")
                        .HasColumnType("bit");

                    b.Property<string>("Json")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProviderId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ProviderId")
                        .IsUnique()
                        .HasFilter("[ProviderId] IS NOT NULL");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("f3acf86b-9f44-44cd-82df-e10d0e7e7cf8"),
                            IsResponsiblePerson = false,
                            Json = "{\"EmailAddress\":\"james.anderson@ofgem.gov.uk\",\"Surname\":\"Anderson\",\"Name\":\"James\"}",
                            ProviderId = "326FA974-7C05-4B37-A8E4-6D5FE6DEB63B"
                        });
                });

            modelBuilder.Entity("Ofgem.API.GGSS.Application.Entities.UserOrganisation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("OrganisationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("OrganisationId");

                    b.HasIndex("UserId");

                    b.ToTable("UserOrganisations");
                });

            modelBuilder.Entity("Ofgem.API.GGSS.Application.Entities.Application", b =>
                {
                    b.HasOne("Ofgem.API.GGSS.Application.Entities.Organisation", "Organisation")
                        .WithMany("Applications")
                        .HasForeignKey("OrganisationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Ofgem.API.GGSS.Application.Entities.Document", b =>
                {
                    b.HasOne("Ofgem.API.GGSS.Application.Entities.Application", "Application")
                        .WithMany("Documents")
                        .HasForeignKey("ApplicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ofgem.API.GGSS.Application.Entities.ResponsiblePerson", null)
                        .WithMany("Documents")
                        .HasForeignKey("ResponsiblePersonOrganisationId", "ResponsiblePersonUserId");
                });

            modelBuilder.Entity("Ofgem.API.GGSS.Application.Entities.ResponsiblePerson", b =>
                {
                    b.HasOne("Ofgem.API.GGSS.Application.Entities.Organisation", "Organisation")
                        .WithMany("ResponsiblePeople")
                        .HasForeignKey("OrganisationId")
                        .HasConstraintName("FK_Organisations_ResponsiblePerson")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ofgem.API.GGSS.Application.Entities.User", "User")
                        .WithMany("ResponsiblePeople")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_Users_ResponsiblePerson")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Ofgem.API.GGSS.Application.Entities.UserOrganisation", b =>
                {
                    b.HasOne("Ofgem.API.GGSS.Application.Entities.Organisation", "Organisation")
                        .WithMany("UserOrganisations")
                        .HasForeignKey("OrganisationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ofgem.API.GGSS.Application.Entities.User", "User")
                        .WithMany("UserOrganisations")
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
