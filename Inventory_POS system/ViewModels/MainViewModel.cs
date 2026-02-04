

/* 
Wiring Inventory + POS Together


*/
using Inventory_POS_system.Services;

namespace Inventory_POS_system.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public InventoryViewModel InventoryVM { get; }
        public POSViewModel POSVM { get; }

        public MainViewModel()
        {
            InventoryVM = new InventoryViewModel();
            POSVM = new POSViewModel(InventoryVM);
        }
        public bool CanAccessInventory => AuthService.IsAdmin;
        public void RefreshPermissions()
        {
            OnPropertyChanged(nameof(CanAccessInventory));
        }
    }
}
