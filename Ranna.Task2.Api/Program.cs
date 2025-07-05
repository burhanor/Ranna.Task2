using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Ranna.Task2.Api.Dtos;
using Ranna.Task2.Api.Transformers;
using Ranna.Task2.Business;
using Ranna.Task2.DataAccess;
using Ranna.Task2.DataAccess.Context;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
	options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
}); 
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddBusinessLayer();


// Token ayar
builder.Services.Configure<TokenOption>(builder.Configuration.GetSection("Token"));
var tokenOptions = builder.Configuration.GetSection("Token").Get<TokenOption>();

builder.Services.AddAuthentication("Bearer")
	.AddJwtBearer("Bearer", options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = tokenOptions!.Issuer,
			ValidAudience = tokenOptions!.Audience,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions!.SecretKey))
		};
	});

builder.Services.AddAuthorization();




var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.MapScalarApiReference();
	app.UseDeveloperExceptionPage();
}

// Veritabaný yoksa oluþturmasý için
using (var scope = app.Services.CreateScope())
{
	var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
	db.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
