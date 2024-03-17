using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManagement.Migrations
{
    /// <inheritdoc />
    public partial class Initialv14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockIn_Ingredient_IngredientId",
                table: "StockIn");

            migrationBuilder.DropForeignKey(
                name: "FK_StockOut_Ingredient_IngredientId",
                table: "StockOut");

            migrationBuilder.DropIndex(
                name: "IX_StockOut_IngredientId",
                table: "StockOut");

            migrationBuilder.DropIndex(
                name: "IX_StockIn_IngredientId",
                table: "StockIn");

            migrationBuilder.DropColumn(
                name: "IngredientId",
                table: "StockOut");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "StockOut");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "StockOut");

            migrationBuilder.DropColumn(
                name: "IngredientId",
                table: "StockIn");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "StockIn");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "StockIn");

            migrationBuilder.CreateTable(
                name: "StockInDetail",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockInId = table.Column<long>(type: "bigint", nullable: false),
                    IngredientId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    NguoiTao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NguoiCapNhat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayCapNhat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockInDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockInDetail_Ingredient_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockInDetail_StockIn_StockInId",
                        column: x => x.StockInId,
                        principalTable: "StockIn",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockOutDetail",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockOutId = table.Column<long>(type: "bigint", nullable: false),
                    IngredientId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    NguoiTao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NguoiCapNhat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayCapNhat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockOutDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockOutDetail_Ingredient_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockOutDetail_StockOut_StockOutId",
                        column: x => x.StockOutId,
                        principalTable: "StockOut",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockInDetail_IngredientId",
                table: "StockInDetail",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_StockInDetail_StockInId",
                table: "StockInDetail",
                column: "StockInId");

            migrationBuilder.CreateIndex(
                name: "IX_StockOutDetail_IngredientId",
                table: "StockOutDetail",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_StockOutDetail_StockOutId",
                table: "StockOutDetail",
                column: "StockOutId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockInDetail");

            migrationBuilder.DropTable(
                name: "StockOutDetail");

            migrationBuilder.AddColumn<long>(
                name: "IngredientId",
                table: "StockOut",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "StockOut",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "StockOut",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "IngredientId",
                table: "StockIn",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "StockIn",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "StockIn",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_StockOut_IngredientId",
                table: "StockOut",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_StockIn_IngredientId",
                table: "StockIn",
                column: "IngredientId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockIn_Ingredient_IngredientId",
                table: "StockIn",
                column: "IngredientId",
                principalTable: "Ingredient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockOut_Ingredient_IngredientId",
                table: "StockOut",
                column: "IngredientId",
                principalTable: "Ingredient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
