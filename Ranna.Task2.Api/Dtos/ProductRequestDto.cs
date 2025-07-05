namespace Ranna.Task2.Api.Dtos
{
	public class ProductRequestDto
	{
		public string Ad { get; set; }
		public string Kod { get; set; }
		public decimal Fiyat { get; set; }
		public string Bilgi { get; set; }
		public IFormFile? Resim { get; set; }
	}
}
