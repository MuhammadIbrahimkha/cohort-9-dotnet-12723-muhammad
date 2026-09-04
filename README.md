# Task Management Tool

A full-stack web-based task management system built as part of the 10Pearls SHINE Internship — Cohort 9, .NET Fullstack (.NET + ReactJS) track.

The system enables users to organize and track tasks, with authenticated, role-based access (Admin / Regular User), full task CRUD, centralized logging, global exception handling, and automated unit tests.

---

## Table of Contents

- [Overview](#overview)
- [Tech Stack](#tech-stack)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Database Schema](#database-schema)
- [Getting Started](#getting-started)
  - [Backend Setup](#backend-setup)
  - [Frontend Setup](#frontend-setup)
- [API Endpoints](#api-endpoints)
- [Authentication & Authorization](#authentication--authorization)
- [Running Tests](#running-tests)
- [Logging](#logging)
- [Exception Handling](#exception-handling)
- [Frontend Screens](#frontend-screens)
- [Default Admin Account](#default-admin-account)
- [Code Quality](#code-quality)

---

## Overview

This project delivers a working task management system with:

- User registration, login, and JWT-based authentication
- Role-based authorization (Admin vs. Regular User)
- Full CRUD operations on tasks, with priorities, categories, and due dates
- Task filtering by status, priority, and category
- A role-aware dashboard showing task counts by status
- Centralized application logging with Serilog
- Global exception handling middleware with consistent error responses
- Unit test coverage across controllers, services, and the data access layer
- A React.js frontend covering the full user flow: auth, dashboard, task list/detail/create/edit, and user profile

The backend follows Clean Architecture principles with a Repository Pattern, kept intentionally simple and readable rather than over-engineered.

---

## Tech Stack

| Layer | Technology |
|---|---|
| Backend Framework | ASP.NET Core Web API (.NET 8) |
| Frontend Framework | React.js (Vite) |
| Data Access | Entity Framework Core |
| Database | SQL Server |
| Authentication | JWT Bearer tokens |
| Logging | Serilog (Console + Rolling File sinks) |
| Testing | xUnit + Moq + EF Core InMemory |
| Code Quality | SonarQube |
| Version Control | Git (feature-branch workflow) |

---

## Architecture

The backend is organized using Clean Architecture, split into five projects:

```
server/
├── TaskManagement.Domain           # Entities, enums — no external dependencies
├── TaskManagement.Application      # Interfaces, DTOs, contracts
├── TaskManagement.Infrastructure   # EF Core DbContext, repository implementations, services
├── TaskManagement.API              # Controllers, Program.cs, middleware, DI wiring
└── TaskManagement.Tests            # xUnit test project
```

**Dependency flow:** `API → Application, Infrastructure` · `Infrastructure → Application, Domain` · `Application → Domain`

Controllers depend only on interfaces defined in the Application layer; they never reference Infrastructure or EF Core directly. Repository interfaces live in `Application`, and their EF Core–backed implementations live in `Infrastructure`, wired together via Dependency Injection in `Program.cs`.

Every repository and service method is fully asynchronous end-to-end — no blocking calls, no synchronous EF Core operations inside async methods.

---

## Project Structure

```
10Pearl_Project/
├── server/
│   ├── TaskManagement.API/
│   │   ├── Controllers/
│   │   │   ├── AuthController.cs
│   │   │   ├── TasksController.cs
│   │   │   ├── DashboardController.cs
│   │   │   ├── CategoriesController.cs
│   │   │   └── UsersController.cs
│   │   ├── Middleware/
│   │   │   └── ExceptionHandlingMiddleware.cs
│   │   ├── Program.cs
│   │   └── appsettings.json
│   ├── TaskManagement.Application/
│   │   ├── DTOs/
│   │   └── Interfaces/
│   │       ├── Repositories/
│   │       └── Services/
│   ├── TaskManagement.Domain/
│   │   ├── Entities/
│   │   └── Enums/
│   ├── TaskManagement.Infrastructure/
│   │   ├── Data/
│   │   │   └── AppDbContext.cs
│   │   ├── Migrations/
│   │   ├── Repositories/
│   │   └── Services/
│   └── TaskManagement.Tests/
│       ├── Controllers/
│       ├── Services/
│       └── Repositories/
├── task-management-client/
│   └── src/
│       ├── api/
│       ├── components/
│       ├── context/
│       └── pages/
├── TaskManagementTool.sln
└── README.md
```

---

## Database Schema

**Users**
| Column | Type | Notes |
|---|---|---|
| Id | int | Primary Key |
| FullName | string | Required |
| Email | string | Required, unique |
| PasswordHash | string | Required (hashed with BCrypt) |
| Role | enum | Admin or User |
| CreatedAt | DateTime | Default: UTC now |

**Tasks**
| Column | Type | Notes |
|---|---|---|
| Id | int | Primary Key |
| Title | string | Required |
| Description | string | Optional |
| Status | enum | Pending, InProgress, Completed |
| Priority | enum | Low, Medium, High |
| CategoryId | int (FK) | References Categories |
| DueDate | DateTime | Optional |
| AssignedToUserId | int (FK) | References Users |
| CreatedByUserId | int (FK) | References Users |
| CreatedAt | DateTime | Default: UTC now |
| UpdatedAt | DateTime | Nullable, set on update |

**Categories**
| Column | Type | Notes |
|---|---|---|
| Id | int | Primary Key |
| Name | string | Required, unique |

---

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server (LocalDB or SQL Express)
- Node.js (v18+) and npm
- Visual Studio 2022 (or any .NET-capable IDE)

### Backend Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/MuhammadIbrahimkha/cohort-9-dotnet-12723-muhammad.git
   cd cohort-9-dotnet-12723-muhammad
   ```

2. Open `TaskManagementTool.sln` in Visual Studio.

3. Update the connection string in `server/TaskManagement.API/appsettings.json` to match your local SQL Server instance:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Data Source=YOUR_SERVER;Initial Catalog=TaskManagementDb;Integrated Security=True;Trust Server Certificate=True"
   }
   ```

4. Run the API (F5, using the **https** launch profile). On first run, the application automatically applies EF Core migrations and seeds a default Admin account.

5. The API will be available at `https://localhost:7069`, with Swagger UI at `https://localhost:7069/swagger`.

### Frontend Setup

1. Navigate to the client folder:
   ```bash
   cd task-management-client
   npm install
   ```

2. Confirm the API base URL in `src/api/axios.js` matches your backend's running port.

3. Start the dev server:
   ```bash
   npm run dev
   ```

4. Open `http://localhost:5173` in your browser.

---

## API Endpoints

### Auth
| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/auth/register` | Register a new user |
| POST | `/api/auth/login` | Authenticate and return a JWT |

### Tasks
| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/tasks` | List tasks (filterable by status, priority, category; role-aware) |
| GET | `/api/tasks/{id}` | Get a single task |
| POST | `/api/tasks` | Create a task |
| PUT | `/api/tasks/{id}` | Update a task |
| DELETE | `/api/tasks/{id}` | Delete a task (Admin only) |

### Dashboard
| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/dashboard` | Task counts by status (own tasks for regular users, all tasks for Admin) |

### Categories
| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/categories` | List categories |
| POST | `/api/categories` | Create a category |

### Users
| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/users` | List users |
| GET | `/api/users/me` | Get the current authenticated user's profile |

---

## Authentication & Authorization

- Authentication is handled via **JWT Bearer tokens**, issued on successful register/login.
- Tokens include the user's ID, email, name, and role as claims.
- Protected endpoints use `[Authorize]`; Admin-restricted endpoints use `[Authorize(Roles = "Admin")]`.
- Passwords are hashed using **BCrypt** — plain-text passwords are never stored.
- The frontend stores the JWT in `localStorage` and attaches it to every API request via an Axios interceptor.

---

## Running Tests

The `TaskManagement.Tests` project covers all three required layers: controllers, services, and the data access layer.

**In Visual Studio:**
1. Open **Test Explorer** (`Test → Test Explorer`)
2. Click **Run All Tests**

**Or via CLI:**
```bash
dotnet test
```

Test coverage includes:
- **Services:** `AuthServiceTests`, `TaskServiceTests`, `DashboardServiceTests`
- **Controllers:** `AuthControllerTests`, `TasksControllerTests`
- **Repositories:** `TaskRepositoryTests`, `UserRepositoryTests` (using EF Core's InMemory provider)

---

## Logging

Application logging is implemented with **Serilog**, configured to write to both the console and rolling daily log files (`logs/log-.txt`). Key events logged include login attempts, task creation/updates, and all unhandled exceptions with contextual trace information.

---

## Exception Handling

A custom global exception handling middleware (`ExceptionHandlingMiddleware`) wraps the entire request pipeline. Unhandled exceptions are caught, logged via Serilog, and converted into a consistent JSON error response:

```json
{
  "statusCode": 500,
  "message": "An unexpected error occurred. Please try again later.",
  "traceId": "..."
}
```

No internal stack traces or implementation details are ever exposed to the client.

---

## Frontend Screens

| Screen | Description |
|---|---|
| Sign Up / Log In | User registration and authentication, redirecting to the dashboard on success |
| Dashboard | Displays Pending / In Progress / Completed task counts, role-aware |
| Task List | Filterable list of tasks with links to detail pages and a "New Task" action |
| Task Detail | Full detail view for a single task |
| New / Edit Task | Form to create or update a task |
| User Profile | Displays the logged-in user's information with a logout action |

---

## Default Admin Account

On first run, the application automatically seeds an Admin account if one doesn't already exist:

- **Email:** `admin@taskmanagement.com`
- **Password:** `Admin@123`

Use this account to test Admin-only functionality, such as deleting tasks and viewing all users' task counts on the dashboard.

---

## Code Quality

The project is integrated with **SonarQube** for static code analysis, covering both the C# backend and the JavaScript/React frontend, to identify code smells, potential bugs, and maintainability issues.

---
 
## Author
 
**Muhammad Ibrahim**
Cohort 9 — .NET Fullstack (.NET + ReactJS), 10Pearls SHINE Internship
GitHub: [@MuhammadIbrahimkha](https://github.com/MuhammadIbrahimkha)
