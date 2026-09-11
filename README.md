# ProzorroDataMining

A service for collecting and analyzing Prozorro procurement data focused on CPV code `09310000-5` (Electrical Energy).

## Tech Stack

**Backend:**
- .NET 10, ASP.NET Core Web API
- PostgreSQL + Entity Framework Core
- Polly (resilience & retry)

**Frontend:**
- React 18 + TypeScript
- Vite
- Recharts

**Infrastructure:**
- Docker / Docker Compose

## Architecture

**Backend** (`backend/`)
- `ProzorroDataMining.API` — REST API endpoints
- `ProzorroDataMining.Worker` — ETL Background Service
- `ProzorroDataMining.Application` — Business logic, services, mappers
- `ProzorroDataMining.Data` — EF Core, repositories, migrations
- `ProzorroDataMining.Domain` — Entities, interfaces
- `ProzorroDataMining.Contracts` — DTOs
- `ProzorroDataMining.Shared` — Constants
- `ProzorroDataMining.UnitTests` — xUnit + Shouldly + NSubstitute

**Frontend** (`frontend/ui/`)
- React dashboard

## Running with Docker Compose

### Requirements
- Docker Desktop installed and running
- Git

### Quick Start

1. Clone the repository:
```bash
git clone https://github.com/YOUR_USERNAME/ProzorroDataMining.git
cd ProzorroDataMining
```

2. Start all services:
```bash
docker-compose up --build
```

3. Open in browser:
   - UI: http://localhost:3000
   - API: http://localhost:8080/api

> On startup, the API automatically applies database migrations and the Worker starts ETL import.

### Additional Commands

Stop all services:
```bash
docker-compose down
```

Stop and reset database:
```bash
docker-compose down -v
```

View logs:
```bash
# All services
docker-compose logs -f

# Specific service
docker-compose logs -f api
docker-compose logs -f worker
```

Services after startup:
- UI: http://localhost:3000
- API: http://localhost:8080/api
- PostgreSQL: localhost:5432

> On startup, the API automatically applies database migrations and the Worker starts ETL import.

## Local Development

### Requirements
- .NET 10 SDK
- PostgreSQL 16
- Node.js 20

### Backend

1. Configure connection string in `backend/ProzorroDataMining.API/appsettings.json` and `backend/ProzorroDataMining.Worker/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=prozorro_db;Username=postgres;Password=postgres"
  }
}
```

2. Apply migrations:
```bash
cd backend
dotnet ef database update --project ProzorroDataMining.Data --startup-project ProzorroDataMining.API
```

3. Start the API:
```bash
dotnet run --project ProzorroDataMining.API
```

4. Start the Worker (separate terminal):
```bash
dotnet run --project ProzorroDataMining.Worker
```

### Frontend

1. Create `.env.local` in `frontend/ui/`:
VITE_API_BASE_URL=http://localhost:5091/api


2. Install dependencies and start:
```bash
cd frontend/ui
npm install
npm run dev
```

UI available at http://localhost:5173

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/analytics/savings` | Budget savings |
| GET | `/api/analytics/top-buyers?top=5` | Top buyers |
| GET | `/api/analytics/top-suppliers?top=5` | Top suppliers |
| POST | `/api/ingestion/run` | Trigger data import |

## ETL Pipeline

1. Fetch tenders from Prozorro API `/tenders` filtered by date (December 2025)
2. Filter by CPV code `09310000-5` and status `complete`
3. Parallel fetch of tender details (SemaphoreSlim, max 5 concurrent requests)
4. Upsert into PostgreSQL (idempotent — safe to re-run)

## Database Schema

```sql
tenders          -- main table
tender_items     -- CPV codes
tender_contracts -- contracts
tender_awards    -- winners/suppliers
```

### Indexes
- `idx_tenders_status` — filter by status
- `idx_tenders_date_created` — filter by date
- `idx_tenders_status_date` — composite index for ETL filter
- `idx_tenders_entity_name` — GROUP BY for analytics
- `idx_tender_awards_supplier_name` — GROUP BY for analytics

Indexes are optimized for millions of records. On small datasets PostgreSQL uses Seq Scan — this is expected behavior.

## Running Tests

```bash
cd backend
dotnet test ProzorroDataMining.UnitTests
```

18 unit tests:
- `TenderMapperTests` — valid/invalid data mapping, null fields handling
- `TenderIngestionServiceTests` — filtering by CPV, status, null API responses