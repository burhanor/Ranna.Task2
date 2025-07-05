using Ranna.Task2.Entities.Interfaces;

namespace Ranna.Task2.DataAccess.Interfaces
{
	public interface IUow
	{
		IRepository<T> GetRepository<T>() where T : class, IEntityBase, new();
		Task BeginTransactionAsync(CancellationToken cancellationToken = default);
		Task CommitTransactionAsync(CancellationToken cancellationToken = default);
		Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
		Task<int> SaveChangesAsync(CancellationToken cancellationToken);
		int SaveChanges();
	}
}
