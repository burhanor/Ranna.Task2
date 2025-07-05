namespace Ranna.Task2.Business.Dto
{
	public class ProductCreateDto
	{
		public string Ad { get; set; }
		public string Kod { get; set; }
		public decimal Fiyat { get; set; }
		public string Bilgi { get; set; }
		public byte[]? Resim { get; set; }

	}
}
