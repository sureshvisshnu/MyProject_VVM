using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class catalogmodelchange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_Products_ProductId",
                table: "Inventories");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryBatches_Products_ProductId",
                table: "InventoryBatches");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Products_ProductId",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseDetails_Products_ProductId",
                table: "PurchaseDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_saledetails_Products_ProductId",
                table: "saledetails");

            migrationBuilder.DropForeignKey(
                name: "FK_stockmovementdetails_Products_ProductId",
                table: "stockmovementdetails");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ProductFamilies");

            migrationBuilder.AddColumn<float>(
                name: "CostPrice",
                table: "CatalogItems",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "CatalogItems",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "HSNCode",
                table: "CatalogItems",
                type: "varchar(14)",
                maxLength: 14,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "MaterialId",
                table: "CatalogItems",
                type: "varchar(14)",
                maxLength: 14,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<float>(
                name: "Msrp",
                table: "CatalogItems",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ProductFamilyId",
                table: "CatalogItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "PurchasePrice",
                table: "CatalogItems",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "QuantityOnHand",
                table: "CatalogItems",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "RetailPrice",
                table: "CatalogItems",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RetailUOM",
                table: "CatalogItems",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "RetailXFactor",
                table: "CatalogItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UOM",
                table: "CatalogItems",
                type: "varchar(14)",
                maxLength: 14,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "UseHsnTax",
                table: "CatalogItems",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "WholdSalePrice",
                table: "CatalogItems",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WholesaleUOM",
                table: "CatalogItems",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "WholesaleXFactor",
                table: "CatalogItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItems_ProductFamilyId",
                table: "CatalogItems",
                column: "ProductFamilyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogItems_CatalogItems_ProductFamilyId",
                table: "CatalogItems",
                column: "ProductFamilyId",
                principalTable: "CatalogItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_CatalogItems_ProductId",
                table: "Inventories",
                column: "ProductId",
                principalTable: "CatalogItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryBatches_CatalogItems_ProductId",
                table: "InventoryBatches",
                column: "ProductId",
                principalTable: "CatalogItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_CatalogItems_ProductId",
                table: "Prescriptions",
                column: "ProductId",
                principalTable: "CatalogItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseDetails_CatalogItems_ProductId",
                table: "PurchaseDetails",
                column: "ProductId",
                principalTable: "CatalogItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_saledetails_CatalogItems_ProductId",
                table: "saledetails",
                column: "ProductId",
                principalTable: "CatalogItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_stockmovementdetails_CatalogItems_ProductId",
                table: "stockmovementdetails",
                column: "ProductId",
                principalTable: "CatalogItems",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatalogItems_CatalogItems_ProductFamilyId",
                table: "CatalogItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_CatalogItems_ProductId",
                table: "Inventories");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryBatches_CatalogItems_ProductId",
                table: "InventoryBatches");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_CatalogItems_ProductId",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseDetails_CatalogItems_ProductId",
                table: "PurchaseDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_saledetails_CatalogItems_ProductId",
                table: "saledetails");

            migrationBuilder.DropForeignKey(
                name: "FK_stockmovementdetails_CatalogItems_ProductId",
                table: "stockmovementdetails");

            migrationBuilder.DropIndex(
                name: "IX_CatalogItems_ProductFamilyId",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "CostPrice",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "HSNCode",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "MaterialId",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "Msrp",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "ProductFamilyId",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "PurchasePrice",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "QuantityOnHand",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "RetailPrice",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "RetailUOM",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "RetailXFactor",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "UOM",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "UseHsnTax",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "WholdSalePrice",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "WholesaleUOM",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "WholesaleXFactor",
                table: "CatalogItems");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_CatalogItems_Id",
                        column: x => x.Id,
                        principalTable: "CatalogItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProductFamilies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFamilies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductFamilies_CatalogItems_Id",
                        column: x => x.Id,
                        principalTable: "CatalogItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ProductFamilyId = table.Column<long>(type: "bigint", nullable: false),
                    CostPrice = table.Column<float>(type: "float", nullable: false),
                    HSNCode = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaterialId = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Msrp = table.Column<float>(type: "float", nullable: false),
                    PurchasePrice = table.Column<float>(type: "float", nullable: false),
                    QuantityOnHand = table.Column<double>(type: "double", nullable: false),
                    RetailPrice = table.Column<float>(type: "float", nullable: false),
                    RetailUOM = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RetailXFactor = table.Column<int>(type: "int", nullable: false),
                    UOM = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UseHsnTax = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    WholdSalePrice = table.Column<float>(type: "float", nullable: false),
                    WholesaleUOM = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WholesaleXFactor = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_CatalogItems_Id",
                        column: x => x.Id,
                        principalTable: "CatalogItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_ProductFamilies_ProductFamilyId",
                        column: x => x.ProductFamilyId,
                        principalTable: "ProductFamilies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductFamilyId",
                table: "Products",
                column: "ProductFamilyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Products_ProductId",
                table: "Inventories",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryBatches_Products_ProductId",
                table: "InventoryBatches",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Products_ProductId",
                table: "Prescriptions",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseDetails_Products_ProductId",
                table: "PurchaseDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_saledetails_Products_ProductId",
                table: "saledetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_stockmovementdetails_Products_ProductId",
                table: "stockmovementdetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
