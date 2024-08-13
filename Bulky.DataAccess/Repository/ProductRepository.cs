
using BulkyBook.BulkyBookDataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository
{
	public class ProductRepository : Repository<Product>, IProductRepository
    {
		private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
			_db = db;
        }

		public void Update(Product product)
		{
			var objFromDb = _db.Products.FirstOrDefault(x => x.Id == product.Id);

			if (objFromDb != null)
			{
				objFromDb.Title = product.Title;
				objFromDb.ISBN = product.ISBN;
				objFromDb.Price = product.Price;
				objFromDb.Price50 = product.Price50;
				objFromDb.Price100 = product.Price100;
				objFromDb.ListPrice = product.ListPrice;
				objFromDb.Description = product.Description;
				objFromDb.Category = product.Category;
				objFromDb.CategoryId = product.CategoryId;
				objFromDb.Author = product.Author;

				if (product.ImageUrl != null)
				{
					objFromDb.ImageUrl = product.ImageUrl;
				}
			}
		}
	}
}
