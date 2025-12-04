# ShopWeb - E-Commerce Platform

ASP.NET Core MVC e-commerce application with comprehensive product management, order processing, and analytics dashboard.

## Features

### Customer Features
- 🛍️ Browse products by category with filter
- ⭐ Product reviews and ratings (one review per product per user)
- 🛒 Shopping cart with VND currency
- 📦 Shipping calculation (20,000đ for orders < 500,000đ)
- 📱 Responsive design with gradient/dark theme
- 👤 User authentication and profile management
- 📋 Order tracking and history

### Admin Features
- 📊 Analytics Dashboard with 6 Chart.js charts:
  - Revenue Trend (profit-based)
  - Order Status Distribution
  - Products by Category
  - Top Selling Products
  - Inventory Status (stock-based)
  - Orders by Month
- 🏷️ Category Management (CRUD with icons)
- 📦 Product Management with:
  - Cost Price (giá nhập)
  - Selling Price (giá bán)
  - Stock Quantity (số lượng)
  - Profit calculation and display
- 📋 Order Management (view, update status, cancel)
- 👥 User Management

## Tech Stack

- **Framework**: ASP.NET Core 10.0 MVC
- **Authentication**: ASP.NET Core Identity
- **Database**: MySQL (Pomelo EF Core provider)
- **ORM**: Entity Framework Core
- **Frontend**: Bootstrap 5, Font Awesome, Chart.js
- **Styling**: Custom CSS with gradient/dark theme

## Database Setup

1. Ensure MySQL is running on `localhost:3306`
2. Database name: `pc_shop2`
3. Connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;port=3306;database=pc_shop2;user=root;password=;"
  }
}
```

## Installation & Running

```powershell
# Restore dependencies
dotnet restore

# Build project
dotnet build

# Run application
dotnet run --urls "http://localhost:5008"
```

Application will be available at: http://localhost:5008

## Default Admin Account

- **Email**: admin@shopweb.com
- **Password**: Admin@123

## Project Structure

```
ShopWeb/
├── Areas/
│   └── Admin/           # Admin area with controllers and views
├── Controllers/         # Public controllers
├── Data/               # DbContext and database configuration
├── Models/             # Entity models
├── Services/           # Business logic services
├── Views/              # Razor views
└── wwwroot/            # Static files (CSS, JS, images)
```

## Key Models

- **Product**: Name, Description, Price, CostPrice, Stock, ImageUrl, CategoryId
- **Category**: Name, Description, IconClass
- **Review**: ProductId, UserId, Rating, Comment
- **Order**: UserId, TotalAmount, Status, ShippingAddress
- **OrderDetail**: OrderId, ProductId, Quantity, Price

## Business Logic

### Revenue Calculation
Revenue = Profit = (Selling Price - Cost Price) × Quantity
- Only counts orders with Status = "Delivered"

### Shipping Rules
- Orders < 500,000đ: +20,000đ shipping
- Orders ≥ 500,000đ: Free shipping

### Inventory Status
- **In Stock** (Green): Stock ≥ 10
- **Low Stock** (Yellow): 1-9 units
- **Out of Stock** (Red): 0 units

### Review Rules
- One review per user per product
- Users can edit/delete their own reviews
- Rating: 1-5 stars

## Deployment Checklist

- [ ] Update connection string in `appsettings.json`
- [ ] Set environment to Production
- [ ] Configure HTTPS redirect
- [ ] Set up proper error handling
- [ ] Configure file upload limits
- [ ] Set up database backups
- [ ] Review security settings
- [ ] Test all features
- [ ] Optimize images and assets
- [ ] Configure CORS if needed

## Testing Guide

### Customer Flow
1. Browse products at http://localhost:5008
2. Filter by category
3. View product details and reviews
4. Add products to cart
5. Proceed to checkout
6. Track order status

### Admin Flow
1. Login at http://localhost:5008/Account/Login
2. Access dashboard at http://localhost:5008/Admin/Dashboard
3. Manage products with cost/stock tracking
4. View analytics and reports
5. Process orders
6. Manage categories and reviews

## Notes

- Currency: Vietnamese Dong (VND) with format N0
- All dates use server timezone
- Images stored in `wwwroot/images/products/`
- MySQL collation differences handled by manual entity loading
- Chart.js requires PascalCase property names

## License

Private project - All rights reserved
