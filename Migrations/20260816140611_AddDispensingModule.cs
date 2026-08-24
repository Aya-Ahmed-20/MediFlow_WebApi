using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediFlowApi.Migrations
{
    /// <inheritdoc />
    public partial class AddDispensingModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDispensed",
                table: "Prescription",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "StockQuantity",
                table: "Medicines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DispensingRecord",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrescriptionId = table.Column<int>(type: "int", nullable: false),
                    PharmacistId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DispensedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DispensingRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DispensingRecord_AspNetUsers_PharmacistId",
                        column: x => x.PharmacistId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DispensingRecord_Prescription_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalTable: "Prescription",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DispensingRecord_PharmacistId",
                table: "DispensingRecord",
                column: "PharmacistId");

            migrationBuilder.CreateIndex(
                name: "IX_DispensingRecord_PrescriptionId",
                table: "DispensingRecord",
                column: "PrescriptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DispensingRecord");

            migrationBuilder.DropColumn(
                name: "IsDispensed",
                table: "Prescription");

            migrationBuilder.DropColumn(
                name: "StockQuantity",
                table: "Medicines");
        }
    }
}
