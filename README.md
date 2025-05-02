# Northwind Order System

This is a full-stack web application built with ASP.NET Core (Clean Architecture) and Angular 16 (standalone components). The system manages customer orders, allowing you to create, update, view, and delete orders. It includes address validation via Google Maps and PDF generation with QuestPDF.

---

## 🧩 Tech Stack

![Angular](https://img.shields.io/badge/Angular-16-red?logo=angular)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-Web_API-blue?logo=dotnet)
![SQL Server](https://img.shields.io/badge/SQL_Server-Relational_DB-cc2927?logo=microsoftsqlserver)
![QuestPDF](https://img.shields.io/badge/QuestPDF-PDF_Generator-orange)
![Google Maps](https://img.shields.io/badge/Google_Maps-API-green?logo=googlemaps)

| Technology | Description |
|------------|-------------|
| <img src="https://angular.io/assets/images/logos/angular/angular.svg" width="28" /> **Angular 16** | Frontend with standalone components, reactive forms, routing |
| <img src="https://upload.wikimedia.org/wikipedia/commons/e/ee/.NET_Core_Logo.svg" width="28" /> **ASP.NET Core Web API** | Backend REST API using Clean Architecture principles |
| <img src="https://upload.wikimedia.org/wikipedia/en/8/8e/Microsoft_SQL_Server_Logo.svg" width="28" /> **SQL Server** | Relational database for customers, orders, products |
| <img src="https://avatars.githubusercontent.com/u/87734795?s=200&v=4" width="28" /> **QuestPDF** | Library for dynamic PDF report generation |
| <img src="https://upload.wikimedia.org/wikipedia/commons/9/99/Google_Maps_Logo_2020.svg" width="28" /> **Google Maps API** | Autocomplete + Geolocation for shipping address validation |

---

## 📚 How It Works

This system handles the full flow of customer orders:

### 🖥️ Frontend (Angular)
- User interface to **create and manage orders**.
- **Dropdowns** for selecting customer and employee.
- **Line item form** to add products with quantity and price.
- **Address validation** with Google Maps autocomplete input.
- Displays parsed address and **embedded map**.
- **PDF generation buttons**:
  - Generate PDF for individual order
  - Generate PDF for all saved orders

### 🛠️ Backend (ASP.NET Core Web API)
- Implements Clean Architecture (Core, Application, Infrastructure, API).
- **Use Cases / Handlers** process business logic.
- **Repositories** abstract the data access layer (SQL Server).
- **PDFs** are generated with QuestPDF and returned as byte arrays.
- CORS enabled for communication with Angular frontend.
- The API runs at: `https://localhost:7093`

- #### API Endpoints

| Method | Endpoint                  | Description                               |
|--------|---------------------------|-------------------------------------------|
| GET    | `/api/orders`             | Get all orders                            |
| GET    | `/api/orders/{id}`        | Get a specific order by ID                |
| POST   | `/api/orders`             | Create a new order                        |
| PUT    | `/api/orders/{id}`        | Update an existing order                  |
| DELETE | `/api/orders/{id}`        | Delete an order                           |
| GET    | `/api/orders/{id}/pdf`    | Download a PDF for a single order         |
| GET    | `/api/orders/pdf`         | Download a PDF containing all orders      |
| GET    | `/api/customers`          | Get all customers                         |
| GET    | `/api/employees`          | Get all employees                         |
| GET    | `/api/products`           | Get all products                          |

---

---

## 🚀 Getting Started

### Backend (ASP.NET Core)

1. Navigate to the backend folder:
   ```bash
   cd northwind-order-backend/NorthwindOrderSystem.API
   ```
2. Restore packages and run the project:
   ```bash
   dotnet restore
   dotnet run
   ```
3. Make sure SQL Server is running and configured properly.

#### 🔧 Database Configuration

Update your connection string in `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=Northwind;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

> ⚠️ Ensure your database is seeded with Customers, Employees, Products.

The backend will run at: `https://localhost:7093`

---

### Frontend (Angular)

1. Navigate to the frontend folder:
   ```bash
   cd northwind-order-frontend
   ```
2. Install dependencies:
   ```bash
   npm install
   ```
3. Start Angular dev server:
   ```bash
   ng serve
   ```
4. App runs at: `http://localhost:4200`

#### 🔐 Google Maps API Key

1. Get a Google Maps API key: https://console.cloud.google.com/
2. Add this inside `index.html`:
```html
<script src="https://maps.googleapis.com/maps/api/js?key=YOUR_API_KEY&libraries=places"></script>
```

---

## 📁 Project Structure

```
NorthwindOrderSystem/
├── northwind-order-backend/
│   ├── Core/                # Entities and interfaces
│   ├── Application/        # Use cases and DTOs
│   ├── Infrastructure/     # Repositories and services
│   └── API/                # Controllers and startup config
├── northwind-order-frontend/
│   └── src/app/            # Angular components and services
└── README.md
```

---

## 🧪 Testing

- ✅ Backend is covered with **xUnit** tests
- ✅ Use case handlers tested in isolation
- ✅ PDF services verified with real data

---

## 👩‍💻 Author

Developed by **María José Menjivar Portillo**
> RSM Trainee Program Final Project 2025
