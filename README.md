# Inventory / POS (Point of Sale) Application
<img width="1250" height="1000" alt="Image" src="https://github.com/user-attachments/assets/b38f25f3-fd65-4603-8b46-0dfe7abbbfe4" />
A desktop Inventory & POS system WPF, .NET 8, and MVVM.
The goal of this project is to create a real, production-ready application that can be easily extended, improved, or deployed.

📌 Project Goals

Build a complete Inventory & POS system step by step

Follow clean architecture and MVVM best practices

Start simple (JSON storage) and scale to a real database

Keep the codebase modular and extensible

🚀 Features
Core Features
Inventory Management

Add, edit, and delete products

Track:

Product ID

Name

Price

Stock quantity

Sales / POS

Add products to a cart

Checkout process

Automatically reduce stock

Calculate:

Total amount

Taxes

Change

Reporting

View daily sales

View current stock levels

Persistence

Save inventory and sales data to JSON

Designed to switch to a database later (SQL Server)

Nice-to-Have Features (Planned)

Product categories

Discounts

Barcode scanning

Receipt printing

🛠 Technology Stack
Category	Technology
Framework	.NET 7/8/9
UI	WPF
Architecture	MVVM
Data Storage	JSON (upgradeable to SQL Server)
IDE	Visual Studio
📂 Project Structure
InventoryPOS/
│
├── Models/
│   ├── Product.cs
│   ├── Sale.cs
│
├── ViewModels/
│   ├── InventoryViewModel.cs
│   ├── POSViewModel.cs
│   └── BaseViewModel.cs
│
├── Views/
│   ├── InventoryView.xaml
│   ├── POSView.xaml
│
├── Services/
│   └── JsonService.cs
│
├── Data/
│   ├── products.json
│   └── sales.json
│
├── App.xaml
└── MainWindow.xaml

🧱 Domain Models
Product

Represents an item in inventory.

Properties:

Id

Name

Price

StockQuantity

(Optional) Category

Sale

Represents a completed transaction.

Properties:

SaleId

Date

Items sold

Total amount

🔁 MVVM Architecture
ViewModels
InventoryViewModel

Manages inventory operations.

Commands:

AddProduct

EditProduct

DeleteProduct

POSViewModel

Handles the point-of-sale logic.

Commands:

AddToCart

RemoveFromCart

Checkout

Data Binding

Uses ObservableCollection<T> for real-time UI updates

Commands are bound to UI actions

No business logic in Views

🎨 UI Design
Inventory View (InventoryView.xaml)

DataGrid → Display products

Buttons → Add / Edit / Delete

Input Fields → Name, Price, Stock

POS View (POSView.xaml)

ComboBox / DataGrid → Available products

ListBox → Cart items

Buttons → Add to Cart, Checkout

TextBlock → Total price

💾 Data Persistence (JSON)
JsonService

Handles saving and loading data.

Responsibilities:

Load products from JSON on startup

Save products and sales on shutdown

Keep persistence logic separate from UI & business logic

🔄 Application Lifecycle
1. Startup

Load products and sales from JSON

Populate ViewModels

Bind data to UI

2. Runtime

User manages inventory

POS handles checkout

Stock updates automatically

Sales are recorded

UI updates via MVVM binding

3. Shutdown

Inventory and sales are saved to JSON

🧪 Testing & Validation

Prevent negative stock values

Validate user input:

Price > 0

Quantity ≥ 0

Improve UI responsiveness

Add reporting filters

🔮 Future Enhancements

Migrate to SQLite or SQL Server

Add user authentication (Admin vs Cashier)

Barcode scanning support

Receipt printing

Cloud synchronization

📜 License

This project is open for learning and extension.
Feel free to fork, modify, and improve it.
