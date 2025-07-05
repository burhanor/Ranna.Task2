using Ranna.Task2.Business.Dto;
using Ranna.Task2.Business.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ranna.Task2.Business.Interfaces
{
	public interface IProductService
	{
		Task<ResponseContainer<ProductDto>> AddProduct(ProductCreateDto productCreateDto, CancellationToken cancellationToken = default);
		Task<ResponseContainer<ProductDto>> UpdateProduct(ProductUpdateDto productUpdateDto, CancellationToken cancellationToken = default);
		Task<ResponseContainer> DeleteProduct(int id, CancellationToken cancellationToken = default);
		Task<ResponseContainer> DeleteProducts(List<int> ids, CancellationToken cancellationToken = default);
		Task<ProductDto?> GetProduct(int id, CancellationToken cancellationToken = default);
		Task<List<ProductDto>> GetProducts(string? searchTerm = null, int? page=null, int? pageSize = null, CancellationToken cancellationToken= default);
		Task<ProductDtoForDataTable> GetProducts(string draw, int start, int length, string searchTerm, CancellationToken cancellationToken = default);
	}
}
