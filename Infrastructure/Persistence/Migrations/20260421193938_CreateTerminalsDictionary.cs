using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace task.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreateTerminalsDictionary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "offices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    CityCode = table.Column<int>(type: "integer", nullable: false),
                    Uuid = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: true),
                    CountryCode = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    AddressRegion = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    AddressCity = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    AddressStreet = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    AddressHouseNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    AddressApartment = table.Column<int>(type: "integer", nullable: true),
                    WorkTime = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    ImportedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    SourceFile = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    latitude = table.Column<double>(type: "double precision", nullable: false),
                    longitude = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_offices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "phones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OfficeId = table.Column<int>(type: "integer", nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Additional = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_phones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_phones_offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_offices_AddressCity_AddressRegion",
                table: "offices",
                columns: new[] { "AddressCity", "AddressRegion" });

            migrationBuilder.CreateIndex(
                name: "IX_offices_CityCode",
                table: "offices",
                column: "CityCode");

            migrationBuilder.CreateIndex(
                name: "IX_offices_Uuid",
                table: "offices",
                column: "Uuid");

            migrationBuilder.CreateIndex(
                name: "IX_phones_OfficeId",
                table: "phones",
                column: "OfficeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "phones");

            migrationBuilder.DropTable(
                name: "offices");
        }
    }
}
