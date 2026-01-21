using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_POS_system.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public List<CartItem> Items { get; set; }
        public decimal Total { get; set; }
        public DateTime Date { get; set; }

        public string ReceiptNumber { get; set; } 

        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
    }
}
