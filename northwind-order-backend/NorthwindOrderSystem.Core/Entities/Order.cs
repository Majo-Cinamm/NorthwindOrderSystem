﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindOrderSystem.Core.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public string? CustomerId { get; set; } // Nullable
        public int? EmployeeId { get; set; } // sNullable
        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public int? ShipVia { get; set; }
        public decimal? Freight { get; set; }
        public string? ShipName { get; set; }
        public string? ShipAddress { get; set; }
        public string? ShipCity { get; set; }
        public string? ShipRegion { get; set; }
        public string? ShipPostalCode { get; set; }
        public string? ShipCountry { get; set; }

        // 🔥 Relaciones también nullable
        public Customer? Customer { get; set; }
        public Employee? Employee { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
