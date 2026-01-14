
using Inventory_POS_system.Models;
using Inventory_POS_system.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
/* 
InventoryViewModel: Manage products

Commands: Add, Edit, Delete

How This Works (Important)
✔ Add

Enabled only if:

Name is not empty

Price > 0

Stock ≥ 0

✔ Edit

Requires a selected product

Updates the existing object (no duplication)

✔ Delete

Removes selected product from inventory

✔ UI Updates Automatically

ObservableCollection

INotifyPropertyChanged

CanExecute logic
*/
namespace Inventory_POS_system.ViewModels
{
    public class InventoryViewModel : BaseViewModel
    {
        // Inventory List
        public ObservableCollection<Product> Products { get; set; }

        // JSON File Path
        private const string InventoryFile = "products.json";

        // Selected Product (from DataGrid/List)
        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged();
                UpdateCommandStates();
            }
        }

        // Input Fields
        private string _name;
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); UpdateCommandStates(); }
        }

        private decimal _price;
        public decimal Price
        {
            get => _price;
            set { _price = value; OnPropertyChanged(); UpdateCommandStates(); }
        }

        private int _stock;
        public int Stock
        {
            get => _stock;
            set { _stock = value; OnPropertyChanged(); UpdateCommandStates(); }
        }

        // Commands
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public InventoryViewModel()
        {
            // Load inventory
            var products = JsonService.Load<ObservableCollection<Product>>(InventoryFile);
            Products = products ?? new ObservableCollection<Product>();

            AddCommand = new RelayCommand(AddProduct, CanAddProduct);
            EditCommand = new RelayCommand(EditProduct, CanEditProduct);
            DeleteCommand = new RelayCommand(DeleteProduct, CanDeleteProduct);

            // Listen for collection changes to auto-save
            Products.CollectionChanged += (s, e) => SaveInventory();
        }
        private void SaveInventory()
        {
            JsonService.Save(InventoryFile, Products);
        }
        // ADD
        private void AddProduct()
        {
            var newProduct = new Product
            {
                Id = Products.Any() ? Products.Max(p => p.Id) + 1 : 1,
                Name = Name,
                Price = Price,
                Stock = Stock
            };

            Products.Add(newProduct);
            //ClearInputs();
        }

        private bool CanAddProduct()
        {
            return !string.IsNullOrWhiteSpace(Name)
                   && Price > 0
                   && Stock >= 0;
        }

        // EDIT
        private void EditProduct()
        {
            SelectedProduct.Name = Name;
            SelectedProduct.Price = Price;
            SelectedProduct.Stock = Stock;

            OnPropertyChanged(nameof(Products));
            ClearInputs();
        }

        private bool CanEditProduct()
        {
            return SelectedProduct != null && CanAddProduct();
        }

        // DELETE
        private void DeleteProduct()
        {
            Products.Remove(SelectedProduct);
            ClearInputs();
        }

        private bool CanDeleteProduct()
        {
            return SelectedProduct != null;
        }

        // Helpers
        private void ClearInputs()
        {
            Name = string.Empty;
            Price = 0;
            Stock = 0;
            SelectedProduct = null;
        }

        private void UpdateCommandStates()
        {
            ((RelayCommand)AddCommand).RaiseCanExecuteChanged();
            ((RelayCommand)EditCommand).RaiseCanExecuteChanged();
            ((RelayCommand)DeleteCommand).RaiseCanExecuteChanged();
        }
    }
}
