using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InstitutionsService.Migrations
{
    public partial class institutions_sqlview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"create view InstitutionAddressVM AS SELECT 
	                inst.Id AS InstitutionId,
	                inst.Name AS InstitutionName,
                    instAddr.Id AS AddressId,
                    instAddr.StreetAddress AS StreetAddress,
                    instAddr.City AS City,
			        instAddr.PostalCode AS PostalCode,
                    instAddr.Province AS Province,
                    instAddr.Region AS Region,
                    instAddr.Region AS Country,
                    instAddr.Latitude AS Latitude,
                    instAddr.Longitude AS Longitude,
                    instDetail.Id AS InstitutionDetailId,
                    instDetail.LanguageId AS LanguageId,
                    instDetail.Description AS InstitutionDescription,
                    lang.Code AS LanguageCode,
                    lang.Name AS LanguageName
                FROM Institution inst
	                LEFT JOIN InstitutionAddress instAddr ON inst.Id = instAddr.InstitutionId
                    LEFT JOIN InstitutionLanguageDetail instDetail ON inst.Id = instDetail.InstitutionId
                    LEFT JOIN MainLanguage lang ON instDetail.LanguageId = lang.Id"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
