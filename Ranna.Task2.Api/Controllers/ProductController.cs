using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ranna.Task2.Api.Dtos;
using Ranna.Task2.Api.Helpers;
using Ranna.Task2.Business.Dto;
using Ranna.Task2.Business.Interfaces;
using Ranna.Task2.Business.Responses;

namespace Ranna.Task2.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class ProductController(IProductService productService) : ControllerBase
	{
		[HttpGet("{id}")]
		public async Task<IActionResult> GetProduct(int id)
		{
			ProductDto? product = await productService.GetProduct(id);
			if (product == null)
			{
				return NotFound();
			}
			return Ok(product);
		}

		[HttpGet]
		public async Task<IActionResult> GetProducts() 
		{
			List<ProductDto> products = await productService.GetProducts();
			return Ok(products);
		}

		[HttpPost]
		public async Task<IActionResult> AddProduct([FromForm] ProductRequestDto model, CancellationToken cancellationToken = default)
		{
			if (model == null)
			{
				return BadRequest("Ürün bilgileri eksik.");
			}
			if(!IFormFileHelper.IsValidImage(model.Resim))
			{
				return BadRequest("Geçerli bir resim dosyası yükleyin.");
			}
			ProductCreateDto productCreateDto = new()
			{
				Ad = model.Ad,
				Kod = model.Kod,
				Fiyat = model.Fiyat,
				Bilgi=model.Bilgi,
				Resim = IFormFileHelper.ToByteArray(model.Resim)
			};
			var response = await productService.AddProduct(productCreateDto, cancellationToken);
			if (response.Status == ResponseStatus.ValidationError)
			{
				return BadRequest(response.ValidationErrors);
			}
			return CreatedAtAction(nameof(GetProduct), new { id = response.Data.Id }, response.Data);
		}


		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductRequestDto model, CancellationToken cancellationToken = default)
		{
			if (model == null)
			{
				return BadRequest("Ürün bilgileri eksik.");
			}
			if (!IFormFileHelper.IsValidImage(model.Resim))
			{
				return BadRequest("Geçerli bir resim dosyası yükleyin.");
			}
			ProductUpdateDto productUpdateDto = new()
			{
				Id = id,
				Ad = model.Ad,
				Kod = model.Kod,
				Fiyat = model.Fiyat,
				Bilgi = model.Bilgi,
				Resim = IFormFileHelper.ToByteArray(model.Resim)
			};
			var response = await productService.UpdateProduct(productUpdateDto, cancellationToken);
			if (response.Status == ResponseStatus.ValidationError)
			{
				return BadRequest(response.ValidationErrors);
			}
			if (response.Status == ResponseStatus.Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, response.Message);
			}
			return Ok(response.Data);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProduct(int id, CancellationToken cancellationToken = default)
		{
			var response = await productService.DeleteProduct(id, cancellationToken);
			if (response.Status == ResponseStatus.ValidationError)
			{
				return BadRequest(response.ValidationErrors);
			}
			if (response.Status == ResponseStatus.Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, response.Message);
			}
			return NoContent();
		}
	}
}
