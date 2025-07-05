using AutoMapper;
using Ranna.Task2.Business.Dto;
using Ranna.Task2.Entities.Models;

namespace Ranna.Task2.Business.Mappings
{
	internal class ProductMapping:Profile
	{
		public ProductMapping()
		{
			CreateMap<ProductCreateDto, Product>().ForMember(dest => dest.Resim, opt =>
			{
				opt.Condition(src => src.Resim != null);
			}); 
			CreateMap<ProductUpdateDto, Product>().ForMember(dest => dest.Resim, opt =>
			{
				opt.Condition(src => src.Resim != null);
			});
			CreateMap<Product, ProductDto>().ForMember(dest => dest.Resim, opt =>
			{
				opt.Condition(src => src.Resim != null);
			});
		}
	}
}
