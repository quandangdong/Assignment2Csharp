using System;

namespace BusinessObject
{
    public class OrderObject
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequireDate { get; set; }
        public DateTime ShippedDate { get; set; }
        public decimal Freight { get; set; }
        public int MemberId { get; set; }
    }
}
    