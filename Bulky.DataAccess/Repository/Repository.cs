
using BulkyBook.BulkyBookDataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
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
			_db.Products.Include(u => u.Category).Include(u => u.CategoryId);
		}
		public void Add(T entity)
		{
			this.dbSet.Add(entity);
		}

		public T Get(Expression<Func<T, bool>> predicate, string? includeProperties = null, bool tracked = false)
		{
			IQueryable<T> query;

            if (tracked)
			{
				query = dbSet;

			}
			else
			{
				query= dbSet.AsNoTracking();
			}
			query = dbSet.Where(predicate);
			if (!string.IsNullOrEmpty(includeProperties))
			{
				foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(property);
				}
			}
			return query.FirstOrDefault();
		}

		public IEnumerable<T> GetAll(Expression<Func<T, bool>>? predicate, string? includeProperties = null)
		{
			IQueryable<T> query = dbSet;
            
			if (predicate != null)
			{
                query = dbSet.Where(predicate);
            }

            if (!string.IsNullOrEmpty(includeProperties))
			{
				foreach (var property in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(property);
				}
			}
			return query.ToList();
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
