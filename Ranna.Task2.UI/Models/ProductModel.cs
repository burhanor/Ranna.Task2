using Ranna.Task2.UI.HelperMethods;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Ranna.Task2.UI.Models
{
	public class ProductModel
	{
		public int Id { get; set; }
		[DisplayName("Ürün Adı")]
		[Required(ErrorMessage = "Ürün adı alanı zorunludur.")]
		[StringLength(100, ErrorMessage = "Ürün adı en fazla 100 karakter olabilir.")]
		public string Ad { get; set; }
		[DisplayName("Ürün Kodu")]
		[Required(ErrorMessage = "Kod alanı zorunludur.")]
		[StringLength(50, ErrorMessage = "Kod en fazla 50 karakter olabilir.")]
		public string Kod { get; set; }
		[DisplayName("Fiyat")]
		[Required(ErrorMessage = "Fiyat alanı zorunludur.")]
		[Range(0.01, int.MaxValue, ErrorMessage = "Fiyat 0'dan büyük olmalıdır.")]
		[DataType(DataType.Currency)]
		public decimal Fiyat { get; set; }
		public string? Bilgi { get; set; }
		[ImageFile(ErrorMessage = "Sadece resim dosyası ekleyebilirsiniz.(Max 5MB)")]
		public IFormFile? Resim { get; set; }
	}
}
