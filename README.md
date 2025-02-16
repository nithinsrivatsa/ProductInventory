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
git clone https://github.com/nithinsrivatsa/ProductInventory
cd ProductInventoryAPI
```
### Configure Database
1. Update the `appsettings.json` file with database connection string: (Use localhost or any Server)
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
### Code Quality and Formatting
* EditorConfig: 
The project includes an .editorconfig file to ensure consistent code formatting across different development environments. It automatically enforces indentation, spacing, and other style rules.
* StyleCop: 
We use StyleCop to enforce coding standards and add file headers. All new files should include the header.
### Product ID Generation
The Product ID generator ensures unique IDs based on the current timestamp and a node identifier. It prevents duplicate IDs by using a sequence counter for milliseconds with high-frequency requests. The generator restricts the number of instances and follows a structured approach to avoid collisions. The final product ID is a computed value derived from the timestamp and a base offset, ensuring uniqueness within the defined range.
## Contributing
1. Fork the repository.
2. Create a new branch (`feature-branch`).
3. Commit your changes.
4. Push the branch and create a PR.
## Contact
For questions, contact **nithinsrivatsa05@gmail.com**.
