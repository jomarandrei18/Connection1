using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connection1.Entities
{
    public class Order
    {
        [Key]
        public int OrId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime AddedDate { get; set; }
        public string AddedBy { get; set; }
    }

    public class OrderSummary
    {
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class OrderSummaryRange
    {
        public DateTime StartDate { get; set; }
        public decimal TotalPriceEndDate { get; set; }
        public decimal TotalPriceStartDate { get; set; }
    }

}
