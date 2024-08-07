

using BulkyBook.BulkyBookDataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;

namespace BulkyBook.DataAccess.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		public ICategoryRepository Category {  get; private set; }

		private ApplicationDbContext _categoryRepo;
		public UnitOfWork(ApplicationDbContext db)
		{
			_categoryRepo = db;
			Category = new CategoryRepository(_categoryRepo);
		}

		public void Save()
		{
			_categoryRepo.SaveChanges();
		}
	}
}
