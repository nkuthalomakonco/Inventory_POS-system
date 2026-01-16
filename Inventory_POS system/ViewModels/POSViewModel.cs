
/* 
POSViewModel: Manage cart & checkout

Commands: AddToCart, RemoveFromCart, Checkout
*/

using Inventory_POS_system.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Inventory_POS_system.ViewModels
{
    public class POSViewModel : BaseViewModel
    {
        private readonly InventoryViewModel _inventory;

        // Inventory reference
        public ObservableCollection<Product> Products => _inventory.Products;

        // Cart
        public ObservableCollection<CartItem> Cart { get; } = new();

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set { _selectedProduct = value; OnPropertyChanged(); UpdateCommands(); }
        }

        private CartItem _selectedCartItem;
        public CartItem SelectedCartItem
        {
            get => _selectedCartItem;
            set
            {
                if (_selectedCartItem != null)
                    _selectedCartItem.PropertyChanged -= CartItem_PropertyChanged;

                _selectedCartItem = value;
                OnPropertyChanged();

                if (_selectedCartItem != null)
                    _selectedCartItem.PropertyChanged += CartItem_PropertyChanged;

                UpdateCommands();
            }
        }
        private void CartItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CartItem.Quantity))
            {
                OnPropertyChanged(nameof(Total));
                UpdateCommands();
            }
        }
        // Totals
        public decimal Total => Cart.Sum(i => i.LineTotal);

        // Commands
        public ICommand AddToCartCommand { get; }
        public ICommand RemoveFromCartCommand { get; }
        public ICommand CheckoutCommand { get; }
        public ICommand IncreaseQuantityCommand { get; }
        public ICommand DecreaseQuantityCommand { get; }

        public POSViewModel(InventoryViewModel inventory)
        {
            _inventory = inventory;

            AddToCartCommand = new RelayCommand(AddToCart, CanAddToCart);
            RemoveFromCartCommand = new RelayCommand(RemoveFromCart, CanRemoveFromCart);
            CheckoutCommand = new RelayCommand(Checkout, CanCheckout);
            IncreaseQuantityCommand = new RelayCommand(IncreaseQuantity, CanIncreaseQuantity);
            DecreaseQuantityCommand = new RelayCommand(DecreaseQuantity, CanDecreaseQuantity);

            Cart.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(Total));
                UpdateCommands();
            };
        }
        // INCREASE/DECREASE QUANTITY
        private void IncreaseQuantity()
        {
            if (SelectedCartItem == null)
                return;

            if (SelectedCartItem.Quantity < SelectedCartItem.Product.Stock)
            {
                SelectedCartItem.Quantity++;
                OnPropertyChanged(nameof(Total)); // 🔥 update footer total
            }
        }

        private bool CanIncreaseQuantity() => SelectedCartItem != null && SelectedCartItem.Quantity < SelectedCartItem.Product.Stock;

        private void DecreaseQuantity()
        {
            if (SelectedCartItem.Quantity > 1)
            {
                SelectedCartItem.Quantity--;
                OnPropertyChanged(nameof(Total));
            }
        }
        private bool CanDecreaseQuantity() => SelectedCartItem != null && SelectedCartItem.Quantity > 1;

        // ADD TO CART
        private void AddToCart()
        {
            var existingItem = Cart.FirstOrDefault(c => c.Product.Id == SelectedProduct.Id);

            if (existingItem != null)
            {
                if (existingItem.Quantity < SelectedProduct.Stock)
                    existingItem.Quantity++;
            }
            else
            {
                Cart.Add(new CartItem
                {
                    Product = SelectedProduct,
                    Quantity = 1
                });
            }

            OnPropertyChanged(nameof(Total));
        }

        private bool CanAddToCart()
        {
            return SelectedProduct != null && SelectedProduct.Stock > 0;
        }

        // REMOVE FROM CART
        private void RemoveFromCart()
        {
            Cart.Remove(SelectedCartItem);
            OnPropertyChanged(nameof(Total));
        }

        private bool CanRemoveFromCart()
        {
            return SelectedCartItem != null;
        }

        // CHECKOUT
        private void Checkout()
        {
            foreach (var item in Cart)
            {
                item.Product.Stock -= item.Quantity;
            }

            Cart.Clear();
            OnPropertyChanged(nameof(Total));
        }

        private bool CanCheckout()
        {
            return Cart.Any();
        }

        private void UpdateCommands()
        {
            ((RelayCommand)AddToCartCommand).RaiseCanExecuteChanged();
            ((RelayCommand)RemoveFromCartCommand).RaiseCanExecuteChanged();
            ((RelayCommand)CheckoutCommand).RaiseCanExecuteChanged();

            ((RelayCommand)IncreaseQuantityCommand).RaiseCanExecuteChanged();
            ((RelayCommand)DecreaseQuantityCommand).RaiseCanExecuteChanged();
        }
    }
}
