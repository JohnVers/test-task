using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:office_type", "pvz,postamat,warehouse");

            migrationBuilder.CreateTable(
                name: "office",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: true),
                    city_code = table.Column<int>(type: "integer", nullable: false),
                    uuid = table.Column<string>(type: "text", nullable: true),
                    type = table.Column<int>(type: "integer", nullable: true),
                    country_code = table.Column<string>(type: "text", nullable: false),
                    latitude = table.Column<double>(type: "double precision", nullable: false),
                    longitude = table.Column<double>(type: "double precision", nullable: false),
                    address_region = table.Column<string>(type: "text", nullable: true),
                    address_city = table.Column<string>(type: "text", nullable: true),
                    address_street = table.Column<string>(type: "text", nullable: true),
                    address_house_number = table.Column<string>(type: "text", nullable: true),
                    address_apartment = table.Column<int>(type: "integer", nullable: true),
                    work_time = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_office", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "phone",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    office_id = table.Column<int>(type: "integer", nullable: false),
                    phone_number = table.Column<string>(type: "text", nullable: false),
                    additional = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_phone", x => x.Id);
                    table.ForeignKey(
                        name: "FK_phone_office_office_id",
                        column: x => x.office_id,
                        principalTable: "office",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_office_address_city",
                table: "office",
                column: "address_city");

            migrationBuilder.CreateIndex(
                name: "IX_office_address_street",
                table: "office",
                column: "address_street");

            migrationBuilder.CreateIndex(
                name: "IX_phone_office_id",
                table: "phone",
                column: "office_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "phone");

            migrationBuilder.DropTable(
                name: "office");
        }
    }
}
