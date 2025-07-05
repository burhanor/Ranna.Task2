using Ranna.Task2.Entities.Interfaces;

namespace Ranna.Task2.Entities.Models
{
	public class Product:IEntityBase
	{
		public int Id { get; set; }
		public string Ad { get; set; }
		public string Kod { get; set; }
		public decimal Fiyat { get; set; }
		public string? Bilgi { get; set; }
		public DateTime OlusturmaTarihi { get; set; }
		public byte[]? Resim { get; set; }
	}
}
