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
}
