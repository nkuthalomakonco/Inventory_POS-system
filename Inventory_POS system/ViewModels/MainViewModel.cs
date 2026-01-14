

/* 
Wiring Inventory + POS Together


*/
namespace Inventory_POS_system.ViewModels
{
    public class MainViewModel
    {
        public InventoryViewModel InventoryVM { get; }
        public POSViewModel POSVM { get; }

        public MainViewModel()
        {
            InventoryVM = new InventoryViewModel();
            POSVM = new POSViewModel(InventoryVM);
        }
    }
}
