using Microsoft.EntityFrameworkCore;
using Ranna.Task2.DataAccess.Context;
using Ranna.Task2.DataAccess.Interfaces;
using Ranna.Task2.Entities.Interfaces;
using System.Linq.Expressions;

namespace Ranna.Task2.DataAccess.Repositories
{

	public class Repository<T>(AppDbContext dbContext) : IRepository<T> where T : class, IEntityBase, new()
	{
		private readonly AppDbContext dbContext = dbContext;
		private DbSet<T> Table { get => dbContext.Set<T>(); }

		public async Task AddAsync(T entity, CancellationToken cancellationToken) => await Table.AddAsync(entity, cancellationToken);

		public void Delete(T entity) => Table.Remove(entity);
		public void DeleteRange(IEnumerable<T> entities) => Table.RemoveRange(entities);
		public void Delete(Expression<Func<T, bool>> predicate) => Table.RemoveRange(Table.Where(predicate));

		public void Delete(List<int> ids) => Table.RemoveRange(Table.Where(x => ids.Contains(x.Id)));

		public bool Update(T entity)
		{
			T? existingEntity = Table.Find(entity.Id);
			if (existingEntity != null)
			{
				dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
				return true;
			}
			return false;
		}


		public async Task<T?> FindAsync(int id, bool enableTracking = false, CancellationToken cancellationToken = default)
		{
			return !enableTracking
				? await Table.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
				: await Table.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
		}
		public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate, bool enableTracking = false, CancellationToken cancellationToken = default)
		{
			IQueryable<T> query = Table;
			if (!enableTracking)
				query = query.AsNoTracking();
			return await query.Where(predicate).FirstOrDefaultAsync(cancellationToken);
		}
		public async Task<TType?> GetAsync<TType>(Expression<Func<T, TType>> select, Expression<Func<T, bool>> predicate, bool enableTracking = false, CancellationToken cancellationToken = default)

		{
			IQueryable<T> query = Table;
			if (!enableTracking)
				query = query.AsNoTracking();
			return await query.Where(predicate).Select(select).FirstOrDefaultAsync(cancellationToken);
		}

		public async Task<IList<T>> GetListAsync(Expression<Func<T, bool>>? predicate = null, bool enableTracking = false, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, int? currentPage = null, int? pageSize = null, CancellationToken cancellationToken = default)
		{
			IQueryable<T> query = Table;
			if (!enableTracking)
				query = query.AsNoTracking();
			if (predicate != null)
				query = query.Where(predicate);
			if (orderBy != null)
				query = orderBy(query);
			if (currentPage.HasValue && pageSize.HasValue)
				query = query.Skip((currentPage.Value - 1) * pageSize.Value).Take(pageSize.Value);
			return await query.ToListAsync(cancellationToken);
		}
		public async Task<IList<TType>> GetListAsync<TType>(Expression<Func<T, TType>> select, Expression<Func<T, bool>>? predicate = null, bool enableTracking = false, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, int? currentPage = null, int? pageSize = null, CancellationToken cancellationToken = default)
		{
			IQueryable<T> query = Table;
			if (!enableTracking)
				query = query.AsNoTracking();
			if (predicate != null)
				query = query.Where(predicate);
			if (orderBy != null)
				query = orderBy(query);
			if (currentPage.HasValue && pageSize.HasValue)
				query = query.Skip((currentPage.Value - 1) * pageSize.Value).Take(pageSize.Value);
			return await query.Select(select).ToListAsync(cancellationToken);
		}
		public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default) => predicate == null ? await Table.CountAsync(cancellationToken) : await Table.CountAsync(predicate, cancellationToken);
		public async Task<bool> ExistAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) => await Table.AnyAsync(predicate, cancellationToken);
		public async Task<bool> UniqueAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) => !await ExistAsync(predicate, cancellationToken);

		public IQueryable<T> Query() => Table;

		public async Task<IList<T>> ToListAsync(IQueryable<T> query, CancellationToken cancellationToken = default)
		{
			return await query.ToListAsync(cancellationToken);
		}

		public async Task<int> CountAsync(IQueryable<T> query, CancellationToken cancellationToken = default)
		{
			return await query.CountAsync();
		}
	}
}
