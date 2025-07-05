using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Ranna.Task2.Business.Dto;
using Ranna.Task2.Business.Interfaces;
using Ranna.Task2.Business.Responses;
using Ranna.Task2.Business.Validators;
using Ranna.Task2.DataAccess.Interfaces;
using Ranna.Task2.Entities.Models;
using System.Net.Http;

namespace Ranna.Task2.Business.Services
{
	public class ProductService(IUow uow,IMapper mapper, IRepository<Product> repository) : IProductService
	{
		public async Task<ResponseContainer<ProductDto>> AddProduct(ProductCreateDto productCreateDto,CancellationToken cancellationToken = default)
		{
			ResponseContainer<ProductDto> response = new();
			try
			{
				ProductValidator validationRules= new();
				Product product = mapper.Map<Product>(productCreateDto);
				var validationResult = await validationRules.ValidateAsync(product, cancellationToken);
				if (!validationResult.IsValid)
				{
					response.ValidationErrors = validationResult.Errors
						.Select(x => new ValidationError
						{
							ErrorMessage = x.ErrorMessage,
							PropertyName = x.PropertyName
						}).ToList();
					response.Status = ResponseStatus.ValidationError;
					return response;
				}
				bool codeIsUnique = await repository.UniqueAsync(x => x.Kod == product.Kod, cancellationToken);
				if(!codeIsUnique)
				{
					response.ValidationErrors.Add(new ValidationError
					{
						ErrorMessage = "Ürün kodu zaten mevcut.",
						PropertyName = nameof(product.Kod)
					});
					response.Status = ResponseStatus.ValidationError;
					return response;
				}
				product.OlusturmaTarihi = DateTime.Now;
				await repository.AddAsync(product, cancellationToken);
				await uow.SaveChangesAsync(cancellationToken);
				if (product.Id>0)
				{

					response.Data = mapper.Map<ProductDto>(product);
					response.Status = ResponseStatus.Success;
					response.Message = "Ürün başarıyla eklendi.";
				}
				else
				{
					response.Message = "Ürün eklenemedi.";
					response.Status = ResponseStatus.Failed;
				}
			}
			catch (Exception ex)
			{
				response.Message = ex.Message;
				response.Status = ResponseStatus.Exception;
			}
			return response;
		}

		public async Task<ResponseContainer> DeleteProduct(int id, CancellationToken cancellationToken = default)
		{
			return await DeleteProducts([id],cancellationToken);
		}

		public async Task<ResponseContainer> DeleteProducts(List<int> ids, CancellationToken cancellationToken = default)
		{
			ResponseContainer response = new();
			try
			{
				repository.Delete(ids);
				await uow.SaveChangesAsync(cancellationToken);
				response.Status = ResponseStatus.Success;
				response.Message = "Ürün silindi.";

			}
			catch (Exception ex)
			{
				response.Message = ex.Message;
				response.Status = ResponseStatus.Exception;
			}
			return response;
		}

		public async Task<ResponseContainer<ProductDto>> UpdateProduct(ProductUpdateDto productUpdateDto, CancellationToken cancellationToken = default)
		{
			ResponseContainer<ProductDto> response = new();
			try
			{
				ProductValidator validationRules = new();
				Product product = mapper.Map<Product>(productUpdateDto);
				var validationResult = await validationRules.ValidateAsync(product, cancellationToken);
				if (!validationResult.IsValid)
				{
					response.ValidationErrors = validationResult.Errors
						.Select(x => new ValidationError
						{
							ErrorMessage = x.ErrorMessage,
							PropertyName = x.PropertyName
						}).ToList();
					response.Status = ResponseStatus.ValidationError;
					return response;
				}
				bool codeIsUnique = await repository.UniqueAsync(x => x.Kod == product.Kod && x.Id!=product.Id, cancellationToken);
				if (!codeIsUnique)
				{
					response.ValidationErrors.Add(new ValidationError
					{
						ErrorMessage = "Ürün kodu zaten mevcut.",
						PropertyName = nameof(product.Kod)
					});
					response.Status = ResponseStatus.ValidationError;
					return response;
				}
				Product? existingProduct = await repository.FindAsync(product.Id);
				if (existingProduct == null)
				{
					response.Message = "Ürün bulunamadı.";
					response.Status = ResponseStatus.Failed;
					return response;
				}
				product.OlusturmaTarihi = existingProduct.OlusturmaTarihi;
				if (product.Resim is null) // Eski resim, yeni resim gelmeden silinmesin diye ekledim
				{
					product.Resim = existingProduct.Resim;
				}
				repository.Update(product);
				await uow.SaveChangesAsync(cancellationToken);
				response.Data = mapper.Map<ProductDto>(product);
				response.Status = ResponseStatus.Success;
				response.Message = "Ürün başarıyla güncellendi.";
			}
			catch (Exception ex)
			{
				response.Message = ex.Message;
				response.Status = ResponseStatus.Exception;
			}

			return response;
		}

		public async Task<ProductDto?> GetProduct(int id, CancellationToken cancellationToken = default)
		{
			Product? product = await repository.FindAsync(id);
			if (product == null)
			{
				return null;
			}
			return mapper.Map<ProductDto>(product);
		}

		public async Task<List<ProductDto>> GetProducts(string? searchTerm = null, int? page=null, int? pageSize = null, CancellationToken cancellationToken=default)
		{
			List<ProductDto> response = new();
			try
			{
				IQueryable<Product> query = repository.Query();
				if (!string.IsNullOrEmpty(searchTerm))
				{
					query = query.Where(x => x.Ad.Contains(searchTerm) || x.Kod.Contains(searchTerm));
				}
				if (page.HasValue && pageSize.HasValue && pageSize > 0)
				{
					if (page < 1)
						page = 1;
					if (pageSize < 1)
						pageSize = 10; 
					query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
				}
				List<Product> products = query.ToList();
				response = mapper.Map<List<ProductDto>>(products);
			}
			catch (Exception ex)
			{
			}
			return response;
		}

		// Sadece server-side datatable ile veri çekmek için ekledim
		public async Task<ProductDtoForDataTable> GetProducts(string draw,int start,int length,string searchTerm,CancellationToken cancellationToken=default)
		{

			int recordsTotal =await repository.CountAsync();

			var query = repository.Query();

			if (!string.IsNullOrEmpty(searchTerm))
			{
				query = query.Where(p => p.Ad.Contains(searchTerm) || p.Kod.Contains(searchTerm) || p.Bilgi.Contains(searchTerm) || p.Fiyat.ToString().Contains(searchTerm));
			}

			int recordsFiltered = query.Count();

			var data = query
				.OrderBy(p => p.Id)
				.Skip(start)
				.Take(length)
				.Select(p => new
				{
					p.Id,
					p.Kod,
					p.Ad,
					p.Fiyat,
					p.Bilgi,
					p.OlusturmaTarihi,
					Resim= p.Resim!=null ? "data:image/png;base64," + Convert.ToBase64String(p.Resim):""
				})
				.ToList();

			return new ProductDtoForDataTable
			{
				Draw = draw,
				RecordsTotal = recordsTotal,
				RecordsFiltered = recordsFiltered,
				Data = data
			};
		}

	}
}
