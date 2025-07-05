using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ranna.Task2.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class initDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Urunler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Kod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Fiyat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Bilgi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Resim = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Urunler", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Urunler",
                columns: new[] { "Id", "Ad", "Bilgi", "Fiyat", "Kod", "OlusturmaTarihi", "Resim" },
                values: new object[,]
                {
                    { 1, "Klavye", "Yazma aracı", 5.99m, "KOD-00001", new DateTime(2024, 9, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 2, "Mouse", "Fare", 15.49m, "KOD-00002", new DateTime(2025, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 3, "Monitör", "Görüntüleme cihazı", 200.50m, "KOD-00003", new DateTime(2025, 5, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 4, "Bilgisayar", "Masaüstü Bilgisayar", 1500.00m, "KOD-00004", new DateTime(2024, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 5, "Yazıcı", "HP Yazıcı", 300.00m, "KOD-00005", new DateTime(2025, 2, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 6, "Tarayıcı", "Toshiba Tarayıcı", 250.00m, "KOD-00006", new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 7, "Webcam", "Logitech Kamera", 50.00m, "KOD-00007", new DateTime(2025, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 8, "Kulaklık", "JBL Kulaklık", 30.00m, "KOD-00008", new DateTime(2023, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 9, "USB Bellek", "256GB USB Bellek", 20.00m, "KOD-00009", new DateTime(2025, 8, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 10, "Harici Disk", "2TB Harici Disk", 100.00m, "KOD-00010", new DateTime(2025, 11, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Urunler_Kod",
                table: "Urunler",
                column: "Kod",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Urunler");
        }
    }
}
