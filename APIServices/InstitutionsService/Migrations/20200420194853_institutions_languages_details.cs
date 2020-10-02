using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InstitutionsService.Migrations
{
    public partial class institutions_languages_details : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Institution",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "MainLanguage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    PhonePrefix = table.Column<string>(nullable: true),
                    PhoneNumberStructure = table.Column<string>(nullable: true),
                    ValidateAddressApiUrl = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainLanguage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InstitutionLanguageDetail",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    InstitutionId = table.Column<int>(nullable: true),
                    LanguageId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstitutionLanguageDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstitutionLanguageDetail_Institution_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "Institution",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionLanguageDetail_MainLanguage_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "MainLanguage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionLanguageDetail_InstitutionId",
                table: "InstitutionLanguageDetail",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionLanguageDetail_LanguageId",
                table: "InstitutionLanguageDetail",
                column: "LanguageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstitutionLanguageDetail");

            migrationBuilder.DropTable(
                name: "MainLanguage");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Institution",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
