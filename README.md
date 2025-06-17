# To-Do Workers Health Management System

A comprehensive ASP.NET Core application built with Clean Architecture principles for managing worker tasks and monitoring worker health. Features include CRUD operations, intelligent load balancing with consistent hashing, and three specialized background workers for automated task management.

## 🏗️ Architecture

This project follows **Clean Architecture** principles with clear separation of concerns:

```
TodoWorkersHealthManagement/
├── src/
│   ├── TodoWorkersHealthManagement.Domain/       # Business entities and interfaces
│   ├── TodoWorkersHealthManagement.Application/  # Business logic and DTOs  
│   ├── TodoWorkersHealthManagement.Infrastructure/# Data access and external services
│   ├── TodoWorkersHealthManagement.API/          # Web API controllers and configuration
│   └── TodoWorkersHealthManagement.LoadBalancer/ # Load balancing with consistent hashing
├── tests/
└── docker-compose.yml
```

## ✨ Key Features

### 🔧 Core Functionality
- **Complete CRUD Operations** for worker task management
- **Worker Health Monitoring** with real-time status tracking
- **Task Priority Management** (Low, Medium, High, Critical)
- **Automated Task Assignment** based on worker availability

### ⚖️ Load Balancing
- **Consistent Hashing Algorithm** for request distribution
- **Virtual Nodes** for improved load distribution
- **Health-aware Routing** with automatic failover
- **Real-time Server Statistics** and monitoring

### 🤖 Background Workers
1. **Health Monitoring Worker** - Monitors worker health based on task load
2. **Task Processing Worker** - Automatically processes pending and in-progress tasks
3. **Workload Balancing Worker** - Distributes unassigned tasks to available workers

### 📊 Monitoring & Analytics
- Worker health status dashboard
- Load balancer statistics
- Task completion metrics
- Server performance monitoring

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK
- SQL Server (or SQL Server LocalDB)
- Docker (optional, for containerized deployment)

### Local Development Setup

1. **Clone the repository**
```bash
git clone <repository-url>
cd TodoWorkersHealthManagement
```

2. **Update connection string** in `appsettings.json`
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TodoWorkersHealthDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

3. **Restore packages and run**
```bash
dotnet restore
dotnet run --project src/TodoWorkersHealthManagement.API
```

4. **Access the application**
- API: `https://localhost:5001`
- Swagger UI: `https://localhost:5001/swagger`

### 🐳 Docker Deployment

1. **Run with Docker Compose**
```bash
docker-compose up --build
```

2. **Access the containerized application**
- API: `http://localhost:5000`
- Swagger UI: `http://localhost:5000/swagger`

## 📖 API Documentation

### Worker Tasks Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/workertasks` | Get all tasks |
| GET | `/api/workertasks/{id}` | Get task by ID |
| GET | `/api/workertasks/worker/{workerName}` | Get tasks by worker |
| GET | `/api/workertasks/status/{status}` | Get tasks by status |
| GET | `/api/workertasks/health-status/{healthStatus}` | Get tasks by health status |
| GET | `/api/workertasks/health-summary` | Get worker health summary |
| POST | `/api/workertasks` | Create new task |
| PUT | `/api/workertasks/{id}` | Update existing task |
| DELETE | `/api/workertasks/{id}` | Delete task |

### Load Balancer Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/loadbalancer/route` | Route request to optimal server |
| GET | `/api/loadbalancer/stats` | Get load balancer statistics |
| GET | `/api/loadbalancer/health` | Get server health status |
| POST | `/api/loadbalancer/servers` | Add new server |
| DELETE | `/api/loadbalancer/servers/{serverName}` | Remove server |
| PUT | `/api/loadbalancer/servers/{serverName}/health` | Update server health |

## 💡 Usage Examples

### Creating a New Task
```bash
curl -X POST "https://localhost:5001/api/workertasks" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Implement user authentication",
    "description": "Add JWT-based authentication system",
    "priority": "High",
    "assignedWorker": "Worker1",
    "estimatedHours": 8,
    "healthNotes": "Standard development task"
  }'
```

### Routing a Request
```bash
curl -X POST "https://localhost:5001/api/loadbalancer/route" \
  -H "Content-Type: application/json" \
  -d '{
    "requestKey": "user123_task456"
  }'
```

### Getting Load Balancer Statistics
```bash
curl -X GET "https://localhost:5001/api/loadbalancer/stats"
```

## 🏃‍♂️ Background Workers

### Health Monitoring Worker
- **Frequency**: Every 5 minutes
- **Function**: Monitors worker task load and updates health status
- **Health Levels**: 
  - Healthy (≤5 active tasks)
  - Overloaded (>10 active tasks)

### Task Processing Worker  
- **Frequency**: Every 2 minutes
- **Function**: Processes pending tasks and completes long-running ones
- **Batch Size**: 5 tasks per cycle

### Workload Balancing Worker
- **Frequency**: Every 3 minutes  
- **Function**: Auto-assigns unassigned tasks to workers with least workload
- **Algorithm**: Least-loaded worker selection

## 🔧 Configuration

### Application Settings
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your SQL Server connection string"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### Load Balancer Configuration
- **Virtual Nodes per Server**: 150 (configurable)
- **Hash Algorithm**: SHA1
- **Default Servers**: API-Server-1, API-Server-2, API-Server-3

## 🏗️ Project Structure Details

### Domain Layer
- **Entities**: Core business objects (`WorkerTask`)
- **Enums**: Task status, priority, and health status
- **Interfaces**: Repository contracts

### Application Layer
- **DTOs**: Data transfer objects for API communication
- **Services**: Business logic implementation
- **Mapping**: Entity to DTO conversions

### Infrastructure Layer  
- **DbContext**: Entity Framework database context
- **Repositories**: Data access implementations
- **Migrations**: Database schema management

### API Layer
- **Controllers**: HTTP endpoints and request handling
- **Background Services**: Long-running background workers
- **Configuration**: Dependency injection and middleware setup

## 🧪 Testing

### Manual Testing with Swagger
1. Navigate to `/swagger` endpoint
2. Test CRUD operations on worker tasks
3. Test load balancer routing functionality
4. Monitor background worker logs

### API Testing Examples
Use the provided curl commands or import into Postman for comprehensive API testing.

## 📊 Monitoring

### Health Metrics
- Worker task distribution
- Server response times
- Load balancer efficiency
- Background worker performance

### Logging
- Structured logging with Serilog-compatible format
- Request/response logging
- Background worker activity logs
- Error tracking and alerts

## 🔒 Security Considerations

- Input validation on all endpoints
- SQL injection prevention through Entity Framework
- CORS configuration for cross-origin requests
- Request size limitations

## 🚀 Deployment

### Production Considerations
- Use production-grade SQL Server
- Configure proper connection pooling
- Set up monitoring and alerting
- Implement proper logging aggregation
- Configure SSL certificates

### Scaling
- Horizontal scaling supported through load balancer
- Database connection pooling for high concurrency
- Background worker distribution across instances

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📋 Requirements Met

✅ **Clean Architecture Implementation**  
✅ **Complete CRUD Operations**  
✅ **Load Balancer with Consistent Hashing**  
✅ **Three Background Workers**  
✅ **Worker Health Management**  
✅ **Docker Containerization**  
✅ **API Documentation**  
✅ **Comprehensive Logging**  
