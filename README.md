# Interview Test API Project

A simple hotel booking API designed for developer interviews. This project demonstrates clean architecture patterns, Domain-Driven Design (DDD) principles, and includes deliberate issues for discussion during technical interviews.

## 🎯 Project Overview

This API manages guests and their hotel bookings with the following features:
- Guest management (CRUD operations)
- Booking management with business logic
- Entity Framework Core with In-Memory database
- RESTful API with Swagger documentation
- Unit tests with xUnit

### Domain Entities
- **Guest**: Represents hotel guests with personal information and address
- **Booking**: Represents room bookings with dates, rates, and status
- **Address**: Value object for guest addresses

## 🚀 Quick Start

### Prerequisites
- .NET 8.0 SDK
- Git

### Clone and Run
```bash
git clone <repository-url>
cd InterviewTest
dotnet build
dotnet run --project src/InterviewTest.Api
```

The API will start on `http://localhost:5124` with Swagger UI available at the root URL.

### Running Tests
```bash
dotnet test
```

## 📋 API Endpoints

### Guests
- `GET /api/guests` - Get all guests
- `GET /api/guests/{id}` - Get guest by ID
- `GET /api/guests/with-bookings` - Get guests with their bookings (⚠️ Performance Issue)
- `GET /api/guests/search?term={searchTerm}` - Search guests
- `POST /api/guests` - Create new guest (🐛 Validation Bug)
- `PUT /api/guests/{id}` - Update guest
- `DELETE /api/guests/{id}` - Delete guest
- `GET /api/guests/{id}/bookings` - Get guest's bookings

### Bookings
- `GET /api/bookings` - Get all bookings
- `GET /api/bookings/{id}` - Get booking by ID
- `GET /api/bookings/active` - Get active bookings
- `POST /api/bookings` - Create new booking (🐛 Validation Bug)
- `PUT /api/bookings/{id}` - Update booking
- `DELETE /api/bookings/{id}` - Delete booking

## 🐛 Built-in Issues for Interview Discussion

### 1. Performance Issue (N+1 Query Problem)
**Location**: `GuestRepository.GetGuestsWithBookingsAsync()` and `BookingService.CalculateBookingRevenueForGuestAsync()`

**Problem**: The `GET /api/guests/with-bookings` endpoint causes N+1 queries.

**Test**: 
```bash
curl http://localhost:5124/api/guests/with-bookings
```

**Discussion Points**:
- How to identify N+1 problems
- Entity Framework's Include() method
- Query optimization strategies

### 2. Validation Bug
**Location**: `GuestsController.CreateGuest()` and `BookingService.CreateBookingAsync()`

**Problem**: Date validation logic is incorrect - uses `>=` instead of `>` for date of birth validation.

**Test**:
```bash
curl -X POST http://localhost:5124/api/guests \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "Test",
    "lastName": "User", 
    "email": "test@example.com",
    "dateOfBirth": "2024-07-29T00:00:00",
    "address": {"street": "123 St", "city": "City", "state": "ST", "postalCode": "12345", "country": "USA"}
  }'
```

**Discussion Points**:
- Debugging validation logic
- Understanding business requirements
- Testing edge cases

### 3. Code Smell - Violation of DDD Principles
**Location**: `BookingService.cs`

**Problems**:
- Service mixes domain logic with data access concerns
- Direct console logging in domain service
- Validation logic should be separated

**Discussion Points**:
- Domain-Driven Design principles
- Separation of concerns
- Service layer responsibilities
- Where to place validation logic

### 4. Unit Test Gap - Failing Test
**Location**: `BookingTests.CalculateTotal_ShouldIncludeTax()`

**Problem**: The `Booking.CalculateTotal()` method doesn't include tax in the calculation.

**Run Test**:
```bash
dotnet test --filter "CalculateTotal_ShouldIncludeTax"
```

**Discussion Points**:
- Test-driven development
- Business logic testing
- Debugging failing tests

## 🎯 Interview Tasks

### Task 1: Fix the Validation Bug (15 minutes)
1. Clone and run the project
2. Test the guest creation endpoint with today's date
3. Identify and fix the validation logic error
4. Verify the fix works

### Task 2: Identify and Explain the Code Smell (10 minutes)
1. Review the `BookingService` class
2. Identify violations of DDD principles
3. Explain how you would refactor it
4. Discuss the benefits of the refactoring

### Task 3: Fix the Failing Unit Test (10 minutes)
1. Run the tests and identify the failing test
2. Debug the `CalculateTotal()` method
3. Fix the business logic bug
4. Verify all tests pass

### Task 4: Optimize the Performance Issue (15 minutes)
1. Test the `/api/guests/with-bookings` endpoint
2. Identify the N+1 query problem
3. Fix the repository method to use eager loading
4. Explain other optimization strategies

### Task 5: Add New Endpoint (Extension - if time permits)
Add a `GET /api/guests/{id}/bookings/active` endpoint that returns only active bookings for a specific guest.

## 💡 Interview Questions

### Setup & Debugging
- How would you approach running this project for the first time?
- What would you do if the API didn't start properly?
- How do you identify performance issues in APIs?

### Domain & Code Quality
- What DDD principles are violated in this codebase?
- How would you improve the separation of concerns?
- What would you refactor first and why?

### Entity Framework & Database
- How would you optimize the database queries?
- What EF Core features would you use for better performance?
- How would you handle database migrations in production?

### Testing & Quality
- What testing strategies would you implement?
- How would you improve the current test coverage?
- What tools would you use for API testing?

## 🛠️ Architecture

```
InterviewTest/
├── src/
│   ├── InterviewTest.Api/         # Web API layer
│   ├── InterviewTest.Core/        # Domain layer (entities, services, interfaces)
│   └── InterviewTest.Infrastructure/ # Data access layer
└── tests/
    ├── InterviewTest.Api.Tests/   # API integration tests
    └── InterviewTest.Core.Tests/  # Unit tests
```

### Key Patterns Used
- Clean Architecture
- Repository Pattern
- Domain-Driven Design
- Dependency Injection
- RESTful API design

## 📚 Learning Resources

- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Domain-Driven Design](https://martinfowler.com/bliki/DomainDrivenDesign.html)
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [ASP.NET Core Web API](https://docs.microsoft.com/en-us/aspnet/core/web-api/)

## 🔧 Troubleshooting

### Common Issues

**Build Failures**
- Ensure .NET 8.0 SDK is installed
- Run `dotnet restore` to restore packages

**Port Already in Use**
- The API runs on port 5124 (HTTP) and 7126 (HTTPS)
- Change ports in `launchSettings.json` if needed

**Database Issues**
- Project uses In-Memory database - no setup required
- Data is seeded automatically on startup

## 📝 Notes for Interviewers

- Allow 5-10 minutes for initial setup and exploration
- Focus on problem-solving approach rather than perfect solutions
- Encourage candidates to explain their thought process
- Use the built-in issues as conversation starters
- Observe coding style and testing practices

Expected completion time: 45-60 minutes for all tasks.