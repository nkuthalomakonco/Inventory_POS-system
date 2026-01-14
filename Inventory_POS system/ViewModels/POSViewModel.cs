
/* 
POSViewModel: Manage cart & checkout

Commands: AddToCart, RemoveFromCart, Checkout
*/

using Inventory_POS_system.Models;
using System.Collections.ObjectModel;
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
            set { _selectedCartItem = value; OnPropertyChanged(); UpdateCommands(); }
        }

        // Totals
        public decimal Total => Cart.Sum(i => i.LineTotal);

        // Commands
        public ICommand AddToCartCommand { get; }
        public ICommand RemoveFromCartCommand { get; }
        public ICommand CheckoutCommand { get; }

        public POSViewModel(InventoryViewModel inventory)
        {
            _inventory = inventory;

            AddToCartCommand = new RelayCommand(AddToCart, CanAddToCart);
            RemoveFromCartCommand = new RelayCommand(RemoveFromCart, CanRemoveFromCart);
            CheckoutCommand = new RelayCommand(Checkout, CanCheckout);

            Cart.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(Total));
                UpdateCommands();
            };
        }

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
        }
    }
}
