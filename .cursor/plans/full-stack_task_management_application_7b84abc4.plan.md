---
name: Full-Stack Task Management Application
overview: Build a complete full-stack task management application with .NET 8 backend, React frontend with Redux, SQL Server database, and bonus Windows Service with RabbitMQ integration.
todos:
  - id: git-setup
    content: Initialize Git repository and create .gitignore file in project root for .NET, React, Node.js, and Windows Service
    status: completed
  - id: backend-setup
    content: Set up .NET 8 backend projects (API, Core, Infrastructure, Service) with EF Core and SQL Server configuration
    status: completed
  - id: backend-models
    content: Create domain models (Task, User) with proper relationships and Entity Framework configuration
    status: pending
  - id: backend-validation
    content: Implement DTOs and FluentValidation validators for all fields (title, description, due date, priority, full name, telephone, email)
    status: pending
  - id: backend-api
    content: Create RESTful API controllers with full CRUD operations for Tasks and Users, including error handling middleware
    status: pending
  - id: frontend-setup
    content: Initialize React app and configure Redux Toolkit store with slices for tasks, users, and UI state
    status: pending
  - id: frontend-api
    content: Create API service layer with Axios and Redux async thunks for all CRUD operations
    status: pending
  - id: frontend-components
    content: Build React components (TaskList, TaskForm with validation, UserForm fields, routing) with responsive UI
    status: pending
  - id: frontend-integration
    content: Integrate Redux state management with components, implement form validation matching backend rules, and add loading/error states
    status: pending
  - id: backend-testing
    content: Write unit and integration tests for backend (API endpoints, validation, repositories) using xUnit and Moq
    status: pending
  - id: frontend-testing
    content: Write unit and component tests for frontend (Redux slices, components, form validation) using Jest and React Testing Library
    status: pending
  - id: rabbitmq-setup
    content: Configure RabbitMQ queue and integrate message publishing in API for overdue tasks
    status: pending
  - id: windows-service
    content: Create Windows Service that polls overdue tasks, publishes to RabbitMQ queue, subscribes to queue, and logs reminders
    status: pending
  - id: documentation
    content: Create comprehensive README with setup instructions, architecture overview, API documentation, and testing guide
    status: pending
---

# Full-Stack Task Management Application - Implementation Plan

## Project Structure

The solution will be organized as follows:

```
SupercomTask/
├── .git/                            # Git version control
├── .gitignore                       # Git ignore rules (single file in root)
├── backend/
│   ├── TaskManagement.API/          # .NET 8 Web API
│   ├── TaskManagement.Core/         # Domain models and interfaces
│   ├── TaskManagement.Infrastructure/  # EF Core, DbContext, repositories
│   └── TaskManagement.Service/      # Business logic
├── frontend/
│   └── task-manager-app/            # React application with Redux
├── service/
│   └── TaskReminderService/         # Windows Service with RabbitMQ
└── README.md
```

## Phase 0: Version Control Setup

### 0.1 Git Repository Initialization

- Initialize Git repository in project root (`git init`)
- Create single `.gitignore` file in project root with rules for:
  - .NET build artifacts (bin/, obj/, *.dll, *.pdb, etc.)
  - Visual Studio files (.vs/, *.suo, *.user, etc.)
  - NuGet packages (packages/, *.nupkg)
  - SQL Server database files (*.mdf, *.ldf)
  - Node.js (node_modules/, npm-debug.log, .npm, etc.)
  - React build output (dist/, build/)
  - Environment files (.env, .env.local, appsettings.Development.json with secrets)
  - IDE files (.vscode/, .idea/, *.swp, etc.)
  - OS files (Thumbs.db, .DS_Store, etc.)
  - Windows Service logs and temporary files
  - RabbitMQ temporary files

## Phase 1: Backend Development

### 1.1 Project Setup and Configuration

- Create .NET 8 Web API project (`TaskManagement.API`)
- Create class library projects for Core, Infrastructure, and Service layers
- Install NuGet packages:
  - Entity Framework Core
  - Entity Framework Core SQL Server
  - Entity Framework Core Tools
  - FluentValidation (for validation)
  - AutoMapper (optional, for DTOs)

### 1.2 Domain Models

Create models in `TaskManagement.Core/Models/`:

- **Task**: Id, Title, Description, DueDate, Priority (enum: Low, Medium, High), UserId (FK)
- **User**: Id, FullName, Telephone, Email, Tasks (collection)
- Configure Entity Framework relationships (one-to-many: User -> Tasks)

### 1.3 Database Context and Configuration

- Create `TaskManagementContext` in Infrastructure layer
- Configure DbSets for Task and User entities
- Set up Entity Framework configuration:
  - Field validations (required fields, string lengths)
  - Indexes (e.g., Email unique)
  - Relationship configurations
- Create initial migration

### 1.4 Repository Pattern (Optional but Recommended)

- Create generic repository interface and implementation
- Create specific repositories: ITaskRepository, IUserRepository
- Implement in Infrastructure layer

### 1.5 DTOs and Validation

- Create DTOs in API project:
  - TaskDto, CreateTaskDto, UpdateTaskDto
  - UserDto, CreateUserDto, UpdateUserDto
- Implement FluentValidation validators for all DTOs:
  - Title: Required, max length
  - Description: Required
  - DueDate: Required, must be future date (or allow past for testing)
  - Priority: Required, valid enum value
  - FullName: Required, max length
  - Telephone: Required, format validation
  - Email: Required, valid email format

### 1.6 API Controllers

Create controllers in `TaskManagement.API/Controllers/`:

- **TasksController**: 
  - GET /api/tasks (get all with user details)
  - GET /api/tasks/{id}
  - POST /api/tasks (create with validation)
  - PUT /api/tasks/{id} (update with validation)
  - DELETE /api/tasks/{id}
- **UsersController**:
  - GET /api/users
  - GET /api/users/{id}
  - POST /api/users (create with validation)
  - PUT /api/users/{id}
  - DELETE /api/users/{id} (cascade delete tasks)

### 1.7 Error Handling and Middleware

- Create global exception handling middleware
- Return standardized error responses
- Handle validation errors from FluentValidation
- Configure CORS for React frontend

### 1.8 Database Configuration

- Configure connection string in appsettings.json
- Set up database initialization (ensure database is created on first run)
- Seed initial data (optional, for testing)

## Phase 2: Frontend Development

### 2.1 React Application Setup

- Initialize React app (using Vite and TS)
- Install dependencies:
  - Redux Toolkit (@reduxjs/toolkit)
  - React-Redux
  - React Router DOM
  - Axios (for API calls)
  - Form validation library (react-hook-form + yup/zod)
  - UI library (Material-UI, Ant Design, or Tailwind CSS)

### 2.2 Redux Store Configuration

- Set up Redux store with Redux Toolkit
- Create store structure:
  - tasks slice (state, actions, reducers)
  - users slice
  - ui slice (loading states, errors)
- Configure store with middleware (Redux Thunk for async actions)

### 2.3 API Service Layer

- Create API service files:
  - `api/taskService.js` - API calls for tasks
  - `api/userService.js` - API calls for users
- Implement async thunks in Redux slices for:
  - Fetching tasks/users
  - Creating tasks/users
  - Updating tasks/users
  - Deleting tasks/users

### 2.4 Components Structure

Create component hierarchy:

- **Layout**: Main layout with navigation
- **TaskList**: Display all tasks in a table/card view
- **TaskForm**: Create/Edit task form with validation
  - Form fields: Title, Description, DueDate (date picker), Priority (dropdown)
  - User selection/creation section: FullName, Telephone, Email
  - Real-time validation feedback
- **UserForm**: Separate user management (if needed)
- **TaskCard/TaskRow**: Individual task display component

### 2.5 Routing

- Set up React Router:
  - `/` - Task list
  - `/tasks/new` - Create task
  - `/tasks/:id/edit` - Edit task
  - `/users` - User management (optional)

### 2.6 Form Validation

- Implement client-side validation:
  - Required field validation
  - Email format validation
  - Telephone format validation
  - Date validation (due date not in past)
  - Display validation errors inline
- Ensure validation matches backend rules

### 2.7 UI/UX Implementation

- Create responsive design (mobile-friendly)
- Implement loading states (spinners, skeletons)
- Error message display
- Success notifications (toast messages)
- Confirmation dialogs for delete operations
- Date picker component for DueDate
- Priority badge/indicator with colors

### 2.8 State Management Integration

- Connect components to Redux store
- Dispatch actions for CRUD operations
- Subscribe to state changes (tasks, loading, errors)
- Handle async operations with proper loading/error states

## Phase 3: Integration and Testing

### 3.1 Backend Testing

- Unit tests for:
  - Validation logic
  - Repository methods
  - Service layer methods
- Integration tests for:
  - API endpoints (all CRUD operations)
  - Database operations
  - Validation error scenarios
- Use xUnit, Moq, and Entity Framework InMemory database for testing

### 3.2 Frontend Testing

- Unit tests for:
  - Redux reducers
  - Utility functions
  - Form validation
- Component tests for:
  - TaskForm (form submission, validation)
  - TaskList (rendering, interactions)
- Integration tests for:
  - User workflows (create task, edit task, delete task)
- Use Jest and React Testing Library

### 3.3 End-to-End Testing

- Test complete user flows:
  - Create task with user details
  - Edit task
  - Delete task
  - Validation error handling
- Manual testing of all CRUD operations
- Cross-browser testing

### 3.4 API Integration Testing

- Test API endpoints with Postman/curl
- Verify all validation rules
- Test error scenarios (invalid data, not found, etc.)
- Test CORS configuration

## Phase 4: Bonus Features (Windows Service + RabbitMQ)

### 4.1 RabbitMQ Setup

- Install RabbitMQ Server (local development)
- Create RabbitMQ configuration in backend API
- Set up queue: "TaskReminder"
- Install RabbitMQ.Client NuGet package in API and Service projects

### 4.2 Windows Service Project

- Create Windows Service project (`TaskReminderService`)
- Install NuGet packages:
  - Entity Framework Core SQL Server (for database access)
  - RabbitMQ.Client
  - System.ServiceProcess.ServiceBase

### 4.3 Service Implementation

- Create service class that extends ServiceBase
- Implement background worker/timer:
  - Poll database for tasks where DueDate < CurrentDate
  - For each overdue task, publish message to "TaskReminder" queue
    - Message format: TaskId, TaskTitle, DueDate
- Implement queue subscriber:
  - Subscribe to "TaskReminder" queue
  - Log message: "Hi your Task is due {Task xxxxx}" (using TaskTitle)
  - Handle concurrent message processing (thread-safe logging)

### 4.4 Service Configuration

- Configure service installer
- Set up connection strings (database, RabbitMQ)
- Configure service to start automatically
- Create installation script (PowerShell/batch)

### 4.5 Integration with API

- Optionally, add endpoint in API to trigger reminder check manually (for testing)
- Ensure service uses same database as API

## Phase 5: Documentation and Deployment

### 5.1 README.md

- Project overview
- Technology stack
- Architecture overview (with diagram)
- Setup instructions:
  - Prerequisites (SQL Server, RabbitMQ, Node.js, .NET 8 SDK)
  - Database setup
  - Backend setup (connection strings, migrations)
  - Frontend setup (npm install, environment variables)
  - Windows Service installation
- API documentation (endpoints, request/response examples)
- Testing instructions
- Troubleshooting guide

### 5.2 Code Documentation

- XML comments on API controllers and methods
- Code comments for complex logic
- Inline documentation for key architectural decisions

### 5.3 Setup Scripts

- Database initialization script (SQL script or EF migration)
- PowerShell script for Windows Service installation
- Environment configuration templates (.env.example, appsettings.Development.json.example)

## Implementation Order Summary

1. **Backend Foundation** (1.1 - 1.4): Set up projects, models, database
2. **Backend API** (1.5 - 1.8): DTOs, validation, controllers, error handling
3. **Frontend Setup** (2.1 - 2.3): React app, Redux store, API services
4. **Frontend UI** (2.4 - 2.8): Components, routing, validation, state integration
5. **Testing** (3.1 - 3.4): Backend tests, frontend tests, integration tests
6. **Bonus Features** (4.1 - 4.5): RabbitMQ, Windows Service
7. **Documentation** (5.1 - 5.3): README, code docs, scripts

## Key Technical Decisions

- **.NET 8**: Latest LTS version for backend
- **Redux Toolkit**: Modern Redux with less boilerplate
- **Entity Framework Core**: Code-first approach with migrations
- **FluentValidation**: Server-side validation with clear error messages
- **Repository Pattern**: Separation of concerns (optional but recommended)
- **React Router**: Client-side routing
- **Axios**: HTTP client for API calls
- **Form Validation**: Client-side validation matching backend rules
- **Error Handling**: Global exception handling middleware
- **Concurrency**: Thread-safe queue processing in Windows Service