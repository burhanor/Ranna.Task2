using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ranna.Task2.DataAccess.Context;
using Ranna.Task2.DataAccess.Interfaces;
using Ranna.Task2.DataAccess.Repositories;
using Ranna.Task2.DataAccess.UnitOfWork;

namespace Ranna.Task2.DataAccess
{
	public static class Registration
	{
		public static void AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<AppDbContext>(options =>
			{
				options.UseSqlServer(configuration.GetConnectionString("MSSQLConnection"));
			});

			services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
			services.AddScoped<IUow, Uow>();
			

		}
	}
}
