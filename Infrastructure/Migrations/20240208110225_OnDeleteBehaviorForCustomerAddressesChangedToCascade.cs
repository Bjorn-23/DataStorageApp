using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OnDeleteBehaviorForCustomerAddressesChangedToCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Addresses_Address_AddressId",
                table: "Customer_Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Addresses_Customers_CustomerId",
                table: "Customer_Addresses");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Addresses_Address_AddressId",
                table: "Customer_Addresses",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Addresses_Customers_CustomerId",
                table: "Customer_Addresses",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Addresses_Address_AddressId",
                table: "Customer_Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Addresses_Customers_CustomerId",
                table: "Customer_Addresses");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Addresses_Address_AddressId",
                table: "Customer_Addresses",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Addresses_Customers_CustomerId",
                table: "Customer_Addresses",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
