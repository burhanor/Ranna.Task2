using Ranna.Task2.DataAccess.Context;
using Ranna.Task2.DataAccess.Interfaces;
using Ranna.Task2.DataAccess.Repositories;

namespace Ranna.Task2.DataAccess.UnitOfWork
{
	public class Uow(AppDbContext dbContext) : IUow
	{
		private readonly AppDbContext dbContext = dbContext;
		public async ValueTask DisposeAsync() => await dbContext.DisposeAsync();
		IRepository<T> IUow.GetRepository<T>() => new Repository<T>(dbContext);
		public async Task<int> SaveChangesAsync(CancellationToken cancellationToken) => await dbContext.SaveChangesAsync(cancellationToken);
		public int SaveChanges() => dbContext.SaveChanges();

		#region Transaction
		public async Task BeginTransactionAsync(CancellationToken cancellationToken = default) => await dbContext.Database.BeginTransactionAsync(cancellationToken);
		public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
		{
			await SaveChangesAsync(cancellationToken);
			await dbContext.Database.CommitTransactionAsync(cancellationToken);
		}
		public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default) => await dbContext.Database.RollbackTransactionAsync(cancellationToken);


		#endregion
	}
}
