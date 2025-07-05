using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Ranna.Task2.Api.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Ranna.Task2.Api.Helpers
{
	public  class TokenHelper(IOptions<TokenOption> options)
	{
		private readonly TokenOption _tokenOption=options.Value;
		public  string GenerateToken()
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOption.SecretKey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var expires = DateTime.Now.AddMinutes(_tokenOption.TokenValidtyInMinutes);
			var token = new JwtSecurityToken(
				issuer: _tokenOption.Issuer,
				audience: _tokenOption.Audience,
				expires: expires,
				signingCredentials: creds
			);
			return new JwtSecurityTokenHandler().WriteToken(token);
		}


		public bool ValidateToken(string token)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.UTF8.GetBytes(_tokenOption.SecretKey);
			tokenHandler.ValidateToken(token, new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(key),
				ValidateIssuer = true,
				ValidIssuer = _tokenOption.Issuer,
				ValidateAudience = true,
				ValidAudience = _tokenOption.Audience,
				ValidateLifetime = true
			}, out SecurityToken validatedToken);
			return validatedToken != null && validatedToken.ValidTo > DateTime.UtcNow;
		}	

	}
}
