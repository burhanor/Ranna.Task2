using Microsoft.Extensions.DependencyInjection;
using Ranna.Task2.Business.Interfaces;
using Ranna.Task2.Business.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ranna.Task2.Business
{
	public static class Registration
	{
		public static void AddBusinessLayer(this IServiceCollection services)
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			services.AddAutoMapper(cfg => cfg.AddMaps(assembly)); 

			services.AddScoped<IProductService, ProductService>();
		}
	}
}
