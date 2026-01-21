/*
 POSViewModel
 ------------
 Responsible for managing Point-of-Sale operations:
 - Product selection
 - Cart management
 - Quantity updates
 - Barcode scanning
 - Checkout and sale persistence

 Commands exposed:
 - AddToCart
 - RemoveFromCart
 - IncreaseQuantity / DecreaseQuantity
 - ScanBarcode
 - Checkout
*/

using Inventory_POS_system.Models;
using Inventory_POS_system.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Inventory_POS_system.ViewModels
{
    /// <summary>
    /// ViewModel that handles POS cart logic, totals calculation,
    /// barcode scanning, and checkout workflow.
    /// </summary>
    public class POSViewModel : BaseViewModel
    {
        private readonly InventoryViewModel _inventory;

        #region Inventory

        /// <summary>
        /// Reference to inventory products (shared with InventoryViewModel)
        /// </summary>
        public ObservableCollection<Product> Products => _inventory.Products;

        #endregion

        #region Cart

        /// <summary>
        /// Items currently added to the cart
        /// </summary>
        public ObservableCollection<CartItem> Cart { get; } = new();

        private Product _selectedProduct;

        /// <summary>
        /// Product selected from the product list
        /// </summary>
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged();
                UpdateCommands();
            }
        }

        private CartItem _selectedCartItem;

        /// <summary>
        /// Currently selected cart item (used for quantity changes/removal)
        /// </summary>
        public CartItem SelectedCartItem
        {
            get => _selectedCartItem;
            set
            {
                // Unsubscribe from old item changes
                if (_selectedCartItem != null)
                    _selectedCartItem.PropertyChanged -= CartItem_PropertyChanged;

                _selectedCartItem = value;
                OnPropertyChanged();

                // Subscribe to quantity changes
                if (_selectedCartItem != null)
                    _selectedCartItem.PropertyChanged += CartItem_PropertyChanged;

                UpdateCommands();
            }
        }

        /// <summary>
        /// Reacts to cart item property changes (e.g. Quantity)
        /// </summary>
        private void CartItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CartItem.Quantity))
            {
                OnPropertyChanged(nameof(Total));
                UpdateCommands();
            }
        }

        #endregion

        #region Totals / Pricing

        /// <summary>
        /// Cart subtotal (sum of all line totals)
        /// </summary>
        public decimal Total => Cart.Sum(i => i.LineTotal);

        private decimal _taxRate = 0.15m;

        /// <summary>
        /// Tax rate applied to subtotal (default 15%)
        /// </summary>
        public decimal TaxRate
        {
            get => _taxRate;
            set
            {
                _taxRate = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalWithTax));
            }
        }

        private decimal _discount;

        /// <summary>
        /// Discount applied before tax
        /// </summary>
        public decimal Discount
        {
            get => _discount;
            set
            {
                _discount = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalWithTax));
            }
        }

        /// <summary>
        /// Final total including tax and discount
        /// </summary>
        public decimal TotalWithTax => (Total - Discount) * (1 + TaxRate);

        #endregion

        #region Barcode Scanning

        private string _scannedBarcode;

        /// <summary>
        /// Barcode input from scanner or manual entry
        /// </summary>
        public string ScannedBarcode
        {
            get => _scannedBarcode;
            set
            {
                _scannedBarcode = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand AddToCartCommand { get; }
        public ICommand RemoveFromCartCommand { get; }
        public ICommand CheckoutCommand { get; }
        public ICommand IncreaseQuantityCommand { get; }
        public ICommand DecreaseQuantityCommand { get; }
        public ICommand ScanBarcodeCommand { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes POSViewModel with inventory reference
        /// </summary>
        public POSViewModel(InventoryViewModel inventory)
        {
            _inventory = inventory;

            // Command bindings
            AddToCartCommand = new RelayCommand(AddToCart, CanAddToCart);
            RemoveFromCartCommand = new RelayCommand(RemoveFromCart, CanRemoveFromCart);
            CheckoutCommand = new RelayCommand(Checkout, CanCheckout);
            IncreaseQuantityCommand = new RelayCommand(IncreaseQuantity, CanIncreaseQuantity);
            DecreaseQuantityCommand = new RelayCommand(DecreaseQuantity, CanDecreaseQuantity);
            ScanBarcodeCommand = new RelayCommand(ScanBarcode);

            // Recalculate totals when cart changes
            Cart.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(Total));
                UpdateCommands();
            };
        }

        #endregion

        #region Quantity Control

        /// <summary>
        /// Increase quantity of selected cart item
        /// </summary>
        private void IncreaseQuantity()
        {
            if (SelectedCartItem == null) return;

            if (SelectedCartItem.Quantity < SelectedCartItem.Product.Stock)
                SelectedCartItem.Quantity++;
        }

        private bool CanIncreaseQuantity() =>
            SelectedCartItem != null &&
            SelectedCartItem.Quantity < SelectedCartItem.Product.Stock;

        /// <summary>
        /// Decrease quantity of selected cart item
        /// </summary>
        private void DecreaseQuantity()
        {
            if (SelectedCartItem.Quantity > 1)
                SelectedCartItem.Quantity--;
        }

        private bool CanDecreaseQuantity() =>
            SelectedCartItem != null && SelectedCartItem.Quantity > 1;

        #endregion

        #region Barcode Logic

        /// <summary>
        /// Finds product by barcode and adds it to cart
        /// </summary>
        private void ScanBarcode()
        {
            if (string.IsNullOrWhiteSpace(ScannedBarcode))
                return;

            var product = Products.FirstOrDefault(p => p.Barcode == ScannedBarcode);
            if (product != null)
                AddToCart(product);

            ScannedBarcode = string.Empty;
        }

        #endregion

        #region Cart Operations

        /// <summary>
        /// Adds selected product to cart
        /// </summary>
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

            OnPropertyChanged(nameof(TotalWithTax));
        }

        /// <summary>
        /// Adds product directly (used by barcode scanning)
        /// </summary>
        private void AddToCart(Product product)
        {
            if (product == null) return;

            var item = Cart.FirstOrDefault(c => c.Product == product);
            if (item != null)
            {
                if (item.Quantity < product.Stock)
                    item.Quantity++;
            }
            else
            {
                Cart.Add(new CartItem { Product = product, Quantity = 1 });
            }

            OnPropertyChanged(nameof(TotalWithTax));
        }

        private bool CanAddToCart() =>
            SelectedProduct != null && SelectedProduct.Stock > 0;

        /// <summary>
        /// Removes selected item from cart
        /// </summary>
        private void RemoveFromCart()
        {
            Cart.Remove(SelectedCartItem);
            OnPropertyChanged(nameof(Total));
        }

        private bool CanRemoveFromCart() =>
            SelectedCartItem != null;

        #endregion

        #region Checkout / Persistence

        /// <summary>
        /// Finalizes sale, updates stock, and saves transaction
        /// </summary>
        private void Checkout()
        {
            foreach (var item in Cart)
                item.Product.Stock -= item.Quantity;

            SaveSale();
            Cart.Clear();

            OnPropertyChanged(nameof(Total));
        }

        /// <summary>
        /// Persists sale to JSON storage
        /// </summary>
        private void SaveSale()
        {
            var sale = new Sale
            {
                Date = DateTime.Now,
                Items = Cart.Select(c =>
                    new CartItem { Product = c.Product, Quantity = c.Quantity }).ToList(),
                Total = Total,
                Tax = Total * TaxRate,
                Discount = Discount
            };

            var sales = JsonService.Load<List<Sale>>("sales.json") ?? new List<Sale>();
            sales.Add(sale);
            JsonService.Save("sales.json", sales);
        }

        private bool CanCheckout() => Cart.Any();

        #endregion

        #region Helpers

        /// <summary>
        /// Refreshes CanExecute state for all commands
        /// </summary>
        private void UpdateCommands()
        {
            ((RelayCommand)AddToCartCommand).RaiseCanExecuteChanged();
            ((RelayCommand)RemoveFromCartCommand).RaiseCanExecuteChanged();
            ((RelayCommand)CheckoutCommand).RaiseCanExecuteChanged();
            ((RelayCommand)IncreaseQuantityCommand).RaiseCanExecuteChanged();
            ((RelayCommand)DecreaseQuantityCommand).RaiseCanExecuteChanged();
        }

        #endregion
    }
}
