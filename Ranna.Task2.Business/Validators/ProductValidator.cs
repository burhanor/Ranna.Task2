using FluentValidation;
using Ranna.Task2.Entities.Models;

namespace Ranna.Task2.Business.Validators
{
	internal class ProductValidator:AbstractValidator<Product>
	{

		public ProductValidator(bool isUpdate=false)
		{
			if (isUpdate)
			{
				RuleFor(m=>m.Id)
					.NotEmpty().WithMessage("Ürün ID'si boş olamaz.")
					.GreaterThan(0).WithMessage("Ürün ID'si 0'dan büyük olmalıdır.");
			}
			RuleFor(x => x.Ad)
				.NotEmpty().WithMessage("Ürün adı boş olamaz.")
				.MaximumLength(100).WithMessage("Ürün adı en fazla 100 karakter olabilir.");
			RuleFor(x => x.Kod)
				.NotEmpty().WithMessage("Ürün kodu boş olamaz.")
				.MaximumLength(50).WithMessage("Ürün kodu en fazla 50 karakter olabilir.");
			RuleFor(x => x.Fiyat)
				.NotEmpty().WithMessage("Ürün fiyatı boş olamaz.")
				.GreaterThan(0).WithMessage("Ürün fiyatı 0'dan büyük olmalıdır.");

			
		}
	}
}
