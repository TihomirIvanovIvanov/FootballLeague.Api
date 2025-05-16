# Football League API

[![.NET](https://img.shields.io/badge/.NET-5.0-blue)](https://dotnet.microsoft.com/)
[![EF Core](https://img.shields.io/badge/EF%20Core-5.0-blue)](https://docs.microsoft.com/ef/)
[![MediatR](https://img.shields.io/badge/MediatR-CQRS-yellowgreen)](https://github.com/jbogard/MediatR)
[![SignalR](https://img.shields.io/badge/SignalR-RealTime-purple)](https://docs.microsoft.com/aspnet/core/signalr)
[![xUnit](https://img.shields.io/badge/xUnit-Tests-red)](https://xunit.net/)
[![Swagger](https://img.shields.io/badge/Swagger-UI-orange)](https://swagger.io/)


A **Clean-Architecture** ASP NET Core (5.0) Web API for managing a simple football league:

- Teams: create, read, update, “retire” (soft-delete)  
- Matches: create played matches, edit scores, delete  
- Rankings: live standings (win = 3, draw = 1, loss = 0)  
- Real-time notifications via SignalR  
- CQRS/MediatR for commands & queries  
- EF Core 5.0 + SQL Server (InMemory for tests)  
- Global exception handling, input validation, Swagger docs  

## 🚀 API Endpoints

_All requests/responses are JSON._

### Teams

| Method | URL                        | Description                                |
| ------ | -------------------------- | ------------------------------------------ |
| GET    | `/api/teams`               | List all active teams                      |
| GET    | `/api/teams/{id}`          | Get a specific team by ID                  |
| POST   | `/api/teams`               | Create a new team                          |
| PUT    | `/api/teams/{id}`          | Rename a team name                         |
| DELETE | `/api/teams/{id}`          | Retire (soft-delete) a team                |
| GET    | `/api/teams/ranking`       | Get current league standings               |
| GET    | `/api/teams/{id}/matches`  | List matches played by a team              |

### Matches

| Method | URL                        | Description                                    |
| ------ | -------------------------- | ---------------------------------------------- |
| GET    | `/api/matches`             | List all matches                               |
| GET    | `/api/matches/{id}`        | Get details for a single match                 |
| POST   | `/api/matches`             | Create a new played match                      |
| PUT    | `/api/matches/{id}`        | Update an existing match’s score               |
| DELETE | `/api/matches/{id}`        | Remove a match                                 |

---

## 📚 Key Concepts

- **CQRS / MediatR**  
  Commands (e.g. `CreateMatchCommand`), Queries (e.g. `GetTeamRankingQuery`) and their handlers decouple read/write logic from Controllers.

- **Clean Architecture**  
  - **Models** (`FootballLeague.Models`) target .NET Standard 2.0.  
  - **Data** & **Business** layers target .NET 5.0.  
  - **API** & **Tests** target .NET 5.0.  

- **EF Core 5.0 + SQL Server**  
  `FootballLeague.Data` implements `FootballLeagueDbContext` with all `DbSet<T>`s, migrations & seeding.

- **SignalR (Kind a Observer Pattern)**  
  Server pushes real-time `MatchCreated`, `MatchUpdated`, `MatchDeleted` and `RankingUpdated` events to connected clients via websockets.

- **Soft-Delete**  
  Teams are “retired” (flagged) instead of being hard-deleted, preserving history match data.

- **Validation & Error Handling**  
  DataAnnotations + custom validators block invalid input; `ExceptionHandlingMiddleware` centralizes HTTP 400/404/500 error responses.

