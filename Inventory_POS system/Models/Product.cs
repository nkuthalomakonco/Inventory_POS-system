

namespace Inventory_POS_system.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Barcode { get; set; } 

    }
}
