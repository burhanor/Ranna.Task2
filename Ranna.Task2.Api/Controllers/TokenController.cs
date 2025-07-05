using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Ranna.Task2.Api.Dtos;
using Ranna.Task2.Api.Helpers;

namespace Ranna.Task2.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TokenController(IOptions<TokenOption> options) : ControllerBase
	{
		[HttpGet]
		public IActionResult GetToken()
		{
			TokenHelper tokenHelper = new TokenHelper(options);
			string token = tokenHelper.GenerateToken(); 
			return Ok(new { Token = token });
		}
	}
}
