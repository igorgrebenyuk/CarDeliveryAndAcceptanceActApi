using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarDeliveryAndAcceptance.Context.Migrations
{
    /// <inheritdoc />
    public partial class name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AcceptanceActs",
                table: "AcceptanceActs");

            migrationBuilder.RenameColumn(
                name: "SalesmanSurname",
                table: "Salesmans",
                newName: "Surname");

            migrationBuilder.RenameColumn(
                name: "SalesmanName",
                table: "Salesmans",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "SalesmanFirstName",
                table: "Salesmans",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "CityName",
                table: "Cities",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "BuyerSurname",
                table: "Buyers",
                newName: "Surname");

            migrationBuilder.RenameColumn(
                name: "BuyerName",
                table: "Buyers",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "BuyerFirstName",
                table: "Buyers",
                newName: "FirstName");

            migrationBuilder.CreateIndex(
                name: "IX_AcceptanceActs",
                table: "AcceptanceActs",
                columns: new[] { "CarId", "BuyerId", "SalesmanId" },
                unique: true,
                filter: "\"DeletedAt\" IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AcceptanceActs",
                table: "AcceptanceActs");

            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "Salesmans",
                newName: "SalesmanSurname");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Salesmans",
                newName: "SalesmanName");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Salesmans",
                newName: "SalesmanFirstName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Cities",
                newName: "CityName");

            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "Buyers",
                newName: "BuyerSurname");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Buyers",
                newName: "BuyerName");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Buyers",
                newName: "BuyerFirstName");

            migrationBuilder.CreateIndex(
                name: "IX_AcceptanceActs",
                table: "AcceptanceActs",
                columns: new[] { "CarId", "BuyerId", "SalesmanId", "DateOfCreation" },
                unique: true,
                filter: "\"DeletedAt\" IS NULL");
        }
    }
}
