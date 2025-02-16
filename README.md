# Product Inventory API
## Overview
The **Product Inventory API** is a backend service built using **.NET** that provides inventory management capabilities. It allows users to manage products and stock levels efficiently. The API includes a **Swagger UI** for easy interaction and testing.
## Features
- CRUD operations for products
- Stock management
- Swagger UI for API testing
- Database integration with **SQL Server**
## Technologies Used
- **.NET 8** (Specify the version used)
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **SQL Server** 
- **Swagger (Swashbuckle)** for API documentation
## Prerequisites
Ensure you have the following installed:
- **.NET SDK**
- **SQL Server**
- **Visual Studio 2022** / **Visual Studio Code**
## Installation
### Clone the Repository
```sh
git clone https://github.com/your-repo/product-inventory-api.git
cd ProductInventoryAPI
```
### Configure Database
1. Update the `appsettings.json` file with your database connection string:
  ```json
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ProductInventory;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true;encrypt=false;"
  }
  ```
2. Apply migrations and update the database:
  ```sh
  dotnet ef database update
  ```
### Run the Application
```sh
dotnet run
```
### Access Swagger UI
Once the application is running, navigate to:
```
https://localhost:7151/swagger/index.html
```
This will open the **Swagger UI**, where you can test the API endpoints.
## API Endpoints

### Products
| Method | Endpoint             | Description         |
|--------|----------------------|---------------------|
| GET    | /api/products        | Get all products   |
| GET    | /api/products/{id}   | Get a product by ID |
| POST   | /api/products        | Add a new product  |
| PUT    | /api/products/{id}   | Update a product   |
| DELETE | /api/products/{id}   | Delete a product   |
### Update Stock
| Method | Endpoint             | Description          |
|--------|----------------------|----------------------|
| PUT    | /api/products/add-to-stock/{id}/{quantity}      | Increment the stock with quantity by Id  |
| PUT   | /api/products/decrement-stock/{id}/{quantity}      | Decrement the stock with quantity by Id  |
## Contributing
1. Fork the repository.
2. Create a new branch (`feature-branch`).
3. Commit your changes.
4. Push the branch and create a PR.
## Contact
For questions, contact **nithinsrivatsa05@gmail.com**.