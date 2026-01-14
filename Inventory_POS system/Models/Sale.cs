using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_POS_system.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public List<Product> Items { get; set; }
        public decimal Total { get; set; }
        public DateTime Date { get; set; }
    }
}
