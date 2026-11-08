# Task Management Tool

![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)
![React](https://img.shields.io/badge/React-Vite-61DAFB?logo=react&logoColor=black)
![SQL Server](https://img.shields.io/badge/Azure_SQL-EF_Core-CC2927?logo=microsoftsqlserver&logoColor=white)
![Azure](https://img.shields.io/badge/Azure-App_Service_%7C_Static_Web_Apps-0078D4?logo=microsoftazure&logoColor=white)
![CI/CD](https://img.shields.io/badge/CI%2FCD-Azure_DevOps_Pipelines-0078D7?logo=azuredevops&logoColor=white)

A full-stack task management system with JWT authentication, role-based access (Admin / User), task CRUD with filtering, categories and a role-aware dashboard. Built during the **10Pearls SHINE Internship (Cohort 9, .NET Fullstack)**, then taken to production on **Azure** with an automated CI/CD pipeline.

## Live Demo

| | Link |
|---|---|
| **Web app (React)** | https://blue-pond-030452f00.6.azurestaticapps.net |
| **API + Swagger docs** | https://taskmgmt-ibrahim-api-dwekfce7efd9h7fe.centralindia-01.azurewebsites.net/swagger |
| **Health check** | https://taskmgmt-ibrahim-api-dwekfce7efd9h7fe.centralindia-01.azurewebsites.net/health |

> Hosted entirely on free Azure tiers, so the first request after a period of inactivity can take 30-60 seconds while the app and database wake up.
> Create a regular account with **Sign up** to try it. Admin demo credentials are available on request.

<!--
Screenshots: add images to docs/screenshots/ and uncomment.

## Screenshots

| Dashboard | Task list |
|---|---|
| ![Dashboard](docs/screenshots/dashboard.png) | ![Tasks](docs/screenshots/tasks.png) |
-->

---

## Table of Contents

- [Highlights](#highlights)
- [Tech Stack](#tech-stack)
- [Architecture](#architecture)
- [CI/CD Pipeline](#cicd-pipeline)
- [Deployment on Azure](#deployment-on-azure)
- [Configuration](#configuration)
- [Project Structure](#project-structure)
- [Database Schema](#database-schema)
- [Getting Started (Local)](#getting-started-local)
- [API Endpoints](#api-endpoints)
- [Authentication & Authorization](#authentication--authorization)
- [Testing](#testing)
- [Logging & Exception Handling](#logging--exception-handling)
- [Frontend Screens](#frontend-screens)
- [Roadmap](#roadmap)
- [Author](#author)

---

## Highlights

- **Clean Architecture** (Domain / Application / Infrastructure / API) with the **Repository pattern**; controllers depend only on interfaces.
- **JWT authentication** with BCrypt password hashing and **role-based authorization** (`[Authorize(Roles = "Admin")]`).
- **Fully async** data access end to end with Entity Framework Core and code-first migrations.
- **Global exception-handling middleware** returning consistent JSON errors (`statusCode`, `message`, `traceId`), plus **Serilog** structured logging.
- **28 automated unit tests** (xUnit, Moq, EF Core InMemory) across controllers, services and repositories.
- **Automated CI/CD**: every push to `develop` builds, tests and deploys the API and the React app to Azure.
- **No secrets in source control**: connection string, JWT key and admin password are supplied through Azure configuration and secret pipeline variables.
- **Self-initializing**: the API applies EF Core migrations and seeds the admin account on startup, with a `/health` endpoint for monitoring.

---

## Tech Stack

| Layer | Technology |
|---|---|
| Backend | ASP.NET Core Web API (.NET 8) |
| Frontend | React (Vite), React Router, Axios |
| Data access | Entity Framework Core |
| Database | SQL Server (local) / Azure SQL Database (production) |
| Authentication | JWT Bearer, BCrypt |
| Logging | Serilog (console + rolling file) |
| Testing | xUnit, Moq, EF Core InMemory |
| CI/CD | Azure DevOps Pipelines |
| Hosting | Azure App Service (API), Azure Static Web Apps (React), Azure SQL Database |
| Version control | Git, feature-branch workflow with pull requests |

---

## Architecture

```
                     +-----------------------+
   Browser  ----->   |  React (Vite)         |   Azure Static Web Apps
                     +-----------+-----------+
                                 | HTTPS + JWT
                     +-----------v-----------+
                     |  ASP.NET Core 8 API   |   Azure App Service (Linux)
                     +-----------+-----------+
                                 | EF Core
                     +-----------v-----------+
                     |  Azure SQL Database   |
                     +-----------------------+
```

The backend is split into five projects:

```
server/
├── TaskManagement.Domain           # Entities, enums (no external dependencies)
├── TaskManagement.Application      # Interfaces, DTOs, contracts
├── TaskManagement.Infrastructure   # EF Core DbContext, repositories, services
├── TaskManagement.API              # Controllers, middleware, DI wiring, Program.cs
└── TaskManagement.Tests            # xUnit test project
```

**Dependency flow:** `API -> Application, Infrastructure` / `Infrastructure -> Application, Domain` / `Application -> Domain`.

Repository interfaces live in `Application`; their EF Core implementations live in `Infrastructure` and are wired together with Dependency Injection. Controllers never reference EF Core directly.

---

## CI/CD Pipeline

Defined in [`azure-pipelines.yml`](./azure-pipelines.yml). Every push to `develop` runs:

```
push to develop
   |
   v
[Build and test]
   |-- Backend:  restore -> build -> run 28 unit tests -> publish API artifact
   |-- Frontend: npm ci -> production build
   |
   v
[Deploy]
   |-- API  -> Azure App Service        (Azure service connection, workload identity federation)
   '-- Web  -> Azure Static Web Apps    (deployment token stored as a secret pipeline variable)
```

- The deploy stage runs only if build and tests pass.
- The Azure service connection uses **workload identity federation**, so there are no long-lived credentials in the pipeline.
- The React build receives the production API address through `VITE_API_URL` at build time.

---

## Deployment on Azure

| Component | Azure service | Tier |
|---|---|---|
| API | App Service (Linux, .NET 8) | Free (F1) |
| Database | Azure SQL Database | Free offer (serverless) |
| Frontend | Static Web Apps | Free |
| CI/CD | Azure DevOps Pipelines | Free tier (Microsoft-hosted agents) |

`staticwebapp.config.json` rewrites unknown routes to `index.html` so React Router deep links work on refresh.

---

## Configuration

ASP.NET Core merges environment variables over `appsettings.json`. A double underscore (`__`) stands for `:`. In production these are set as Azure App Service application settings.

| Setting | Purpose |
|---|---|
| `ConnectionStrings__DefaultConnection` | Database connection string |
| `Jwt__Key` | JWT signing key (use 32+ random characters) |
| `Admin__Password` | Password for the seeded admin account (defaults to a development value locally) |
| `Cors__AllowedOrigins` | Comma-separated list of allowed frontend origins |
| `VITE_API_URL` | API base URL for the React app (set at build time) |

---

## Project Structure

```
.
├── azure-pipelines.yml
├── server/
│   ├── TaskManagement.API/
│   │   ├── Controllers/        # Auth, Tasks, Dashboard, Categories, Users
│   │   ├── Middleware/         # ExceptionHandlingMiddleware
│   │   ├── Program.cs
│   │   └── appsettings.json
│   ├── TaskManagement.Application/   # DTOs, Interfaces/{Repositories,Services}
│   ├── TaskManagement.Domain/        # Entities, Enums
│   ├── TaskManagement.Infrastructure/# Data (AppDbContext), Migrations, Repositories, Services
│   └── TaskManagement.Tests/         # Controllers, Services, Repositories
├── task-management-client/
│   ├── public/staticwebapp.config.json
│   └── src/ {api, components, context, pages}
├── TaskManagementTool/TaskManagementTool.sln
└── README.md
```

---

## Database Schema

**Users**
| Column | Type | Notes |
|---|---|---|
| Id | int | Primary key |
| FullName | string | Required |
| Email | string | Required, unique |
| PasswordHash | string | BCrypt hash |
| Role | enum | Admin or User |
| CreatedAt | DateTime | Default: UTC now |

**Tasks**
| Column | Type | Notes |
|---|---|---|
| Id | int | Primary key |
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
| Id | int | Primary key |
| Name | string | Required, unique |

---

## Getting Started (Local)

### Prerequisites

- .NET 8 SDK
- SQL Server (LocalDB or SQL Express)
- Node.js 18+ and npm
- Visual Studio 2022 (or any .NET-capable IDE)

### Backend

1. Clone the repository:
   ```bash
   git clone https://github.com/MuhammadIbrahimkha/cohort-9-dotnet-12723-muhammad.git
   cd cohort-9-dotnet-12723-muhammad
   ```
2. Open `TaskManagementTool/TaskManagementTool.sln` in Visual Studio.
3. In `server/TaskManagement.API/appsettings.json`, set your connection string and a JWT key of at least 32 characters:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Data Source=YOUR_SERVER;Initial Catalog=TaskManagementDb;Integrated Security=True;Trust Server Certificate=True"
   },
   "Jwt": { "Key": "use-a-long-random-key-of-at-least-32-characters" }
   ```
4. Run the API. On first start it applies EF Core migrations and seeds the admin account.
5. Swagger UI is served at `/swagger`.

### Frontend

```bash
cd task-management-client
npm install
npm run dev
```

Open `http://localhost:5173`. The app calls `http://localhost:5014/api` by default. To point at a different API, create `.env.local` with `VITE_API_URL=<your API base URL>/api`.

### First steps after starting

Sign in as the seeded admin and create a few **categories** (`POST /api/categories`, or through Swagger) so the task form's category dropdown has options.

**Local admin account:** `admin@taskmanagement.com` with the development default password `Admin@123`. In production the admin password is set through `Admin__Password` and is not published.

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
| GET | `/api/tasks` | List tasks (filter by status, priority, category; role-aware) |
| GET | `/api/tasks/{id}` | Get a single task |
| POST | `/api/tasks` | Create a task |
| PUT | `/api/tasks/{id}` | Update a task |
| DELETE | `/api/tasks/{id}` | Delete a task (Admin only) |

### Dashboard
| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/dashboard` | Task counts by status (own tasks for users, all tasks for Admin) |

### Categories
| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/categories` | List categories |
| POST | `/api/categories` | Create a category |

### Users
| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/users` | List users |
| GET | `/api/users/me` | Current user's profile |

### Operations
| Method | Endpoint | Description |
|---|---|---|
| GET | `/health` | Health check used for monitoring |

---

## Authentication & Authorization

- JWT Bearer tokens are issued on register/login and carry the user's ID, email, name and role as claims.
- Protected endpoints use `[Authorize]`; Admin-only endpoints use `[Authorize(Roles = "Admin")]`.
- Passwords are hashed with **BCrypt**; plain-text passwords are never stored.
- The frontend keeps the token in `localStorage` and attaches it to each request with an Axios interceptor; a `ProtectedRoute` component guards authenticated pages.

---

## Testing

The `TaskManagement.Tests` project holds **28 unit tests** covering all three layers:

- **Services:** `AuthServiceTests`, `TaskServiceTests`, `DashboardServiceTests`
- **Controllers:** `AuthControllerTests`, `TasksControllerTests`
- **Repositories:** `TaskRepositoryTests`, `UserRepositoryTests` (EF Core InMemory provider)

Run them with:

```bash
dotnet test TaskManagementTool/TaskManagementTool.sln
```

The same command runs in the CI pipeline, and a failing test blocks deployment.

---

## Logging & Exception Handling

- **Serilog** writes to the console and to rolling daily files (`logs/log-.txt`). Logged events include login attempts, task changes and unhandled exceptions with trace information.
- A global `ExceptionHandlingMiddleware` catches unhandled exceptions, logs them and returns a consistent response without exposing stack traces:

```json
{
  "statusCode": 500,
  "message": "An unexpected error occurred. Please try again later.",
  "traceId": "..."
}
```

---

## Frontend Screens

| Screen | Description |
|---|---|
| Sign Up / Log In | Registration and authentication, redirecting to the dashboard |
| Dashboard | Pending / In Progress / Completed counts (role-aware) |
| Task List | Filterable list with links to detail pages and a "New Task" action |
| Task Detail | Full detail view of one task |
| New / Edit Task | Form to create or update a task |
| Profile | The signed-in user's details and logout |

---

## Roadmap

- Seed default categories on startup so a fresh database works without manual setup
- Containerize the API and frontend with Docker
- Integration tests with `WebApplicationFactory`
- Pagination for task lists, JWT refresh tokens
- Application Insights for production monitoring

---

## Author

**Muhammad Ibrahim**
Cohort 9, .NET Fullstack (.NET + ReactJS), 10Pearls SHINE Internship
GitHub: [@MuhammadIbrahimkha](https://github.com/MuhammadIbrahimkha)
