using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace App.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "marque",
                columns: table => new
                {
                    id_brand = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name_brand = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marque", x => x.id_brand);
                });

            migrationBuilder.CreateTable(
                name: "product_type",
                columns: table => new
                {
                    id_product_type = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name_product_type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_type", x => x.id_product_type);
                });

            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    id_product = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name_product = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    name_photo = table.Column<string>(type: "text", nullable: false),
                    uri_photo = table.Column<string>(type: "text", nullable: false),
                    id_product_type = table.Column<int>(type: "integer", nullable: true),
                    id_brand = table.Column<int>(type: "integer", nullable: true),
                    stock_real = table.Column<int>(type: "integer", nullable: true),
                    stock_min = table.Column<int>(type: "integer", nullable: false),
                    stock_max = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product", x => x.id_product);
                    table.ForeignKey(
                        name: "FK_products_brand",
                        column: x => x.id_brand,
                        principalTable: "marque",
                        principalColumn: "id_brand");
                    table.ForeignKey(
                        name: "FK_products_product_type",
                        column: x => x.id_product_type,
                        principalTable: "product_type",
                        principalColumn: "id_product_type");
                });

            migrationBuilder.CreateIndex(
                name: "IX_product_id_brand",
                table: "product",
                column: "id_brand");

            migrationBuilder.CreateIndex(
                name: "IX_product_id_product_type",
                table: "product",
                column: "id_product_type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product");

            migrationBuilder.DropTable(
                name: "marque");

            migrationBuilder.DropTable(
                name: "product_type");
        }
    }
}
