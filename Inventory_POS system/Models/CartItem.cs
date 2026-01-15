

using System.ComponentModel;

namespace Inventory_POS_system.Models
{
    public class CartItem : INotifyPropertyChanged
    {
        public Product Product { get; set; }
        public int _quantity { get; set; }
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                    OnPropertyChanged(nameof(LineTotal)); // 🔥 REQUIRED
                }
            }
        }
        public decimal LineTotal => Product.Price * Quantity;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
