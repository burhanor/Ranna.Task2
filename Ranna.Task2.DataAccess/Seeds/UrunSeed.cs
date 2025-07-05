using Ranna.Task2.Entities.Models;

namespace Ranna.Task2.DataAccess.Seeds
{
	public class UrunSeed
	{

		public static List<Product> Urunler =
		[
			new() {
				Id = 1,
				Ad = "Klavye",
				Kod = "KOD-00001",
				Fiyat = 5.99m,
				Bilgi = "Yazma aracı",
				OlusturmaTarihi = new DateTime(2024, 9, 12)
			},
			new() {
				Id = 2,
				Ad = "Mouse",
				Kod = "KOD-00002",
				Fiyat = 15.49m,
				Bilgi = "Fare",
				OlusturmaTarihi = new DateTime(2025, 3, 22)
			},
			new() {
				Id = 3,
				Ad = "Monitör",
				Kod = "KOD-00003",
				Fiyat = 200.50m,
				Bilgi = "Görüntüleme cihazı",
				OlusturmaTarihi = new DateTime(2025, 5, 11)
			},
			new() {
				Id = 4,
				Ad = "Bilgisayar",
				Kod = "KOD-00004",
				Fiyat = 1500.00m,
				Bilgi = "Masaüstü Bilgisayar",
				OlusturmaTarihi = new DateTime(2024, 4, 8)
			},
			new() {
				Id = 5,
				Ad = "Yazıcı",
				Kod = "KOD-00005",
				Fiyat = 300.00m,
				Bilgi = "HP Yazıcı",
				OlusturmaTarihi = new DateTime(2025, 2, 6)
			},
			new() {
				Id = 6,
				Ad = "Tarayıcı",
				Kod = "KOD-00006",
				Fiyat = 250.00m,
				Bilgi = "Toshiba Tarayıcı",
				OlusturmaTarihi = new DateTime(2025, 1, 4)
			},
			new() {
				Id = 7,
				Ad = "Webcam",
				Kod = "KOD-00007",
				Fiyat = 50.00m,
				Bilgi = "Logitech Kamera",
				OlusturmaTarihi = new DateTime(2025, 12, 3)
			},
			new() {
				Id = 8,
				Ad = "Kulaklık",
				Kod = "KOD-00008",
				Fiyat = 30.00m,
				Bilgi = "JBL Kulaklık",
				OlusturmaTarihi = new DateTime(2023, 10, 5)
			},
			new() {
				Id = 9,
				Ad = "USB Bellek",
				Kod = "KOD-00009",
				Fiyat = 20.00m,
				Bilgi = "256GB USB Bellek",
				OlusturmaTarihi= new DateTime(2025, 8, 7)
			},
			new() {
				Id = 10,
				Ad = "Harici Disk",
				Kod = "KOD-00010",
				Fiyat = 100.00m,
				Bilgi = "2TB Harici Disk",
				OlusturmaTarihi = new DateTime(2025, 11, 9)
			}
		];
	}
}
