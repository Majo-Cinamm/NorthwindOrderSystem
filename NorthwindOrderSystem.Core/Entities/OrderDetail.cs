using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindOrderSystem.Core.Entities
{
    public class OrderDetail
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public short? Quantity { get; set; }
        public float Discount { get; set; }

        // 🔥 Relaciones nullable por seguridad
        public Order Order { get; set; }
        public Product? Product { get; set; }
    }
}
