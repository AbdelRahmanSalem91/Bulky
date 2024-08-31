
using BulkyBook.BulkyBookDataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository
{
	public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
		private ApplicationDbContext _db;
        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
			_db = db;
        }

		public void Update(ShoppingCart shoppingCart)
		{
			_db.ShoppingCarts.Update(shoppingCart);
		}
	}
}
