# Integration-Tests-Databases
## This is a test project for Back-End Test Technologies January 2024 Course @ SoftUni.
---
Testing Console-based applications built using .Net Core Framework.

### Project Description

**Integration-Tests-Databases** is a test project designed to practice and demonstrate integration testing with databases for console-based applications built using the .NET Core Framework and Entity Framework. 

## Key Concepts

- **Integration Testing**: Ensuring different parts of the application work together as expected.
- **Database Testing**: Verifying that the application correctly interacts with the database.

## Technologies Used

- **.NET Core**: The primary framework used for building the console applications.
- **SQL Server / MongoDB**: The database management systems used for storing application data.
- **Entity Framework Core**: ORM and database drivers for interacting with databases.
- **nUnit**: A unit-testing framework for .NET applications.

## Project Structure

- **IntegrationTestsDatabases**: The main project folder containing the console application and integration tests.
    - **IntegrationTestsDatabases.csproj**: Project file for the console application.
    - **Program.cs**: The main entry point of the application.
    - **DatabaseContext.cs**: The Entity Framework context for managing database operations (if using SQL Server).
    - **DataModel.cs**: The model representing the data structure.
    - **DataService.cs**: Contains business logic for managing data.
    - **AppConfig.json**: Configuration file for database connection settings.

- **IntegrationTestsDatabases.Tests**: The test project folder containing integration tests.
    - **IntegrationTestsDatabases.Tests.csproj**: Project file for the test project.
    - **DataServiceTests.cs**: Contains integration tests for the `DataService` class.
    - **TestUtilities.cs**: Utility methods for setting up and tearing down test environments.

### Prerequisites

- .NET Core SDK 3.1 or higher
- SQL Server / MongoDB / Other Database Systems (local or remote)
- Visual Studio 2019 or later / Visual Studio Code

