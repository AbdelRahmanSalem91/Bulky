﻿
namespace BulkyBook.Models.ViewModel
{
    public class OrderVM
    {
        public OrderHeader OrderHeader { get; set; } = new OrderHeader();
        public IEnumerable<OrderDetail> OrderDetails { get; set; }
    }
}
