using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StranaKart.Assignment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "offices",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    uuid = table.Column<Guid>(type: "uuid", nullable: true),
                    code = table.Column<string>(type: "text", nullable: true),
                    city_code = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: true),
                    country_code = table.Column<string>(type: "text", nullable: false),
                    address_region = table.Column<string>(type: "text", nullable: true),
                    address_city = table.Column<string>(type: "text", nullable: true),
                    address_street = table.Column<string>(type: "text", nullable: true),
                    address_house_number = table.Column<string>(type: "text", nullable: true),
                    address_apartment = table.Column<int>(type: "integer", nullable: true),
                    latitude = table.Column<double>(type: "double precision", nullable: true),
                    longitude = table.Column<double>(type: "double precision", nullable: true),
                    work_time = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_offices", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "office_phones",
                columns: table => new
                {
                    office_id = table.Column<int>(type: "integer", nullable: false),
                    phone_number = table.Column<string>(type: "text", nullable: false),
                    additional = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_office_phones", x => new { x.office_id, x.phone_number });
                    table.ForeignKey(
                        name: "fk_office_phones_offices_office_id",
                        column: x => x.office_id,
                        principalTable: "offices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_offices_address_city",
                table: "offices",
                column: "address_city");

            migrationBuilder.CreateIndex(
                name: "ix_offices_address_city_type",
                table: "offices",
                columns: new[] { "address_city", "type" });

            migrationBuilder.CreateIndex(
                name: "ix_offices_city_code",
                table: "offices",
                column: "city_code");

            migrationBuilder.CreateIndex(
                name: "ix_offices_city_code_type",
                table: "offices",
                columns: new[] { "city_code", "type" });

            migrationBuilder.CreateIndex(
                name: "ix_offices_uuid",
                table: "offices",
                column: "uuid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "office_phones");

            migrationBuilder.DropTable(
                name: "offices");
        }
    }
}
