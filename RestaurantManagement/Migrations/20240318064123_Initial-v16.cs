using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManagement.Migrations
{
    /// <inheritdoc />
    public partial class Initialv16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrder_AspNetUsers_CustomerId",
                table: "PurchaseOrder");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "PurchaseOrder",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "AddressCustomerOther",
                table: "PurchaseOrder",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerOther",
                table: "PurchaseOrder",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrder_AspNetUsers_CustomerId",
                table: "PurchaseOrder",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrder_AspNetUsers_CustomerId",
                table: "PurchaseOrder");

            migrationBuilder.DropColumn(
                name: "AddressCustomerOther",
                table: "PurchaseOrder");

            migrationBuilder.DropColumn(
                name: "CustomerOther",
                table: "PurchaseOrder");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "PurchaseOrder",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrder_AspNetUsers_CustomerId",
                table: "PurchaseOrder",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
