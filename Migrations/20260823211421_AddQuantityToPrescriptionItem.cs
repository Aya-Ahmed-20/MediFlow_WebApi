using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediFlowApi.Migrations
{
    /// <inheritdoc />
    public partial class AddQuantityToPrescriptionItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "PrescriptionItem",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "PrescriptionItem");
        }
    }
}
