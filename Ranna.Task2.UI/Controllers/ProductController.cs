using Microsoft.AspNetCore.Mvc;
using Ranna.Task2.Business.Dto;
using Ranna.Task2.Business.Interfaces;
using Ranna.Task2.Business.Responses;
using Ranna.Task2.UI.HelperMethods;
using Ranna.Task2.UI.Models;

namespace Ranna.Task2.UI.Controllers
{
	public class ProductController(IProductService productService) : Controller
	{
		public async Task<IActionResult> Index()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> GetProductById(int id)
		{
			var product = await productService.GetProduct(id);
			if (product == null)
			{
				return NotFound();
			}
			return Json(new
			{
				product.Id,
				product.Kod,
				product.Ad,
				product.Fiyat,
				product.Bilgi,
				product.OlusturmaTarihi,
				Resim = product.Resim != null ? "data:image/png;base64," + Convert.ToBase64String(product.Resim) : ""
			});	
		}


		[HttpPost]
		public async Task<IActionResult> GetProducts()
		{

			var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
			var start = Convert.ToInt32(HttpContext.Request.Form["start"].FirstOrDefault());
			var length = Convert.ToInt32(HttpContext.Request.Form["length"].FirstOrDefault());
			var searchValue = HttpContext.Request.Form["search[value]"].FirstOrDefault();


			ProductDtoForDataTable result = await productService.GetProducts(draw,start,length,searchValue);

			return Json(new
			{
				draw = result.Draw,
				recordsTotal = result.RecordsTotal,
				recordsFiltered = result.RecordsFiltered,
				data = result.Data
			});
		}


		[HttpDelete]
		public async Task<IActionResult> DeleteProducts(List<int> ids)
		{
			await productService.DeleteProducts(ids);
			return NoContent();

		}


		[HttpPost]
		public async Task<IActionResult> AddProduct(ProductModel model)
		{
			if (!IFormFileHelper.IsValidImage(model.Resim))
			{

				return BadRequest("Sadece resim dosyası yükleyebilirsiniz");
			}
			var productCreateDto = new ProductCreateDto
			{
				Kod = model.Kod,
				Ad = model.Ad,
				Fiyat = model.Fiyat,
				Bilgi = model.Bilgi,
				Resim = IFormFileHelper.ToByteArray(model.Resim), 
			};
			var response = await productService.AddProduct(productCreateDto);
			
			if (response.Status == ResponseStatus.ValidationError)
			{
				
				return BadRequest(response.ValidationErrors);
			}
			else if (response.Status == ResponseStatus.Failed)
			{
				return BadRequest(response.Message);
			}
			else
			{
				return CreatedAtAction(nameof(GetProductById), new { id = response.Data.Id }, response.Data);
			}
		}


		[HttpPut]
		public async Task<IActionResult> UpdateProduct(ProductModel model)
		{
			
			
			var productUpdateDto = new ProductUpdateDto
			{
				Id = model.Id,
				Kod = model.Kod,
				Ad = model.Ad,
				Fiyat = model.Fiyat,
				Bilgi = model.Bilgi,
				Resim = IFormFileHelper.ToByteArray(model.Resim),
			};
			var response = await productService.UpdateProduct(productUpdateDto);

			if (response.Status == ResponseStatus.ValidationError)
			{
				return BadRequest(response.ValidationErrors);
			}
			else if (response.Status == ResponseStatus.Failed)
			{
				return BadRequest(response.Message);
			}
			else
			{
				return Ok(response.Data);
			}
		}
	}
}
