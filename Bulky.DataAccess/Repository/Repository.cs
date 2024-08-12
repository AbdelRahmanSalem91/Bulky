
using BulkyBook.BulkyBookDataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BulkyBook.DataAccess.Repository
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly ApplicationDbContext _db;
		internal DbSet<T> dbSet;
		public Repository(ApplicationDbContext db)
		{
			_db = db;
			this.dbSet = _db.Set<T>();
		}
		public void Add(T entity)
		{
			this.dbSet.Add(entity);
		}

		public T Get(Expression<Func<T, bool>> predicate)
		{
			IQueryable<T> query = dbSet.Where(predicate);
			return query.FirstOrDefault();
		}

		public IEnumerable<T> GetAll()
		{
			return dbSet.ToList();
		}

		public void Remove(T entity)
		{
			dbSet.Remove(entity);
		}

		public void RemoveRange(IEnumerable<T> entities)
		{
			dbSet.RemoveRange(entities);
		}
	}
}
