
using BulkyBook.BulkyBookDataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository
{
	public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
		private ApplicationDbContext _db;
        public OrderDetailRepository(ApplicationDbContext db) : base(db)
        {
			_db = db;
        }

		public void Update(OrderDetail orderDetail)
		{
			_db.OrderDetails.Update(orderDetail);
		}
	}
}
