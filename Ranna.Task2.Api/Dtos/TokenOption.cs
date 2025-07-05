namespace Ranna.Task2.Api.Dtos
{
	public class TokenOption
	{
		public string Audience { get; set; }
		public string Issuer { get; set; }
		public string SecretKey { get; set; }
		public int TokenValidtyInMinutes { get; set; }
		public int RefreshTokenValidtyInDays { get; set; }
	
	}
}
