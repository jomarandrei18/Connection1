using Connection1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Connection1.Class
{
    public class OrderList 
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string CategName { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Abbreviation { get; set; }
    }

    public class AddOrderList : OrderList
    {
        private List<Order> _customerOrder;
        public AddOrderList()
        {
            _customerOrder = new List<Order>();
        }
        public List<Order> Add(int count, List<OrderList> _orderList)
        {
            foreach (var order in _orderList)
            {
                var _orders = new Order();

                _orders.ProductId = order.ProductId;
                _orders.OrderId = count;
                _orders.Quantity = order.Quantity;
                _orders.TotalPrice = order.Quantity * order.Price;
                _orders.AddedDate = DateTime.Now;
                _orders.AddedBy = "jomar";

                _customerOrder.Add(_orders);
            }
            return _customerOrder;
        }
    }

}
