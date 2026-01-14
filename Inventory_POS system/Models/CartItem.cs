

namespace Inventory_POS_system.Models
{
    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public decimal LineTotal => Product.Price * Quantity;
    }
}
