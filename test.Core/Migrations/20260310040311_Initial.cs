using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace test.Core.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Offices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    CityCode = table.Column<int>(type: "integer", nullable: false),
                    Uuid = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: true),
                    CountryCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Address_Region = table.Column<string>(type: "text", nullable: true),
                    Address_City = table.Column<string>(type: "text", nullable: true),
                    Address_Street = table.Column<string>(type: "text", nullable: true),
                    Address_HouseNumber = table.Column<string>(type: "text", nullable: true),
                    Address_Apartment = table.Column<int>(type: "integer", nullable: true),
                    WorkTime = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PhonesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Coordinates",
                columns: table => new
                {
                    OfficeId = table.Column<int>(type: "integer", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coordinates", x => x.OfficeId);
                    table.ForeignKey(
                        name: "FK_Coordinates_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Phones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PhoneNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Additional = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    OfficeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Phones_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Phones_OfficeId",
                table: "Phones",
                column: "OfficeId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coordinates");

            migrationBuilder.DropTable(
                name: "Phones");

            migrationBuilder.DropTable(
                name: "Offices");
        }
    }
}
