using Connection1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connection1.Class
{
    public class ForOnHoldOrderList
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }
        public List<OrderList> OnHoldOrderList { get; set; }
    }

    public class AddForOnHoldOrderList
    {
        public ForOnHoldOrderList Add(int count, List<OrderList> _orderList)
        {
            var _forOnHold = new ForOnHoldOrderList();
            _forOnHold.Id = count;
            _forOnHold.OrderDate = DateTime.Now;
            _forOnHold.CustomerName = $"Jomar{_forOnHold.Id}";
            _forOnHold.OnHoldOrderList = new List<OrderList>(_orderList);
            return _forOnHold;
        }
    }


}
