using System.ComponentModel.DataAnnotations;

namespace Ranna.Task2.UI.HelperMethods
{
	public class ImageFileAttribute: ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var file = value as IFormFile;

			if (file == null)
				return ValidationResult.Success;

			var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

			if (!extension.StartsWith("image"))
			{
				return new ValidationResult($"Sadece resim dosyası ekleyebilirsiniz");
			}

			if (file.Length > 5 * 1024 * 1024)
			{
				return new ValidationResult("Dosya boyutu 5MB'yi geçmemelidir.");
			}

			return ValidationResult.Success;
		}
	}
}
