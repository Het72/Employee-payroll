# Employee Payroll System

ASP.NET Core 8 Web API project for employee, department and payroll management.

## Features

- JWT authentication with Register/Login
- BCrypt password hashing
- Role-based authorization (Admin/Employee)
- Employee CRUD
- Department CRUD
- Payroll generation and net salary calculation
- Search, filtering and pagination
- Entity Framework Core + SQL Server
- Dependency Injection
- Async database operations
- Swagger/OpenAPI
- Global exception handling middleware
- Docker + SQL Server Compose setup

## Requirements

- .NET 8 SDK
- SQL Server (local) OR Docker Desktop

## Run locally

1. Update `appsettings.json` connection string and JWT key.
2. Run:

```bash
dotnet restore
dotnet run
```

The Development profile opens Swagger at `https://localhost:7090/swagger`.

The application uses `EnsureCreated()` for a simple demo setup and seeds:

- Admin email: `admin@payroll.local`
- Admin password: `Admin@12345`

Change/remove these demo credentials before any real deployment.

## API flow

1. POST `/api/auth/register`
2. POST `/api/auth/login`
3. Copy the JWT token.
4. Click **Authorize** in Swagger and enter `Bearer <token>`.
5. Use protected endpoints.

## Docker

```bash
docker compose up --build
```

Swagger will be available at `http://localhost:8080/swagger`.

## Main endpoints

- `POST /api/auth/register`
- `POST /api/auth/login`
- `GET /api/departments`
- `POST /api/departments` (Admin)
- `GET /api/employees`
- `POST /api/employees` (Admin)
- `PUT /api/employees/{id}` (Admin)
- `DELETE /api/employees/{id}` (Admin)
- `GET /api/payroll`
- `POST /api/payroll` (Admin)
- `DELETE /api/payroll/{id}` (Admin)
