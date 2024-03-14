using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManagement.Migrations
{
    /// <inheritdoc />
    public partial class Inittialv10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_Ingredient_IngredientId",
                table: "Recipe");

            migrationBuilder.DropIndex(
                name: "IX_Recipe_IngredientId",
                table: "Recipe");

            migrationBuilder.DropColumn(
                name: "IngredientId",
                table: "Recipe");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Recipe");

            migrationBuilder.RenameColumn(
                name: "Unit",
                table: "Recipe",
                newName: "RecipeName");

            migrationBuilder.AlterColumn<string>(
                name: "Step",
                table: "Recipe",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Recipe",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RecipeName",
                table: "Recipe",
                newName: "Unit");

            migrationBuilder.AlterColumn<string>(
                name: "Step",
                table: "Recipe",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Recipe",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "IngredientId",
                table: "Recipe",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Recipe",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Recipe_IngredientId",
                table: "Recipe",
                column: "IngredientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_Ingredient_IngredientId",
                table: "Recipe",
                column: "IngredientId",
                principalTable: "Ingredient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
